#!/bin/bash
# script requires gnu parallel package, and the bash calculator
#
# apt install parallel bc
#
totalruns=$1
threads=$2
input=$3
starttime=$4

#########################################################################################################################################################
#  callservice method - uses separate threads to call AWS Lambda in parallel
#  each thread captures results of one service request, and outputs CSV data...
#########################################################################################################################################################
callservice() {
  totalruns=$1
  threadid=$3
  inputnum=$2
  onesecond=1000

  if [ $threadid -eq 1 ]
  then
    echo "run_id,thread_id,json,output,elapsed_time,sleep_time_ms"

  fi

  for (( i=1 ; i <= $totalruns; i++ ))
  do

   # echo $inputnum	
    time1=( $(($(date +%s%N)/1000000)) )
    output=`curl "https://memorybound.azurewebsites.net/api/MemoryBound?input=$inputnum" >> /dev/null`

    time2=( $(($(date +%s%N)/1000000)) )
    elapsedtime=`expr $time2 - $time1`
    sleeptime=`echo $onesecond - $elapsedtime | bc -l`
    sleeptimems=`echo $sleeptime/$onesecond | bc -l`

    echo "$i,$threadid,$json,$output,$elapsedtime,$sleeptimems"
    if (( $sleeptime > 0 ))
    then
      sleep $sleeptimems
    fi
  done
}
export -f callservice

#########################################################################################################################################################
#  The START of the Script
#########################################################################################################################################################
runsperthread=`echo $totalruns/$threads | bc -l`
runsperthread=${runsperthread%.*}

# If a starttime is provide, loop until we reach the start time before calling service
if [ ! -z "$starttime" ]
then
 t1=`date --date="$starttime" +%s`
 echo "Start script at $t1"
 while : ; do
 dt2=`date +%Y-%m-%d\ %H:%M:%S`
 # Compute the seconds since epoch for date 2
 current_time=`date --date="$dt2" +%s`
 sleep .1
 #echo "compare $current_time >= $t1"
 [ "$current_time" -lt "$t1" ] || break
 done
echo "Starting script now... $current_time"
fi
date
echo "Setting up test: runsperthread=$runsperthread threads=$threads totalruns=$totalruns"
for (( i=1 ; i <= $threads ; i ++))
do
  arpt+=($runsperthread)
done
ainput=$input
#########################################################################################################################################################
# Launch threads to call AWS Lambda in parallel
#########################################################################################################################################################
parallel --no-notice -j $threads -k callservice {1} {2} {"#"} ::: "${arpt[@]}" ::: "${ainput[@]}"
#exit

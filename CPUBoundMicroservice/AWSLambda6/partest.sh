#!/bin/bash
# script requires gnu parallel package, and the bash calculator
#
# apt install parallel bc
#
totalruns=$1
threads=$2
vmreport=$3
contreport=$4
containers=()
cuses=()
ctimes=()

#########################################################################################################################################################
#  callservice method - uses separate threads to call AWS Lambda in parallel
#  each thread captures results of one service request, and outputs CSV data...
#########################################################################################################################################################
callservice() {
  totalruns=$1
  threadid=$2
  host=172.31.50.159
  port=8080
  onesecond=1000

  filename="https://ox1vswxp0c.execute-api.us-east-1.amazonaws.com/DEV/FiboPerfTest"
  #while read -r line
  #do
  #  parurl=$line
  #done < "$filename"

  if [ $threadid -eq 1 ]
  then
    echo "run_id,thread_id,uuid,cputype,cpusteal,vmuptime,pid,cpuusr,cpukrn,elapsed_time,sleep_time_ms,new_container"
  fi
  for (( i=1 ; i <= $totalruns; i++ ))
  do

	json="30"


    time1=( $(($(date +%s%N)/1000000)) )
    
    output=`curl -H "Content-Type: application/text" -X POST -d  $json $parurl 2>/dev/null`


    # grab time
    time2=( $(($(date +%s%N)/1000000)) )

	
	#echo $output
    
    elapsedtime=`expr $time2 - $time1`
    sleeptime=`echo $onesecond - $elapsedtime | bc -l`
    sleeptimems=`echo $sleeptime/$onesecond | bc -l`
    #echo "$i,$threadid,$uuid,$cputype,$cpusteal,$vuptime,$pid,$cpuusr,$cpukrn,$elapsedtime,$sleeptimems,$newcont"
    #echo "$uuid,$elapsedtime,$vuptime,$newcont" >> .uniqcont

	echo "$i,$threadid,$json,$elapsedtime,$sleeptimems"
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
date
echo "Setting up test: runsperthread=$runsperthread threads=$threads totalruns=$totalruns"
for (( i=1 ; i <= $threads ; i ++))
do
  arpt+=($runsperthread)
done
#########################################################################################################################################################
# Launch threads to call AWS Lambda in parallel
#########################################################################################################################################################
parallel --no-notice -j $threads -k callservice {1} {#} ::: "${arpt[@]}"
#exit











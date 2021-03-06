#!/bin/bash
# script requires gnu parallel package, and the bash calculator
#
# apt install parallel bc
#
totalruns=$1
threads=$2
fibo=$3
starttime=$4
#vmreport=$3
#contreport=$4
#containers=()
#cuses=()
#ctimes=()

#########################################################################################################################################################
#  callservice method - uses separate threads to call AWS Lambda in parallel
#  each thread captures results of one service request, and outputs CSV data...
#########################################################################################################################################################
callservice() {
  totalruns=$1
  threadid=$3
  fibonum=$2
  host=18.207.3.175
  port=8080
  onesecond=1000

 # parurl="https://ox1vswxp0c.execute-api.us-east-1.amazonaws.com/DEV/FiboPerfTest"
  #while read -r line
  #do
  #  parurl=$line
  #done < "$filename"

  if [ $threadid -eq 1 ]
  then
    echo "run_id,thread_id,json,elapsed_time,sleep_time_ms"

  fi

 if [ $fibonum == "\"\"" ] || [ $fibonum == "\"0\"" ]
 then
   fibonum=`echo "${RANDOM}0"`
 #echo "Random fibo $fibonum"
 fi

  for (( i=1 ; i <= $totalruns; i++ ))
  do

   # echo$fibonum	
   json=$fibonum
   # json="\"$fibonum\""
    time1=( $(($(date +%s%N)/1000000)) )
    
   #output=`curl -H "Content-Type: application/text" -X POST -d  $json $parurl 2>/dev/null`

  output=`aws lambda invoke --invocation-type RequestResponse --function-name FibonacciCLI --region us-east-1 --log-type Tail --payload "$json" --profile suganjoe outputfile1.txt`


#output="outputfile.txt"
#while read -r line
#  do
#    outputoutput="outputfile.txt"
#while re=$line
#  done < "$outputfile.txt"

    # grab time
    time2=( $(($(date +%s%N)/1000000)) )

	
	#echo $output
    
    elapsedtime=`expr $time2 - $time1`
    sleeptime=`echo $onesecond - $elapsedtime | bc -l`
    sleeptimems=`echo $sleeptime/$onesecond | bc -l`
    #echo "$i,$threadid,$uuid,$cputype,$cpusteal,$vuptime,$pid,$cpuusr,$cpukrn,$elapsedtime,$sleeptimems,$newcont"
    #echo "$uuid,$elapsedtime,$vuptime,$newcont" >> .uniqcont

	echo "$i,$threadid,$fibonum,$elapsedtime,$sleeptimems"
    #echo "I'm here"
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
#afibo="\"$fibo\""
afibo=$fibo
#########################################################################################################################################################
# Launch threads to call AWS Lambda in parallel
#########################################################################################################################################################
parallel --no-notice -j $threads -k callservice {1} {2} {"#"} ::: "${arpt[@]}" ::: "${afibo[@]}"
#exit

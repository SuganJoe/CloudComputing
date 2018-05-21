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

  filename="parurl"
  while read -r line
  do
    parurl=$line
  done < "$filename"

  if [ $threadid -eq 1 ]
  then
    echo "run_id,thread_id,uuid,cputype,cpusteal,vmuptime,pid,cpuusr,cpukrn,elapsed_time,sleep_time_ms,new_container"
  fi
  for (( i=1 ; i <= $totalruns; i++ ))
  do
    #CALCS - uncomment JSON line for desired number of calcs
    #(0) - no calcs - 0
    #json={"\"name\"":"\"\",\"calcs\"":0,\"sleep\"":0,\"loops\"":0}

    #(1) - very light calcs - 2,000
    #json={"\"name\"":"\"\",\"calcs\"":100,\"sleep\"":0,\"loops\"":20}

    #(2) - light calcs - 20,000
    #json={"\"name\"":"\"\",\"calcs\"":1000,\"sleep\"":0,\"loops\"":20}

    #(3) - medium calcs 200,000 
    #json={"\"name\"":"\"\",\"calcs\"":10000,\"sleep\"":0,\"loops\"":20}

    #(4) - somewhat heavy calcs - 500,000
    #json={"\"name\"":"\"\",\"calcs\"":25000,\"sleep\"":0,\"loops\"":20}

	json="30"



    #(5) - heavy calcs - 2,000,000
    #json={"\"name\"":"\"\",\"calcs\"":100000,\"sleep\"":0,\"loops\"":20}

    #(6) - many calcs no memory stress - results in more kernel time - 6,000,000
    #json={"\"name\"":"\"\",\"calcs\"":20,\"sleep\"":0,\"loops\"":300000}

    #(7) - many calcs low memory stress - 10,000,000
    #json={"\"name\"":"\"\",\"calcs\"":100,\"sleep\"":0,\"loops\"":100000}

    #(8) - many calcs higher memory stress - 10,000,000
    #json={"\"name\"":"\"\",\"calcs\"":10000,\"sleep\"":0,\"loops\"":1000}

    #(9) - many calcs even higher memory stress - 10,000,000
    #json={"\"name\"":"\"\",\"calcs\"":100000,\"sleep\"":0,\"loops\"":100}

    time1=( $(($(date +%s%N)/1000000)) )
    #uuid=`curl -H "Content-Type: application/json" -X POST -d "{\"name\": \"Fred\"}" https://ue5e0irnce.execute-api.us-east-1.amazonaws.com/test/test 2>/dev/null | cut -d':' -f 3 | cut -d'"' -f 2` 
    ####output=`curl -H "Content-Type: application/json" -X POST -d  $json https://a9gseqxep9.execute-api.us-east-1.amazonaws.com/test2/test 2>/dev/null`
    ###output=`curl -H "Content-Type: application/json" -X POST -d  $json https://ctbiwxx3f3.execute-api.us-east-1.amazonaws.com/dev1 2>/dev/null`
    ###output=`curl -H "Content-Type: application/json" -X POST -d  $json https://ue5e0irnce.execute-api.us-east-1.amazonaws.com/test/test 2>/dev/null`
    output=`curl -H "Content-Type: application/text" -X POST -d  $json $parurl 2>/dev/null`
    #output=`curl -H "Content-Type: application/json" -X POST -d  $json https://b3euo2n6s7.execute-api.us-east-1.amazonaws.com/test 2>/dev/null`
    ########################output=`curl -H "Content-Type: application/json" -X POST -d  $json https://i1dc63pzgh.execute-api.us-east-1.amazonaws.com/test5/ 2>/dev/null`
    #output=`curl -H "Content-Type: application/json" -X POST -d  $json https://ue5e0irnce.execute-api.us-east-1.amazonaws.com/test/test 2>/dev/null | cut -d':' -f 3 | cut -d'"' -f 2` 

    # grab time
    time2=( $(($(date +%s%N)/1000000)) )

    # parsing when /proc/cpuinfo is not requested  
    #uuid=`echo $output | cut -d':' -f 3 | cut -d'"' -f 2`
    #cpuusr=`echo $output | cut -d':' -f 4 | cut -d',' -f 1`
    #cpukrn=`echo $output | cut -d':' -f 5 | cut -d',' -f 1`
    #pid=`echo $output | cut -d':' -f 6 | cut -d',' -f 1`
    #cputype="unknwn"

    # parsing when /proc/stat is requested
    #uuid=`echo $output | cut -d',' -f 2 | cut -d':' -f 2 | cut -d'"' -f 2`
    #cpuusr=`echo $output | cut -d',' -f 3 | cut -d':' -f 2`
    #cpukrn=`echo $output | cut -d',' -f 4 | cut -d':' -f 2 | cut -d'"' -f 2`
    #pid=`echo $output | cut -d',' -f 5 | cut -d':' -f 2 | cut -d'"' -f 2`
    #cpusteal=`echo $output | cut -d'"' -f 4 | cut -d' ' -f 9`
    #cputype="unknwn"
	
	echo $output
    ## parsing when /proc/cpuinfo is requested
    #uuid=`echo $output | cut -d',' -f 2 | cut -d':' -f 2 | cut -d'"' -f 2`
    #cpuusr=`echo $output | cut -d',' -f 3 | cut -d':' -f 2`
    #cpukrn=`echo $output | cut -d',' -f 4 | cut -d':' -f 2 | cut -d'"' -f 2`
    #pid=`echo $output | cut -d',' -f 5 | cut -d':' -f 2 | cut -d'"' -f 2`
    #cputype="unknown"
    #cputype=`echo $output | cut -d',' -f 1 | cut -d':' -f 7 | cut -d'\' -f 1 | xargs`
    #cpusteal=`echo $output | cut -d',' -f 13 | cut -d':' -f 2`
    #vuptime=`echo $output | cut -d',' -f 14 | cut -d':' -f 2`
    #newcont=`echo $output | cut -d',' -f 15 | cut -d':' -f 2`
    
    elapsedtime=`expr $time2 - $time1`
    sleeptime=`echo $onesecond - $elapsedtime | bc -l`
    sleeptimems=`echo $sleeptime/$onesecond | bc -l`
    #echo "$i,$threadid,$uuid,$cputype,$cpusteal,$vuptime,$pid,$cpuusr,$cpukrn,$elapsedtime,$sleeptimems,$newcont"
    #echo "$uuid,$elapsedtime,$vuptime,$newcont" >> .uniqcont
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
newconts=0
recycont=0
recyvms=0










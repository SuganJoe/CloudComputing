#cript requires gnu parallel package, and the bash calculator
#
# apt install parallel bc
#
totalruns=$1
threads=$2
fibo=$3
starttime=$4

#########################################################################################################################################################
#  callservice method - uses separate threads to call Azure functions in parallel
#  each thread captures results of one service request, and outputs CSV data...
#########################################################################################################################################################
callservice() {
  totalruns=$1
  threadid=$3
  fibonum=$2
   host=104.42.211.122
   port=8080
   onesecond=1000
 parurl="https://funcforsyncfibo.azurewebsites.net/api/Function1?code=2KgC2Q8wR0uEeh9vZPpHCdkoQ4k63EcyGuf2at/Xq9xTYcF/biIRvA=="

 #  parurl="https://fiborestsync.azurewebsites.net/api/Function1?code=ZFOryEcyy0Uz9Ehognz0hc7/kXLZ2umdviEFarnOh9c5A9wDJVhKPg=="
  parurl="https://fiborestsyncnew.azurewebsites.net/api/Function1?code=qH6ctNdlvOJQZhR3M4z9BJPqt1sJLEm2k/RMedj7KDCyy9SNAF2KSQ=="		        
  if [ $threadid -eq 1 ]
  then
  echo "run_id,thread_id,json,output,elapsed_time,sleep_time_ms,latency"

  fi

  if [ $fibonum == "\"\"" ] || [ $fibonum == "\"0\"" ]
  then
  fibonum=`echo "${RANDOM}0"`
   #echo "Random fibo $fibonum"
  fi

  for (( i=1 ; i <= $totalruns; i++ ))
  do
   json=$fibonum
  #json='{"number":"$fibonum"}'
   # json="\"$fibonum\""
  time1=($(($(date +%s%N)/1000000)))

           #output=`curl -H "Content-Type: application/text" -X POST -d  $json $parurl 2>/dev/null`
	   
	   output=`curl -X POST -d $json $parurl 2>/dev/null`

		    # grab time
		        time2=( $(($(date +%s%N)/1000000)) )
                         elapsedtime2="$(echo $output | cut -d'=' -f3)"
			                                 #echo $elapsedtime2
	   		fibo_output="$(echo $output | cut -d'=' -f2 | cut -d';' -f1)"

			    elapsedtime=`expr $time2 - $time1`
			        sleeptime=`echo $onesecond - $elapsedtime | bc -l`
				    sleeptimems=`echo $sleeptime/$onesecond | bc -l`
					latency=$(expr "$elapsedtime" - "$elapsedtime2")
						echo "$i,$threadid,$json,$fibo_output,$elapsedtime,$sleeptimems,$latency"
						           # echo "$i,$threadid,$json,$output,$elapsedtime,$sleeptimems"
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
 # Launch threads to call Azure function in parallel
											    #################################################################################################
 parallel --no-notice -j $threads -k callservice {1} {2} {"#"} ::: "${arpt[@]}" ::: "${afibo[@]}"
											    #exit

										  

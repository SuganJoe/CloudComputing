






parallel-scp -h hosts  -l ubuntu -x "-oStrictHostKeyChecking=no -i 1stEC2.pem" networkio.sh  /home/ubuntu/networkio.sh

parallel-ssh -h hosts -l ubuntu -x "-oStrictHostKeyChecking=no -i 1stEC2.pem" 'chmod 777 networkio.sh'

parallel-scp -h hosts -l ubuntu -x "-oStrictHostKeyChecking=no -i 1stEC2.pem" 'mkdir outdir_kir'

parallel-ssh -h hosts -t 100000000 -l ubuntu -x "-oStrictHostKeyChecking=no -i 1stEC2.pem" -o outdir_kir -e errdir './networkio.sh 100 1'
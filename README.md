# CloudComputing
Comparison of serverless computing platforms- AWS Lambda, Azure functions

Prerequisites for AWS and Azure setup:

AWS Prerequisite Commands
1. pip install awscli --upgrade --user
2. wget -q packages-microsoft-prod.deb https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb
3. ubuntu@ip-10-0-0-52:~$ sudo dpkg -i packages-microsoft-prod.deb
4. ubuntu@ip-10-0-0-52:~$ sudo apt-get install apt-transport-https
5. sudo apt-get update
6. sudo apt-get install dotnet-sdk-2.1.200

AZURE Prerequisite on linux Commands
1. sudo apt-get install linuxbrew-wrapper
2. test -d ~/.linuxbrew && PATH="$HOME/.linuxbrew/bin:$HOME/.linuxbrew/sbin:$PATH"
test -d /home/linuxbrew/.linuxbrew && PATH="/home/linuxbrew/.linuxbrew/bin:/home/linuxbrew/.linuxbrew/sbin:$PATH"
test -r ~/.bash_profile && echo "export PATH='$(brew --prefix)/bin:$(brew --prefix)/sbin'":'"$PATH"' >>~/.bash_profile
         echo "export PATH='$(brew --prefix)/bin:$(brew --prefix)/sbin'":'"$PATH"' >>~/.profile
3. brew update
4. brew install azure-cli

Others
1. sudo apt-get update
2. sudo apt-get install python3.6

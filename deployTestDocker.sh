branchName="$(Build.SourceBranch)"
prStart=${branchName:10:100}
firstSlash=$(expr index "$prStart" '/')
prNumber=${prStart:0:$firstSlash-1}
echo "PrNumber:"$prNumber
portNumber=$((($prNumber%100)+$(BasePort)))
echo "PortNumber:"$portNumber
docker pull $(ImageName):$prNumber
docker stop $(ContainerName)_$prNumber || true 
docker rm $(ContainerName)_$prNumber || true 
docker run --detach --name=$(ContainerName)_$prNumber -p $portNumber:80 -e MONGO_CONNECTION_STRING=$(MONGO_CONNECTION_STRING) -e TEST_ENV=$(TEST_ENV) $(ImageName):$prNumber
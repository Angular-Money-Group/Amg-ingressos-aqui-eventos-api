#!/bin/bash 
container_name=events-api-v1

#ECR Login
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin awsaccountid.dkr.ecr.us-east-1.am

#Pulling image from ECR
docker pull 590184025050.dkr.ecr.sa-east-1.amazonaws.com/api-events-prod:latest

##Changing image tag
docker image tag 590184025050.dkr.ecr.sa-east-1.amazonaws.com/api-events-prod:latest $container_name:latest

#stop and remove the current container docker rm -f $container_name

#Creating and starting a docker container using a new image
docker run -d -p 80:80 --name $container_name $container_name:latest
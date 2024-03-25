#!/bin/bash 
container_name=events-api-v1

#ECR Login
aws ecr get-login-password --region us-east-1 | docker login --username AWS --password-stdin 590184025050.dkr.ecr.us-east-1.amazonaws.com

#Pulling image from ECR
docker pull 590184025050.dkr.ecr.us-east-1.amazonaws.com/events-api-prod:latest

##Changing image tag
docker image tag 590184025050.dkr.ecr.us-east-1.amazonaws.com/events-api-prod:latest $container_name:latest

#stop and remove the current container docker rm -f $container_name
docker stop $container_name
docker rm $container_name
#Creating and starting a docker container using a new image
docker run -d -p 3001:80 --name $container_name $container_name:latest
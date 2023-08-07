git switch develop
git pull
docker container stop apieventos
docker container rm -f apieventos
docker build  -t eventos-api .
docker run -d -p 3002:80 -v /root/Amg-ingressos-aqui-eventos-api/Amg-ingressos-aqui-eventos-api/images:/app/images --name apieventos eventos-api
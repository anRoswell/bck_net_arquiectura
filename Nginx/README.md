docker build -t mi_nginx .
docker run -d --name nginx_proxy --network ngin?red -p 80:80 mi_nginx

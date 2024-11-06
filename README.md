Construir la imagen
docker build -t backop360dev .
docker build -t backop360qa .

// Con variables de entorno
docker build --build-arg AppEnv=Development -t backop360dev .
docker build --build-arg AppEnv=QA -t backop360qa .

Construir contenedor
// Backend
docker run -p 8083:80 73197546/backop360:1.0.1
docker run -d -p 8083:80 73197546/backop360:1.0.1
docker run -d -p 8083:80 backop360
docker run -p 8083:80 backop360

docker run -p 8083:80 backop360dev
docker run -p 8083:80 backop360qa

// DEV
docker run -p 8083:80 -v D:\dockerdata:/app/archivos 73197546/backop360dev:1.0.1
docker run -d -p 8083:80 -v /home/sysadmin/op360files:/app/archivos 73197546/backop360dev:1.0.1
docker run -d -p 8083:80 -v /home/sysadmin/filesOp360/op360files:/app/archivos 73197546/backop360dev:1.0.1 => Producción

// QA
docker run -p 8083:80 -v D:\dockerdata:/app/archivos 73197546/backop360qa:1.0.1
docker run -d -p 8083:80 -v /home/sysadmin/op360files:/app/archivos 73197546/backop360qa:1.0.1
docker run -d -p 8083:80 -v /home/sysadmin/filesOp360/op360files:/app/archivos 73197546/backop360qa:1.0.1 => Producción

// Front end DEV
docker run -p 8083:80 73197546/backop360dev:1.0.1
docker run -d -p 8083:80 73197546/backop360dev:1.0.1

// Front end QA
docker run -p 8083:80 73197546/backop360qa:1.0.1
docker run -d -p 8083:80 73197546/backop360qa:1.0.1

// Cambiar nombre a la imagen
docker image tag backop360dev 73197546/backop360dev:1.0.1
docker image tag backop360qa 73197546/backop360qa:1.0.1

// PUSH
docker push 73197546/backop360dev:1.0.1
docker push 73197546/backop360qa:1.0.1

// PULL
docker pull 73197546/backop360dev:1.0.1
docker pull 73197546/backop360qa:1.0.1

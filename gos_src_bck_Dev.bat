@echo off

:: Establecer colores para los títulos
color 0E &:: Amarillo sobre fondo negro

title EJECUTANDO DOCKER DEV

:: OBTENIENDO GIT
@echo git pull
git pull
git add .
git commit -m "Commit automatico realizado por bat"
@echo git finalizado exitosamente!!!

:: BUILD
@echo Ejecutando docker build --build-arg AppEnv=Development -t src_gos_back_op360_dev .
docker build --build-arg AppEnv=Development -t src_gos_back_op360_dev .

:: CHANGE NAME
color 0A &:: Verde sobre fondo negro
@echo CHANGE TAG
docker image tag src_gos_back_op360_dev 73197546/src_gos_back_op360_dev:1.0.1
color 0E &:: Amarillo sobre fondo negro
@echo Cambio de Tag finalizado

:: PUSH
color 0B &:: Cyan sobre fondo negro
@echo Ejecutando docker push 73197546/src_gos_back_op360_dev:1.0.1
docker push 73197546/src_gos_back_op360_dev:1.0.1

color 0F &:: Blanco sobre fondo negro
@echo Finalizado
pause

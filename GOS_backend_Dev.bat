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
<<<<<<<< HEAD:GOS_backend_Dev.bat
@echo Ejecutando docker build --build-arg AppEnv=Development -t backop360devgos .
docker build --build-arg AppEnv=Development -t backop360devgos .
========
@echo Ejecutando docker build --build-arg AppEnv=Development -t src_gos_back_op360_dev .
docker build --build-arg AppEnv=Development -t src_gos_back_op360_dev .
>>>>>>>> f7f1173051134e2d3e91fc62a3226e138ab601bd:src_gos_back_op360_DEV.bat

:: CHANGE NAME
color 0A &:: Verde sobre fondo negro
@echo CHANGE TAG
<<<<<<<< HEAD:GOS_backend_Dev.bat
docker image tag backop360devgos 73197546/backop360devgos:1.0.1
========
docker image tag src_gos_back_op360_dev 73197546/src_gos_back_op360_dev:1.0.1
>>>>>>>> f7f1173051134e2d3e91fc62a3226e138ab601bd:src_gos_back_op360_DEV.bat
color 0E &:: Amarillo sobre fondo negro
@echo Cambio de Tag finalizado

:: PUSH
color 0B &:: Cyan sobre fondo negro
<<<<<<<< HEAD:GOS_backend_Dev.bat
@echo Ejecutando docker push 73197546/backop360devgos:1.0.1
docker push 73197546/backop360devgos:1.0.1
========
@echo Ejecutando docker push 73197546/src_gos_back_op360_dev:1.0.1
docker push 73197546/src_gos_back_op360_dev:1.0.1
>>>>>>>> f7f1173051134e2d3e91fc62a3226e138ab601bd:src_gos_back_op360_DEV.bat

color 0F &:: Blanco sobre fondo negro
@echo Finalizado
pause

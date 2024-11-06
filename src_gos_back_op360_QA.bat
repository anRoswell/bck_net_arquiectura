@echo off

:: Establecer colores para los títulos
color 0E &:: Amarillo sobre fondo negro

title EJECUTANDO DOCKER QA

:: OBTENIENDO GIT
@echo git pull
git pull
git add .
git commit -m "Commit automaico realizado por bat"
@echo git finalizado exitosamente!!!

:: BUILD
@echo Ejecutando docker build --build-arg AppEnv=QA -t backop360qa .
docker build --build-arg AppEnv=QA -t backop360qa .

:: CHANGE NAME
color 0A &:: Verde sobre fondo negro
@echo CHANGE TAG
docker image tag backop360qa 73197546/backop360qa:1.0.1
color 0E &:: Amarillo sobre fondo negro
@echo Cambio de Tag finalizado

:: PUSH
color 0B &:: Cyan sobre fondo negro
@echo Ejecutando docker push 73197546/backop360qa:1.0.1
docker push 73197546/backop360qa:1.0.1

color 0F &:: Blanco sobre fondo negro
@echo Finalizado
pause

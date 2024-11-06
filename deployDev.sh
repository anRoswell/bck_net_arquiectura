#!/bin/bash

# Establecer colores para los mensajes
YELLOW='\033[1;33m'
GREEN='\033[0;32m'
CYAN='\033[0;36m'
RED='\033[0;31m'
NC='\033[0m' # Sin color

# Función para verificar el resultado de un comando
check_command() {
  if [ $? -ne 0 ]; then
    echo -e "${RED}Error: $1 falló.${NC}"
    exit 1
  fi
}

# Título (no hay una forma directa de establecer un título en la terminal de macOS como en Windows)
echo -e "${YELLOW}EJECUTANDO DOCKER DEV${NC}"

# OBTENIENDO GIT
# echo "git pull"
# git pull
# git add .
# git commit -m "Commit automatico realizado por bash"
# echo "git finalizado exitosamente!!!"

# Limpiar espacio de Docker
echo -e "${CYAN}Limpiando espacio de Docker...${NC}"
docker container prune -f && docker image prune -a -f && docker volume prune -f && docker network prune -f && docker system prune -a -f
check_command "docker prune"

# Login en Docker Hub
# echo -e "${CYAN}Iniciando sesión en Docker Hub...${NC}"
# docker login
# check_command "docker login"

# BUILD
echo -e "${YELLOW}Ejecutando docker build --platform linux/amd64 --build-arg AppEnv=Development -t backop360dev .${NC}"
docker build --platform linux/amd64 --build-arg AppEnv=Development -t backop360dev .
check_command "docker build"

# Verificar si la imagen fue creada
docker images | grep backop360dev
check_command "Verificación de la imagen Docker"

# CHANGE NAME
echo -e "${GREEN}CHANGE TAG${NC}"
docker image tag backop360dev 73197546/backop360dev:1.0.1
check_command "docker image tag"
echo -e "${YELLOW}Cambio de Tag finalizado${NC}"

# PUSH
echo -e "${CYAN}Ejecutando docker push 73197546/backop360dev:1.0.1${NC}"
docker push 73197546/backop360dev:1.0.1
check_command "docker push"

# Finalizado
echo -e "${NC}Finalizado"
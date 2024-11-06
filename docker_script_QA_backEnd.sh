COLOR_TITLE='\e[1;33m'  # Amarillo brillante
COLOR_RESET='\e[0m'

# PULL
echo -e "${COLOR_TITLE}"
echo "ejecutando docker pull 73197546/backop360qa:1.0.1"
docker pull 73197546/backop360qa:1.0.1
echo -e "${COLOR_RESET}"

# RUN
echo -e "${COLOR_TITLE}"
echo "ejecutando docker run -d -p 8183:80 -v /image01/op360files/scr/files:/app/scr/files -v /image01/op360files/scr/images:/app/scr/images -v /image01/op360files/gos/files:/app/gos/files -v /image01/op360files/gos/images:/app/gos/images 73197546/backop360qa:1.0.1"
docker run -d -p 8183:80 -v /image01/op360files/scr/files:/app/scr/files -v /image01/op360files/scr/images:/app/scr/images -v /image01/op360files/gos/files:/app/gos/files -v /image01/op360files/gos/images:/app/gos/images 73197546/backop360qa:1.0.1
echo -e "${COLOR_RESET}"

# Finalizado
echo -e "${COLOR_TITLE}"
echo "finalizado"
echo -e "${COLOR_RESET}"

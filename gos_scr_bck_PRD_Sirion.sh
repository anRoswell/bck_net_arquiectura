COLOR_TITLE='\e[1;33m'  # Amarillo brillante
COLOR_RESET='\e[0m'

# PULL
echo -e "${COLOR_TITLE}"
echo "podman pull 73197546/src_gos_backop360prd:1.0.1"
      podman pull 73197546/src_gos_backop360prd:1.0.1
echo -e "${COLOR_RESET}"

# RUN
echo -e "${COLOR_TITLE}"
echo "podman run -d -p 8183:80 -v /data/image01/op360files/filesOp360/op360files/scr/files:/app/scr/files -v /data/image01/op360files/filesOp360/op360files/scr/images:/app/scr/images -v /data/image01/op360files/filesOp360/op360files/gos/files:/app/gos/files -v /data/image01/op360files/filesOp360/op360files/gos/images:/app/gos/images 73197546/src_gos_backop360prd:1.0.1"
      podman run -d -p 8183:80 -v /data/image01/op360files/filesOp360/op360files/scr/files:/app/scr/files -v /data/image01/op360files/filesOp360/op360files/scr/images:/app/scr/images -v /data/image01/op360files/filesOp360/op360files/gos/files:/app/gos/files -v /data/image01/op360files/filesOp360/op360files/gos/images:/app/gos/images 73197546/src_gos_backop360prd:1.0.1
echo -e "${COLOR_RESET}"

# Finalizado
echo -e "${COLOR_TITLE}"
echo "finalizado"
echo -e "${COLOR_RESET}"


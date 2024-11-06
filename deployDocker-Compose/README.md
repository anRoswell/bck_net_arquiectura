4. Crea un archivo de servicio en /etc/systemd/system/docker-compose-app.service:

sudo nano /etc/systemd/system/docker-compose-app.service

[Unit]
Description=Docker Compose App
Requires=docker.service
After=docker.service

[Service]
WorkingDirectory=/ruta/a/tu/directorio
ExecStart=/usr/local/bin/docker-compose up -d
ExecStop=/usr/local/bin/docker-compose down
Restart=always

[Install]
WantedBy=multi-user.target

5. Habilitar e iniciar el servicio:

//Ejecuta los siguientes comandos para habilitar e iniciar el servicio:

sudo systemctl enable docker-compose-app
sudo systemctl start docker-compose-app

6. Verificar que los contenedores se inicien automáticamente:

// Reinicia tu sistema para verificar que los contenedores se inicien automáticamente al arrancar.

sudo reboot

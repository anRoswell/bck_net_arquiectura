Web
	[50%]-- Listar Ordenes
			--[GET] https://localhost:44326/api/Op360Ordenes/ObtenerOrdenesTrabajo
			--Retorna todas las ordenes 
			--Esta conectado al query que nos paso el inge Pedrito.
			*-falta definir si se conecta a un sp
			*-falta integrar la paginación.
			*-pruebas
			
	[20%]-- Actualizar Ordenes
			--[PUT] https://localhost:44326/api/Op360Ordenes/ActualizarOrden
			--Por el momento recibo el id_orden
			--Metodo Dummi
			*-falta integrarlo con un sp y probar
			*-pruebas
			
			aire.pkg_g_ordenes.prc_actualizar_orden (e_orden in out aire.ord_ordenes%rowtype, s_respuesta out aire.tip_respuesta)
			
			
	[40%]-- Consultar Ordenes
			--[GET] https://localhost:44326/api/Op360Ordenes/Search?id_orden=25
			--Recibe un parametro Id_Orden
			--Retorna una orden utilizando el query el inge Pedrito y filtrando por Id_Orden.
			*-falta definir si se conecta a un sp
			*-pruebas
			
			
	[50%]-- Exportar Ordenes
			--[GET] https://localhost:44326/api/Op360Ordenes/ExportarOrdenesExcel
			--Este endPoint Retorna un excel con las ordenes de trabajo.
			
			
	[xxx]-- Registrar Ordenes POST 
			--va asignar el contratista a la orden. id_oden, id_contratista.
			
			aire.pkg_g_ordenes.prc_registrar_orden (e_orden in out aire.ord_ordenes%rowtype,s_respuesta	out	aire.tip_respuesta)
			
			
	[xxx]-- Guardado Masivo de ordenes POST 
			--me envian una lista de ordenes por medio de un excel.
			--recibir un excel
			--es consumir el sp de registrar orden por medio de un ciclo.
			
Otros	
	[99%]-- Guardado Archivos 
			--[GET] https://localhost:44326/api/Op360Files/SaveFiles
			-- Recibe multiples archivos y los guarda.		
			
Movil
	[50%]-- Listar Ordenes 
			--[GET] https://localhost:44326/api/Op360mOrdenes/ObtenerOrdenesTrabajoMovil
			--Retorna todas las ordenes 
			--Esta conectado al query que nos paso el inge Pedrito.
			*-falta definir si se conecta a un sp
			*-falta integrar la paginación.
			*-pruebas
			
	[xxx]-- Registrar Orden 
			--update de la orden. el contratista llenaria el formulario y me lo envia.
			
Incidencia
	[xxx]-- Guardar Incidencia
	
Seguridad
	[80%]--Login Consulta de Menu, perfiles y acceso:
			--[GET] https://localhost:44326/api/Op360Seguridad/LoginExterno?Id_Usuario=7CE82FEC1636B7EBE159D6D480369BF8&Token_Apex=A070A659C03DE3E286A7E0D1D18B4C56
			--Ya esta conectada al sp aire.pkg_g_seguridad.prc_validar_token_informacion
			*-Falta probar 
			*-Falta probar jwt
Incidencias:
	[50%]-- Registrar Incidencias 
			--[POST] https://localhost:44326/api/Op360Incidencias/GestionIncidencia
			--Procedimiento para registrar las incidencias

				aire.pkg_g_generales.prc_gestion_incidencia (
						e_incidencia_informacion	in clob,
						s_respuesta	                out 	aire.tip_respuesta
					);
				ejemplo del json que se debe pasar en el parametro e_incidencia_informacion
				{
					"usuario": 12345, --id_usuario
					"descripcion": "sjiufdffdl",
					"evidencias": [
						{
							"nombre": "a","formato": "b","peso": 0.6666,"url": "c"
						},
						{
							"nombre": "fykjfkjfds",
							"formato": "ffdkkjf",
							"peso": 0.6666,
							"url": "hjgshjgsajsagfdsf"
						}
					]
				}
			--Falta definir que parametros de entrada se reciben.
			--Falta integrar con Angular y trabajar con data real
------------------------------
LISTA DE ENDPOINT QUE NECESITAMOS
 
*- VALIDACION DE LLENADO DE ENCUESTA CUANDO INICIA SESION  (GET id_tecnico)
	-- SOLICITUD TIPO GET CON CRITERIO DE BUSQUEDA ID_TECNICO
		-- ID_TECNICO
		-- FECHA DE INGRESO VS FECHA DE REGISTRO 
	-- RESPUESTA
		-- SI YA SE REALIZÓ
		{
			estado: 200
			mensaje: "Encuenta ya se realizó",
			datos: []
		}
		-- NO SE HA REALIZADO
		{
			estado: 200
			mensaje: "Encuenta a realizar",
			datos: [
				{
				"id_cuestionario": 1,
				"nombre": "Cuestionario Inicial de salud y seguridad",
				"codigo_tipo_formulario":"FISCR",
				"descripcion_tipo_formulario": "Formulario inicial SCR",
				"preguntas": [
					{
						"id_pregunta": 1,
						"pregunta": "¿Porque 1 + 1 es 2?",
						"configuracion": {
							"codigo_tipo_pregunta": "IMP", //dominio valor
							"descripcion_tipo_pregunta": "Imput", // dominio valor
							"requerida": "S",
							"valores" : [				
							  {
								key: "SI, BUEN ESTADO",
								value: "SI, BUEN ESTADO"
							  },
							  {
								key: "SI, MAL ESTADO",
								value: "SI, MAL ESTADO"
							  },
							  {
								key: "NO",
								value: "NO"
							  }
							]
						}
					}
			]
}
			]		
		}
*- REGISTRAR ENCUENSTA DE INICIO SESION (POST)
	-- SOLICITUD TIPO POST 
		ENVIO JSON
		{
			datos: [
				{
					"id_cuestionario": 1,
					"id_orden": 254,
					"id_contratista_persona": 2081,
					"respuestas": [
						{
							"id_pregunta": 1,
							"respuesta": " Porque asi es la matematica"
						}
					]
				}
			]		
		}
	-- RESPUESTA 
		-- EXITOSO
			{
				estado: 200
				mensaje: "Registro de encuesta exitoso"
			}
		-- ERROR
			{
				estado: 400
				mensaje: "Registro de encuesta con errores"
			}

 
*- LISTAR ORDENES ASIGNADAS (GET)
	-- SOLICITUD TIPO GET (ID_TECNICO)
		-- ID_TECNICO
		-- TOKEN INICIO SESION
	{
		estado: 200
		mensaje: "Informacion de las ordenes"
		datos: [
			{
				Tipo: Tipo de orden (TO501, TO502, TO503, TO504, TO505, TO506)
				Nic: Informacion del cliente
				Orden: Numero de la orden traida de OSF
				Direccion: direccion donde se va a realizar la orden
				Estado: Estado de la Orden
				Longintud: Posicion del lugar
				Latitud: Posicion del lugar	
				N_Orden:
				Acta:
				Descripcion:
				Cliente: 
				Municipio
				Direccion:
				Tarifa:
				Telefono:
				Carga_Contratada:
				Medidor:
				Marca:
				Ultima_Factura:
				Fecha_Ultima_Factura:
				Deuda:
				Cantidad_Factura:
				Comentarios:
			}
		]

	}		
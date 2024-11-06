namespace Core.Tools
{
    using Core.CustomEntities;
    using Core.Enumerations;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;

    public class Mail
    {
        readonly string From = Email_Configuration.From; //de quien procede, puede ser un alias
        readonly string To;  //a quien vamos a enviar el mail
        readonly string Message;  //mensaje
        readonly string Subject; //asunto
        readonly List<string> ListadoAdjuntos_URL; //lista de archivos a enviar
        readonly List<Email_Archivo> ListadoAdjuntos_Archivo; //lista de archivos a enviar
        readonly string De = Email_Configuration.From; //nuestro usuario de smtp
        readonly string Pass = Email_Configuration.Pass; //nuestro password de smtp

        MailMessage Email;

        public string error = "";

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="FROM">Procedencia</param>
        /// <param name="Para">Mail al cual se enviara</param>
        /// <param name="Mensaje">Mensaje del mail</param>
        /// <param name="Asunto">Asunto del mail</param>
        /// <param name="ArchivoPedido_">Archivo a adjuntar, no es obligatorio</param>
        public Mail(string Para, string Mensaje, string Asunto, string alias = Email_Configuration.From)
        {
            From = alias;
            To = Para;
            Message = Mensaje;
            Subject = Asunto;
            ListadoAdjuntos_URL = new List<string>();
            ListadoAdjuntos_Archivo = new List<Email_Archivo>();
        }

        /// <summary>
        /// Método para adjuntar archivos en el correo, a través de un archivo Stream.
        /// </summary>
        /// <param name="adjunto"></param>
        public void AgregarAdjunto_PathFileServer(string adjunto)
        {
            ListadoAdjuntos_URL.Add(adjunto);
        }

        /// <summary>
        /// Método para adjuntar archivos en el correo, a través de una ruta.
        /// </summary>
        /// <param name="adjunto"></param>
        public void AgregarAdjunto_Documento(Email_Archivo adjunto)
        {
            ListadoAdjuntos_Archivo.Add(adjunto);
        }

        /// <summary>
        /// metodo que envia el mail
        /// </summary>
        /// <returns></returns>
        public bool EnviaMail()
        {

            //una validación básica
            if (To.Trim().Equals("") || Message.Trim().Equals("") || Subject.Trim().Equals(""))
            {
                error = "El mail, el asunto y el mensaje son obligatorios";
                return false;
            }

            //aqui comenzamos el proceso
            //comienza-------------------------------------------------------------------------
            try
            {
                //creamos un objeto tipo MailMessage
                //este objeto recibe el sujeto o persona que envia el mail,
                //la direccion de procedencia, el asunto y el mensaje
                Email = new MailMessage(From, To, Subject, Message);

                //si viene archivo a adjuntar
                //realizamos un recorrido por todos los adjuntos enviados en la lista
                //la lista se llena con direcciones fisicas, por ejemplo: c:/pato.txt
                if (ListadoAdjuntos_URL.Any())
                {
                    //agregado de archivo
                    foreach (string archivo in ListadoAdjuntos_URL)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (File.Exists(@archivo))
                            Email.Attachments.Add(new Attachment(@archivo));
                    }
                }

                if (ListadoAdjuntos_Archivo.Any())
                {
                    //agregado de archivo
                    foreach (Email_Archivo adjunto in ListadoAdjuntos_Archivo)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        Email.Attachments.Add(new Attachment(new MemoryStream(adjunto.Archivo), adjunto.Nombre, adjunto.TipoArchivo));
                    }
                }

                Email.IsBodyHtml = true; //definimos si el contenido sera html
                Email.From = new MailAddress(From); //definimos la direccion de procedencia

                //aqui creamos un objeto tipo SmtpClient el cual recibe el servidor que utilizaremos como smtp
                //en este caso me colgare de gmail
                SmtpClient smtpMail = new SmtpClient(Email_Configuration.Host)
                {
                    EnableSsl = true,//le definimos si es conexión ssl
                    UseDefaultCredentials = false, //le decimos que no utilice la credencial por defecto
                    Host = Email_Configuration.Host, //agregamos el servidor smtp, ej: smtp.gmail.com
                    Port = Email_Configuration.Port, //le asignamos el puerto, en el caso de gmail utiliza el 465
                    Credentials = new NetworkCredential(De, Pass) //agregamos nuestro usuario y pass de correo
                };

                //enviamos el mail
                smtpMail.Send(Email);

                //eliminamos el objeto
                smtpMail.Dispose();

                //regresamos true
                return true;
            }
            catch (Exception ex)
            {
                //si ocurre un error regresamos false y el error
                error = "Ocurrio un error: " + ex.Message;
                return false;
            }
        }
    }
}

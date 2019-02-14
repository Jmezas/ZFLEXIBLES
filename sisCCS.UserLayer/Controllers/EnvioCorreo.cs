using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
namespace sisCCS.UserLayer.Controllers
{
    public class EnvioCorreo
    {

        public void SendMailFactura(string Asunto, string mensaje, string Correo,string File,string CorreoEmpresa, string NombreEmpresa,string Contrasenia)
        {

            MailMessage msg = new MailMessage();
            msg.To.Add(Correo);
            msg.From = new MailAddress(CorreoEmpresa, NombreEmpresa, System.Text.Encoding.UTF8);
            msg.Subject = Asunto;
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            msg.Body = mensaje;
            msg.BodyEncoding = System.Text.Encoding.UTF8;
           
            Attachment data = new Attachment(File);
            msg.Attachments.Add(data);
            msg.IsBodyHtml = true;

            //Aquí es donde se hace lo especial
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(CorreoEmpresa, Contrasenia);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true; //Esto es para que vaya a través de SSL que es obligatorio con GMail
            try
            {
                client.Send(msg);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}
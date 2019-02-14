using sisCCS.BusinessLayer;
using sisCCS.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace sisCCS.UserLayer.Models
{
    public class Authentication
    {
        private BUsuario BUsuario = BUsuario.ObtenerInstancia();

        public const int SinPermisos = 1;
        public const int DirectorioActivo = 2;
        public const int ServidorIris = 3;

        public const string SessionCookieName = "SessionCookie";
        public const string SessionUser = "UsuarioLogueado";

        public EUsuario UserLogued { get; set; }
        public int idComprobantePago { get; set; }
        public int idTipoComprobantePago { get; set; }

        public HttpRequest Request { get; set; }
        public HttpResponse Response { get; set; }
        public HttpCookie SessionCookie { get; set; }
        public HttpSessionState Session { get; set; }

        public Authentication()
        {
            Request = HttpContext.Current.Request;
            Response = HttpContext.Current.Response;
            SessionCookie = Request.Cookies["SessionCookie"];
            idComprobantePago = 0;

            AssignUser();

            if (UserLogued != null)
                Session = HttpContext.Current.Session;
        }

        /// <summary>
        /// Restart session by user logued
        /// </summary>
        public void RestartSession()
        {
            if (UserLogued != null)
            {
                Session.Timeout = (int)(DateTime.Now.AddDays(1) - DateTime.Now).TotalSeconds; // Definición del tiempo de sesión con respecto a la cookie
                Session.Add("Usuario", UserLogued); // Guardado del usuario logueado en sesión
                return;
            }
            throw new InvalidOperationException("There is not an user logued to restart session.");
        }

        /// <summary>
        /// Assign user logued to local var in the class
        /// </summary>
        private void AssignUser()
        {
            // Si existe un usuario logueado
            if (SessionCookie != null)
            {
                // Se captura un usuario
                string sUsuario = SessionCookie.Values["Usuario"];
                UserLogued = BUsuario.BuscarPorUsuario(sUsuario);
                List<EMenu> lPermisos = BUsuario.ListarMenuPorUsuario(sUsuario);
                if (lPermisos.Count > 0)
                {
                    UserLogued.Menu = lPermisos;
                }
            }
        }

        /// <summary>
        /// Check if view id received is valid from the user logued
        /// </summary>
        /// <param name="Id">View id</param>
        /// <returns></returns>
        public bool IsValidView(long Id)
        {
            if (UserLogued != null)
            {
                foreach (EMenu Permiso in UserLogued.Menu)
                {
                    if (Id == Permiso.Id)
                        return true;
                }
            }
            return false;
        }
    }
}
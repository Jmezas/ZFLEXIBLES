using sisCCS.BusinessLayer;
using sisCCS.EntityLayer;
using sisCCS.UserLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace sisCCS.UserLayer.Controllers
{
    public class SeguridadController : Controller
    {
        Authentication GrifosoftAuthentication = new Authentication();

        BUsuario BUsuario = BUsuario.ObtenerInstancia();
        BGeneral BGeneral = BGeneral.ObtenerInstancia();
        BPerfil BPerfil = BPerfil.ObtenerInstancia();
       // BSucursal BSucursal = BSucursal.ObtenerInstancia();


        public ActionResult Login()
        {
            if (GrifosoftAuthentication.SessionCookie != null)
            {
                return RedirectToAction("Principal", "Seguridad");
            }
            return View();
        }

      

        public ActionResult Empresa()
        {
            return View();
        }

        [SessionFilter]
        public ActionResult Principal()
        {
            return View();
        }

        [SessionFilter]
        public ActionResult Perfiles()
        {
            return View();
        }

        [SessionFilter]
        public ActionResult ListadoPerfiles()
        {
            return View();
        }

        [SessionFilter]
        public ActionResult AccesosPorPerfil(int id)
        {
            ViewBag.Perfil = BPerfil.BuscarPerfilPorId(id);
            ViewBag.MenuPerfil = BPerfil.ListarAccesosporPerfil(id);
            return View();
        }

        [SessionFilter]
        public ActionResult VerAccesosPorPerfil(int id)
        {
            ViewBag.Perfil = BPerfil.BuscarPerfilPorId(id);
            ViewBag.MenuPerfil = BPerfil.ListarAccesosporPerfil(id);
            return View();
        }


        //[SessionFilter]
        //public ActionResult Usuarios()
        //{
        //    SelectList lTiposDocumento = new SelectList(BGeneral.ListarTiposDocumentoIdentidad(), "Id", "Sigla");
        //    SelectList lDepartamentos = new SelectList(BGeneral.ListarUbigeo("D", "", ""), "CodigoDepartamento", "Nombre");
        //    SelectList lPerfiles = new SelectList(BPerfil.Listar(), "Id", "Nombre");

        //    ViewData["TiposDocumento"] = lTiposDocumento;
        //    ViewData["Departamentos"] = lDepartamentos;
        //    ViewData["Perfiles"] = lPerfiles;

        //    return View();
        //}

        [HttpPost]
        public ActionResult Login(EUsuario Model)
        {
            try
            {
                if (!string.IsNullOrEmpty(Model.Usuario) && !string.IsNullOrEmpty(Model.Password))
                {
                    GrifosoftAuthentication.UserLogued = BUsuario.Login(Model.Usuario, Model.Password);
                    if (GrifosoftAuthentication.UserLogued != null)
                    {
                        GrifosoftAuthentication.UserLogued.Menu = BUsuario.ListarMenuPorUsuario(Model.Usuario);
                        if (GrifosoftAuthentication.UserLogued.Menu.Count > 0)
                        {
                            // Guardado de actividad en sesión
                            GrifosoftAuthentication.SessionCookie = new HttpCookie("SessionCookie"); // Creación de cookie
                            GrifosoftAuthentication.SessionCookie.Expires = DateTime.Now.AddDays(1);
                            GrifosoftAuthentication.SessionCookie.Values.Add("Usuario", Model.Usuario); // Guardado del usuario en sesión
                            GrifosoftAuthentication.SessionCookie.Values.Add("iIdComprobantePago", "0"); // Guardado del usuario en sesión
                            GrifosoftAuthentication.SessionCookie.Values.Add("idTipoComprobantePago", "0"); // Guardado del usuario en sesión
                            GrifosoftAuthentication.SessionCookie.Values.Add("idTerminal", "0"); // Guardado del usuario en sesión
                            GrifosoftAuthentication.SessionCookie.Values.Add("idNotaDespacho", "0"); // Guardado del usuario en sesión
                            
                            Debug.WriteLine(GrifosoftAuthentication.SessionCookie.Values);
                            Response.Cookies.Add(GrifosoftAuthentication.SessionCookie);

                            Session.Timeout = (int)(GrifosoftAuthentication.SessionCookie.Expires - DateTime.Now).TotalSeconds;
                            Session["Usuario"] = GrifosoftAuthentication.UserLogued;

                            if (GrifosoftAuthentication.UserLogued.Perfil.Id == 11)
                            {
                                return RedirectToAction("Terminal", "Seguridad");
                            }
                            else
                            {
                                return RedirectToAction("Principal", "Seguridad");
                            }


                        }
                        else
                        {
                            ViewBag.Message = "El rol que usted posee asignado no tiene permisos asignados.";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "Credenciales Incorrectas";
                    }
                }
                else
                {
                    ViewBag.Message = "Debe ingresar un usuario y contraseña.";
                }
            }
            catch (Exception Exception)
            {
                ViewBag.Message = Exception.Message;
            }
            return View();
        }

      

        public ActionResult Logout()
        {
            // Eliminación de cookie
            GrifosoftAuthentication.SessionCookie = new HttpCookie("SessionCookie");
            GrifosoftAuthentication.SessionCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(GrifosoftAuthentication.SessionCookie);

            // Cierre de sesión
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Login", "Seguridad");
        }

        public ActionResult Error404()
        {
           return View();
        }

        public ActionResult DefaultError()
        {
           return View();
        }


        [HttpPost]
        public void RegistrarUsuario(EUsuario Model)
        {
            try
            {
                Model.UsuarioCreador = new EUsuario { Id = GrifosoftAuthentication.UserLogued.Id };
                Utils.WriteMessage(BUsuario.Registrar(Model));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }

        [HttpPost]
        public void RegistrarPerfil(EPerfil Model)
        {
            try
            {
                Model.EmpresaHolding = new EEmpresaHolding { Id = GrifosoftAuthentication.UserLogued.Perfil.EmpresaHolding.Empresa.Id };
                Model.UsuarioCreador = new EUsuario { Id = GrifosoftAuthentication.UserLogued.Id };
                Utils.WriteMessage(BPerfil.Registrar(Model));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }


        [HttpPost]
        public void ModificarUsuario(EUsuario Model)
        {
            try
            {
                Utils.WriteMessage(BUsuario.Actualizar(Model));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }

        [HttpPost]
        public void ModificarPerfil(EPerfil Model)
        {
            try
            {
                Utils.WriteMessage(BPerfil.Actualizar(Model));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }

        [HttpPost]
        public void EliminarPerfil()
        {
            try
            {
                int iIdPerfil = int.Parse(Request.Params["iIdPerfil"].ToString());
                Utils.WriteMessage(BPerfil.Eliminar(iIdPerfil));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }

        [HttpPost]
        public void EliminarAsignacion()
        {
            try
            {
                int iIdAsignacion = int.Parse(Request.Params["iIdAsignacion"].ToString());
                Utils.WriteMessage(BUsuario.EliminarAsignacion(iIdAsignacion));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }

        [HttpPost]
        public void ListarUsuariosPaginacion()
        {
            try
            {
                int iComienzo = int.Parse(Request.Params["iComienzo"].ToString());
                int iMedida = int.Parse(Request.Params["iMedida"].ToString());
                string sNroDocumento = Request.Params["sNroDocumento"].ToString();
                string sNombre = Request.Params["sNombre"].ToString();
                Utils.Write(ResponseType.JSON, BUsuario.ListarPaginacion(iComienzo, iMedida, sNroDocumento, sNombre));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }

        [HttpPost]
        public void ListarPerfilesPaginacion()
        {
            try
            {
                int iComienzo = int.Parse(Request.Params["iComienzo"].ToString());
                int iMedida = int.Parse(Request.Params["iMedida"].ToString());
                string sFiltro = Request.Params["sFiltro"].ToString();
                Utils.Write(ResponseType.JSON, BPerfil.Listar(iComienzo, iMedida, sFiltro));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }
     

        [HttpPost]
        public void ListarAccesosPorPerfil()
        {
            try
            {
                int id = int.Parse(Request.Params["id"].ToString());
                Utils.Write(ResponseType.JSON, BPerfil.ListarAccesosporPerfil(id));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }

        [HttpPost]
        public void ActualizarAccesosPorPerfil(int Id, string Menus)
        {
            try
            {
                Utils.WriteMessage(BPerfil.ActualizarAccesosPorPerfil(Id, Menus));
            }
            catch (Exception Exception)
            {
                Utils.WriteMessage("error|" + Exception.Message);
            }
        }


    }
}
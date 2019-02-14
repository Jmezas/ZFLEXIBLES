using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using sisCCS.BusinessLayer;
using sisCCS.EntityLayer;
using sisCCS.UserLayer.Models;

namespace sisCCS.UserLayer.Controllers
{
    public class MantenimientoController : Controller
    {
        Authentication Authentication = new Authentication();
        BProducto Producto = new BProducto();
        BGeneral General = new BGeneral();
        BCliente Cliente = new BCliente();
        // GET: Mantenimiento
        public ActionResult ListaProdcuto()
        {
            return View();
        }
        public ActionResult ListaCliente()
        {
            return View();
        }


        public ActionResult ListarProductosPaginacion()
        {
            var List = Producto.ListaProducto();
            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void ModificarProducto(int IdMaterial)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Producto.ObtenerProducto(IdMaterial)
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        [HttpPost]
        public void ListaUnidad()
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    General.ListarUnidad()
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }
        [HttpPost]
        public void RegistrarProducto(EProducto eProducto, string Usuario)
        {
            try
            {
                Usuario = Authentication.UserLogued.Usuario;
                Utils.WriteMessage(Producto.Registrar_Update(eProducto, Usuario)
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 2, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }
        public ActionResult ListadoCliente()
        {
            var List = Cliente.ListaCliente();
            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void BuscarClienteId(int IdCliente)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Cliente.ListaEdit(IdCliente)
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }
        [HttpPost]
        public void RegistrarCliente(ECliente eCliente, string Usuario)
        {
            try
            {
                Usuario = Authentication.UserLogued.Usuario;
                Utils.WriteMessage(Cliente.Insertar_Update(eCliente, Usuario)
                    );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 2, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }


      

    }
}
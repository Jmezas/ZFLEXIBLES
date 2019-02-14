using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using sisCCS.BusinessLayer;
using sisCCS.UserLayer.Models;

using sisCCS.EntityLayer;
namespace sisCCS.UserLayer.Controllers
{
    public class VentaController : Controller
    {

        Authentication Authentication = new Authentication();
        BGeneral general = new BGeneral();
        BCotizacion Cotizacion = new BCotizacion();
        BFactura Factura = new BFactura();
        BGuia GiaR = new BGuia();
        // GET: Venta
        public ActionResult NuevoCotizacion()
        {
            return View();
        }
        public ActionResult VentaFactura()
        {
            return View();
        }
        public ActionResult GuiaRemision()
        {
            return View();
        }

        [HttpPost]
        public void SerieNumeroDoc()
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListaDocCot()
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
        public void BuscarClientePorRUC(string pRuc, string Filtro2)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListaClienteRuc(pRuc, Filtro2)
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
        public void BuscarClientePorDNI(string pRuc, string Filtro2)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListaClienteDNI(pRuc, Filtro2)
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
        public void GenerarCotizacion(ECotizacionCab cotizacionCab, List<ECotizacionDet> Detalle, string usuario)
        {
            try
            {
                usuario = Authentication.UserLogued.Usuario;
                var sMensaje = Cotizacion.RegistrarCotizacion(cotizacionCab, Detalle, usuario);
                Utils.WriteMessage(sMensaje, Utils.WithAdditionals);
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
        public void ListaDocumento()
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListaDocFactura()
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
        public void ListaSerieDoc(int tipo, int comprobante)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Factura.ListaNroFac(tipo, comprobante)
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
        public void ListaDoc(int documento)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Factura.ListaDocFactura(documento)
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
        public void VerigicarStock(int idProducto,string iCantidad)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Factura.VerificarStock(idProducto, iCantidad)
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
        public void InsFacturaBol(EFacturaCab oFactura, List<EFacturaDet> FacturaDetalle, string usuario)
        {
            try
            {
                usuario = Authentication.UserLogued.Usuario;
                var sMensaje = Factura.RegistrarFactura(oFactura, FacturaDetalle, usuario);
                Utils.WriteMessage(sMensaje, Utils.WithAdditionals);
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
        public void ObtenerCotizacion(int Codigo)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Cotizacion.ObtenerCotizacion(Codigo)
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
        public void InsGuiaRemision(EGuiaCab oGuiaRemision, List<EGuiadet> oDetalle, string Usuario)
        {
            try
            {
                Usuario = Authentication.UserLogued.Usuario;
                var sMensaje = GiaR.RegistrarGia(oGuiaRemision, oDetalle, Usuario);
                Utils.WriteMessage(sMensaje, Utils.WithAdditionals);
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }
    }
}
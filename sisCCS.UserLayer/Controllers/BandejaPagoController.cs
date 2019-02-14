using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using sisCCS.BusinessLayer;
using sisCCS.UserLayer.Models;
using sisCCS.EntityLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.IO;
namespace sisCCS.UserLayer.Controllers
{
    public class BandejaPagoController : Controller
    {
        Authentication Authentication = new Authentication();


        BGeneral general = new BGeneral();
        BProveedor Proveedor = new BProveedor();
        BOrdenCompra BOrdenCompra = new BOrdenCompra();
        BCuentaPago Cuenta = new BCuentaPago();

        // GET: BandejaPago
        public ActionResult BandejaDocumentos()
        {
            return View();
        }
        public ActionResult PagarDocumento(int IdComprobante)
        {
            ViewBag.PagarDocumento = IdComprobante;
            return View();
        }

        [HttpPost]
        public void ListaComprobante(int iComienzo, int iMedia, string Numero, string Cliente, int Tipodocumento, string FechaInicio, string FechaFin)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Cuenta.ListaPago(iComienzo, iMedia, Numero, Cliente, Tipodocumento, FechaInicio, FechaFin)
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
        public void ListarPagos(int iComienzo, int iMedia, int tipoDoc)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Cuenta.ListaCuenta(iComienzo, iMedia, tipoDoc)
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
        public void ListComprobanteId(int idComprobante)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Cuenta.ObtenerCuenta(idComprobante)
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
        public void InsrtarPago(EPago Comprobante, string Usuario)
        {
            try
            {
                Usuario = Authentication.UserLogued.Usuario;
                var sMensaje = Cuenta.PagarDocumento(Comprobante, Usuario);
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

        public ActionResult ReportePago(int Codigo)
        {
            Document pdfDoc = new Document(PageSize.A7, 10, 10, 20, 20);
            EPago pago = Cuenta.ReportePago(Codigo);
          
            try
            {
                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/OrdenCompra/" + "Documento_" + Codigo + "Boucher"  + ".pdf";

                // Se utiliza system.IO para crear o sobreescribe el archivo si existe
                FileStream file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

                //'iTextSharp para escribir en el documento PDF, sin esto no podemos ver el contenido
                PdfWriter.GetInstance(pdfDoc, file);

                //  'Open PDF Document to write data 
                pdfDoc.Open();
                //'Armando el diseño de la factura

                iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 8, iTextSharp.text.Font.NORMAL, Color.BLACK);
                iTextSharp.text.Font _standarTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 16, iTextSharp.text.Font.BOLD, Color.BLACK);
                iTextSharp.text.Font _standarTexto = new iTextSharp.text.Font(iTextSharp.text.Font.HELVETICA, 10, iTextSharp.text.Font.BOLD, Color.BLACK);

                PdfPTable tblPrueba = new PdfPTable(2);
                tblPrueba.WidthPercentage = 100;

                PdfPTable TableCabecera = new PdfPTable(1);
                TableCabecera.WidthPercentage = 100;

                float[] celdasCabecera = new float[] { 2.0F };
                TableCabecera.SetWidths(celdasCabecera);

                PdfPCell celCospan = new PdfPCell();
                celCospan.Colspan = 2;
                celCospan.Border = 0;
                TableCabecera.AddCell(celCospan);
                              
                PdfPTable tableIzq = new PdfPTable(1);
                



                PdfPCell celContacto = new PdfPCell(new Phrase("***** Pago ******:", _standardFont));
                celContacto.HorizontalAlignment = 1;
                celContacto.Border = 0;
                tableIzq.AddCell(celContacto);

                PdfPCell celIzq = new PdfPCell(tableIzq);
                celIzq.Border = 0;
                TableCabecera.AddCell(celIzq);



                PdfPCell cCeldaDetallec = new PdfPCell(TableCabecera);
                cCeldaDetallec.Colspan = 2;
                cCeldaDetallec.Border = 0;
                cCeldaDetallec.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaDetallec);

                PdfPCell cEspacio = new PdfPCell(new Phrase("      "));
                cEspacio.Colspan = 2;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio);

                float[] celdas1 = new float[] { 2.0F, 5.0F };
                tblPrueba.SetWidths(celdas1);

                PdfPCell cTitulo1 = new PdfPCell(new Phrase("Tipo Doc:", _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 0;
                cTitulo1.PaddingTop = 2;
                tblPrueba.AddCell(cTitulo1);


                PdfPCell cRazonsocial = new PdfPCell(new Phrase(pago.Comprobante.Nombre, _standardFont));
                cRazonsocial.Border = 0; // borde cero
                cRazonsocial.HorizontalAlignment = 0;
                cRazonsocial.PaddingTop = 2;
                tblPrueba.AddCell(cRazonsocial);

                PdfPCell cTitulo2 = new PdfPCell(new Phrase("Fecha:", _standardFont));
                cTitulo2.Border = 0;
                cTitulo2.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo2);


                PdfPCell cRuc = new PdfPCell(new Phrase(pago.FechaPago, _standardFont));
                cRuc.Border = 0;
                cRuc.HorizontalAlignment = 0;
                tblPrueba.AddCell(cRuc);



                PdfPCell cTitulo3 = new PdfPCell(new Phrase("Hora:", _standardFont));
                cTitulo3.Border = 0;
                cTitulo3.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo3);


                PdfPCell chora = new PdfPCell(new Phrase(pago.Hora, _standardFont));
                chora.Border = 0;
                chora.HorizontalAlignment = 0;
                tblPrueba.AddCell(chora);


                PdfPCell cTitulo4 = new PdfPCell(new Phrase("Nro de Documento:", _standardFont));
                cTitulo4.Border = 0;
                cTitulo4.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo4);

                PdfPCell cDireccion = new PdfPCell(new Phrase(pago.Factura.Serie, _standardFont));
                cDireccion.Border = 0;
                cDireccion.HorizontalAlignment = 0;
                tblPrueba.AddCell(cDireccion);


                PdfPCell cTitulo5 = new PdfPCell(new Phrase("Monto:", _standardFont));
                cTitulo5.Border = 0;
                cTitulo5.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo5);

                PdfPCell cMonto = new PdfPCell(new Phrase(pago.Monto, _standardFont));
                cMonto.Border = 0;
                cMonto.HorizontalAlignment = 0;
                tblPrueba.AddCell(cMonto);


                PdfPCell cTitulo6 = new PdfPCell(new Phrase("Cliente:", _standardFont));
                cTitulo6.Border = 0;
                cTitulo6.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo6);

                PdfPCell cCliente = new PdfPCell(new Phrase(pago.Cliente.Nombre, _standardFont));
                cCliente.Border = 0;
                cCliente.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCliente);



                PdfPCell cCeldaTotal = new PdfPCell(new Phrase("     "));
                cCeldaTotal.Colspan = 2;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);

                
             

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                Response.ContentType = "application/pdf";
                // Set default file Name as current datetime 
                Response.AddHeader("content-disposition", "attachment; filename=Documento_" + Codigo + "_Boucher"  + ".pdf");
                Response.Write(pdfDoc);

                Response.Flush();
                Response.End();

                return View();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public ActionResult ReportePagoCab(int iComienzo, int iMedia, string Numero, string Cliente, int Tipodocumento, string FechaInicio, string FechaFin)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            EGeneralJson<ECuentaPago> ListaGuia = Cuenta.ListaPago(iComienzo, iMedia, Numero, Cliente, Tipodocumento, FechaInicio, FechaFin);

            if (ListaGuia.Total > 0)
            {
                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

                //Definiendo parametros para la fuente de la cabecera y pie de pagina
                Font fuente = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, Color.BLACK);

                //Se define la cabecera del documento
                HeaderFooter cabecera = new HeaderFooter(new Phrase("Fecha: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), fuente), false);//'el valor es false porque no habra numeración
                pdfDoc.Header = cabecera;
                cabecera.Border = 0;// Rectangle.BOTTOM_BORDER
                cabecera.Alignment = HeaderFooter.ALIGN_RIGHT;


                HeaderFooter pie = new HeaderFooter(new Phrase("pagia", fuente), true);

                pdfDoc.Footer = pie;
                pie.Border = Rectangle.TOP_BORDER;
                pie.Alignment = HeaderFooter.ALIGN_RIGHT;

                //Open PDF Document to write data 
                pdfDoc.Open();

                PdfPTable tblPrueba = new PdfPTable(1);
                tblPrueba.WidthPercentage = 100;


                //Se define la fuente para el texto del reporte
                Font _standardFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD);

                PdfPTable Tableitems = new PdfPTable(10);
                Tableitems.WidthPercentage = 100;
                Tableitems.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                PdfPCell celCospan = new PdfPCell(new Phrase("REPORTE BANDEJA DE PAGO"));
                celCospan.Colspan = 10;
                celCospan.Border = 0;
                celCospan.HorizontalAlignment = 1;
                Tableitems.AddCell(celCospan);


                PdfPCell cEspacio = new PdfPCell(new Phrase("              "));
                cEspacio.Colspan = 10;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 1;
                Tableitems.AddCell(cEspacio);

                Single[] celdas2 = new Single[] { 0.5F, 1.0F, 1.0F, 2.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloItem = new PdfPCell(new Phrase("Item", _standardFont));
                cTituloItem.Border = 1;
                cTituloItem.BorderWidthBottom = 1;
                cTituloItem.HorizontalAlignment = 1;
                cTituloItem.Padding = 5;
                Tableitems.AddCell(cTituloItem);



                PdfPCell cTituloCant = new PdfPCell(new Phrase("Tipo Doc.", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1;
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);

                PdfPCell CTituloSerie = new PdfPCell(new Phrase("Nro. Doc. Cliente", _standardFont));
                CTituloSerie.Border = 1;
                CTituloSerie.BorderWidthBottom = 1;
                CTituloSerie.HorizontalAlignment = 1;
                CTituloSerie.Padding = 5;
                Tableitems.AddCell(CTituloSerie);

                PdfPCell cTitutloNroDocumento = new PdfPCell(new Phrase("Cliente", _standardFont));
                cTitutloNroDocumento.Border = 1;
                cTitutloNroDocumento.BorderWidthBottom = 1;
                cTitutloNroDocumento.HorizontalAlignment = 1;
                cTitutloNroDocumento.Padding = 5;
                Tableitems.AddCell(cTitutloNroDocumento);

                PdfPCell cTitutloFecha = new PdfPCell(new Phrase("Moneda", _standardFont));
                cTitutloFecha.Border = 1;
                cTitutloFecha.BorderWidthBottom = 1;
                cTitutloFecha.HorizontalAlignment = 1;
                cTitutloFecha.Padding = 5;
                Tableitems.AddCell(cTitutloFecha);

                PdfPCell cTituloCliente = new PdfPCell(new Phrase("Serie-Numero", _standardFont));
                cTituloCliente.Border = 1;
                cTituloCliente.BorderWidthBottom = 1;
                cTituloCliente.HorizontalAlignment = 1;
                cTituloCliente.Padding = 5;
                Tableitems.AddCell(cTituloCliente);

                PdfPCell cCantidad = new PdfPCell(new Phrase("Fecha Emision", _standardFont));
                cCantidad.Border = 1;
                cCantidad.BorderWidthBottom = 1;
                cCantidad.HorizontalAlignment = 1;
                cCantidad.Padding = 5;
                Tableitems.AddCell(cCantidad);

                PdfPCell cMonto = new PdfPCell(new Phrase("Monto", _standardFont));
                cMonto.Border = 1;
                cMonto.BorderWidthBottom = 1;
                cMonto.HorizontalAlignment = 1;
                cMonto.Padding = 5;
                Tableitems.AddCell(cMonto);

                PdfPCell cRestante= new PdfPCell(new Phrase("Monto Restante", _standardFont));
                cRestante.Border = 1;
                cRestante.BorderWidthBottom = 1;
                cRestante.HorizontalAlignment = 1;
                cRestante.Padding = 5;
                Tableitems.AddCell(cRestante);

                PdfPCell cEstado= new PdfPCell(new Phrase("Estado", _standardFont));
                cEstado.Border = 1;
                cEstado.BorderWidthBottom = 1;
                cEstado.HorizontalAlignment = 1;
                cEstado.Padding = 5;
                Tableitems.AddCell(cEstado);



                Font letrasDatosTabla = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);

                foreach (ECuentaPago cuentaPago in ListaGuia.Datos)
                {
                    PdfPCell cItem = new PdfPCell(new Phrase(cuentaPago.Item.ToString(), letrasDatosTabla));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    Tableitems.AddCell(cItem);

                    PdfPCell cdocumento = new PdfPCell(new Phrase(cuentaPago.Comprobante.Nombre, letrasDatosTabla));
                    cdocumento.Border = 0;
                    cdocumento.Padding = 2;
                    cdocumento.PaddingBottom = 2;
                    cdocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cdocumento);

                    PdfPCell cSerie = new PdfPCell(new Phrase(cuentaPago.Cliente.NroDocumento, letrasDatosTabla));
                    cSerie.Border = 0;
                    cSerie.Padding = 2;
                    cSerie.PaddingBottom = 2;
                    cSerie.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSerie);

                    PdfPCell cFecha = new PdfPCell(new Phrase(cuentaPago.Cliente.Nombre, letrasDatosTabla));
                    cFecha.Border = 0;
                    cFecha.Padding = 2;
                    cFecha.PaddingBottom = 2;
                    cFecha.HorizontalAlignment = 1;
                    Tableitems.AddCell(cFecha);

                    PdfPCell cNroDocumento = new PdfPCell(new Phrase(cuentaPago.Moneda.Nombre, letrasDatosTabla));
                    cNroDocumento.Border = 0;
                    cNroDocumento.Padding = 2;
                    cNroDocumento.PaddingBottom = 2;
                    cNroDocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNroDocumento);

                    PdfPCell cProveedor = new PdfPCell(new Phrase(cuentaPago.Factura.Serie, letrasDatosTabla));
                    cProveedor.Border = 0;
                    cProveedor.Padding = 2;
                    cProveedor.PaddingBottom = 2;
                    cProveedor.HorizontalAlignment = 1;
                    Tableitems.AddCell(cProveedor);

                    PdfPCell cMoneda = new PdfPCell(new Phrase(cuentaPago.Pago.FechaPago.ToString(), letrasDatosTabla));
                    cMoneda.Border = 0;
                    cMoneda.Padding = 2;
                    cMoneda.PaddingBottom = 2;
                    cMoneda.HorizontalAlignment = 1;
                    Tableitems.AddCell(cMoneda);

                    PdfPCell cTotal = new PdfPCell(new Phrase(cuentaPago.Factura.Total.ToString(), letrasDatosTabla));
                    cTotal.Border = 0;
                    cTotal.Padding = 2;
                    cTotal.PaddingBottom = 2;
                    cTotal.HorizontalAlignment = 1;
                    Tableitems.AddCell(cTotal);


                    PdfPCell cPendiente= new PdfPCell(new Phrase(cuentaPago.Pendiente, letrasDatosTabla));
                    cPendiente.Border = 0;
                    cPendiente.Padding = 2;
                    cPendiente.PaddingBottom = 2;
                    cPendiente.HorizontalAlignment = 1;
                    Tableitems.AddCell(cPendiente);

                    PdfPCell cEstadoFactura = new PdfPCell(new Phrase(cuentaPago.EstadoFactura, letrasDatosTabla));
                    cEstadoFactura.Border = 0;
                    cEstadoFactura.Padding = 2;
                    cEstadoFactura.PaddingBottom = 2;
                    cEstadoFactura.HorizontalAlignment = 1;
                    Tableitems.AddCell(cEstadoFactura);

                }

                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Border = 1;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.HorizontalAlignment = 1;
                cCeldaDetalle.Padding = 5;
                tblPrueba.AddCell(cCeldaDetalle);



                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();



                Response.ContentType = "application/pdf";

                //Set default file Name as current datetime 
                Response.AddHeader("content-disposition", "attachment; filename=ReportePago.pdf");
                Response.Write(pdfDoc);
                Response.Flush();
                Response.End();
            }
            else
            {
                throw new Exception("No se encontró ningún registro.");

            }
            return View();

        }
    }
}
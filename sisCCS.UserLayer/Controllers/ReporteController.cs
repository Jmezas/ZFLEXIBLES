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
    public class ReporteController : Controller
    {
        Authentication Authentication = new Authentication();
        BGeneral general = new BGeneral();
        BProveedor Proveedor = new BProveedor();
        BCotizacion Cotizacion = new BCotizacion();
        BFactura Factura = new BFactura();
        BGuia Guia = new BGuia();
        EnvioCorreo Correo = new EnvioCorreo();
        
        // GET: Reporte
        public ActionResult ListaCotizacion()
        {
            return View();
        }

        public ActionResult ListaFactura()
        {
            return View();
        }

        public ActionResult ListaGuiaRemision()
        {
            return View();
        }

        [HttpPost]
        public void ListaCotizacion(int iComienzo, int iMedia, string Numero, string Numdoc, string Cliente, string TipoDocumento, string FechaInicio, string FechaFin)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Cotizacion.ListaReporte(iComienzo, iMedia, Numero, Numdoc, Cliente, TipoDocumento, FechaInicio, FechaFin)
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
        public void ListaReporteFac(int iComienzo, int iMedia, string Numero, string Cliente, int TipoDocumento, string FechaInicio, string FechaFin)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Factura.ListaFacBol(iComienzo, iMedia, Numero, Cliente, TipoDocumento, FechaInicio, FechaFin)
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
        public void ListaReportGuia(int iComienzo, int iMedia, string Numero, string Numdoc, string Cliente, int Doc, string FechaInicio, string FechaFin)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Guia.ListaGia(iComienzo, iMedia, Numero, Numdoc, Cliente, Doc, FechaInicio, FechaFin)
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
        //Cotizacion
        public ActionResult ImprimirCot(int iComienzo, int iMedia, string Numero, string Numdoc, string Cliente, string TipoDocumento, string FechaInicio, string FechaFin)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            EGeneralJson<ECotizacionCab> Catizacion = Cotizacion.ListaReporte(iComienzo, iMedia, Numero, Numdoc, Cliente, TipoDocumento, FechaInicio, FechaFin);

            if (Catizacion.Total > 0)
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

                PdfPTable Tableitems = new PdfPTable(9);
                Tableitems.WidthPercentage = 100;
                Tableitems.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                PdfPCell celCospan = new PdfPCell(new Phrase("REPORTE DE COTIZACION"));
                celCospan.Colspan = 9;
                celCospan.Border = 0;
                celCospan.HorizontalAlignment = 1;
                Tableitems.AddCell(celCospan);


                PdfPCell cEspacio = new PdfPCell(new Phrase("              "));
                cEspacio.Colspan = 9;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 1;
                Tableitems.AddCell(cEspacio);

                Single[] celdas2 = new Single[] { 0.5F, 1.0F, 1.0F, 2.5F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloItem = new PdfPCell(new Phrase("Item", _standardFont));
                cTituloItem.Border = 1;
                cTituloItem.BorderWidthBottom = 1;
                cTituloItem.HorizontalAlignment = 1;
                cTituloItem.Padding = 5;
                Tableitems.AddCell(cTituloItem);


                PdfPCell cTituloCant = new PdfPCell(new Phrase("Serie-Num.", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1;
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);

                PdfPCell CTituloSerie = new PdfPCell(new Phrase("Fecha Registro", _standardFont));
                CTituloSerie.Border = 1;
                CTituloSerie.BorderWidthBottom = 1;
                CTituloSerie.HorizontalAlignment = 1;
                CTituloSerie.Padding = 5;
                Tableitems.AddCell(CTituloSerie);

                PdfPCell cTitutloFecha = new PdfPCell(new Phrase("Proveedor", _standardFont));
                cTitutloFecha.Border = 1;
                cTitutloFecha.BorderWidthBottom = 1;
                cTitutloFecha.HorizontalAlignment = 1;
                cTitutloFecha.Padding = 5;
                Tableitems.AddCell(cTitutloFecha);


                PdfPCell cTituloCliente = new PdfPCell(new Phrase("Moneda", _standardFont));
                cTituloCliente.Border = 1;
                cTituloCliente.BorderWidthBottom = 1;
                cTituloCliente.HorizontalAlignment = 1;
                cTituloCliente.Padding = 5;
                Tableitems.AddCell(cTituloCliente);

                PdfPCell cCantidad = new PdfPCell(new Phrase("Cantidad", _standardFont));
                cCantidad.Border = 1;
                cCantidad.BorderWidthBottom = 1;
                cCantidad.HorizontalAlignment = 1;
                cCantidad.Padding = 5;
                Tableitems.AddCell(cCantidad);

                PdfPCell cTituloIGV = new PdfPCell(new Phrase("IGV", _standardFont));
                cTituloIGV.Border = 1;
                cTituloIGV.BorderWidthBottom = 1;
                cTituloIGV.HorizontalAlignment = 1;
                cTituloIGV.Padding = 5;
                Tableitems.AddCell(cTituloIGV);


                PdfPCell cTituloSubTotal = new PdfPCell(new Phrase("SubTotal.", _standardFont));
                cTituloSubTotal.Border = 1;
                cTituloSubTotal.BorderWidthBottom = 1;
                cTituloSubTotal.HorizontalAlignment = 1;
                cTituloSubTotal.Padding = 5;
                Tableitems.AddCell(cTituloSubTotal);


                PdfPCell cTituloTotal = new PdfPCell(new Phrase("Total", _standardFont));
                cTituloTotal.Border = 1;
                cTituloTotal.BorderWidthBottom = 1;
                cTituloTotal.HorizontalAlignment = 1;
                cTituloTotal.Padding = 5;
                Tableitems.AddCell(cTituloTotal);

                Font letrasDatosTabla = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);
                foreach (ECotizacionCab Cotizacion in Catizacion.Datos)
                {
                    PdfPCell cItem = new PdfPCell(new Phrase(Cotizacion.Item.ToString(), letrasDatosTabla));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    Tableitems.AddCell(cItem);

                    PdfPCell cSerie = new PdfPCell(new Phrase(Cotizacion.Serie.ToString(), letrasDatosTabla));
                    cSerie.Border = 0;
                    cSerie.Padding = 2;
                    cSerie.PaddingBottom = 2;
                    cSerie.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSerie);

                    PdfPCell cFecha = new PdfPCell(new Phrase(Cotizacion.FechaEmision.ToString(), letrasDatosTabla));
                    cFecha.Border = 0;
                    cFecha.Padding = 2;
                    cFecha.PaddingBottom = 2;
                    cFecha.HorizontalAlignment = 1;
                    Tableitems.AddCell(cFecha);

                    PdfPCell cProveedor = new PdfPCell(new Phrase(Cotizacion.Cliente.Nombre.ToString(), letrasDatosTabla));
                    cProveedor.Border = 0;
                    cProveedor.Padding = 2;
                    cProveedor.PaddingBottom = 2;
                    cProveedor.HorizontalAlignment = 1;
                    Tableitems.AddCell(cProveedor);

                    PdfPCell cMoneda = new PdfPCell(new Phrase(Cotizacion.Moneda.Nombre.ToString(), letrasDatosTabla));
                    cMoneda.Border = 0;
                    cMoneda.Padding = 2;
                    cMoneda.PaddingBottom = 2;
                    cMoneda.HorizontalAlignment = 1;
                    Tableitems.AddCell(cMoneda);

                    PdfPCell cCantidaddet = new PdfPCell(new Phrase(Cotizacion.Cantidad.ToString(), letrasDatosTabla));
                    cCantidaddet.Border = 0;
                    cCantidaddet.Padding = 2;
                    cCantidaddet.PaddingBottom = 2;
                    cCantidaddet.HorizontalAlignment = 1;
                    Tableitems.AddCell(cCantidaddet);

                    PdfPCell cSubTotal = new PdfPCell(new Phrase(Cotizacion.SubTotal.ToString(), letrasDatosTabla));
                    cSubTotal.Border = 0;
                    cSubTotal.Padding = 2;
                    cSubTotal.PaddingBottom = 2;
                    cSubTotal.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSubTotal);

                    PdfPCell cIGV = new PdfPCell(new Phrase(Cotizacion.IGV.ToString(), letrasDatosTabla));
                    cIGV.Border = 0;
                    cIGV.Padding = 2;
                    cIGV.PaddingBottom = 2;
                    cIGV.HorizontalAlignment = 1;
                    Tableitems.AddCell(cIGV);

                    PdfPCell cTotal = new PdfPCell(new Phrase(Cotizacion.Total.ToString(), letrasDatosTabla));
                    cTotal.Border = 0;
                    cTotal.Padding = 2;
                    cTotal.PaddingBottom = 2;
                    cTotal.HorizontalAlignment = 1;
                    Tableitems.AddCell(cTotal);
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
                Response.AddHeader("content-disposition", "attachment; filename=ReporteCotizacion.pdf");
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

        public ActionResult ImprimirCotizacion(int IdCotizacion, int Envio)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            List<EEmpresa> Empresa = general.ListaEmpresa();
            string ruc = "";
            string Razon = "";
            string ubgeo = "";
            string direccion = "";
            string fecha = "";
            string correo = "";
            string contrasenia = "";
            string telefono = "";
            string celular = "";

            foreach (EEmpresa Listempresa in Empresa)
            {
                ruc = Listempresa.RUC;
                Razon = Listempresa.RazonSocial;
                ubgeo = Listempresa.Ubigeo;
                direccion = Listempresa.Direccion;
                fecha = Listempresa.Fecha;
                correo = Listempresa.Correo;
                contrasenia = Listempresa.Contrasenia;
                telefono = Listempresa.Telefono;
                celular = Listempresa.Celular;


            }
            EGeneralJson<EReporteCotizacion> Ordem = Cotizacion.ListaReporte(IdCotizacion);
            try
            {


                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                string serie = "";
                string FechaEmi = "";
                string NombreCliente = "";
                string Documento = "";
                string Direccion = "";
                string SubTotal = "";
                string IGV = "";
                string Total = "";
                string Moneda = "";
                string Motivo = "";
                string Mensaje = "";
                string Email = "";
                foreach (EReporteCotizacion ReporteCab in Ordem.Datos)
                {
                    serie = ReporteCab.CotizacionCab.Serie;
                    FechaEmi = ReporteCab.CotizacionCab.FechaEmision;
                    NombreCliente = ReporteCab.Cliente.Nombre;
                    Documento = ReporteCab.Cliente.NroDocumento;
                    Direccion = ReporteCab.Cliente.Direccion;
                    SubTotal = ReporteCab.CotizacionCab.SubTotal;
                    IGV = ReporteCab.CotizacionCab.IGV;
                    Total = ReporteCab.CotizacionCab.Total;
                    Moneda = ReporteCab.Moneda.Nombre;
                    Motivo = ReporteCab.CotizacionCab.Asunto;
                    Mensaje = ReporteCab.CotizacionCab.Mensaje;
                    Email = ReporteCab.Cliente.Email;




                }
                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/Cotizacion/" + "Documento_" + IdCotizacion + "_OC_" + serie + ".pdf";

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

                PdfPTable TableCabecera = new PdfPTable(2);
                TableCabecera.WidthPercentage = 100;

                float[] celdasCabecera = new float[] { 2.0F, 2.0F };
                TableCabecera.SetWidths(celdasCabecera);

                PdfPCell celCospan = new PdfPCell();
                celCospan.Colspan = 2;
                celCospan.Border = 0;
                TableCabecera.AddCell(celCospan);

                //'AGREGANDO IMAGEN:      

                string imagePath = Server.MapPath("/Assets/img.JPG");

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
                jpg.ScaleToFit(160.0F, 100.0F);

                PdfPTable tableIzq = new PdfPTable(1);
                PdfPCell celImg = new PdfPCell(jpg);
                celImg.HorizontalAlignment = 1;
                celImg.Border = 0;

                tableIzq.AddCell(celImg);


                PdfPCell celInfo = new PdfPCell(new Phrase(Razon + ":", _standardFont));
                celInfo.HorizontalAlignment = 1;
                celInfo.Border = 0;
                tableIzq.AddCell(celInfo);

                PdfPCell celContacto = new PdfPCell(new Phrase("Contacto e Información :", _standardFont));
                celContacto.HorizontalAlignment = 1;
                celContacto.Border = 0;
                tableIzq.AddCell(celContacto);

                PdfPCell celDireccion = new PdfPCell(new Phrase(direccion, _standardFont));
                celDireccion.HorizontalAlignment = 1;
                celDireccion.Border = 0;
                tableIzq.AddCell(celDireccion);

                PdfPCell celTelefono = new PdfPCell(new Phrase("Telf.: " + telefono + "| E-mail:" + correo, _standardFont));
                celTelefono.HorizontalAlignment = 1;
                celTelefono.Border = 0;
                tableIzq.AddCell(celTelefono);

                PdfPCell celIzq = new PdfPCell(tableIzq);
                celIzq.Border = 0;
                TableCabecera.AddCell(celIzq);

                PdfPTable tableDer = new PdfPTable(1);
                tableDer.WidthPercentage = 10;
                PdfPCell celEspacio1 = new PdfPCell(new Phrase("     "));
                celEspacio1.Border = 0;
                tableDer.AddCell(celEspacio1);

                //


                PdfPCell cRucs = new PdfPCell(new Phrase("COTIZACION", _standarTitulo));
                cRucs.Border = 0;
                cRucs.HorizontalAlignment = 2;
                tableDer.AddCell(cRucs);

                PdfPCell cTipoDoc = new PdfPCell(new Phrase("FECHA:" + FechaEmi, _standarTexto));
                cTipoDoc.HorizontalAlignment = 2;
                cTipoDoc.Border = 0;
                tableDer.AddCell(cTipoDoc);

                PdfPCell cNumFac = new PdfPCell(new Phrase("O.C. N°:" + serie, _standarTexto));
                cNumFac.Border = 0;
                cNumFac.HorizontalAlignment = 2;
                tableDer.AddCell(cNumFac);

                PdfPCell celDer = new PdfPCell(tableDer);
                celDer.Border = 0;
                TableCabecera.AddCell(celDer);

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

                float[] celdas1 = new float[] { 1.0F, 4.0F };
                tblPrueba.SetWidths(celdas1);

                PdfPCell cTitulo1 = new PdfPCell(new Phrase("Documento:", _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 0;
                cTitulo1.PaddingTop = 2;
                tblPrueba.AddCell(cTitulo1);

                PdfPCell cRazonsocial = new PdfPCell(new Phrase(Documento, _standardFont));
                cRazonsocial.Border = 0; // borde cero
                cRazonsocial.HorizontalAlignment = 0;
                cRazonsocial.PaddingTop = 2;
                tblPrueba.AddCell(cRazonsocial);

                PdfPCell cTitulo2 = new PdfPCell(new Phrase("Nombre Cliente:", _standardFont));
                cTitulo2.Border = 0;
                cTitulo2.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo2);

                PdfPCell cRuc = new PdfPCell(new Phrase(NombreCliente, _standardFont));
                cRuc.Border = 0;
                cRuc.HorizontalAlignment = 0;
                tblPrueba.AddCell(cRuc);


                PdfPCell cTitulo3 = new PdfPCell(new Phrase("DIRECCIÓN:", _standardFont));
                cTitulo3.Border = 0;
                cTitulo3.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo3);

                PdfPCell cDireccion = new PdfPCell(new Phrase(Direccion, _standardFont));
                cDireccion.Border = 0;
                cDireccion.HorizontalAlignment = 0;
                tblPrueba.AddCell(cDireccion);

                PdfPCell cEspacio1 = new PdfPCell(new Phrase("      "));
                cEspacio1.Colspan = 2;
                cEspacio1.Border = 0;
                cEspacio1.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio1);

                PdfPTable Tableitems = new PdfPTable(6);
                Tableitems.WidthPercentage = 100;

                float[] celdas2 = new float[] { 1.0F, 2.0F, 1.0F, 1.0F, 1.0F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloCodigo = new PdfPCell(new Phrase("Item.", _standardFont));
                cTituloCodigo.Border = 1;
                cTituloCodigo.BorderWidthBottom = 1;
                cTituloCodigo.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCodigo.Padding = 5;
                Tableitems.AddCell(cTituloCodigo);

                PdfPCell cTituloDesc = new PdfPCell(new Phrase("Codigo", _standardFont));
                cTituloDesc.Border = 1;
                cTituloDesc.BorderWidthBottom = 1;
                cTituloDesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDesc.Padding = 5;
                Tableitems.AddCell(cTituloDesc);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("Descripción", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloPrecio = new PdfPCell(new Phrase("Cant.", _standardFont));
                cTituloPrecio.Border = 1;
                cTituloPrecio.BorderWidthBottom = 1;
                cTituloPrecio.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloPrecio.Padding = 5;
                Tableitems.AddCell(cTituloPrecio);

                PdfPCell cTituloSubT = new PdfPCell(new Phrase("Pre. Unit", _standardFont));
                cTituloSubT.Border = 1;
                cTituloSubT.BorderWidthBottom = 1;
                cTituloSubT.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubT.Padding = 5;
                Tableitems.AddCell(cTituloSubT);

                PdfPCell cTituloSubTimport = new PdfPCell(new Phrase("Importe", _standardFont));
                cTituloSubTimport.Border = 1;
                cTituloSubTimport.BorderWidthBottom = 1;
                cTituloSubTimport.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubTimport.Padding = 5;
                Tableitems.AddCell(cTituloSubTimport);

                foreach (EReporteCotizacion ListaRep in Ordem.Datos)
                {

                    PdfPCell cItem = new PdfPCell(new Phrase(ListaRep.Item.ToString(), _standardFont));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    Tableitems.AddCell(cItem);

                    PdfPCell cCodigo = new PdfPCell(new Phrase(ListaRep.Producto.Codigo, _standardFont));
                    cCodigo.Border = 0;
                    cCodigo.Padding = 2;
                    cCodigo.PaddingBottom = 2;
                    cCodigo.HorizontalAlignment = 1;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.Producto.NombreMat, _standardFont));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.CotizacionDet.Cantidad, _standardFont));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 2;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(ListaRep.CotizacionDet.Precio, _standardFont));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 2;
                    Tableitems.AddCell(cPrecio);

                    PdfPCell cImporte = new PdfPCell(new Phrase(ListaRep.CotizacionDet.Importe, _standardFont));
                    cImporte.Border = 0;
                    cImporte.Padding = 2;
                    cImporte.PaddingBottom = 2;
                    cImporte.HorizontalAlignment = 2;
                    Tableitems.AddCell(cImporte);
                }

                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Colspan = 5;
                cCeldaDetalle.Border = 0;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.PaddingBottom = 5;
                tblPrueba.AddCell(cCeldaDetalle);

                PdfPCell cCeldaTotal = new PdfPCell(new Phrase("     "));
                cCeldaTotal.Colspan = 2;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);

                PdfPTable TableValor = new PdfPTable(6);
                TableValor.WidthPercentage = 100;

                float[] celdas = new float[] { 1.0F, 2.0F, 1.0F, 1.0F, 1.0F, 1.0F };
                TableValor.SetWidths(celdas2);

                PdfPCell cTituloSub = new PdfPCell(new Phrase("SUB TOTAL S/", _standardFont));
                cTituloSub.Border = 0;
                cTituloSub.Colspan = 5;
                cTituloSub.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloSub);

                PdfPCell cCeldaSubtotal = new PdfPCell(new Phrase(SubTotal, _standardFont));
                cCeldaSubtotal.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubtotal.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaSubtotal);

                PdfPCell cTituloIgv = new PdfPCell(new Phrase("I.G.V " + " S/.", _standardFont));
                cTituloIgv.Border = 0;
                cTituloIgv.Colspan = 5;
                cTituloIgv.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloIgv);

                PdfPCell cCeldaIgv = new PdfPCell(new Phrase(IGV, _standardFont));
                cCeldaIgv.Border = 0;
                cCeldaIgv.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaIgv);

                PdfPCell cTituloImporte = new PdfPCell(new Phrase("IMPORTE TOTAL S/", _standardFont));
                cTituloImporte.Border = 0;
                cTituloImporte.Colspan = 5;
                cTituloImporte.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloImporte);

                PdfPCell cCeldaTotalDoc = new PdfPCell(new Phrase(Total, _standardFont));
                cCeldaTotalDoc.Border = 0;
                cCeldaTotalDoc.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaTotalDoc);

                //
                PdfPCell cTituloMoneda = new PdfPCell(new Phrase("Moneda", _standardFont));
                cTituloMoneda.Border = 0;
                cTituloMoneda.Colspan = 5;
                cTituloMoneda.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloMoneda);

                PdfPCell cCeldaMoneda = new PdfPCell(new Phrase(Moneda, _standardFont));
                cCeldaMoneda.Border = 0;
                cCeldaMoneda.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaMoneda);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 6;
                cCeldaValor.Border = 0;
                cCeldaValor.BorderWidthBottom = 1;
                cCeldaValor.PaddingBottom = 5;
                tblPrueba.AddCell(cCeldaValor);

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                Response.ContentType = "application/pdf";

                // Set default file Name as current datetime 
                Response.AddHeader("content-disposition", "attachment; filename=Documento_" + IdCotizacion + "_OC_" + serie + ".pdf");
                Response.Write(pdfDoc);

                Response.Flush();
                Response.End();
                if (Envio == 0)
                {
                    string File = path;
                    //enviar correo
                    Correo.SendMailFactura(Motivo, Mensaje, Email, File, correo, Razon, contrasenia);
                }
                
                return View();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }

        //factura
        public ActionResult Imprimir(int iComienzo, int iMedia, string Numero, string Cliente, int TipoDocumento, string FechaInicio, string FechaFin)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            EGeneralJson<EFacturaCab> ListaFactura = Factura.ListaFacBol(iComienzo, iMedia, Numero, Cliente, TipoDocumento, FechaInicio, FechaFin);

            if (ListaFactura.Total > 0)
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

                PdfPCell celCospan = new PdfPCell(new Phrase("REPORTE DE VENTA POR CLIENTE"));
                celCospan.Colspan = 10;
                celCospan.Border = 0;
                celCospan.HorizontalAlignment = 1;
                Tableitems.AddCell(celCospan);


                PdfPCell cEspacio = new PdfPCell(new Phrase("              "));
                cEspacio.Colspan = 10;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 1;
                Tableitems.AddCell(cEspacio);

                Single[] celdas2 = new Single[] { 0.5F, 1.0F, 1.0F, 1.0F, 1.0F, 2.0F, 1.0F, 1.0F, 1.0F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloItem = new PdfPCell(new Phrase("Item", _standardFont));
                cTituloItem.Border = 1;
                cTituloItem.BorderWidthBottom = 1;
                cTituloItem.HorizontalAlignment = 1;
                cTituloItem.Padding = 5;
                Tableitems.AddCell(cTituloItem);



                PdfPCell cTituloCant = new PdfPCell(new Phrase("Doc.", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1;
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);

                PdfPCell CTituloSerie = new PdfPCell(new Phrase("Serie-Numero", _standardFont));
                CTituloSerie.Border = 1;
                CTituloSerie.BorderWidthBottom = 1;
                CTituloSerie.HorizontalAlignment = 1;
                CTituloSerie.Padding = 5;
                Tableitems.AddCell(CTituloSerie);

                PdfPCell cTitutloNroDocumento = new PdfPCell(new Phrase("FechaEmi", _standardFont));
                cTitutloNroDocumento.Border = 1;
                cTitutloNroDocumento.BorderWidthBottom = 1;
                cTitutloNroDocumento.HorizontalAlignment = 1;
                cTitutloNroDocumento.Padding = 5;
                Tableitems.AddCell(cTitutloNroDocumento);

                PdfPCell cTitutloFecha = new PdfPCell(new Phrase("Nro. Doc", _standardFont));
                cTitutloFecha.Border = 1;
                cTitutloFecha.BorderWidthBottom = 1;
                cTitutloFecha.HorizontalAlignment = 1;
                cTitutloFecha.Padding = 5;
                Tableitems.AddCell(cTitutloFecha);

                PdfPCell cTituloCliente = new PdfPCell(new Phrase("Cliente", _standardFont));
                cTituloCliente.Border = 1;
                cTituloCliente.BorderWidthBottom = 1;
                cTituloCliente.HorizontalAlignment = 1;
                cTituloCliente.Padding = 5;
                Tableitems.AddCell(cTituloCliente);

                PdfPCell cCantidad = new PdfPCell(new Phrase("Moneda", _standardFont));
                cCantidad.Border = 1;
                cCantidad.BorderWidthBottom = 1;
                cCantidad.HorizontalAlignment = 1;
                cCantidad.Padding = 5;
                Tableitems.AddCell(cCantidad);


                PdfPCell cIGV = new PdfPCell(new Phrase("IGV", _standardFont));
                cIGV.Border = 1;
                cIGV.BorderWidthBottom = 1;
                cIGV.HorizontalAlignment = 1;
                cIGV.Padding = 5;
                Tableitems.AddCell(cIGV);


                PdfPCell cSubTotal = new PdfPCell(new Phrase("SubTotal", _standardFont));
                cSubTotal.Border = 1;
                cSubTotal.BorderWidthBottom = 1;
                cSubTotal.HorizontalAlignment = 1;
                cSubTotal.Padding = 5;
                Tableitems.AddCell(cSubTotal);

                PdfPCell cTotal = new PdfPCell(new Phrase("Total", _standardFont));
                cTotal.Border = 1;
                cTotal.BorderWidthBottom = 1;
                cTotal.HorizontalAlignment = 1;
                cTotal.Padding = 5;
                Tableitems.AddCell(cTotal);


                Font letrasDatosTabla = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);

                foreach (EFacturaCab eFactura in ListaFactura.Datos)
                {
                    PdfPCell cItem = new PdfPCell(new Phrase(eFactura.Item.ToString(), letrasDatosTabla));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    Tableitems.AddCell(cItem);

                    PdfPCell cdocumento = new PdfPCell(new Phrase(eFactura.Comprobante.Nombre, letrasDatosTabla));
                    cdocumento.Border = 0;
                    cdocumento.Padding = 2;
                    cdocumento.PaddingBottom = 2;
                    cdocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cdocumento);

                    PdfPCell cSerie = new PdfPCell(new Phrase(eFactura.Serie, letrasDatosTabla));
                    cSerie.Border = 0;
                    cSerie.Padding = 2;
                    cSerie.PaddingBottom = 2;
                    cSerie.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSerie);

                    PdfPCell cFecha = new PdfPCell(new Phrase(eFactura.FechaEmisio, letrasDatosTabla));
                    cFecha.Border = 0;
                    cFecha.Padding = 2;
                    cFecha.PaddingBottom = 2;
                    cFecha.HorizontalAlignment = 1;
                    Tableitems.AddCell(cFecha);

                    PdfPCell cNroDocumento = new PdfPCell(new Phrase(eFactura.Cliente.NroDocumento.ToString(), letrasDatosTabla));
                    cNroDocumento.Border = 0;
                    cNroDocumento.Padding = 2;
                    cNroDocumento.PaddingBottom = 2;
                    cNroDocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNroDocumento);

                    PdfPCell cProveedor = new PdfPCell(new Phrase(eFactura.Cliente.Nombre.ToString(), letrasDatosTabla));
                    cProveedor.Border = 0;
                    cProveedor.Padding = 2;
                    cProveedor.PaddingBottom = 2;
                    cProveedor.HorizontalAlignment = 1;
                    Tableitems.AddCell(cProveedor);

                    PdfPCell cMoneda = new PdfPCell(new Phrase(eFactura.Moneda.Nombre.ToString(), letrasDatosTabla));
                    cMoneda.Border = 0;
                    cMoneda.Padding = 2;
                    cMoneda.PaddingBottom = 2;
                    cMoneda.HorizontalAlignment = 1;
                    Tableitems.AddCell(cMoneda);

                    PdfPCell celIGV = new PdfPCell(new Phrase(eFactura.IGV.ToString(), letrasDatosTabla));
                    celIGV.Border = 0;
                    celIGV.Padding = 2;
                    celIGV.PaddingBottom = 2;
                    celIGV.HorizontalAlignment = 1;
                    Tableitems.AddCell(celIGV);

                    PdfPCell celSubTotal = new PdfPCell(new Phrase(eFactura.SubTotal.ToString(), letrasDatosTabla));
                    celSubTotal.Border = 0;
                    celSubTotal.Padding = 2;
                    celSubTotal.PaddingBottom = 2;
                    celSubTotal.HorizontalAlignment = 1;
                    Tableitems.AddCell(celSubTotal);

                    PdfPCell celTotal = new PdfPCell(new Phrase(eFactura.Total.ToString(), letrasDatosTabla));
                    celTotal.Border = 0;
                    celTotal.Padding = 2;
                    celTotal.PaddingBottom = 2;
                    celTotal.HorizontalAlignment = 1;
                    Tableitems.AddCell(celTotal);

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
                Response.AddHeader("content-disposition", "attachment; filename=ReporteReporte.pdf");
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


        public ActionResult ImprimirFactura(int Codigo,int Envio)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            List<EEmpresa> Empresa = general.ListaEmpresa();
            string ruc = "";
            string Razon = "";
            string ubgeo = "";
            string direccion = "";
            string fecha = "";
            string correo = "";
            string contrasenia = "";
            string telefono = "";
            string celular = "";

            foreach (EEmpresa Listempresa in Empresa)
            {
                ruc = Listempresa.RUC;
                Razon = Listempresa.RazonSocial;
                ubgeo = Listempresa.Ubigeo;
                direccion = Listempresa.Direccion;
                fecha = Listempresa.Fecha;
                correo = Listempresa.Correo;
                contrasenia = Listempresa.Contrasenia;
                telefono = Listempresa.Telefono;
                celular = Listempresa.Celular;


            }
            EGeneralJson<EReporteFactura> oFactura = Factura.ListaFactura(Codigo);
            try
            {


               
                string serie = "";
                string FechaEmi = "";
                string NombreCliente = "";
                string Documento = "";
                string Direccion = "";
                string SubTotal = "";
                string IGV = "";
                string Total = "";
                string Moneda = "";
                string EstadoFactura = "";
                string Motivo = "";
                string Mensaje = "";
                string Email = "";
                foreach (EReporteFactura ReporteCab in oFactura.Datos)
                {
                    serie = ReporteCab.FacturaCab.Serie;
                    FechaEmi = ReporteCab.FacturaCab.FechaEmisio;
                    NombreCliente = ReporteCab.Cliente.Nombre;
                    Documento = ReporteCab.Cliente.NroDocumento;
                    Direccion = ReporteCab.Cliente.Direccion;
                    SubTotal = ReporteCab.FacturaCab.SubTotal;
                    IGV = ReporteCab.FacturaCab.IGV;
                    Total = ReporteCab.FacturaCab.Total;
                    Moneda = ReporteCab.Moneda.Nombre;
                    EstadoFactura = ReporteCab.FacturaCab.Comprobante.Nombre;
                    Motivo = ReporteCab.FacturaCab.Motivo;
                    Mensaje = ReporteCab.FacturaCab.Mensaje;
                    Email = ReporteCab.Cliente.Email;
                }

                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/Factura/" + "Documento_" + Codigo + "_OC_" + serie + ".pdf";

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

                PdfPTable TableCabecera = new PdfPTable(2);
                TableCabecera.WidthPercentage = 100;

                float[] celdasCabecera = new float[] { 2.0F, 2.0F };
                TableCabecera.SetWidths(celdasCabecera);

                PdfPCell celCospan = new PdfPCell();
                celCospan.Colspan = 2;
                celCospan.Border = 0;
                TableCabecera.AddCell(celCospan);

                //'AGREGANDO IMAGEN:      

                string imagePath = Server.MapPath("/Assets/img.JPG");

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
                jpg.ScaleToFit(160.0F, 100.0F);

                PdfPTable tableIzq = new PdfPTable(1);
                PdfPCell celImg = new PdfPCell(jpg);
                celImg.HorizontalAlignment = 1;
                celImg.Border = 0;

                tableIzq.AddCell(celImg);


                PdfPCell celInfo = new PdfPCell(new Phrase(Razon + ":", _standardFont));
                celInfo.HorizontalAlignment = 1;
                celInfo.Border = 0;
                tableIzq.AddCell(celInfo);

                PdfPCell celContacto = new PdfPCell(new Phrase("Contacto e Información :", _standardFont));
                celContacto.HorizontalAlignment = 1;
                celContacto.Border = 0;
                tableIzq.AddCell(celContacto);

                PdfPCell celDireccion = new PdfPCell(new Phrase(direccion, _standardFont));
                celDireccion.HorizontalAlignment = 1;
                celDireccion.Border = 0;
                tableIzq.AddCell(celDireccion);

                PdfPCell celTelefono = new PdfPCell(new Phrase("Telf.: " + telefono + "| E-mail:" + correo, _standardFont));
                celTelefono.HorizontalAlignment = 1;
                celTelefono.Border = 0;
                tableIzq.AddCell(celTelefono);

                PdfPCell celIzq = new PdfPCell(tableIzq);
                celIzq.Border = 0;
                TableCabecera.AddCell(celIzq);

                PdfPTable tableDer = new PdfPTable(1);
                tableDer.WidthPercentage = 10;
                PdfPCell celEspacio1 = new PdfPCell(new Phrase("     "));
                celEspacio1.Border = 0;
                tableDer.AddCell(celEspacio1);

                //


                PdfPCell cRucs = new PdfPCell(new Phrase("RUC - " + ruc, _standarTitulo));
                cRucs.Border = 0;
                cRucs.HorizontalAlignment = 2;
                tableDer.AddCell(cRucs);

                PdfPCell cTipoDoc = new PdfPCell(new Phrase(EstadoFactura, _standarTexto));
                cTipoDoc.HorizontalAlignment = 2;
                cTipoDoc.Border = 0;
                tableDer.AddCell(cTipoDoc);

                PdfPCell cNumFac = new PdfPCell(new Phrase("N°: " + serie, _standarTexto));
                cNumFac.Border = 0;
                cNumFac.HorizontalAlignment = 2;
                tableDer.AddCell(cNumFac);

                PdfPCell celDer = new PdfPCell(tableDer);
                celDer.Border = 0;
                TableCabecera.AddCell(celDer);

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

                float[] celdas1 = new float[] { 1.0F, 4.0F };
                tblPrueba.SetWidths(celdas1);

                PdfPCell cTitulo1 = new PdfPCell(new Phrase("Documento:", _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 0;
                cTitulo1.PaddingTop = 2;
                tblPrueba.AddCell(cTitulo1);

                PdfPCell cRazonsocial = new PdfPCell(new Phrase(Documento, _standardFont));
                cRazonsocial.Border = 0; // borde cero
                cRazonsocial.HorizontalAlignment = 0;
                cRazonsocial.PaddingTop = 2;
                tblPrueba.AddCell(cRazonsocial);

                PdfPCell cTitulo2 = new PdfPCell(new Phrase("Nombre Cliente:", _standardFont));
                cTitulo2.Border = 0;
                cTitulo2.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo2);

                PdfPCell cRuc = new PdfPCell(new Phrase(NombreCliente, _standardFont));
                cRuc.Border = 0;
                cRuc.HorizontalAlignment = 0;
                tblPrueba.AddCell(cRuc);


                PdfPCell cTitulo3 = new PdfPCell(new Phrase("DIRECCIÓN:", _standardFont));
                cTitulo3.Border = 0;
                cTitulo3.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo3);

                PdfPCell cDireccion = new PdfPCell(new Phrase(Direccion, _standardFont));
                cDireccion.Border = 0;
                cDireccion.HorizontalAlignment = 0;
                tblPrueba.AddCell(cDireccion);

                PdfPCell cEspacio1 = new PdfPCell(new Phrase("      "));
                cEspacio1.Colspan = 2;
                cEspacio1.Border = 0;
                cEspacio1.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio1);

                PdfPTable Tableitems = new PdfPTable(6);
                Tableitems.WidthPercentage = 100;

                float[] celdas2 = new float[] { 1.0F, 2.0F, 1.0F, 1.0F, 1.0F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloCodigo = new PdfPCell(new Phrase("Item.", _standardFont));
                cTituloCodigo.Border = 1;
                cTituloCodigo.BorderWidthBottom = 1;
                cTituloCodigo.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCodigo.Padding = 5;
                Tableitems.AddCell(cTituloCodigo);

                PdfPCell cTituloDesc = new PdfPCell(new Phrase("Codigo", _standardFont));
                cTituloDesc.Border = 1;
                cTituloDesc.BorderWidthBottom = 1;
                cTituloDesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDesc.Padding = 5;
                Tableitems.AddCell(cTituloDesc);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("Descripción", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloPrecio = new PdfPCell(new Phrase("Cant.", _standardFont));
                cTituloPrecio.Border = 1;
                cTituloPrecio.BorderWidthBottom = 1;
                cTituloPrecio.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloPrecio.Padding = 5;
                Tableitems.AddCell(cTituloPrecio);

                PdfPCell cTituloSubT = new PdfPCell(new Phrase("Pre. Unit", _standardFont));
                cTituloSubT.Border = 1;
                cTituloSubT.BorderWidthBottom = 1;
                cTituloSubT.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubT.Padding = 5;
                Tableitems.AddCell(cTituloSubT);

                PdfPCell cTituloSubTimport = new PdfPCell(new Phrase("Importe", _standardFont));
                cTituloSubTimport.Border = 1;
                cTituloSubTimport.BorderWidthBottom = 1;
                cTituloSubTimport.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubTimport.Padding = 5;
                Tableitems.AddCell(cTituloSubTimport);

                foreach (EReporteFactura ListaRep in oFactura.Datos)
                {

                    PdfPCell cItem = new PdfPCell(new Phrase(ListaRep.Item.ToString(), _standardFont));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    Tableitems.AddCell(cItem);

                    PdfPCell cCodigo = new PdfPCell(new Phrase(ListaRep.Producto.Codigo, _standardFont));
                    cCodigo.Border = 0;
                    cCodigo.Padding = 2;
                    cCodigo.PaddingBottom = 2;
                    cCodigo.HorizontalAlignment = 1;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.Producto.NombreMat, _standardFont));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.FacturaCab.Cantidad, _standardFont));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 2;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(ListaRep.Detalle.Precio, _standardFont));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 2;
                    Tableitems.AddCell(cPrecio);

                    PdfPCell cImporte = new PdfPCell(new Phrase(ListaRep.Detalle.Importe, _standardFont));
                    cImporte.Border = 0;
                    cImporte.Padding = 2;
                    cImporte.PaddingBottom = 2;
                    cImporte.HorizontalAlignment = 2;
                    Tableitems.AddCell(cImporte);
                }

                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Colspan = 5;
                cCeldaDetalle.Border = 0;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.PaddingBottom = 5;
                tblPrueba.AddCell(cCeldaDetalle);

                PdfPCell cCeldaTotal = new PdfPCell(new Phrase("     "));
                cCeldaTotal.Colspan = 2;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);

                PdfPTable TableValor = new PdfPTable(6);
                TableValor.WidthPercentage = 100;

                float[] celdas = new float[] { 1.0F, 2.0F, 1.0F, 1.0F, 1.0F, 1.0F };
                TableValor.SetWidths(celdas2);

                PdfPCell cTituloSub = new PdfPCell(new Phrase("SUB TOTAL S/", _standardFont));
                cTituloSub.Border = 0;
                cTituloSub.Colspan = 5;
                cTituloSub.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloSub);

                PdfPCell cCeldaSubtotal = new PdfPCell(new Phrase(SubTotal, _standardFont));
                cCeldaSubtotal.Border = 0;
                // cCeldaSubtotal.Colspan = 2
                cCeldaSubtotal.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaSubtotal);

                PdfPCell cTituloIgv = new PdfPCell(new Phrase("I.G.V " + " S/.", _standardFont));
                cTituloIgv.Border = 0;
                cTituloIgv.Colspan = 5;
                cTituloIgv.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloIgv);

                PdfPCell cCeldaIgv = new PdfPCell(new Phrase(IGV, _standardFont));
                cCeldaIgv.Border = 0;
                cCeldaIgv.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaIgv);

                PdfPCell cTituloImporte = new PdfPCell(new Phrase("IMPORTE TOTAL S/", _standardFont));
                cTituloImporte.Border = 0;
                cTituloImporte.Colspan = 5;
                cTituloImporte.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloImporte);

                PdfPCell cCeldaTotalDoc = new PdfPCell(new Phrase(Total, _standardFont));
                cCeldaTotalDoc.Border = 0;
                cCeldaTotalDoc.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaTotalDoc);

                //
                PdfPCell cTituloMoneda = new PdfPCell(new Phrase("Moneda", _standardFont));
                cTituloMoneda.Border = 0;
                cTituloMoneda.Colspan = 5;
                cTituloMoneda.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloMoneda);

                PdfPCell cCeldaMoneda = new PdfPCell(new Phrase(Moneda, _standardFont));
                cCeldaMoneda.Border = 0;
                cCeldaMoneda.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaMoneda);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 6;
                cCeldaValor.Border = 0;
                cCeldaValor.BorderWidthBottom = 1;
                cCeldaValor.PaddingBottom = 5;
                tblPrueba.AddCell(cCeldaValor);

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                Response.ContentType = "application/pdf";

                // Set default file Name as current datetime 
                Response.AddHeader("content-disposition", "attachment; filename=Documento_" + Codigo + "_OC_" + serie + ".pdf");
                Response.Write(pdfDoc);

                Response.Flush();
                Response.End();
                if (Envio == 0)
                {
                    string File = path;
                    //enviar correo
                    Correo.SendMailFactura(Motivo, Mensaje, Email, File, correo, Razon, contrasenia);
                }
                return View();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }

        public ActionResult ImprimirGuiaCab(int iComienzo, int iMedia, string Numero, string Numdoc, string Cliente, int Doc, string FechaInicio, string FechaFin)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            EGeneralJson<EGuiaCab> ListaGuia = Guia.ListaGia(iComienzo, iMedia, Numero, Numdoc, Cliente, Doc, FechaInicio, FechaFin);

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

                PdfPTable Tableitems = new PdfPTable(7);
                Tableitems.WidthPercentage = 100;
                Tableitems.HorizontalAlignment = Element.ALIGN_JUSTIFIED;

                PdfPCell celCospan = new PdfPCell(new Phrase("REPORTE DE GUIA"));
                celCospan.Colspan = 7;
                celCospan.Border = 0;
                celCospan.HorizontalAlignment = 1;
                Tableitems.AddCell(celCospan);


                PdfPCell cEspacio = new PdfPCell(new Phrase("              "));
                cEspacio.Colspan = 7;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 1;
                Tableitems.AddCell(cEspacio);

                Single[] celdas2 = new Single[] { 0.5F, 1.0F, 1.0F, 1.0F, 1.0F, 2.0F, 1.0F };
                Tableitems.SetWidths(celdas2);

                PdfPCell cTituloItem = new PdfPCell(new Phrase("Item", _standardFont));
                cTituloItem.Border = 1;
                cTituloItem.BorderWidthBottom = 1;
                cTituloItem.HorizontalAlignment = 1;
                cTituloItem.Padding = 5;
                Tableitems.AddCell(cTituloItem);



                PdfPCell cTituloCant = new PdfPCell(new Phrase("Serie-Numero", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1;
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);

                PdfPCell CTituloSerie = new PdfPCell(new Phrase("Fecha Emisión", _standardFont));
                CTituloSerie.Border = 1;
                CTituloSerie.BorderWidthBottom = 1;
                CTituloSerie.HorizontalAlignment = 1;
                CTituloSerie.Padding = 5;
                Tableitems.AddCell(CTituloSerie);

                PdfPCell cTitutloNroDocumento = new PdfPCell(new Phrase("Nro Cotización", _standardFont));
                cTitutloNroDocumento.Border = 1;
                cTitutloNroDocumento.BorderWidthBottom = 1;
                cTitutloNroDocumento.HorizontalAlignment = 1;
                cTitutloNroDocumento.Padding = 5;
                Tableitems.AddCell(cTitutloNroDocumento);

                PdfPCell cTitutloFecha = new PdfPCell(new Phrase("Nro. Doc. Cliente", _standardFont));
                cTitutloFecha.Border = 1;
                cTitutloFecha.BorderWidthBottom = 1;
                cTitutloFecha.HorizontalAlignment = 1;
                cTitutloFecha.Padding = 5;
                Tableitems.AddCell(cTitutloFecha);

                PdfPCell cTituloCliente = new PdfPCell(new Phrase("Cliente", _standardFont));
                cTituloCliente.Border = 1;
                cTituloCliente.BorderWidthBottom = 1;
                cTituloCliente.HorizontalAlignment = 1;
                cTituloCliente.Padding = 5;
                Tableitems.AddCell(cTituloCliente);

                PdfPCell cCantidad = new PdfPCell(new Phrase("Motivo", _standardFont));
                cCantidad.Border = 1;
                cCantidad.BorderWidthBottom = 1;
                cCantidad.HorizontalAlignment = 1;
                cCantidad.Padding = 5;
                Tableitems.AddCell(cCantidad);


                Font letrasDatosTabla = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.NORMAL);

                foreach (EGuiaCab oGuia in ListaGuia.Datos)
                {
                    PdfPCell cItem = new PdfPCell(new Phrase(oGuia.Item.ToString(), letrasDatosTabla));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    Tableitems.AddCell(cItem);

                    PdfPCell cdocumento = new PdfPCell(new Phrase(oGuia.Serie, letrasDatosTabla));
                    cdocumento.Border = 0;
                    cdocumento.Padding = 2;
                    cdocumento.PaddingBottom = 2;
                    cdocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cdocumento);

                    PdfPCell cSerie = new PdfPCell(new Phrase(oGuia.FechaEmision, letrasDatosTabla));
                    cSerie.Border = 0;
                    cSerie.Padding = 2;
                    cSerie.PaddingBottom = 2;
                    cSerie.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSerie);

                    PdfPCell cFecha = new PdfPCell(new Phrase(oGuia.Comprobante.Serie, letrasDatosTabla));
                    cFecha.Border = 0;
                    cFecha.Padding = 2;
                    cFecha.PaddingBottom = 2;
                    cFecha.HorizontalAlignment = 1;
                    Tableitems.AddCell(cFecha);

                    PdfPCell cNroDocumento = new PdfPCell(new Phrase(oGuia.Cliente.NroDocumento.ToString(), letrasDatosTabla));
                    cNroDocumento.Border = 0;
                    cNroDocumento.Padding = 2;
                    cNroDocumento.PaddingBottom = 2;
                    cNroDocumento.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNroDocumento);

                    PdfPCell cProveedor = new PdfPCell(new Phrase(oGuia.Cliente.Nombre.ToString(), letrasDatosTabla));
                    cProveedor.Border = 0;
                    cProveedor.Padding = 2;
                    cProveedor.PaddingBottom = 2;
                    cProveedor.HorizontalAlignment = 1;
                    Tableitems.AddCell(cProveedor);

                    PdfPCell cMoneda = new PdfPCell(new Phrase(oGuia.Motivo.ToString(), letrasDatosTabla));
                    cMoneda.Border = 0;
                    cMoneda.Padding = 2;
                    cMoneda.PaddingBottom = 2;
                    cMoneda.HorizontalAlignment = 1;
                    Tableitems.AddCell(cMoneda);

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
                Response.AddHeader("content-disposition", "attachment; filename=ReporteGuia.pdf");
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


        public ActionResult ImprimirGuia(int Codigo,int Envio)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            List<EEmpresa> Empresa = general.ListaEmpresa();
            string ruc = "";
            string Razon = "";
            string ubgeo = "";
            string direccion = "";
            string fecha = "";
            string correo = "";
            string contrasenia = "";
            string telefono = "";
            string celular = "";

            foreach (EEmpresa Listempresa in Empresa)
            {
                ruc = Listempresa.RUC;
                Razon = Listempresa.RazonSocial;
                ubgeo = Listempresa.Ubigeo;
                direccion = Listempresa.Direccion;
                fecha = Listempresa.Fecha;
                correo = Listempresa.Correo;
                contrasenia = Listempresa.Contrasenia;
                telefono = Listempresa.Telefono;
                celular = Listempresa.Celular;


            }
            EGeneralJson<EGuiaCab> oGuia = Guia.ReporteGuia(Codigo);
            try
            {
                string Numero = "";
                string PuntoLlegada = "";
                string FechaEmi = "";
                string NombreCliente = "";
                string Documento = "";
                string Direccion = "";
                string Marca = "";
                string Placa = "";
                string Licencia = "";
                string NomEmpresa = "";
                string RucEmpresa = "";
                string NroComprobante = "";
                string Motivo = "";
                string UbigeoDestino = "";
                string Asunto = "";
                string Mensaje = "";
                string Email = "";


                foreach (EGuiaCab ReporteCab in oGuia.Datos)
                {
                    Numero = ReporteCab.Numero;
                    PuntoLlegada = ReporteCab.PuntoLlegada;
                    FechaEmi = ReporteCab.FechaEmision;
                    NombreCliente = ReporteCab.Cliente.Nombre;
                    Documento = ReporteCab.Cliente.NroDocumento;
                    Direccion = ReporteCab.Cliente.Direccion;
                    Marca = ReporteCab.Marca;
                    Placa = ReporteCab.Placa;
                    Licencia = ReporteCab.Licencia;
                    NomEmpresa = ReporteCab.NombreEmpresa;
                    RucEmpresa = ReporteCab.RucEmpresa;
                    NroComprobante = ReporteCab.Comprobante.Serie;
                    Motivo = ReporteCab.Motivo;
                    UbigeoDestino = ReporteCab.Ubigeo.UbicacionGeografica;
                    Asunto = ReporteCab.Asunto;
                    Mensaje = ReporteCab.Mensaje;
                    Email = ReporteCab.Cliente.Email;
                }
                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);

                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/Guia/" + "Documento_" + Codigo + "_" + Numero + ".pdf";

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

                PdfPTable tblPrueba = new PdfPTable(4);
                tblPrueba.WidthPercentage = 100;

                PdfPTable TableCabecera = new PdfPTable(2);
                TableCabecera.WidthPercentage = 100;

                float[] celdasCabecera = new float[] { 2.0F, 2.0F };
                TableCabecera.SetWidths(celdasCabecera);

                PdfPCell celCospan = new PdfPCell();
                celCospan.Colspan = 2;
                celCospan.Border = 0;
                TableCabecera.AddCell(celCospan);

                //'AGREGANDO IMAGEN:      

                string imagePath = Server.MapPath("/Assets/img.JPG");

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imagePath);
                jpg.ScaleToFit(160.0F, 100.0F);

                PdfPTable tableIzq = new PdfPTable(1);
                PdfPCell celImg = new PdfPCell(jpg);
                celImg.HorizontalAlignment = 1;
                celImg.Border = 0;

                tableIzq.AddCell(celImg);


                PdfPCell celInfo = new PdfPCell(new Phrase(Razon + ":", _standardFont));
                celInfo.HorizontalAlignment = 1;
                celInfo.Border = 0;
                tableIzq.AddCell(celInfo);

                PdfPCell celContacto = new PdfPCell(new Phrase("Contacto e Información :", _standardFont));
                celContacto.HorizontalAlignment = 1;
                celContacto.Border = 0;
                tableIzq.AddCell(celContacto);

                PdfPCell celDireccion = new PdfPCell(new Phrase(direccion, _standardFont));
                celDireccion.HorizontalAlignment = 1;
                celDireccion.Border = 0;
                tableIzq.AddCell(celDireccion);

                PdfPCell celTelefono = new PdfPCell(new Phrase("Telf.: " + telefono + "| E-mail:" + correo, _standardFont));
                celTelefono.HorizontalAlignment = 1;
                celTelefono.Border = 0;
                tableIzq.AddCell(celTelefono);

                PdfPCell celIzq = new PdfPCell(tableIzq);
                celIzq.Border = 0;
                TableCabecera.AddCell(celIzq);

                PdfPTable tableDer = new PdfPTable(1);
                tableDer.WidthPercentage = 10;
                PdfPCell celEspacio1 = new PdfPCell(new Phrase("     "));
                celEspacio1.Border = 0;
                tableDer.AddCell(celEspacio1);

                //


                PdfPCell cRucs = new PdfPCell(new Phrase("RUC - " + ruc, _standarTitulo));
                cRucs.Border = 0;
                cRucs.HorizontalAlignment = 2;
                tableDer.AddCell(cRucs);

                PdfPCell cTipoDoc = new PdfPCell(new Phrase("GUIA DE REMISION", _standarTexto));
                cTipoDoc.HorizontalAlignment = 2;
                cTipoDoc.Border = 0;
                tableDer.AddCell(cTipoDoc);

                PdfPCell cNumFac = new PdfPCell(new Phrase("N°: " + Numero, _standarTexto));
                cNumFac.Border = 0;
                cNumFac.HorizontalAlignment = 2;
                tableDer.AddCell(cNumFac);

                PdfPCell celDer = new PdfPCell(tableDer);
                celDer.Border = 0;
                TableCabecera.AddCell(celDer);

                PdfPCell cCeldaDetallec = new PdfPCell(TableCabecera);
                cCeldaDetallec.Colspan = 4;
                cCeldaDetallec.Border = 0;
                cCeldaDetallec.BorderWidthBottom = 0;
                tblPrueba.AddCell(cCeldaDetallec);

                PdfPCell cEspacio = new PdfPCell(new Phrase("      "));
                cEspacio.Colspan = 4;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio);

                float[] celdas1 = new float[] { 1.0F, 2.0F, 1.0F, 2.0F };
                tblPrueba.SetWidths(celdas1);

                PdfPCell cCeldaValorLinea = new PdfPCell();
                cCeldaValorLinea.Colspan = 4;
                cCeldaValorLinea.Border = 0;
                cCeldaValorLinea.BorderWidthBottom = 1;
                tblPrueba.AddCell(cCeldaValorLinea);

                PdfPCell cTitulo1 = new PdfPCell(new Phrase("Punto de Partida:", _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 0;
                cTitulo1.PaddingTop = 2;
                cTitulo1.Border = Rectangle.LEFT_BORDER;
                cTitulo1.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo1);

                PdfPCell cRazonsocial = new PdfPCell(new Phrase(direccion + " " + ubgeo, _standardFont));
                cRazonsocial.Border = 0; // borde cero
                cRazonsocial.HorizontalAlignment = 0;
                cRazonsocial.PaddingTop = 2;
                cRazonsocial.BorderWidthBottom = 1;
                tblPrueba.AddCell(cRazonsocial);

                PdfPCell cTitulo2 = new PdfPCell(new Phrase("Punto de Llegada:", _standardFont));
                cTitulo2.Border = 0;
                cTitulo2.HorizontalAlignment = 0;
                cTitulo2.Border = Rectangle.LEFT_BORDER;
                cTitulo2.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo2);

                PdfPCell cRuc = new PdfPCell(new Phrase(PuntoLlegada +"  "+ UbigeoDestino, _standardFont));
                cRuc.Border = 0;
                cRuc.HorizontalAlignment = 0;
                cRuc.Border = Rectangle.RIGHT_BORDER;
                cRuc.BorderWidthBottom = 1;

                tblPrueba.AddCell(cRuc);


                PdfPCell cTitulo3 = new PdfPCell(new Phrase("Fecha Emision:", _standardFont));
                cTitulo3.Border = 0;
                cTitulo3.HorizontalAlignment = 0;
                cTitulo3.Border = Rectangle.LEFT_BORDER;
                cTitulo3.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo3);

                PdfPCell cDireccion = new PdfPCell(new Phrase(FechaEmi, _standardFont));
                cDireccion.Border = 0;
                cDireccion.HorizontalAlignment = 0;
                cDireccion.Border = Rectangle.RIGHT_BORDER;
                cDireccion.BorderWidthBottom = 1;

                tblPrueba.AddCell(cDireccion);


                PdfPCell cTitulo4 = new PdfPCell(new Phrase("Nombre o Razón social del destinatario: ", _standardFont));
                cTitulo4.Border = 0;
                cTitulo4.HorizontalAlignment = 0;
                cTitulo4.Border = Rectangle.LEFT_BORDER;
                cTitulo4.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo4);

                PdfPCell cNombreCliente = new PdfPCell(new Phrase(NombreCliente, _standardFont));
                cNombreCliente.Border = 0;
                cNombreCliente.HorizontalAlignment = 0;
                cNombreCliente.Border = Rectangle.RIGHT_BORDER;
                cNombreCliente.BorderWidthBottom = 1;
                tblPrueba.AddCell(cNombreCliente);


                //////
                PdfPCell cTitulo5 = new PdfPCell(new Phrase("Costo:", _standardFont));
                cTitulo5.Border = 0;
                cTitulo5.HorizontalAlignment = 0;
                cTitulo5.Border = Rectangle.LEFT_BORDER;
                cTitulo5.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo5);

                PdfPCell cNinguno = new PdfPCell(new Phrase("ninguno", _standardFont));
                cNinguno.Border = 0;
                cNinguno.HorizontalAlignment = 0;
                cNinguno.BorderWidthBottom = 1;
                tblPrueba.AddCell(cNinguno);



                PdfPCell cTitulo6 = new PdfPCell(new Phrase("Número de RUC o DNI del destinatario: ", _standardFont));
                cTitulo6.Border = 0;
                cTitulo6.HorizontalAlignment = 0;
                cTitulo6.Border = Rectangle.LEFT_BORDER;
                cTitulo6.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo6);

                PdfPCell cRucEmpre = new PdfPCell(new Phrase(Documento, _standardFont));
                cRucEmpre.Border = 0;
                cRucEmpre.HorizontalAlignment = 0;
                cRucEmpre.Border = Rectangle.RIGHT_BORDER;
                cRucEmpre.BorderWidthBottom = 1;
                tblPrueba.AddCell(cRucEmpre);



                PdfPCell cTitulo75 = new PdfPCell(new Phrase("", _standardFont));
                cTitulo75.Border = 0;
                cTitulo75.HorizontalAlignment = 0;
                cTitulo75.Border = Rectangle.LEFT_BORDER;
                cTitulo75.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo75);

                PdfPCell cTitulo7 = new PdfPCell(new Phrase("Unidad de Transporte y conductor:", _standardFont));
                cTitulo7.Border = 0;
                cTitulo7.HorizontalAlignment = 1;
                cTitulo7.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo7);

                PdfPCell cTitulo85 = new PdfPCell(new Phrase("", _standardFont));
                cTitulo85.Border = 0;
                cTitulo85.HorizontalAlignment = 0;
                cTitulo85.Border = Rectangle.LEFT_BORDER;
                cTitulo85.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo85);

                //-
                PdfPCell cTitulo8 = new PdfPCell(new Phrase("Empresa de trasnporte :", _standardFont));
                cTitulo8.Border = 0;
                cTitulo8.HorizontalAlignment = 1;
                cTitulo8.Border = Rectangle.RIGHT_BORDER;
                cTitulo8.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo8);

                PdfPCell cTitulo9 = new PdfPCell(new Phrase("Marca y Numero deplaca:", _standardFont));
                cTitulo9.Border = 0;
                cTitulo9.HorizontalAlignment = 1;
                cTitulo9.Border = Rectangle.LEFT_BORDER;
                cTitulo9.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo9);

                PdfPCell cTitulo10 = new PdfPCell(new Phrase(Marca + Placa, _standardFont));
                cTitulo10.Border = 0;
                cTitulo10.HorizontalAlignment = 1;
                cTitulo10.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo10);


                PdfPCell cTitulo11 = new PdfPCell(new Phrase("Nombre o Razón social :", _standardFont));
                cTitulo11.Border = 0;
                cTitulo11.HorizontalAlignment = 1;
                cTitulo11.Border = Rectangle.LEFT_BORDER;
                cTitulo11.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo11);

                PdfPCell cTitulo12 = new PdfPCell(new Phrase(NomEmpresa, _standardFont));
                cTitulo12.Border = 0;
                cTitulo12.HorizontalAlignment = 1;
                cTitulo12.Border = Rectangle.RIGHT_BORDER;
                cTitulo12.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo12);


                PdfPCell cTitulo13 = new PdfPCell(new Phrase("Nº(s) de lincencia(s) de  conducir:", _standardFont));
                cTitulo13.Border = 0;
                cTitulo13.HorizontalAlignment = 1;
                cTitulo13.Border = Rectangle.LEFT_BORDER;
                cTitulo13.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo13);

                PdfPCell cTitulo14 = new PdfPCell(new Phrase(Licencia, _standardFont));
                cTitulo14.Border = 0;
                cTitulo14.HorizontalAlignment = 1;
                cTitulo14.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo14);

                PdfPCell cTitulo15 = new PdfPCell(new Phrase("Número de RUC :", _standardFont));
                cTitulo15.Border = 0;
                cTitulo15.HorizontalAlignment = 1;
                cTitulo15.Border = Rectangle.LEFT_BORDER;
                cTitulo15.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo15);

                PdfPCell cTitulo16 = new PdfPCell(new Phrase(RucEmpresa, _standardFont));
                cTitulo16.Border = 0;
                cTitulo16.HorizontalAlignment = 1;
                cTitulo16.Border = Rectangle.RIGHT_BORDER;
                cTitulo16.BorderWidthBottom = 1;
                tblPrueba.AddCell(cTitulo16);


                PdfPCell cEspacio1 = new PdfPCell(new Phrase("      "));
                cEspacio1.Colspan = 4;
                cEspacio1.Border = 0;
                cEspacio1.HorizontalAlignment = 0;
                tblPrueba.AddCell(cEspacio1);

                PdfPTable Tableitems = new PdfPTable(5);
                Tableitems.WidthPercentage = 100;

                float[] celdas3 = new float[] { 0.5F, 1.0F, 1.0F, 1.0F, 1.0F };
                Tableitems.SetWidths(celdas3);

                PdfPCell cTituloCodigo = new PdfPCell(new Phrase("Item.", _standardFont));
                cTituloCodigo.Border = 1;

                cTituloCodigo.BorderWidthBottom = 1;
                cTituloCodigo.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCodigo.Padding = 5;
                Tableitems.AddCell(cTituloCodigo);

                PdfPCell cTituloDesc = new PdfPCell(new Phrase("Cantidad", _standardFont));
                cTituloDesc.Border = 1;
                cTituloDesc.BorderWidthBottom = 1;
                cTituloDesc.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloDesc.Padding = 5;
                Tableitems.AddCell(cTituloDesc);

                PdfPCell cTituloCant = new PdfPCell(new Phrase("Descripción", _standardFont));
                cTituloCant.Border = 1;
                cTituloCant.BorderWidthBottom = 1;
                cTituloCant.HorizontalAlignment = 1; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloCant.Padding = 5;
                Tableitems.AddCell(cTituloCant);

                PdfPCell cTituloPrecio = new PdfPCell(new Phrase("Unidad.", _standardFont));
                cTituloPrecio.Border = 1;
                cTituloPrecio.BorderWidthBottom = 1;
                cTituloPrecio.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloPrecio.Padding = 5;
                Tableitems.AddCell(cTituloPrecio);

                PdfPCell cTituloSubT = new PdfPCell(new Phrase("Peso total", _standardFont));

                cTituloSubT.Border = 1;
                cTituloSubT.BorderWidthBottom = 1;
                cTituloSubT.HorizontalAlignment = 2; // 0 = izquierda, 1 = centro, 2 = derecha
                cTituloSubT.Padding = 5;
                Tableitems.AddCell(cTituloSubT);



                foreach (EGuiaCab ListaRep in oGuia.Datos)
                {

                    PdfPCell cItem = new PdfPCell(new Phrase(ListaRep.Item.ToString(), _standardFont));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    Tableitems.AddCell(cItem);

                    PdfPCell cCodigo = new PdfPCell(new Phrase(ListaRep.Cantidad, _standardFont));
                    cCodigo.Border = 0;
                    cCodigo.Padding = 2;
                    cCodigo.PaddingBottom = 2;
                    cCodigo.HorizontalAlignment = 1;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.Producto.NombreMat, _standardFont));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.Producto.UM.MedNom, _standardFont));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 2;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(" - ", _standardFont));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 1;
                    Tableitems.AddCell(cPrecio);


                }

                PdfPCell cCeldaDetalle = new PdfPCell(Tableitems);
                cCeldaDetalle.Colspan = 4;
                cCeldaDetalle.Border = 0;
                cCeldaDetalle.BorderWidthBottom = 1;
                cCeldaDetalle.PaddingBottom = 4;
                tblPrueba.AddCell(cCeldaDetalle);

                PdfPCell cCeldaTotal = new PdfPCell(new Phrase("     "));
                cCeldaTotal.Colspan = 4;
                cCeldaTotal.Border = 0;
                cCeldaTotal.HorizontalAlignment = 0;
                tblPrueba.AddCell(cCeldaTotal);

                PdfPTable TableValor = new PdfPTable(2);
                TableValor.WidthPercentage = 100;

                float[] celdas = new float[] { 1.0F, 5.0F };
                TableValor.SetWidths(celdas);

                PdfPCell cTituloSub = new PdfPCell(new Phrase("Nro Comprobante:", _standardFont));
                cTituloSub.Border = 0;
                cTituloSub.HorizontalAlignment = 0; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloSub);

                PdfPCell cTituloSubc = new PdfPCell(new Phrase(NroComprobante.ToString(), _standardFont));
                cTituloSubc.Border = 0;
                cTituloSubc.HorizontalAlignment = 0; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloSubc);

                ////------------------------******************************----------------------

                PdfPTable TableValor2 = new PdfPTable(2);
                TableValor2.WidthPercentage = 100;

                float[] celdaNew = new float[] { 6.0F, 5.0F };
                TableValor2.SetWidths(celdaNew);

                PdfPCell cTituloMotivo = new PdfPCell(new Phrase("MOTIVO DE TRASLADO:", _standardFont));
                cTituloMotivo.Border = 0;
                cTituloMotivo.Colspan = 1;
                cTituloMotivo.HorizontalAlignment = 1; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor2.AddCell(cTituloMotivo);

                PdfPCell cTituloFirmaLinea = new PdfPCell(new Phrase("______________________", _standardFont));
                cTituloFirmaLinea.Border = 0;
                cTituloFirmaLinea.Colspan = 2;
                cTituloFirmaLinea.HorizontalAlignment = 1; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor2.AddCell(cTituloFirmaLinea);

                PdfPCell cTituloMotivsu = new PdfPCell(new Phrase(Motivo, _standardFont));
                cTituloMotivsu.Border = 0;
                cTituloMotivsu.Colspan = 1;
                cTituloMotivsu.HorizontalAlignment = 1; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor2.AddCell(cTituloMotivsu);

                PdfPCell cTituloFirma = new PdfPCell(new Phrase("Firma", _standardFont));
                cTituloFirma.Border = 0;
                cTituloFirma.Colspan = 2;
                cTituloFirma.HorizontalAlignment = 1; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor2.AddCell(cTituloFirma);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 4;
                cCeldaValor.Border = 0;
                // cCeldaValor.BorderWidthBottom = 1;
                cCeldaValor.PaddingBottom = 4;
                tblPrueba.AddCell(cCeldaValor);

                PdfPCell cCeldaValor2 = new PdfPCell(TableValor2);
                cCeldaValor2.Colspan = 4;
                cCeldaValor2.Border = 0;
                //cCeldaValor2.BorderWidthBottom = 1;                
                tblPrueba.AddCell(cCeldaValor2);

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                Response.ContentType = "application/pdf";

                // Set default file Name as current datetime 
                Response.AddHeader("content-disposition", "attachment; filename=Documento_" + Codigo + "_" + Numero + ".pdf");
                Response.Write(pdfDoc);

                Response.Flush();
                Response.End();
                if (Envio == 0)
                {
                    string File = path;
                    //enviar correo
                    Correo.SendMailFactura(Asunto, Mensaje, Email, File, correo, Razon, contrasenia);
                }

                return View();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

        }
    }
}
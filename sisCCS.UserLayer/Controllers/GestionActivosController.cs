using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web.Script.Serialization;

using sisCCS.BusinessLayer;
using sisCCS.UserLayer.Models;
using sisCCS.EntityLayer;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using System.IO;

namespace sisCCS.UserLayer.Controllers
{
    public class GestionActivosController : Controller
    {
        Authentication Authentication = new Authentication();

        JavaScriptSerializer Serializer = new JavaScriptSerializer();
        BGeneral general = new BGeneral();
        BProveedor Proveedor = new BProveedor();
        BOrdenCompra BOrdenCompra = new BOrdenCompra();

        // GET: GestionActivos
        public ActionResult NuevoOrdenCompra()
        {
            return View();
        }
        public ActionResult OrdenCompra()
        {
            return View();
        }


        [HttpPost]
        public void SerieNumDoc()
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListarSerieDoc()
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
        public void ListaProveedor()
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    Proveedor.ListaProveedor()
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
        public void GeneraraOrdenCompra(EOrdenCompraCab OrdenCompra, List<EOrdenCompraDet> OrdenCompraDetalle, string Usuario)
        {
            try
            {
                Usuario = Authentication.UserLogued.Usuario;
                var sMensaje = BOrdenCompra.RegistrarOrdenCompra(OrdenCompra, OrdenCompraDetalle, Usuario);
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
        public void ListaOrdenCompra(int iComienzo, int iMedia, string fechaInicio, string fechafin, string numero)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    BOrdenCompra.ListaOrden(iComienzo, iMedia, fechaInicio, fechafin, numero)
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

        public ActionResult ImpresionOrdenCompra(int IdDoc)
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

            EGeneralJson<EReporteCompraID> Ordem = BOrdenCompra.ReporteCompraIDs(IdDoc);
            try
            {


                //Permite visualizar el contenido del documento que es descargado en "Mis descargas"
                PdfWriter.GetInstance(pdfDoc, System.Web.HttpContext.Current.Response.OutputStream);
                string serie = "";
                string Fecha = "";
                string NombreProveedor = "";
                string Ubigeo = "";
                string Direccion = "";
                string SubTotal = "";
                string IGV = "";
                string Total = "";
                string Moneda = "";
                foreach (EReporteCompraID ReporteCab in Ordem.Datos)
                {
                    serie = ReporteCab.OrdenCompraCab.Serie;
                    Fecha = ReporteCab.OrdenCompraCab.FechaRegistro;
                    NombreProveedor = ReporteCab.Proveedor.Nombre;
                    Ubigeo = ReporteCab.Proveedor.Ubigeo.UbicacionGeografica;
                    Direccion = ReporteCab.Proveedor.Direccion;
                    SubTotal = ReporteCab.OrdenCompraCab.SutTotal;
                    IGV = ReporteCab.OrdenCompraCab.IGv;
                    Total = ReporteCab.OrdenCompraCab.Total;
                    Moneda = ReporteCab.Moneda.Descripcion;
                }
                //Ruta dentro del servidor, aqui se guardara el PDF
                var path = Server.MapPath("/") + "Reporte/OrdenCompra/" + "Documento_" + IdDoc + "_OC_" + serie + ".pdf";

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


                PdfPCell cRucs = new PdfPCell(new Phrase("ORDEN DE COMPRA", _standarTitulo));
                cRucs.Border = 0;
                cRucs.HorizontalAlignment = 2;
                tableDer.AddCell(cRucs);

                PdfPCell cTipoDoc = new PdfPCell(new Phrase("FECHA:" + Fecha, _standarTexto));
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

                PdfPCell cTitulo1 = new PdfPCell(new Phrase("PROVEEDOR:", _standardFont));
                cTitulo1.Border = 0;
                cTitulo1.HorizontalAlignment = 0;
                cTitulo1.PaddingTop = 2;
                tblPrueba.AddCell(cTitulo1);

                PdfPCell cRazonsocial = new PdfPCell(new Phrase(NombreProveedor, _standardFont));
                cRazonsocial.Border = 0; // borde cero
                cRazonsocial.HorizontalAlignment = 0;
                cRazonsocial.PaddingTop = 2;
                tblPrueba.AddCell(cRazonsocial);

                PdfPCell cTitulo2 = new PdfPCell(new Phrase("DIRECION UBIGEO:", _standardFont));
                cTitulo2.Border = 0;
                cTitulo2.HorizontalAlignment = 0;
                tblPrueba.AddCell(cTitulo2);

                PdfPCell cRuc = new PdfPCell(new Phrase(Ubigeo, _standardFont));
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
                cTituloDesc.HorizontalAlignment = 0; // 0 = izquierda, 1 = centro, 2 = derecha
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

                foreach (EReporteCompraID ListaRep in Ordem.Datos)
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
                    cCodigo.HorizontalAlignment = 0;
                    Tableitems.AddCell(cCodigo);

                    PdfPCell cNombreMat = new PdfPCell(new Phrase(ListaRep.Producto.NombreMat, _standardFont));
                    cNombreMat.Border = 0;
                    cNombreMat.Padding = 2;
                    cNombreMat.PaddingBottom = 2;
                    cNombreMat.HorizontalAlignment = 1;
                    Tableitems.AddCell(cNombreMat);

                    PdfPCell cCantidad = new PdfPCell(new Phrase(ListaRep.CompraDetalle.Cantidad, _standardFont));
                    cCantidad.Border = 0;
                    cCantidad.Padding = 2;
                    cCantidad.PaddingBottom = 2;
                    cCantidad.HorizontalAlignment = 2;
                    Tableitems.AddCell(cCantidad);

                    PdfPCell cPrecio = new PdfPCell(new Phrase(ListaRep.CompraDetalle.Precio, _standardFont));
                    cPrecio.Border = 0;
                    cPrecio.Padding = 2;
                    cPrecio.PaddingBottom = 2;
                    cPrecio.HorizontalAlignment = 2;
                    Tableitems.AddCell(cPrecio);

                    PdfPCell cImporte = new PdfPCell(new Phrase(ListaRep.CompraDetalle.Importe, _standardFont));
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
                cTituloSub.HorizontalAlignment = 2; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloSub);

                PdfPCell cCeldaSubtotal = new PdfPCell(new Phrase(SubTotal, _standardFont));
                cCeldaSubtotal.Border = 0;                 
                cCeldaSubtotal.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaSubtotal);

                PdfPCell cTituloIgv = new PdfPCell(new Phrase("I.G.V ", _standardFont));
                cTituloIgv.Border = 0;
                cTituloIgv.Colspan = 5;
                cTituloIgv.HorizontalAlignment = 2; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloIgv); 

                PdfPCell cCeldaIgv = new PdfPCell(new Phrase(IGV, _standardFont));
                cCeldaIgv.Border = 0;
                cCeldaIgv.HorizontalAlignment = 2;
                TableValor.AddCell(cCeldaIgv);

                PdfPCell cTituloImporte = new PdfPCell(new Phrase("IMPORTE TOTAL S/", _standardFont));
                cTituloImporte.Border = 0;
                cTituloImporte.Colspan = 5;
                cTituloImporte.HorizontalAlignment = 2; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cTituloImporte);

                PdfPCell cCeldaTotalDoc = new PdfPCell(new Phrase(Total, _standardFont));
                cCeldaTotalDoc.Border = 0;
                cCeldaTotalDoc.HorizontalAlignment = 2; //0 = izquierda, 1 = centro, 2 = derecha
                TableValor.AddCell(cCeldaTotalDoc);

                PdfPCell cCeldaValor = new PdfPCell(TableValor);
                cCeldaValor.Colspan = 6;
                cCeldaValor.Border = 0;
                cCeldaValor.BorderWidthBottom = 1;
                cCeldaValor.PaddingBottom = 6;
                tblPrueba.AddCell(cCeldaValor);

                pdfDoc.Add(tblPrueba);

                // Close your PDF 
                pdfDoc.Close();

                Response.ContentType = "application/pdf";

                // Set default file Name as current datetime 
                Response.AddHeader("content-disposition", "attachment; filename=Documento_" + IdDoc + "_OC_" + serie + ".pdf");
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


        public ActionResult ImprimirOrden(int iComienzo, int iMedia, string fechaInicio, string fechafin, string numero)
        {
            Document pdfDoc = new Document(PageSize.A4, 10, 10, 20, 20);
            EGeneralJson<EOrdenCompraCab> Orden = BOrdenCompra.ListaOrden(iComienzo, iMedia, fechaInicio, fechafin, numero);

            if (Orden.Total > 0)
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

                PdfPCell celCospan = new PdfPCell(new Phrase("REPORTE DE VENTA POR CLIENTE"));
                celCospan.Colspan = 9;
                celCospan.Border = 0;
                celCospan.HorizontalAlignment = 1;
                Tableitems.AddCell(celCospan);


                PdfPCell cEspacio = new PdfPCell(new Phrase("              "));
                cEspacio.Colspan = 9;
                cEspacio.Border = 0;
                cEspacio.HorizontalAlignment = 1;
                Tableitems.AddCell(cEspacio);

                Single[] celdas2 = new Single[] { 0.5F, 2.0F, 1.5F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F, 1.0F };
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
                foreach (EOrdenCompraCab Compra in Orden.Datos)
                {
                    PdfPCell cItem = new PdfPCell(new Phrase(Compra.Item.ToString(), letrasDatosTabla));
                    cItem.Border = 0;
                    cItem.Padding = 2;
                    cItem.PaddingBottom = 2;
                    cItem.HorizontalAlignment = 1;
                    Tableitems.AddCell(cItem);

                    PdfPCell cSerie = new PdfPCell(new Phrase(Compra.Serie.ToString(), letrasDatosTabla));
                    cSerie.Border = 0;
                    cSerie.Padding = 2;
                    cSerie.PaddingBottom = 2;
                    cSerie.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSerie);

                    PdfPCell cFecha = new PdfPCell(new Phrase(Compra.FechaRegistro.ToString(), letrasDatosTabla));
                    cFecha.Border = 0;
                    cFecha.Padding = 2;
                    cFecha.PaddingBottom = 2;
                    cFecha.HorizontalAlignment = 1;
                    Tableitems.AddCell(cFecha);

                    PdfPCell cProveedor = new PdfPCell(new Phrase(Compra.proveed.Nombre.ToString(), letrasDatosTabla));
                    cProveedor.Border = 0;
                    cProveedor.Padding = 2;
                    cProveedor.PaddingBottom = 2;
                    cProveedor.HorizontalAlignment = 1;
                    Tableitems.AddCell(cProveedor);

                    PdfPCell cMoneda = new PdfPCell(new Phrase(Compra.Moneda.Nombre.ToString(), letrasDatosTabla));
                    cMoneda.Border = 0;
                    cMoneda.Padding = 2;
                    cMoneda.PaddingBottom = 2;
                    cMoneda.HorizontalAlignment = 1;
                    Tableitems.AddCell(cMoneda);

                    PdfPCell cCantidaddet = new PdfPCell(new Phrase(Compra.Cantidad.ToString(), letrasDatosTabla));
                    cCantidaddet.Border = 0;
                    cCantidaddet.Padding = 2;
                    cCantidaddet.PaddingBottom = 2;
                    cCantidaddet.HorizontalAlignment = 1;
                    Tableitems.AddCell(cCantidaddet);

                    PdfPCell cSubTotal = new PdfPCell(new Phrase(Compra.SutTotal.ToString(), letrasDatosTabla));
                    cSubTotal.Border = 0;
                    cSubTotal.Padding = 2;
                    cSubTotal.PaddingBottom = 2;
                    cSubTotal.HorizontalAlignment = 1;
                    Tableitems.AddCell(cSubTotal);

                    PdfPCell cIGV = new PdfPCell(new Phrase(Compra.IGv.ToString(), letrasDatosTabla));
                    cIGV.Border = 0;
                    cIGV.Padding = 2;
                    cIGV.PaddingBottom = 2;
                    cIGV.HorizontalAlignment = 1;
                    Tableitems.AddCell(cIGV);

                    PdfPCell cTotal = new PdfPCell(new Phrase(Compra.Total.ToString(), letrasDatosTabla));
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
                Response.AddHeader("content-disposition", "attachment; filename=ReporteCompra.pdf");
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

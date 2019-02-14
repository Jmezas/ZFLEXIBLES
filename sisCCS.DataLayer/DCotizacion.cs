using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sisCCS.EntityLayer;
using Factory;
using System.Data;
using System.Data.SqlClient;
namespace sisCCS.DataLayer
{
    public class DCotizacion : DBHelper
    {
        private static DCotizacion Instancia;
        private DataBase BaseDeDatos;

        public DCotizacion(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DCotizacion ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DCotizacion(BaseDeDatos);
            }
            return Instancia;
        }

        public string RegistrarCotizacion(ECotizacionCab cotizacionCab, List<ECotizacionDet> Detalle, string usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("SP_LOG_Cotizacion_Cab");
                    CreateHelper(Connection, tran);
                    AddInParameter("@iICotizacion", cotizacionCab.IdCotizacion);
                    AddInParameter("@iIdDocumento", cotizacionCab.Documento.Codigo);
                    AddInParameter("@iIdCliente", cotizacionCab.Cliente.IdCliente);
                    AddInParameter("@iIdMoneda", cotizacionCab.Moneda.idMoneda);
                    AddInParameter("@dFechaEmision", cotizacionCab.FechaEmision);
                    AddInParameter("@sSerie", cotizacionCab.Serie);
                    AddInParameter("@sNumero", cotizacionCab.Numero);
                    AddInParameter("@sMensaje", cotizacionCab.Mensaje);
                    AddInParameter("@fCantidadCab", cotizacionCab.Cantidad);
                    AddInParameter("@fSubTotal", cotizacionCab.SubTotal);
                    AddInParameter("@fIGVCab", cotizacionCab.IGV);
                    AddInParameter("@fTotalCab", cotizacionCab.Total);
                    AddInParameter("@Usuario", usuario);
                    AddInParameter("@sAsunto", cotizacionCab.Asunto);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIdCotizacion = int.Parse(vValues[0]);

                        string[] dMensaje;
                        foreach (ECotizacionDet oDetalle in Detalle)
                        {
                            oDetalle.CotizacionCab.IdCotizacion = iIdCotizacion;
                            oDetalle.Usuario.Usuario = usuario;
                            SetQuery("SP_LOG_Cotizacion_Det");
                            CreateHelper(Connection, tran);
                            AddInParameter("@iICotizacion", oDetalle.CotizacionCab.IdCotizacion);
                            AddInParameter("@iIdMat", oDetalle.Producto.IdMaterial);
                            AddInParameter("@fCantidad", oDetalle.Cantidad);
                            AddInParameter("@fprecio", oDetalle.Precio);
                            AddInParameter("@Usuario", oDetalle.Usuario.Usuario);
                            AddInParameter("@fImport", oDetalle.Importe);
                            AddInParameter("@FImportSnIGV", oDetalle.fImportSnIGV);
                            AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                            ExecuteQuery();
                            dMensaje = GetOutput("@Mensaje").ToString().Split('|');
                            if (!dMensaje[0].Equals("success"))
                            {
                                throw new Exception();
                            }
                        }
                    }
                    else
                    {
                        throw new Exception();
                    }
                    tran.Commit();
                    return sMensaje;
                }
                catch (Exception Exception)
                {
                    sMensaje = "error|" + Exception.Message;
                    return sMensaje;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public EGeneralJson<EReporteCotizacion> ListaReporte(int idCotizcion)
        {
            EGeneralJson<EReporteCotizacion> oLista = new EGeneralJson<EReporteCotizacion>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_LOG_ReportCotizacion");
                    CreateHelper(Connection);
                    AddInParameter("@iiCotizaicon", idCotizcion);
                    using (var Reader = ExecuteReader())
                    {
                        oLista.Datos = new List<EReporteCotizacion>();
                        while (Reader.Read())
                        {
                            EReporteCotizacion obj = new EReporteCotizacion();
                            obj.CotizacionCab.IdCotizacion = int.Parse(Reader["iIdCotizacion"].ToString());
                            obj.Documento.Descripcion = Reader["documento"].ToString();
                            obj.Cliente.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Cliente.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Cliente.Email = Reader["sEmail"].ToString();
                            obj.Moneda.Nombre = Reader["moneda"].ToString();
                            obj.Cliente.Direccion = Reader["sDireccion"].ToString();
                            obj.CotizacionCab.FechaEmision = Reader["fechaEmision"].ToString();
                            obj.CotizacionCab.Serie = Reader["NroDoc"].ToString();
                            obj.CotizacionCab.Mensaje = Reader["sMensaje"].ToString();
                            obj.CotizacionCab.Asunto = Reader["sAsunto"].ToString();
                            obj.CotizacionCab.Cantidad = Reader["fCantidadCab"].ToString();
                            obj.CotizacionCab.IGV = Reader["fIGVCab"].ToString();
                            obj.CotizacionCab.SubTotal = Reader["fSubtotalCab"].ToString();
                            obj.CotizacionCab.Total = Reader["fTotalCab"].ToString();
                            obj.Producto.IdMaterial = int.Parse(Reader["iIdMat"].ToString());
                            obj.Producto.Codigo = Reader["sCodigoMat"].ToString();
                            obj.Producto.NombreMat = Reader["sNomMat"].ToString();
                            obj.Producto.UM.IdMed = int.Parse(Reader["sUNIMat"].ToString());
                            obj.Producto.UM.MedNom = Reader["unidad"].ToString();
                            obj.CotizacionDet.Cantidad = Reader["fCantidad"].ToString();
                            obj.CotizacionDet.Precio = Reader["fprecio"].ToString();
                            obj.CotizacionDet.Importe = Reader["fImporte"].ToString();
                            obj.Item = int.Parse(Reader["item"].ToString());
                            oLista.Datos.Add(obj);
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
                return oLista;
            }
        }

        public EGeneralJson<ECotizacionCab> ListaReporte(int iComienzo, int iMedia, string Numero, string Doc, string Cliente, string TipoDocumento, string FechaInicio, string FechaFin)
        {
            EGeneralJson<ECotizacionCab> oLista = new EGeneralJson<ECotizacionCab>();
            using (var Connetion = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connetion.Open();
                    SetQuery("SP_LOG_ListaCotizacion");
                    CreateHelper(Connetion);
                    AddInParameter("@iComienzo", iComienzo);
                    AddInParameter("@iMedida", iMedia);
                    AddInParameter("@numero", Numero);
                    AddInParameter("@sNrumerodoc", Doc);
                    AddInParameter("@Cliente", Cliente);
                    AddInParameter("@tipoDocumento", TipoDocumento);
                    AddInParameter("@dFecDesde", FechaInicio);
                    AddInParameter("@dFecHasta", FechaFin);
                    using (var Reader = ExecuteReader())
                    {
                        oLista.Datos = new List<ECotizacionCab>();
                        while (Reader.Read())
                        {


                            ECotizacionCab obj = new ECotizacionCab();
                            obj.Item = int.Parse(Reader["item"].ToString());
                            obj.IdCotizacion = int.Parse(Reader["iIdCotizacion"].ToString());
                            obj.Documento.Descripcion = Reader["Documento"].ToString();
                            obj.Serie = Reader["NroDoc"].ToString();
                            obj.Documento.Codigo = int.Parse(Reader["ncodite"].ToString());
                            obj.FechaEmision = Reader["fechaEmision"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Cliente.Direccion = Reader["sDireccion"].ToString();
                            obj.Cantidad = Reader["fCantidadCab"].ToString();
                            obj.IGV = Reader["fIGVCab"].ToString();
                            obj.SubTotal = Reader["fSubtotalCab"].ToString();
                            obj.Total = Reader["fTotalCab"].ToString();
                            obj.Moneda.Nombre = Reader["moneda"].ToString();
                            obj.Mensaje = Reader["sMensaje"].ToString();
                            oLista.Datos.Add(obj);
                            oLista.Total = int.Parse(Reader["Total"].ToString());
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connetion.Close();
                }
                return oLista;
            }
        }

        public List<EObtenerCotizacion> ObtenerCotizacion(int IdCotizacion)
        {
            List<EObtenerCotizacion> oLista = new List<EObtenerCotizacion>();
            using (var Connection = GetConnection(BaseDeDatos))
            {       
                try
                {
                    Connection.Open();
                    SetQuery("SP_LOG_ReportCotizacion");
                    CreateHelper(Connection);
                    AddInParameter("@iiCotizaicon", IdCotizacion);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EObtenerCotizacion obj = new EObtenerCotizacion();
                            obj.Cotizacion.IdCotizacion = int.Parse(Reader["iIdCotizacion"].ToString());
                            obj.Documento.Descripcion = Reader["documento"].ToString();
                            obj.Cliente.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Cliente.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Cliente.Email = Reader["sEmail"].ToString();
                            obj.Moneda.Nombre = Reader["moneda"].ToString();
                            obj.Cliente.Direccion = Reader["sDireccion"].ToString();
                            obj.Cotizacion.FechaEmision = Reader["fechaEmision"].ToString();
                            obj.Cotizacion.Serie = Reader["NroDoc"].ToString();
                            obj.Cotizacion.Mensaje = Reader["sMensaje"].ToString();
                            obj.Cotizacion.Asunto = Reader["sAsunto"].ToString();
                            obj.Cotizacion.Cantidad = Reader["fCantidadCab"].ToString();
                            obj.Cotizacion.SubTotal = Reader["fIGVCab"].ToString();
                            obj.Cotizacion.IGV = Reader["fSubtotalCab"].ToString();
                            obj.Cotizacion.Total = Reader["fTotalCab"].ToString();
                            obj.Producto.IdMaterial = int.Parse(Reader["iIdMat"].ToString());
                            obj.Producto.Codigo = Reader["sCodigoMat"].ToString();
                            obj.Producto.NombreMat = Reader["sNomMat"].ToString();
                            obj.Producto.UM.IdMed = int.Parse(Reader["sUNIMat"].ToString());
                            obj.Producto.UM.MedNom = Reader["unidad"].ToString();
                            obj.Detalle.Cantidad = Reader["fCantidad"].ToString();
                            obj.Detalle.Precio = Reader["fprecio"].ToString();
                            obj.Detalle.Importe = Reader["fImporte"].ToString();
                            obj.Item = int.Parse(Reader["item"].ToString());
                            oLista.Add(obj);

                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
                return oLista;
            }
        }
    }
}

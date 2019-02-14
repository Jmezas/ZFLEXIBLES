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
    public class DOrdenCompra : DBHelper
    {
        private static DOrdenCompra Instancia;
        private DataBase BaseDeDatos;

        public DOrdenCompra(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DOrdenCompra ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DOrdenCompra(BaseDeDatos);
            }
            return Instancia;
        }

        public string RegistrarOrdenCompra(EOrdenCompraCab Orden, List<EOrdenCompraDet> Detale, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("SP_LOG_Orden_compra_Cab");
                    CreateHelper(Connection, tran);
                    AddInParameter("@iIdOrdenCompra", Orden.IdOrden);
                    AddInParameter("@iIdPorv", Orden.proveed.IdProveedor);
                    AddInParameter("@sSerieDoc", Orden.Serie);
                    AddInParameter("@sNroDoc", Orden.NumeroDoc);
                    AddInParameter("@fCantidad", Orden.Cantidad);
                    AddInParameter("@fSubTotal", Orden.SutTotal);
                    AddInParameter("@fIGVCab", Orden.IGv);
                    AddInParameter("@fTotalCab", Orden.Total);
                    AddInParameter("@Moneda", Orden.Moneda.idMoneda);
                    AddInParameter("@sUser", Usuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIdComprobantePago = int.Parse(vValues[0]);

                        string[] dMensaje;
                        foreach (EOrdenCompraDet oDetalle in Detale)
                        {
                            oDetalle.OrdenCompraCab.IdOrden = iIdComprobantePago;
                            oDetalle.Usuario.Usuario = Usuario;
                            SetQuery("SP_LOG_Orden_Compra_Det");
                            CreateHelper(Connection, tran);
                            AddInParameter("@iIdORdenCompra", oDetalle.OrdenCompraCab.IdOrden);
                            AddInParameter("@iIdMat", oDetalle.Producto.IdMaterial);
                            AddInParameter("@fCantidad", oDetalle.Cantidad);
                            AddInParameter("@fImport", oDetalle.Importe);
                            AddInParameter("@fprecio", oDetalle.Precio);
                            AddInParameter("@Usuario", oDetalle.Usuario.Usuario);
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
        public EGeneralJson<EOrdenCompraCab> ListaOrden(int iComienzo, int iMedia, string FechaInicio, string FechaFin, string Serie)
        {
            EGeneralJson<EOrdenCompraCab> Lista = new EGeneralJson<EOrdenCompraCab>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_LOG_ListaOrdenCompra");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", iComienzo);
                    AddInParameter("@iMedida", iMedia);
                    AddInParameter("@FechaInicio", FechaInicio);
                    AddInParameter("@FechaFin", FechaFin);
                    AddInParameter("@Serie", Serie);
                    using (var Reader = ExecuteReader())
                    {
                        Lista.Datos = new List<EOrdenCompraCab>();
                        while (Reader.Read())
                        {
                            EOrdenCompraCab Compra = new EOrdenCompraCab();
                            Compra.Item = int.Parse(Reader["item"].ToString());
                            Compra.IdOrden = int.Parse(Reader["iIdOrdenComCab"].ToString());
                            Compra.Serie = Reader["SerieNum"].ToString();
                            Compra.FechaRegistro = Reader["dFechaRegistro"].ToString();
                            Compra.proveed.Nombre = Reader["sRazonSocial"].ToString();
                            Compra.Cantidad = Reader["fCantidadTotalCab"].ToString();
                            Compra.SutTotal = Reader["fSubTotalCab"].ToString();
                            Compra.IGv = Reader["fIGVCab"].ToString();
                            Compra.Total = Reader["fTotalCab"].ToString();
                            Compra.Moneda.Nombre = Reader["Moneda"].ToString();
                            Lista.Datos.Add(Compra);
                            Lista.Total = int.Parse(Reader["Total"].ToString());
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
                return Lista;
            }
        }

        public EGeneralJson<EReporteCompraID> ReporteCompraIDs(int IdCompra)
        {
            EGeneralJson<EReporteCompraID> oDatos = new EGeneralJson<EReporteCompraID>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_LOG_ReportOC");
                    CreateHelper(Connection);
                    AddInParameter("@iIdOrdenCompra", IdCompra);
                    using (var Reader = ExecuteReader())
                    {
                        oDatos.Datos = new List<EReporteCompraID>();
                        while (Reader.Read())
                        {
                            EReporteCompraID Reporte = new EReporteCompraID();
                            Reporte.OrdenCompraCab.IdOrden = int.Parse(Reader["iIdOrdenComCab"].ToString());
                            Reporte.Proveedor.IdProveedor = int.Parse(Reader["iIdProv"].ToString());
                            Reporte.Proveedor.NroDocumento = Reader["sNroDoc"].ToString();
                            Reporte.Proveedor.Nombre = Reader["sRazonSocial"].ToString();
                            Reporte.Proveedor.Email = Reader["sEmail"].ToString();
                            Reporte.Proveedor.Telefono = Reader["sTelefono"].ToString();
                            Reporte.Proveedor.Ubigeo.UbicacionGeografica = Reader["cpcDescripcionUbicacionGeografica"].ToString();
                            Reporte.Proveedor.Direccion = Reader["sDireccion"].ToString();
                            Reporte.OrdenCompraCab.Serie = Reader["NumSerie"].ToString();
                            Reporte.OrdenCompraCab.FechaRegistro = Reader["dFechaRegistro"].ToString();
                            Reporte.OrdenCompraCab.Cantidad = Reader["fCantidadTotalCab"].ToString();
                            Reporte.OrdenCompraCab.SutTotal = Reader["fSubTotalCab"].ToString();
                            Reporte.OrdenCompraCab.IGv = Reader["fIGVCab"].ToString();
                            Reporte.OrdenCompraCab.Total = Reader["fTotalCab"].ToString();
                            Reporte.Item = int.Parse(Reader["item"].ToString());
                            Reporte.Producto.IdMaterial = int.Parse(Reader["iID_Material"].ToString());
                            Reporte.Producto.Codigo = Reader["sCodigoMat"].ToString();
                            Reporte.Producto.NombreMat = Reader["sNomMat"].ToString();
                            Reporte.CompraDetalle.Cantidad = Reader["fCantidad"].ToString();
                            Reporte.CompraDetalle.Precio = Reader["fPrecio"].ToString();
                            Reporte.CompraDetalle.Importe = Reader["fImport"].ToString();
                            Reporte.Moneda.Nombre = Reader["item"].ToString();
                            oDatos.Datos.Add(Reporte);

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
                return oDatos;
            }
        }
    }
}

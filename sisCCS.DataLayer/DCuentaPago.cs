using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using sisCCS.EntityLayer;
using Factory;
using System.Data.SqlClient;
using System.Data;
namespace sisCCS.DataLayer
{
    public class DCuentaPago : DBHelper
    {
        private static DCuentaPago Instancia;
        private DataBase BaseDeDatos;

        public DCuentaPago(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DCuentaPago ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DCuentaPago(BaseDeDatos);
            }
            return Instancia;
        }

        public EGeneralJson<ECuentaPago> ListaPago(int iComienzo, int iMedia, string Numero, string Cliente, int Tipodocumento, string FechaInicio, string FechaFin)
        {
            EGeneralJson<ECuentaPago> oLista = new EGeneralJson<ECuentaPago>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_ListaComprobantePago");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", iComienzo);
                    AddInParameter("@iMedida", iMedia);
                    AddInParameter("@numero", Numero);
                    AddInParameter("@Cliente", Cliente);
                    AddInParameter("@tipoDocumento", Tipodocumento);
                    AddInParameter("@dFecDesde", FechaInicio);
                    AddInParameter("@dFecHasta", FechaFin);
                    using (var Reader = ExecuteReader())
                    {
                        oLista.Datos = new List<ECuentaPago>();
                        while (Reader.Read())
                        {
                            ECuentaPago obj = new ECuentaPago();
                            obj.Item = int.Parse(Reader["item"].ToString());
                            obj.IdBandeja = int.Parse(Reader["iIdCuenta"].ToString());
                            obj.Comprobante.Id = int.Parse(Reader["iIdComprobante"].ToString());
                            obj.Comprobante.Nombre = Reader["Tipo"].ToString();
                            obj.Factura.IdVenta = int.Parse(Reader["iIdVenta"].ToString());
                            obj.Cliente.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Cliente.NroDocumento = Reader["Doc"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Moneda.idMoneda = int.Parse(Reader["sCodMoneda"].ToString());
                            obj.Moneda.Nombre = Reader["Moneda"].ToString();
                            obj.Factura.Serie = Reader["sSerieNumero"].ToString();
                            obj.Pago.FechaPago = Reader["dFechaEmision"].ToString();
                            obj.Factura.Total = Reader["ntotalCab"].ToString();
                            obj.Pagado = Reader["nPagado"].ToString();
                            obj.Pendiente = Reader["nPendiente"].ToString();
                            obj.EstadoFactura = Reader["Estado"].ToString();
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
                    Connection.Close();
                }
                return oLista;
            }
        }

        public EGeneralJson<EPago> ListaCuenta(int iComienzo, int iMedia, int iComporbante)
        {
            EGeneralJson<EPago> oLista = new EGeneralJson<EPago>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("ConsultarPago");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", iComienzo);
                    AddInParameter("@iMedida", iMedia);
                    AddInParameter("@iIdComprobante", iComporbante);
                    using (var Reader = ExecuteReader())
                    {
                        oLista.Datos = new List<EPago>();
                        while (Reader.Read())
                        {
                            EPago obj = new EPago();
                            obj.Item = int.Parse(Reader["item"].ToString());
                            obj.IdPago = int.Parse(Reader["iIdPago"].ToString());
                            obj.Hora = Reader["sHora"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Factura.Serie = Reader["sNroDocumento"].ToString();
                            obj.Monto = Reader["nMonto"].ToString();
                            obj.FechaPago = Reader["dFecha"].ToString();
                            obj.Usuario.Nombre = Reader["Nombre"].ToString();
                            obj.Factura.IdVenta = int.Parse(Reader["iIdVenta"].ToString());
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
                    Connection.Close();
                }
                return oLista;
            }
        }

        public ECuentaPago ObtenerCuenta(int idComprobante)
        {
            ECuentaPago oDatos = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_ObtenerIdComprobante");
                    CreateHelper(Connection);
                    AddInParameter("@iIdComprobante", idComprobante);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            oDatos = new ECuentaPago();
                            oDatos.IdBandeja = int.Parse(Reader["iIdCuenta"].ToString());
                            oDatos.Comprobante.Id = int.Parse(Reader["iIdComprobante"].ToString());
                            oDatos.Comprobante.Nombre = Reader["vte1gen"].ToString();
                            oDatos.Cliente.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            oDatos.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            oDatos.Cliente.Direccion = Reader["sDireccion"].ToString();
                            oDatos.Factura.Serie = Reader["sSerieNumero"].ToString();
                            oDatos.Pago.FechaPago = Reader["dFechaEmision"].ToString();
                            oDatos.Factura.Total = Reader["ntotalCab"].ToString();
                            oDatos.Pagado = Reader["nPagado"].ToString();
                            oDatos.Pendiente = Reader["nPendiente"].ToString();

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

        public string PagarDocumento(EPago Pago, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("FAC_PagarDocumento");
                    CreateHelper(Connection, tran);
                    AddInParameter("@iIdComprobante", Pago.IdPago);
                    AddInParameter("@nMontoPagar", Pago.Monto);
                    AddInParameter("@iIdCuenta", Pago.Factura.IdVenta);
                    AddInParameter("@sUsuReg", Usuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();

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

        public EPago ReportePago(int IdPago)
        {
            EPago oPago = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_ReportPagoID");
                    CreateHelper(Connection);
                    AddInParameter("@iiPago", IdPago);
                    using (var Reader = ExecuteReader())
                    {
                        while(Reader.Read()) {
                            oPago = new EPago();
                            oPago.IdPago = int.Parse(Reader["iIdPago"].ToString());
                            oPago.Hora = Reader["sHora"].ToString();
                            oPago.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            oPago.Factura.Serie = Reader["sNroDocumento"].ToString();
                            oPago.Monto = Reader["nMonto"].ToString();
                            oPago.FechaPago = Reader["dFecha"].ToString();
                            oPago.Usuario.Usuario = Reader["Nombre"].ToString();
                            oPago.Factura.IdVenta = int.Parse(Reader["iIdVenta"].ToString());
                            oPago.Comprobante.Nombre = Reader["Doc"].ToString();
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
                return oPago;
            }
        }
    }
}

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
    public class DFactura : DBHelper
    {

        private static DFactura Instancia;
        private DataBase BaseDeDatos;

        public DFactura(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DFactura ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DFactura(BaseDeDatos);
            }
            return Instancia;
        }

        public EFacturaCab ListaNroFac(int Tipo, int Comprobante)
        {
            EFacturaCab oDatos = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("FAC_VerSerieNumeroDoc");
                    CreateHelper(Connection);
                    AddInParameter("@iTipo", Tipo);
                    AddInParameter("@iIdComprobante", Comprobante);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            oDatos = new EFacturaCab();
                            oDatos.Serie = Reader["sSerie"].ToString();
                            oDatos.Numero = Reader["iNumero"].ToString();
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

        public List<EComprobanteFac> ListaDocFactura(int Doc)
        {
            List<EComprobanteFac> oDatos = new List<EComprobanteFac>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_ListaDoc");
                    CreateHelper(Connection);
                    AddInParameter("@Doc", Doc);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EComprobanteFac obj = new EComprobanteFac();
                            obj.Id = int.Parse(Reader["Codigo"].ToString());
                            obj.Nombre = Reader["descripcion"].ToString();
                            oDatos.Add(obj);
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
        public string VerificarStock(int IdProducto, string Cantidad)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_LOG_VerificarStock");
                    CreateHelper(Connection);
                    AddInParameter("@idProducto", IdProducto);
                    AddInParameter("@iCantidad", Cantidad);
                    AddOutParameter("@Mensaje", DbType.String);
                    ExecuteQuery();
                    return GetOutput("@Mensaje").ToString();
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public string RegistrarFactura(EFacturaCab facturaCab, List<EFacturaDet> Detalle, string Usuario)

        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("SP_FAC_FacturaCab");
                    CreateHelper(Connection, tran);
                    AddInParameter("@iIdVenta", facturaCab.IdVenta);
                    AddInParameter("@iIdCliente", facturaCab.Cliente.IdCliente);
                    AddInParameter("@iIdComprobante", facturaCab.Comprobante.Id);
                    AddInParameter("@sSerie", facturaCab.Serie);
                    AddInParameter("@iNumero", facturaCab.Numero);
                    AddInParameter("@sIdEstadoDoc", facturaCab.EstadoDocumento);
                    AddInParameter("@sCodMoneda", facturaCab.Moneda.idMoneda);
                    AddInParameter("@nCantTotalCab", facturaCab.Cantidad);
                    AddInParameter("@nSubtotalCab", facturaCab.SubTotal);
                    AddInParameter("@nIgvCab", facturaCab.IGV);
                    AddInParameter("@nTotalCab", facturaCab.Total);
                    AddInParameter("@sCodReg", Usuario);
                    AddInParameter("@sTipoSerieComprobante", facturaCab.TipoSerieComprobante);
                    AddInParameter("@sMensaje", facturaCab.Mensaje, AllowNull);
                    AddInParameter("@Observacion", facturaCab.Observacion, AllowNull);
                    AddInParameter("@sMotivo", facturaCab.Motivo, AllowNull);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIVenta = int.Parse(vValues[0]);

                        string[] dMensaje;
                        foreach (EFacturaDet oDetalle in Detalle)
                        {
                            oDetalle.FacturaCab.IdVenta = iIVenta;
                            oDetalle.Usuario.Usuario = Usuario;
                            SetQuery("SP_FAC_FacturaDet");
                            CreateHelper(Connection, tran);
                            AddInParameter("@iIdVenta", oDetalle.FacturaCab.IdVenta);
                            AddInParameter("@iIdMat", oDetalle.Producto.IdMaterial);
                            AddInParameter("@nCantidad", oDetalle.Cantidad);
                            AddInParameter("@nPrecio", oDetalle.Precio);
                            AddInParameter("@sCodReg", oDetalle.Usuario.Usuario);
                            AddInParameter("@fImport", oDetalle.Importe);
                            AddInParameter("@fImportSnIGV", oDetalle.fImportSnIGV);
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

                finally { Connection.Close(); }
            }
        }


        public EGeneralJson<EReporteFactura> ListaFactura(int idVenta)
        {
            EGeneralJson<EReporteFactura> oLista = new EGeneralJson<EReporteFactura>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_FAC_ReportFac");
                    CreateHelper(Connection);
                    AddInParameter("@IdVenta", idVenta);
                    using (var Reader = ExecuteReader())
                    {
                        oLista.Datos = new List<EReporteFactura>();
                        while (Reader.Read())
                        {
                            EReporteFactura obj = new EReporteFactura();
                            obj.FacturaCab.IdVenta = int.Parse(Reader["iIdVenta"].ToString());
                            obj.FacturaCab.Comprobante.Nombre = Reader["sNombreDocumento"].ToString();
                            obj.Cliente.Documento.Descripcion = Reader["Doc"].ToString();
                            obj.Cliente.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Cliente.Email = Reader["sEmail"].ToString();
                            obj.Cliente.Direccion = Reader["sDireccion"].ToString();
                            obj.Cliente.Telefono = Reader["sTelefono"].ToString();
                            obj.Cliente.Celular = Reader["sCelular"].ToString();
                            obj.Comprobante.Id = int.Parse(Reader["iId_TipoDocumento"].ToString());
                            obj.Comprobante.Nombre = Reader["sNombreDocumento"].ToString();
                            obj.FacturaCab.Serie = Reader["SerNumero"].ToString();
                            obj.FacturaCab.Mensaje = Reader["sMensaje"].ToString();
                            obj.FacturaCab.Motivo = Reader["sMotivo"].ToString();
                            obj.FacturaCab.FechaEmisio = Reader["dFecEmision"].ToString();
                            obj.FacturaCab.EstadoDocumento = Reader["EstadoDoc"].ToString();
                            obj.Moneda.Nombre = Reader["Moneda"].ToString();
                            obj.FacturaCab.Cantidad = Reader["nCantTotalCab"].ToString();
                            obj.FacturaCab.SubTotal = Reader["nSubtotalCab"].ToString();
                            obj.FacturaCab.IGV = Reader["nIgvCab"].ToString();
                            obj.FacturaCab.Total = Reader["nTotalCab"].ToString();
                            obj.Producto.IdMaterial = int.Parse(Reader["iIdProducto"].ToString());
                            obj.Producto.Codigo = Reader["sCodigoMat"].ToString();
                            obj.Producto.NombreMat = Reader["sNomMat"].ToString();
                            obj.Producto.UM.MedNom = Reader["unidad"].ToString();
                            obj.Detalle.Cantidad = Reader["nCantidad"].ToString();
                            obj.Detalle.Precio = Reader["nPrecio"].ToString();
                            obj.Detalle.Importe = Reader["fImport"].ToString();
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
        public EGeneralJson<EFacturaCab> ListaFacBol(int iComienzo, int iMedio, string Numero, string Cliente, int TipoDocumento, string FechaInicio, string FechaFin)
        {
            EGeneralJson<EFacturaCab> oLista = new EGeneralJson<EFacturaCab>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_FAC_ReporteFacBol");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", iComienzo);
                    AddInParameter("@iMedida", iMedio);
                    AddInParameter("@numero", Numero);
                    AddInParameter("@Cliente", Cliente);
                    AddInParameter("@tipoDocumento", TipoDocumento);
                    AddInParameter("@dFecDesde", FechaInicio);
                    AddInParameter("@dFecHasta", FechaFin);
                    using (var Reader = ExecuteReader())
                    {
                        oLista.Datos = new List<EFacturaCab>();
                        while (Reader.Read())
                        {
                            EFacturaCab obj = new EFacturaCab();
                            obj.Item = int.Parse(Reader["item"].ToString());
                            obj.IdVenta = int.Parse(Reader["iIdVenta"].ToString());
                            obj.Comprobante.Nombre = Reader["sNombreDocumento"].ToString();
                            obj.Serie = Reader["SerNumero"].ToString();
                            obj.FechaEmisio = Reader["dFecEmision"].ToString();
                            obj.Cliente.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Cliente.Telefono = Reader["Telefono"].ToString();
                            obj.Moneda.Nombre = Reader["Moneda"].ToString();
                            obj.Cantidad = Reader["nCantTotalCab"].ToString();
                            obj.IGV = Reader["nIgvCab"].ToString();
                            obj.SubTotal = Reader["nSubtotalCab"].ToString();
                            obj.Total = Reader["nTotalCab"].ToString();
                            obj.EstadoDocumento = Reader["EstadoDoc"].ToString();
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
    }
}

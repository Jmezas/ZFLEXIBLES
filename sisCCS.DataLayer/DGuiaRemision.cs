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
    public class DGuiaRemision : DBHelper
    {

        private static DGuiaRemision Instancia;
        private DataBase BaseDeDatos;

        public DGuiaRemision(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DGuiaRemision ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DGuiaRemision(BaseDeDatos);
            }
            return Instancia;
        }

        public string RegistrarGia(EGuiaCab Gia, List<EGuiadet> Detalle, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                string sMensaje = "";
                try
                {
                    Connection.Open();
                    SqlTransaction tran = (SqlTransaction)Connection.BeginTransaction();
                    SetQuery("SP_LOG_GuiaRemisionCab");
                    CreateHelper(Connection, tran);
                    AddInParameter("@iId_Guia", Gia.IdGuia);
                    AddInParameter("@sSerie", Gia.Serie);
                    AddInParameter("@iNumero", Gia.Numero);
                    AddInParameter("@sPuntoLlegada", Gia.PuntoLlegada);
                    AddInParameter("@sUbigeoDestino", Gia.Ubigeo.IdUbigeo);
                    AddInParameter("@sMarcaTransporte", Gia.Marca, AllowNull);
                    AddInParameter("@sPlacaTransporte", Gia.Placa, AllowNull);
                    AddInParameter("@sLicencia", Gia.Licencia, AllowNull);
                    AddInParameter("@sNomEmpresa", Gia.NombreEmpresa, AllowNull);
                    AddInParameter("@sRucEmpresa", Gia.RucEmpresa, AllowNull);
                    AddInParameter("@dFechaEmision", Gia.FechaEmision);
                    AddInParameter("@dFechaOperacion", Gia.FechaOperacion);
                    AddInParameter("@iIdComprobante", Gia.Comprobante.IdCotizacion);
                    AddInParameter("@iIdCliente", Gia.Cliente.IdCliente);
                    AddInParameter("@sMotivoTraslado", Gia.Motivo);
                    AddInParameter("@scodReg", Usuario);
                    AddInParameter("@Asunto", Gia.Asunto);
                    AddInParameter("@sMensaje", Gia.Mensaje);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    sMensaje = GetOutput("@Mensaje").ToString();
                    string[] vMensaje = sMensaje.Split('|');
                    if (vMensaje[0].Equals("success"))
                    {
                        string[] vValues = vMensaje[2].Split('&');
                        int iIVenta = int.Parse(vValues[0]);

                        string[] dMensaje;
                        foreach (EGuiadet oDetalle in Detalle)
                        {
                            oDetalle.EGuia.IdGuia = iIVenta;
                            oDetalle.Usuario.Usuario = Usuario;
                            SetQuery("SP_LOG_GuiaRemisionDet");
                            CreateHelper(Connection, tran);
                            AddInParameter("@iId_Guia", oDetalle.EGuia.IdGuia);
                            AddInParameter("@iIdMaterial", oDetalle.Producto.IdMaterial);
                            AddInParameter("@nCantidad", oDetalle.Cantidad);
                            AddInParameter("@nPrecio", oDetalle.Precio);
                            AddInParameter("@scodReg", oDetalle.Usuario.Usuario);
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

        public EGeneralJson<EGuiaCab> ListaGia(int iComienzo, int iMedia, string Numero, string sNumeroDocumento, string Cliente, int TipoDocumento, string FechaInicio, string FechaFin)
        {
            EGeneralJson<EGuiaCab> oLista = new EGeneralJson<EGuiaCab>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_LOG_ListaGuia");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", iComienzo);
                    AddInParameter("@iMedida", iMedia);
                    AddInParameter("@numero", Numero);
                    AddInParameter("@sNrumerodoc", sNumeroDocumento);
                    AddInParameter("@Cliente", Cliente);
                    AddInParameter("@tipoDocumento", TipoDocumento);
                    AddInParameter("@dFecDesde", FechaInicio);
                    AddInParameter("@dFecHasta", FechaFin);
                    using (var Reader = ExecuteReader())
                    {
                        oLista.Datos = new List<EGuiaCab>();
                        while (Reader.Read())
                        {
                            EGuiaCab obj = new EGuiaCab();
                            obj.Item = int.Parse(Reader["item"].ToString());
                            obj.IdGuia = int.Parse(Reader["iid_guia"].ToString());
                            obj.Serie = Reader["sNumero"].ToString();
                            obj.FechaEmision = Reader["dFechaEmision"].ToString();
                            obj.Comprobante.IdCotizacion = int.Parse(Reader["iIdCotizacion"].ToString());
                            obj.Comprobante.Serie = Reader["NroCot"].ToString();
                            obj.Cliente.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Cliente.NroDocumento = Reader["Doc"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Motivo = Reader["Motivo"].ToString();
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

        public EGeneralJson<EGuiaCab> ReporteGuia(int IdGuia)
        {
            EGeneralJson<EGuiaCab> oLista = new EGeneralJson<EGuiaCab>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_GUIA_Reporte");
                    CreateHelper(Connection);
                    AddInParameter("@codigo", IdGuia);
                    using (var Reader = ExecuteReader())
                    {
                        oLista.Datos = new List<EGuiaCab>();
                        while (Reader.Read())
                        {
                            EGuiaCab obj = new EGuiaCab();
                            obj.Item = int.Parse(Reader["item"].ToString());
                            obj.IdGuia = int.Parse(Reader["iId_Guia"].ToString());
                            obj.Numero = Reader["Numero"].ToString();
                            obj.PuntoLlegada = Reader["sPuntoLlegada"].ToString();
                            obj.Ubigeo.UbicacionGeografica = Reader["ubigeo"].ToString();
                            obj.Marca = Reader["Marca"].ToString();
                            obj.Placa = Reader["Placa"].ToString();
                            obj.Licencia = Reader["Licencia"].ToString();
                            obj.NombreEmpresa = Reader["NombreEmpresa"].ToString();
                            obj.RucEmpresa = Reader["RucEmpresa"].ToString();
                            obj.FechaEmision = Reader["dFechaEmision"].ToString();
                            obj.FechaOperacion = Reader["dFechaOperacion"].ToString();
                            obj.Comprobante.IdCotizacion = int.Parse(Reader["iIdComprobante"].ToString());
                            obj.Comprobante.Serie = Reader["NroComprobante"].ToString();
                            obj.Cliente.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Cliente.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Cliente.Telefono = Reader["Telefono"].ToString();
                            obj.Cliente.Direccion = Reader["sDireccion"].ToString();
                            obj.Cliente.Email = Reader["sEmail"].ToString();      
                            obj.Motivo = Reader["Motivo"].ToString();
                            obj.Cantidad = Reader["nCantidad"].ToString();
                            obj.Precio = Reader["nPrecio"].ToString();
                            obj.Producto.IdMaterial = int.Parse(Reader["iIdMaterial"].ToString());
                            obj.Producto.Codigo = Reader["sCodigoMat"].ToString();
                            obj.Producto.NombreMat = Reader["sNomMat"].ToString();
                            obj.Producto.UM.MedNom = Reader["vte1gen"].ToString();
                            obj.Asunto = Reader["sAsuntoEnviar"].ToString();
                            obj.Mensaje = Reader["sMensajeEnviar"].ToString();
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
    }
}

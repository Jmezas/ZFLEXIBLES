using System;
using System.Collections.Generic;
using sisCCS.EntityLayer;
using Factory;
using System.Data;
using System.Xml.Schema;

namespace sisCCS.DataLayer
{
    public class DGeneral : DBHelper
    {
        private static DGeneral Instancia;
        private DataBase BaseDeDatos;

        public DGeneral(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DGeneral ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DGeneral(BaseDeDatos);
            }
            return Instancia;
        }




        public List<EUnidadMedida> ListarUnidad()
        {
            List<EUnidadMedida> lMonedas = new List<EUnidadMedida>();
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_ListaUnidadMedida");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EUnidadMedida oTipoMoneda = new EUnidadMedida();
                            oTipoMoneda.IdMed = int.Parse(Reader["Id"].ToString());
                            oTipoMoneda.MedNom = Reader["Nombre"].ToString();

                            lMonedas.Add(oTipoMoneda);
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
            }
            return lMonedas;
        }
        public List<ETipoDocumentoIdentidad> ListaDoc()
        {
            List<ETipoDocumentoIdentidad> Doc = new List<ETipoDocumentoIdentidad>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_ListaDocumento");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ETipoDocumentoIdentidad Obj = new ETipoDocumentoIdentidad();
                            Obj.Codigo = int.Parse(Reader["codigo"].ToString());
                            Obj.Descripcion = Reader["descripcion"].ToString();
                            Doc.Add(Obj);
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
                return Doc;
            }
        }

        public List<EUbigeo> ListarUbigeo(string vTipo, string vDpto, string vProv)
        {
            List<EUbigeo> lUbigeo = new List<EUbigeo>();
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_Ubigeo");
                    CreateHelper(Connection);
                    AddInParameter("@vTipo", vTipo);
                    AddInParameter("@vDpto", vDpto);
                    AddInParameter("@vProv", vProv);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EUbigeo oUbigeo = new EUbigeo();
                            oUbigeo.Id = int.Parse(Reader["Id"].ToString());
                            oUbigeo.Nombre = Reader["Nombre"].ToString();
                            oUbigeo.CodigoInei = Reader["Cod_Inei"].ToString();
                            oUbigeo.CodigoDepartamento = Reader["Cod_Departamento"].ToString();
                            oUbigeo.CodigoProvincia = Reader["Cod_Provincia"].ToString();
                            oUbigeo.CodigoDistrito = Reader["Cod_Distrito"].ToString();
                            lUbigeo.Add(oUbigeo);
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
            }
            return lUbigeo;
        }
        public EGeneral ListarSerieDoc()
        {
            EGeneral obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_GetSerieNumero_Orden_Compra");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new EGeneral();
                            obj.Text = Reader["numero"].ToString();
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
                return obj;
            }
        }
        public List<ETipoMoneda> ListaMoneda()
        {
            List<ETipoMoneda> oDatos = new List<ETipoMoneda>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_ListaModenda");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ETipoMoneda Moneda = new ETipoMoneda();
                            Moneda.idMoneda = int.Parse(Reader["id"].ToString());
                            Moneda.Nombre = Reader["descripcion"].ToString();
                            oDatos.Add(Moneda);
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

        public List<EEmpresa> ListaEmpresa()
        {
            List<EEmpresa> Lista = new List<EEmpresa>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_sel_ListEmpresa");
                    CreateHelperText(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EEmpresa obj = new EEmpresa();
                            obj.Id = int.Parse(Reader["cpnID_Empresa"].ToString());
                            obj.RUC = Reader["cpcRUC"].ToString();
                            obj.RazonSocial = Reader["cpcRazonSocialEmpresa"].ToString();
                            obj.Ubigeo = Reader["Ubigeo"].ToString();
                            obj.Direccion = Reader["cpcdirecion"].ToString();
                            obj.Fecha = Reader["cpdFechaHoraReg"].ToString();
                            obj.Correo = Reader["sCorreoEmp"].ToString();
                            obj.Contrasenia = Reader["sContrasenia"].ToString();
                            obj.Telefono = Reader["sTelefono"].ToString();
                            obj.Celular = Reader["sCelular"].ToString();
                            Lista.Add(obj);
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
        public EOrdenCompraCab ListaDocCot()
        {
            EOrdenCompraCab Lista = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_GetSerieNumero_Cotizacion");
                    CreateHelperText(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            Lista = new EOrdenCompraCab();
                            Lista.Serie = Reader["numero"].ToString();

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

        public List<ECliente> ListaClienteDNI(string Filtro, string Filtro2)
        {
            List<ECliente> oLista = new List<ECliente>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_Sel_ClientesPorFiltroDNI");
                    CreateHelper(Connection);
                    AddInParameter("vFiltro", Filtro);
                    AddInParameter("vFiltro1", Filtro2);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ECliente obj = new ECliente();
                            obj.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Documento.Codigo = int.Parse(Reader["iIdTipoDoc"].ToString());
                            obj.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Direccion = Reader["sDireccion"].ToString();
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
        public List<ECliente> ListaClienteRuc(string Filtro, string Filtro2)
        {
            List<ECliente> oLista = new List<ECliente>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_Sel_ClientesPorFiltro");
                    CreateHelper(Connection);
                    AddInParameter("@vFiltro", Filtro);
                    AddInParameter("@vFiltro1", Filtro2);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ECliente obj = new ECliente();
                            obj.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Documento.Codigo = int.Parse(Reader["iIdTipoDoc"].ToString());
                            obj.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Direccion = Reader["sDireccion"].ToString();
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

        public List<EComprobanteFac> ListaDocFactura()
        {
            List<EComprobanteFac> oDatos = new List<EComprobanteFac>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_ListaValFac");
                    CreateHelper(Connection);
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

        public List<EComprobanteFac> ListaFacBol(int IdDoc)
        {
            List<EComprobanteFac> oDatos = new List<EComprobanteFac>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_ListarDocFacBol");
                    CreateHelper(Connection);
                    AddInParameter("@IdDoc", IdDoc);
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
    }
}

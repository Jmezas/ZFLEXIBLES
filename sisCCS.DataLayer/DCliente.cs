using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sisCCS.EntityLayer;
using Factory;
using System.Data;
namespace sisCCS.DataLayer
{
    public class DCliente : DBHelper
    {
        private static DCliente Instancia;
        private DataBase BaseDeDatos;

        public DCliente(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DCliente ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DCliente(BaseDeDatos);
            }
            return Instancia;
        }

        public List<ECliente> ListaCliente()
        {
            List<ECliente> oDatos = new List<ECliente>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_listaCliente");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            ECliente Cliente = new ECliente();
                            Cliente.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            Cliente.Documento.Descripcion = Reader["Documento"].ToString();
                            Cliente.NroDocumento = Reader["sNroDoc"].ToString();
                            Cliente.Nombre = Reader["sRazonSocial"].ToString();
                            Cliente.Telefono = Reader["sTelefono"].ToString();
                            Cliente.Celular = Reader["sCelular"].ToString();
                            Cliente.Email = Reader["sEmail"].ToString();
                            Cliente.Direccion = Reader["sDireccion"].ToString();
                            oDatos.Add(Cliente);
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
        public ECliente ListaEdit(int ID)
        {
            ECliente obj = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try{
                    Connection.Open();
                    SetQuery("SP_ListaEdit");
                    CreateHelper(Connection);
                    AddInParameter("@iidCliente", ID);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            obj = new ECliente();
                            obj.IdCliente = int.Parse(Reader["iIdCliente"].ToString());
                            obj.Documento.Codigo = int.Parse(Reader["iIdTipoDoc"].ToString());
                            obj.NroDocumento = Reader["sNroDoc"].ToString();
                            obj.Nombre = Reader["sRazonSocial"].ToString();
                            obj.Telefono = Reader["sTelefono"].ToString();
                            obj.Celular = Reader["sCelular"].ToString();
                            obj.Email = Reader["sEmail"].ToString();
                            obj.Direccion = Reader["sDireccion"].ToString();
                            obj.Estado = Reader["sEstado"].ToString();
                            
                        }
                    }
                }
                catch(Exception Exception)
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
        public string Insertar_Update(ECliente Cliente, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_Ins_Cliente");
                    CreateHelper(Connection);
                    AddInParameter("@idCliente", Cliente.IdCliente);
                    AddInParameter("@idDocumento", Cliente.Documento.Codigo);
                    AddInParameter("@NumeroDoc", Cliente.NroDocumento);
                    AddInParameter("@Razonsocial", Cliente.Nombre);
                    AddInParameter("@telefono", Cliente.Telefono,AllowNull);
                    AddInParameter("@Celular", Cliente.Celular, AllowNull);
                    AddInParameter("@Email", Cliente.Email);
                    AddInParameter("@Direccion", Cliente.Direccion);
                    AddInParameter("@usuario", Usuario);
                    AddInParameter("@Estado", Cliente.Estado);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    var smensaje = GetOutput("@Mensaje").ToString();
                    return GetOutput("@Mensaje").ToString();
                }
                catch(Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }
    }
}

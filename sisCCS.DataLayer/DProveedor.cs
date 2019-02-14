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
    public class DProveedor : DBHelper
    {
        private static DProveedor Instancia;
        private DataBase BaseDeDatos;

        public DProveedor(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DProveedor ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DProveedor(BaseDeDatos);
            }
            return Instancia;
        }
        public List<EProveedor> ListaProveedor()
        {
            List<EProveedor> oDatos = new List<EProveedor>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_listaProveedor");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EProveedor obj = new EProveedor();
                            obj.IdProveedor = int.Parse(Reader["id"].ToString());
                            obj.Nombre = Reader["descripcion"].ToString();
                            oDatos.Add(obj);
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                return oDatos;
            }
        }
    }
}

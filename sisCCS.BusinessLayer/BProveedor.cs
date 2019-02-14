using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using sisCCS.EntityLayer;
using sisCCS.DataLayer;
using Factory;
using System.Data;
namespace sisCCS.BusinessLayer
{
    public class BProveedor
    {
        private static BProveedor Instancia;
        private DProveedor Data = DProveedor.ObtenerInstancia(DataBase.SqlServer);

        public static BProveedor ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BProveedor();
            }
            return Instancia;
        }
        public List<EProveedor> ListaProveedor()
        {
            try
            {
                return Data.ListaProveedor();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}

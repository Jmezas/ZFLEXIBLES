using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Factory;
using sisCCS.DataLayer;
using sisCCS.EntityLayer;
namespace sisCCS.BusinessLayer
{
    public class BProducto
    {
        private static BProducto Instancia;
        private DProducto Data = DProducto.ObtenerInstancia(DataBase.SqlServer);

        public static BProducto ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BProducto();
            }
            return Instancia;
        }

        public List<EProducto> ListaProducto()
        {
            try
            {
                return Data.ListaProducto();
            }catch(Exception Exception)
            {
                throw Exception;
            }
        }
        public EProducto ObtenerProducto(int ID)
        {
            try
            {
                return Data.ObtenerProducto(ID);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string Registrar_Update(EProducto Producto, string Usuario)
        {
            try
            {
                return Data.Registrar_Update(Producto, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EProducto Producto()
        {
            try
            {
                return Data.Producto();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}

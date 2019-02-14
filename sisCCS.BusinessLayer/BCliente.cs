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
   public class BCliente
    {
        private static BCliente Instancia;
        private DCliente Data = DCliente.ObtenerInstancia(DataBase.SqlServer);

        public static BCliente ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BCliente();
            }
            return Instancia;
        }
        public List<ECliente> ListaCliente()
        {
            try
            {
                return Data.ListaCliente();
            }catch(Exception Exception)
            {
                throw Exception;
            }
        }
        public ECliente ListaEdit(int ID)
        {
            try
            {
                return Data.ListaEdit(ID);
            }catch(Exception ex)
            {
                throw ex;
            }
        }
        public string Insertar_Update(ECliente Cliente, string Usuario)
        {
            try
            {
                return Data.Insertar_Update(Cliente, Usuario);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }

}

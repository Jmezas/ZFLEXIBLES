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
    public class BOrdenCompra
    {
        private static BOrdenCompra Instancia;
        private DOrdenCompra Data = DOrdenCompra.ObtenerInstancia(DataBase.SqlServer);

        public static BOrdenCompra ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BOrdenCompra();
            }
            return Instancia;
        }
        public string RegistrarOrdenCompra(EOrdenCompraCab Orden, List<EOrdenCompraDet> Detale, string Usuario)
        {
            try
            {
                return Data.RegistrarOrdenCompra(Orden, Detale, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<EReporteCompraID> ReporteCompraIDs(int IdCompra)
        {
            try
            {
                return Data.ReporteCompraIDs(IdCompra);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<EOrdenCompraCab> ListaOrden(int iComienzo, int iMedia, string FechaInicio, string FechaFin, string Serie)
        {
            try
            {
                return Data.ListaOrden(iComienzo, iMedia, FechaInicio, FechaFin, Serie);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}

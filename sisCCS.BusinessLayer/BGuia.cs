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
    public class BGuia
    {
        private static BGuia Instancia;
        private DGuiaRemision Data = DGuiaRemision.ObtenerInstancia(DataBase.SqlServer);

        public static BGuia ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BGuia();
            }
            return Instancia;
        }

        public string RegistrarGia(EGuiaCab Gia, List<EGuiadet> Detalle, string Usuario)
        {
            try
            {
                return Data.RegistrarGia(Gia, Detalle, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<EGuiaCab> ListaGia(int iComienzo, int iMedia, string Numero, string sNumeroDocumento, string Cliente, int TipoDocumento, string FechaInicio, string FechaFin)
        {
            try
            {
                return Data.ListaGia(iComienzo, iMedia, Numero, sNumeroDocumento, Cliente, TipoDocumento, FechaInicio, FechaFin);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<EGuiaCab> ReporteGuia(int IdGuia)
        {
            try
            {
                return Data.ReporteGuia(IdGuia);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

    }
}

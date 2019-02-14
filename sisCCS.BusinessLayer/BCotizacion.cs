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
    public class BCotizacion
    {
        private static BCotizacion Instancia;
        private DCotizacion Data = DCotizacion.ObtenerInstancia(DataBase.SqlServer);

        public static BCotizacion ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BCotizacion();
            }
            return Instancia;
        }
        public string RegistrarCotizacion(ECotizacionCab cotizacionCab, List<ECotizacionDet> Detalle, string usuario)
        {
            try
            {
                return Data.RegistrarCotizacion(cotizacionCab, Detalle, usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<EReporteCotizacion> ListaReporte(int idCotizcion)
        {
            try
            {
                return Data.ListaReporte(idCotizcion);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<ECotizacionCab> ListaReporte(int iComienzo, int iMedia, string Numero, string Doc, string Cliente, string TipoDocumento, string FechaInicio, string FechaFin)
        {
            try
            {
                return Data.ListaReporte(iComienzo, iMedia, Numero, Doc, Cliente, TipoDocumento, FechaInicio, FechaFin);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EObtenerCotizacion> ObtenerCotizacion(int IdCotizacion)
        {
            try
            {
                return Data.ObtenerCotizacion(IdCotizacion);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}

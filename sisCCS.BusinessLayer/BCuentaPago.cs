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
    public class BCuentaPago
    {
        private static BCuentaPago Instancia;
        private DCuentaPago Data = DCuentaPago.ObtenerInstancia(DataBase.SqlServer);

        public static BCuentaPago ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BCuentaPago();
            }
            return Instancia;
        }
        public EGeneralJson<ECuentaPago> ListaPago(int iComienzo, int iMedia, string Numero, string Cliente, int Tipodocumento, string FechaInicio, string FechaFin)
        {
            try
            {
                return Data.ListaPago(iComienzo, iMedia, Numero, Cliente, Tipodocumento, FechaInicio, FechaFin);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<EPago> ListaCuenta(int iComienzo, int iMedia, int iComporbante)
        {
            try
            {
                return Data.ListaCuenta(iComienzo, iMedia, iComporbante);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public ECuentaPago ObtenerCuenta(int idComprobante)
        {
            try
            {
                return Data.ObtenerCuenta(idComprobante);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string PagarDocumento(EPago Pago, string Usuario)
        {
            try
            {
                return Data.PagarDocumento(Pago, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EPago ReportePago(int IdPago)
        {
            try
            {
                return Data.ReportePago(IdPago);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}

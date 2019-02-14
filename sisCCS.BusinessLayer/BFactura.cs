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
    public class BFactura
    {
        private static BFactura Instancia;
        private DFactura Data = DFactura.ObtenerInstancia(DataBase.SqlServer);

        public static BFactura ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BFactura();
            }
            return Instancia;
        }

        public EFacturaCab ListaNroFac(int Tipo, int Comprobante)
        {
            try
            {
                return Data.ListaNroFac(Tipo, Comprobante);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EComprobanteFac> ListaDocFactura(int Doc)
        {
            try
            {
                return Data.ListaDocFactura(Doc);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string VerificarStock(int IdProducto, string Cantidad)
        {
            try
            {
                return Data.VerificarStock(IdProducto, Cantidad);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string RegistrarFactura(EFacturaCab facturaCab, List<EFacturaDet> Detalle, string Usuario)
        {
            try
            {
                return Data.RegistrarFactura(facturaCab, Detalle, Usuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<EReporteFactura> ListaFactura(int idVenta)
        {
            try
            {
                return Data.ListaFactura(idVenta);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneralJson<EFacturaCab> ListaFacBol(int iComienzo, int iMedio, string Numero, string Cliente, int TipoDocumento, string FechaInicio, string FechaFin)
        {
            try
            {
                return Data.ListaFacBol(iComienzo, iMedio, Numero, Cliente, TipoDocumento, FechaInicio, FechaFin);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}

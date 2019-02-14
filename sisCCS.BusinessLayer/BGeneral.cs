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
    public class BGeneral
    {
        private static BGeneral Instancia;
        private DGeneral Data = DGeneral.ObtenerInstancia(DataBase.SqlServer);

        public static BGeneral ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BGeneral();
            }
            return Instancia;
        }
        public List<EUnidadMedida> ListarUnidad()
        {
            try
            {
                return Data.ListarUnidad();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ETipoDocumentoIdentidad> ListaDoc()
        {
            try
            {
                return Data.ListaDoc();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EGeneral ListarSerieDoc()
        {
            try
            {
                return Data.ListarSerieDoc();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ETipoMoneda> ListaMoneda()
        {
            try
            {
                return Data.ListaMoneda();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EEmpresa> ListaEmpresa()
        {
            try
            {
                return Data.ListaEmpresa();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public EOrdenCompraCab ListaDocCot()
        {
            try
            {
                return Data.ListaDocCot();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ECliente> ListaClienteDNI(string Filtro, string Filtro2)
        {

            try
            {
                return Data.ListaClienteDNI(Filtro, Filtro2);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<ECliente> ListaClienteRuc(string Filtro, string Filtro2)
        {
            try
            {
                return Data.ListaClienteRuc(Filtro, Filtro2);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EComprobanteFac> ListaDocFactura()
        {
            try
            {
                return Data.ListaDocFactura();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EUbigeo> ListarUbigeo(string vTipo, string vDpto, string vProv)
        {
            try
            {
                return Data.ListarUbigeo(vTipo, vDpto, vProv);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public List<EComprobanteFac> ListaFacBol(int IdDoc)
        {
            try
            {
                return Data.ListaFacBol(IdDoc);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
    }
}

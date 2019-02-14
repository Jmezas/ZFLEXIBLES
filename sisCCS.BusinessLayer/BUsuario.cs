using Factory;
using sisCCS.DataLayer;
using sisCCS.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.BusinessLayer
{
    public class BUsuario
    {
        private static BUsuario Instancia;
        private DUsuario Data = DUsuario.ObtenerInstancia(DataBase.SqlServer);

        public static BUsuario ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BUsuario();
            }
            return Instancia;
        }

        public string Registrar(EUsuario oUsuario)
        {
            try
            {
                return Data.Registrar(oUsuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

     
        public string Actualizar(EUsuario oUsuario)
        {
            try
            {
                return Data.Actualizar(oUsuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public string EliminarAsignacion(int iIdAsignacion)
        {
            try
            {
                return Data.EliminarAsignacion(iIdAsignacion);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public EUsuario Login(string sUsuario, string sClave)
        {
            try
            {
                return Data.Login(sUsuario, sClave);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public string Login(int iIdSucursal, int iIdLineaNegocio, DateTime dFechaOperacion, int iIdTurno, string sUsuario, string sPassword)
        {
            try
            {
                return Data.Login(iIdSucursal, iIdLineaNegocio, dFechaOperacion, iIdTurno, sUsuario, sPassword);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public EUsuario BuscarPorUsuario(string sUsuario)
        {
            try
            {
                return Data.BuscarPorUsuario(sUsuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EUsuario> ListarVendedores(DateTime dFechaOperacion, int iIdturno, int iIdUsuario)
        {
            try
            {
                return Data.ListarVendedores(dFechaOperacion, iIdturno, iIdUsuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EMenu> ListarMenuPorUsuario(string sUsuario)
        {
            try
            {
                return Data.ListarMenuPorUsuario(sUsuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public EGeneralJson<EUsuario> ListarPaginacion(int iComienzo, int iMedida, string sNroDocumento, string sNombre)
        {
            try
            {
                return Data.ListarPaginacion(iComienzo, iMedida, sNroDocumento, sNombre);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

      

        public EUsuario obtenerNombreUsuario(long idusuario)
        {
            try
            {
                return Data.obtenerNombreUsuario(idusuario);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public EUsuario ObtenerVendedorPorTurnoyFecha(int iIdcara, int iIdTurno, DateTime fecahOperacion)
        {
            try
            {
                return Data.ObtenerVendedorPorTurnoyFecha(iIdcara, iIdTurno, fecahOperacion);
            }
            catch (Exception)
            {


                throw;
            }
        }

        public EUsuario ObtenerTerminalVendedorPorTurnoyFecha(int iIdTerminal, int iIdTurno, DateTime fecahOperacion)
        {
            try
            {
                return Data.ObtenerTerminalVendedorPorTurnoyFecha(iIdTerminal, iIdTurno, fecahOperacion);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        
    }
}

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
    public class BPerfil
    {
        private static BPerfil Instancia;
        private DPerfil Data = DPerfil.ObtenerInstancia(DataBase.SqlServer);

        public static BPerfil ObtenerInstancia()
        {
            if (Instancia == null)
            {
                Instancia = new BPerfil();
            }
            return Instancia;
        }

        public string Registrar(EPerfil oPerfil)
        {
            try
            {
                return Data.Registrar(oPerfil);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public string Actualizar(EPerfil oPerfil)
        {
            try
            {
                return Data.Actualizar(oPerfil);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public string Eliminar(int iIdPerfil)
        {
            try
            {
                return Data.Eliminar(iIdPerfil);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public List<EPerfil> Listar()
        {
            try
            {
                return Data.Listar();
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public EGeneralJson<EPerfil> Listar(int iComienzo, int iMedida, string sFiltro)
        {
            try
            {
                return Data.Listar(iComienzo, iMedida, sFiltro);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        public EPerfil BuscarPerfilPorId(long Id)
        {
            try
            {
                return Data.BuscarPerfilPorId(Id);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }


        public List<EMenu> ListarAccesosporPerfil(int id)
        {
            try
            {
                return Data.ListarAccesosPorPerfil(id);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }
        public string ActualizarAccesosPorPerfil(int Id, string  menus)
        {
            try
            {
                return Data.ActualizarAccesosPorPerfil(Id, menus);
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }



    }
}

using Factory;
using sisCCS.EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.DataLayer
{
    public class DPerfil : DBHelper
    {
        private static DPerfil Instancia;
        private DataBase BaseDeDatos;

        public DPerfil(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DPerfil ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DPerfil(BaseDeDatos);
            }
            return Instancia;
        }

        public string Registrar(EPerfil oPerfil)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Ins_Perfil");
                    CreateHelper(Connection);
                    AddInParameter("@iIdEmpresa", oPerfil.EmpresaHolding.Id);
                    AddInParameter("@vNombre", oPerfil.Nombre);
                    AddInParameter("@iIdUsuarioCreador", oPerfil.UsuarioCreador.Id);
                    AddOutParameter("@Mensaje", System.Data.DbType.String);
                    ExecuteQuery();
                    return GetOutput("@Mensaje").ToString();
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public string Actualizar(EPerfil oPerfil)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Act_Perfil");
                    CreateHelper(Connection);
                    AddInParameter("@iIdPerfil", oPerfil.Id);
                    AddInParameter("@vNombre", oPerfil.Nombre);
                    AddOutParameter("@Mensaje", System.Data.DbType.String);
                    ExecuteQuery();
                    return GetOutput("@Mensaje").ToString();
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public string Eliminar(int iIdPerfil)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Eli_Perfil");
                    CreateHelper(Connection);
                    AddInParameter("@iIdPerfil", iIdPerfil);
                    AddOutParameter("@Mensaje", System.Data.DbType.String);
                    ExecuteQuery();
                    return GetOutput("@Mensaje").ToString();
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }

        public List<EPerfil> Listar()
        {
            List<EPerfil> lPerfiles = new List<EPerfil>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_Perfiles");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EPerfil oPerfil = new EPerfil();
                            oPerfil.Id = int.Parse(Reader["IdPerfil"].ToString());
                            oPerfil.Nombre = Reader["Perfil"].ToString();
                            lPerfiles.Add(oPerfil);
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
            return lPerfiles;
        }

        public EGeneralJson<EPerfil> Listar(int iComienzo, int iMedida, string sFiltro)
        {
            EGeneralJson<EPerfil> lPerfiles = new EGeneralJson<EPerfil>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_PerfilesPaginacion");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", iComienzo);
                    AddInParameter("@iMedida", iMedida);
                    AddInParameter("@vFiltro", sFiltro);
                    using (var Reader = ExecuteReader())
                    {
                        lPerfiles.Datos = new List<EPerfil>();
                        while (Reader.Read())
                        {
                            EPerfil oPerfil = new EPerfil();
                            oPerfil.Id = int.Parse(Reader["IdPerfil"].ToString());
                            oPerfil.Nombre = Reader["Perfil"].ToString();
                            lPerfiles.Datos.Add(oPerfil);
                            lPerfiles.Total = int.Parse(Reader["Total"].ToString());
                        }
                        lPerfiles.Visualizados = lPerfiles.Datos.Count;
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
            return lPerfiles;
        }
        public EPerfil BuscarPerfilPorId(long Id)
        {
            EPerfil oPerfil = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_PerfilPorID");
                    CreateHelper(Connection);
                    AddInParameter("@iIdPerfilUsuario", Id);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            oPerfil = new EPerfil();
                            oPerfil.Id = int.Parse(Reader["Id"].ToString());
                            oPerfil.IdEmpresaHolding = int.Parse(Reader["IdEmpresaHolding"].ToString());
                            oPerfil.NombrePerfil = Reader["NombrePerfil"].ToString();
                            oPerfil.IdUsuarioReg = int.Parse(Reader["IdUsuarioReg"].ToString());
                            oPerfil.FechaHoraReg = Reader["FechaHoraReg"].ToString();
                            oPerfil.Estado = bool.Parse(Reader["Estado"].ToString());
                            oPerfil.Menu = int.Parse(Reader["Menu"].ToString());                            
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
            return oPerfil;
        }

        public List<EMenu> ListarAccesosPorPerfil(int id)
        {
            List<EMenu> lPerfiles = new List<EMenu>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_AccesosPorPerfilId");
                    CreateHelper(Connection);
                    AddInParameter("@iIdPerfilUsuario", id);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMenu oPerfil = new EMenu();
                            oPerfil.Id = int.Parse(Reader["Menu"].ToString());
                            oPerfil.Padre = new EMenu
                            {
                                Id = int.Parse(Reader["MenuPadre"].ToString())
                        };
                            oPerfil.Nombre = Reader["DescripcionMenu"].ToString();
                            oPerfil.TieneAcceso =bool.Parse(Reader["TieneAcceso"].ToString());
                            lPerfiles.Add(oPerfil);
                        }
                    }
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
            return lPerfiles;
        }

        public string ActualizarAccesosPorPerfil(int Id, string Menus)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Act_AccesosPorPerfilId");
                    CreateHelper(Connection);
                    AddInParameter("@iIdPerfilUsuario", Id);
                    AddInParameter("@vMenu", Menus);
                    AddOutParameter("@Mensaje", System.Data.DbType.String);
                    ExecuteQuery();
                    return GetOutput("@Mensaje").ToString();
                }
                catch (Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
            }
        }


    }
}

using Factory;
using sisCCS.EntityLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.DataLayer
{
    public class DUsuario : DBHelper
    {
        private static DUsuario Instancia;
        private DataBase BaseDeDatos;

        public DUsuario(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DUsuario ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DUsuario(BaseDeDatos);
            }
            return Instancia;
        }

        public string Registrar(EUsuario oUsuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Ins_Usuario");
                    CreateHelper(Connection);
                    AddInParameter("@iIdTipoDocumento", oUsuario.TipoDocumento.Id);
                    AddInParameter("@vNroDocumento", oUsuario.NroDocumento);
                    AddInParameter("@vNombre", oUsuario.Nombre);
                    AddInParameter("@vApellidoPaterno", oUsuario.ApellidoPaterno);
                    AddInParameter("@vApellidoMaterno", oUsuario.ApellidoMaterno);
                    AddInParameter("@dFechaNacimiento", oUsuario.FechaNacimiento, AllowNull);
                    AddInParameter("@vTelefono", oUsuario.Telefono, AllowNull);
                    AddInParameter("@vCorreoElectronico", oUsuario.CorreoElectronico, AllowNull);
                    AddInParameter("@iIdPerfil", oUsuario.Perfil.Id);
                    AddInParameter("@vUsuario", oUsuario.Usuario);
                    AddInParameter("@vClave", oUsuario.Password);
                    AddInParameter("@iIdUbigeo", oUsuario.Ubigeo.Id, AllowNull);
                    AddInParameter("@vDireccion", oUsuario.Direccion, AllowNull);
                    AddInParameter("@iIdUsuarioCreador", oUsuario.UsuarioCreador.Id);
                    AddOutParameter("Mensaje", System.Data.DbType.String);
                    ExecuteQuery();
                    return GetOutput("Mensaje").ToString();
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


        public string Actualizar(EUsuario oUsuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Act_Usuario");
                    CreateHelper(Connection);
                    AddInParameter("@iIdUsuario", oUsuario.Id);
                    AddInParameter("@iIdTipoDocumento", oUsuario.TipoDocumento.Id);
                    AddInParameter("@vNroDocumento", oUsuario.NroDocumento);
                    AddInParameter("@vNombre", oUsuario.Nombre);
                    AddInParameter("@vApellidoPaterno", oUsuario.ApellidoPaterno);
                    AddInParameter("@vApellidoMaterno", oUsuario.ApellidoMaterno);
                    AddInParameter("@dFechaNacimiento", oUsuario.FechaNacimiento, AllowNull);
                    AddInParameter("@vTelefono", oUsuario.Telefono, AllowNull);
                    AddInParameter("@vCorreoElectronico", oUsuario.CorreoElectronico, AllowNull);
                    AddInParameter("@iIdPerfil", oUsuario.Perfil.Id);
                    AddInParameter("@vUsuario", oUsuario.Usuario);
                    AddInParameter("@iIdUbigeo", oUsuario.Ubigeo.Id, AllowNull);
                    AddInParameter("@vDireccion", oUsuario.Direccion, AllowNull);
                    AddOutParameter("Mensaje", System.Data.DbType.String);
                    ExecuteQuery();
                    return GetOutput("Mensaje").ToString();
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

        public string EliminarAsignacion(int iIdAsignacion)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Eli_AsignacionSucursalUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@iIdAsignacion", iIdAsignacion);
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

        public EUsuario Login(string sUsuario, string sClave)
        {
            EUsuario oUsuario = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_Login");
                    CreateHelper(Connection);
                    AddInParameter("@vUsuario", sUsuario);
                    AddInParameter("@vClave", sClave);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            oUsuario = new EUsuario();
                            oUsuario.Id = int.Parse(Reader["IdUsuario"].ToString());
                            oUsuario.Usuario = sUsuario;
                            oUsuario.Nombre = Reader["Nombre"].ToString();
                            oUsuario.ApellidoPaterno = Reader["ApellidoPaterno"].ToString();
                            oUsuario.ApellidoMaterno = Reader["ApellidoMaterno"].ToString();

                            oUsuario.Perfil = new EPerfil
                            {
                                Id = int.Parse(Reader["IdPerfil"].ToString()),
                                Nombre = Reader["Perfil"].ToString(),
                                EmpresaHolding = new EEmpresaHolding
                                {
                                    Empresa = new EEmpresa
                                    {
                                        Id = int.Parse(Reader["IdEmpresa"].ToString()),
                                        Nombre = Reader["Empresa"].ToString(),
                                        RazonSocial = Reader["EmpresaRazonSocial"].ToString(),
                                        RUC = Reader["EmpresaRUC"].ToString(),
                                        DireccionTexto = Reader["EmpresaDomicilio"].ToString()

                                    }
                                }
                            };

                            oUsuario.Nombre = Reader["Nombre"].ToString();
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
            return oUsuario;
        }

        public string Login(int iIdSucursal, int iIdLineaNegocio, DateTime dFechaOperacion, int iIdTurno, string sUsuario, string sPassword)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_LoginLiquidacion");
                    CreateHelper(Connection);
                    AddInParameter("@iIdSucursal", iIdSucursal);
                    AddInParameter("@iIdLineaNegocio", iIdLineaNegocio);
                    AddInParameter("@dFechaOperacion", dFechaOperacion);
                    AddInParameter("@iIdTurno", iIdTurno);
                    AddInParameter("@vUsuario", sUsuario);
                    AddInParameter("@vClave", sPassword);
                    AddOutParameter("@Mensaje", DbType.String);
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
        //este es
        public EUsuario BuscarPorUsuario(string sUsuario)
        {
            EUsuario oUsuario = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_UsuarioPorUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@vUsuario", sUsuario);
                    using (var Reader = ExecuteReader())
                    {
                        if (Reader.Read())
                        {
                            oUsuario = new EUsuario();
                            oUsuario.Id = int.Parse(Reader["IdUsuario"].ToString());
                            oUsuario.Nombre = Reader["Nombre"].ToString();
                            oUsuario.ApellidoPaterno = Reader["ApellidoPaterno"].ToString();
                            oUsuario.ApellidoMaterno = Reader["ApellidoMaterno"].ToString();
                            oUsuario.Usuario = Reader["Usuario"].ToString();
                            oUsuario.Perfil = new EPerfil
                            {
                                Id = int.Parse(Reader["IdPerfil"].ToString()),
                                Nombre = Reader["Perfil"].ToString(),
                                EmpresaHolding = new EEmpresaHolding
                                {
                                    Empresa = new EEmpresa
                                    {
                                        Id = int.Parse(Reader["IdEmpresa"].ToString()),
                                        Nombre = Reader["Empresa"].ToString(),
                                        RazonSocial = Reader["EmpresaRazonSocial"].ToString(),
                                        RUC = Reader["EmpresaRUC"].ToString(),
                                      //  DireccionTexto = Reader["EmpresaDomicilio"].ToString()
                                    }
                                }
                            };

                            oUsuario.Nombre = Reader["Nombre"].ToString();
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
            return oUsuario;
        }

        public List<EUsuario> ListarVendedores(DateTime dFechaOperacion, int iIdturno, int iIdUsuario)
        {
            List<EUsuario> lUsuarios = new List<EUsuario>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_VendedoresParaLiquidacion");
                    CreateHelper(Connection);
                    AddInParameter("@dFechaOperacion", dFechaOperacion);
                    AddInParameter("@iIdTurno", iIdturno);
                    AddInParameter("@iIdUsuario", iIdUsuario);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EUsuario oUsuario = new EUsuario();
                            oUsuario.Id = int.Parse(Reader["IdVendedor"].ToString());
                            oUsuario.Nombre = Reader["Vendedor"].ToString();
                            lUsuarios.Add(oUsuario);
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
            return lUsuarios;
        }

        public List<EMenu> ListarMenuPorUsuario(string sUsuario)
        {
            List<EMenu> lMenu = new List<EMenu>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_MenuPorUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@vUsuario", sUsuario);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EMenu oMenu = new EMenu();
                            oMenu.Id = int.Parse(Reader["IdMenu"].ToString());
                            oMenu.Nombre = Reader["Menu"].ToString();
                            oMenu.Orden = int.Parse(Reader["Orden"].ToString());
                            oMenu.Controlador = Reader["Controlador"].ToString();
                            oMenu.Vista = Reader["Vista"].ToString();
                            oMenu.Padre = new EMenu
                            {
                                Id = !string.IsNullOrEmpty(Reader["IdPadre"].ToString()) ? int.Parse(Reader["IdPadre"].ToString()) : -1
                            };
                            lMenu.Add(oMenu);
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
            return lMenu;
        }

        public EGeneralJson<EUsuario> ListarPaginacion(int iComienzo, int iMedida, string sNroDocumento, string sNombre)
        {
            EGeneralJson<EUsuario> lUsuarios = new EGeneralJson<EUsuario>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_UsuariosPaginacion");
                    CreateHelper(Connection);
                    AddInParameter("@iComienzo", iComienzo);
                    AddInParameter("@iMedida", iMedida);
                    AddInParameter("@vNroDocumento", sNroDocumento);
                    AddInParameter("@vNombre", sNombre);
                    using (var Reader = ExecuteReader())
                    {
                        lUsuarios.Datos = new List<EUsuario>();
                        while (Reader.Read())
                        {
                            EUsuario oUsuario = new EUsuario();
                            oUsuario.Id = int.Parse(Reader["IdUsuario"].ToString());
                            oUsuario.Nombre = Reader["Nombre"].ToString();
                            oUsuario.ApellidoPaterno = Reader["ApellidoPaterno"].ToString();
                            oUsuario.ApellidoMaterno = Reader["ApellidoMaterno"].ToString();
                            oUsuario.Perfil = new EPerfil
                            {
                                Id = int.Parse(Reader["IdPerfil"].ToString()),
                                Nombre = Reader["Perfil"].ToString(),
                                EmpresaHolding = new EEmpresaHolding
                                {
                                    Empresa = new EEmpresa
                                    {
                                        Id = int.Parse(Reader["IdEmpresa"].ToString()),
                                        Nombre = Reader["Empresa"].ToString()
                                    }
                                }
                            };
                            oUsuario.Nombre = Reader["Nombre"].ToString();
                            oUsuario.ApellidoPaterno = Reader["ApellidoPaterno"].ToString();
                            oUsuario.ApellidoMaterno = Reader["ApellidoMaterno"].ToString();
                            oUsuario.TipoDocumento = new EGeneral
                            {
                                Id = !string.IsNullOrEmpty(Reader["IdTipoDocumento"].ToString()) ? int.Parse(Reader["IdTipoDocumento"].ToString()) : -1
                            };
                            oUsuario.NroDocumento = Reader["NroDocumento"].ToString();
                            oUsuario.FechaNacimiento = !string.IsNullOrEmpty(Reader["FechaNacimiento"].ToString()) ? (DateTime?)DateTime.Parse(Reader["FechaNacimiento"].ToString()) : null;
                            oUsuario.Telefono = Reader["Telefono"].ToString();
                            oUsuario.CorreoElectronico = Reader["CorreoElectronico"].ToString();
                            oUsuario.Usuario = Reader["Usuario"].ToString();
                            oUsuario.Ubigeo = new EUbigeo
                            {
                                Id = !string.IsNullOrEmpty(Reader["IdUbigeo"].ToString()) ? int.Parse(Reader["IdUbigeo"].ToString()) : -1,
                                CodigoDepartamento = Reader["CodigoDepartamento"].ToString(),
                                CodigoProvincia = Reader["CodigoProvincia"].ToString(),
                                CodigoDistrito = Reader["CodigoDistrito"].ToString()
                            };
                            oUsuario.Direccion = Reader["Direccion"].ToString();
                            lUsuarios.Datos.Add(oUsuario);
                            lUsuarios.Total = int.Parse(Reader["Total"].ToString());
                        }
                        lUsuarios.Visualizados = lUsuarios.Datos.Count;
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
            return lUsuarios;
        }


        /*Poblar Usuario*/
        public EUsuario obtenerNombreUsuario(long idusuario)
        {
            var objUsuario = new EUsuario();


            using (var Connection = GetConnection(BaseDeDatos))
            {

                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_NombreUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@iIdUsuario", idusuario);

                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            objUsuario.Nombres = Reader["Nombre"].ToString();

                        }

                    }


                    return objUsuario;

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
        //turno de operación
        public EUsuario ObtenerVendedorPorTurnoyFecha(int iIdcara, int iIdTurno, DateTime fecahOperacion)
        {
            EUsuario oUsuario = new EUsuario();
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_sel_cara_Por_Turno_fecha");
                    CreateHelper(Connection);
                    AddInParameter("@iIdCara", iIdcara);
                    AddInParameter("@iIdTurno", iIdTurno);
                    AddInParameter("@fechaOperacion", fecahOperacion);




                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {

                            oUsuario.Id = int.Parse(Reader["idUsuario"].ToString());
                            oUsuario.Nombres = Reader["NombreVendedor"].ToString();

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
            return oUsuario;
        }


        public EUsuario ObtenerTerminalVendedorPorTurnoyFecha(int iIdTerminal, int iIdTurno, DateTime fecahOperacion)
        {
            EUsuario oUsuario = new EUsuario();
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("SP_sel_cara_Por_Turno_fecha");
                    CreateHelper(Connection);
                    AddInParameter("@iIdTerminal", iIdTerminal);
                    AddInParameter("@iIdTurno", iIdTurno);
                    AddInParameter("@fechaOperacion", fecahOperacion);




                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {

                            oUsuario.Id = int.Parse(Reader["idUsuario"].ToString());
                            oUsuario.Nombres = Reader["NombreVendedor"].ToString();

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
            return oUsuario;
        }



        public List<EUsuario> ListarVendedores(int iIdEmpresa, int IdSucursal, int iIdLineaNegocio)
        {
            List<EUsuario> lEmpleados = new List<EUsuario>();
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("sp_sel_Vendedores");
                    CreateHelper(Connection);
                    AddInParameter("@iIdEmpresa", iIdEmpresa);
                    AddInParameter("@idSucursal", IdSucursal);
                    AddInParameter("@iIdLineaNegico", iIdLineaNegocio);


                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EUsuario oUsuario = new EUsuario();
                            oUsuario.Id = int.Parse(Reader["idUsuario"].ToString());
                            oUsuario.Nombres = Reader["NombreVendedor"].ToString();
                            lEmpleados.Add(oUsuario);
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
            return lEmpleados;
        }
        


        public string DeshabilitarUsuario(int IdUsuario)
        {
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Act_DeshabilitarUsuario");
                    CreateHelper(Connection);
                    AddInParameter("@iIDUsuario", IdUsuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
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


        public string CambiarPassword(int IdUsuario, string newpass, string oldpass)
        {
            using (var Connection = GetConnection(this.BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Act_CambioPassword");
                    CreateHelper(Connection);
                    AddInParameter("@vOldPass", oldpass);
                    AddInParameter("@vNewPass", newpass);
                    AddInParameter("@iIDUsuario", IdUsuario);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
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

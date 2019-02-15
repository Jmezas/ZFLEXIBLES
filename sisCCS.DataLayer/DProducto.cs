using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sisCCS.EntityLayer;
using Factory;
using System.Data;
namespace sisCCS.DataLayer
{
    public class DProducto : DBHelper
    {
        private static DProducto Instancia;
        private DataBase BaseDeDatos;

        public DProducto(DataBase BaseDeDatos) : base(BaseDeDatos)
        {
            this.BaseDeDatos = BaseDeDatos;
        }

        public static DProducto ObtenerInstancia(DataBase BaseDeDatos)
        {
            if (Instancia == null)
            {
                Instancia = new DProducto(BaseDeDatos);
            }
            return Instancia;
        }

        public List<EProducto> ListaProducto()
        {
            List<EProducto> oDatos = new List<EProducto>();
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_ListadoProductosGstv");
                    CreateHelper(Connection);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            EProducto oProducto = new EProducto();
                            oProducto.IdMaterial = int.Parse(Reader["Id"].ToString());
                            oProducto.Codigo = Reader["CodigoPrincipal"].ToString();
                            oProducto.NombreMat = Reader["Nombre"].ToString();
                            oProducto.Fechereg = Reader["Fecha"].ToString();
                            oProducto.UM.MedNom = Reader["Unidad"].ToString();
                            oProducto.PrecioCompra = Reader["compra"].ToString();
                            oProducto.PrecioVenta = Reader["venta"].ToString();
                            oDatos.Add(oProducto);
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
                return oDatos;
            }
        }

        public EProducto ObtenerProducto(int ID)
        {
            EProducto oProducto = null;
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("Sp_Sel_ProductoPorID");
                    CreateHelper(Connection);
                    AddInParameter("@iIdProducto", ID);
                    using (var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            oProducto = new EProducto();
                            oProducto.IdMaterial = int.Parse(Reader["Id"].ToString());
                            oProducto.Codigo = Reader["CodigoPrincipal"].ToString();
                            oProducto.Descripcion = Reader["Descripcion"].ToString();
                            oProducto.NombreMat = Reader["Nombre"].ToString();
                            oProducto.UM.IdMed = int.Parse(Reader["IdUnidadMedida"].ToString());
                            oProducto.UM.MedNom = Reader["UnidadMedida"].ToString();
                            oProducto.PrecioCompra = Reader["PrecioCompra"].ToString();
                            oProducto.PrecioVenta = Reader["PrecioVenta"].ToString();
                            oProducto.EstadoMat = Reader["Estado"].ToString();
                            oProducto.Impuesto = Reader["Impuesto"].ToString();
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
                return oProducto;
            }
        }
        public string Registrar_Update(EProducto Producto, string Usuario)
        {
            using (var Connection = GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_Ins_Upd");
                    CreateHelper(Connection);
                    AddInParameter("@idMaterial", Producto.IdMaterial);
                    AddInParameter("@scodigo", Producto.Codigo);
                    AddInParameter("@sNomMat", Producto.NombreMat);
                    AddInParameter("@sDescripcion", Producto.Descripcion, AllowNull);
                    AddInParameter("@sUni", Producto.UM.IdMed);
                    AddInParameter("@nPrecioVenta", Producto.PrecioVenta);
                    AddInParameter("@nPrecioCompra", Producto.PrecioCompra);
                    AddInParameter("@Stock", Producto.Stock);
                    AddInParameter("@user", Usuario);
                    AddInParameter("@sEstado", Producto.EstadoMat);
                    AddOutParameter("@Mensaje", (DbType)SqlDbType.VarChar);
                    ExecuteQuery();
                    var smensaje = GetOutput("@Mensaje").ToString();
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
        public EProducto Producto()
        {
            EProducto oDatos = new EProducto();
            using (var Connection= GetConnection(BaseDeDatos))
            {
                try
                {
                    Connection.Open();
                    SetQuery("LOG_Codigo_Autogenerado_x_Producto");
                    CreateHelper(Connection);
                    using(var Reader = ExecuteReader())
                    {
                        while (Reader.Read())
                        {
                            oDatos.Codigo = Reader["Codigo"].ToString();
                        }
                    }
                }catch(Exception Exception)
                {
                    throw Exception;
                }
                finally
                {
                    Connection.Close();
                }
                return oDatos;
            }
        }
    }
}

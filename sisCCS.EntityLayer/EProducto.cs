using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
   public class EProducto
    {
        public EProducto() {
            UM = new EUnidadMedida();
        }
        public int IdMaterial { get; set; }
        public string Codigo { get; set; }
        public string NombreMat { get; set; }
        public string Descripcion { get; set; }
        public string Fechereg { get; set; }
        public string PrecioCompra { get; set; }
        public string PrecioVenta { get; set; }
        public string EstadoMat { get; set; }
        public string Stock { get; set; }
        public string Impuesto { get; set; }
        public EUnidadMedida UM { get; set; }

        
    }
}

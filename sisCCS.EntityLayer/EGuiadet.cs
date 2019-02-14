using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
   public class EGuiadet
    {
        public EGuiadet()
        {
            EGuia = new EGuiaCab();
            Producto = new EProducto();
            Usuario = new EUsuario();
        }
        public EGuiaCab EGuia { get; set; }
        public EProducto Producto { get; set; }
        public decimal Cantidad { get; set; }
        public Decimal Precio { get; set; }
        public EUsuario Usuario { get; set; }
        
    }
}

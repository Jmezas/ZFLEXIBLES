using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EOrdenCompraDet
    {
        public EOrdenCompraDet()
        {
            OrdenCompraCab = new EOrdenCompraCab();
            Producto = new EProducto();
            Usuario = new EUsuario();
        }
        public EOrdenCompraCab OrdenCompraCab { get; set; }
        public EProducto Producto { get; set; }
        public string Cantidad { get; set; }
        public string Precio { get; set; }
        public string Importe { get; set; }
        public EUsuario Usuario { get; set; }
    }
}

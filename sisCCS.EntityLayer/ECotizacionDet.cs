using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class ECotizacionDet
    { 
        public ECotizacionDet()
        {
            CotizacionCab = new ECotizacionCab();
            Producto = new EProducto();
            Usuario = new EUsuario();
        }

        public ECotizacionCab CotizacionCab { get; set; }
        public EProducto Producto { get; set; }
        public string Cantidad { get; set; }
        public string Precio { get; set; }
        public string Importe { get; set; }
        public string fImportSnIGV { get; set; }
        public EUsuario Usuario { get; set; }
    }
}

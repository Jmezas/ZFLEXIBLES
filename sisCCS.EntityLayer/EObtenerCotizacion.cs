using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EObtenerCotizacion
    {   
        public EObtenerCotizacion()
        {
            Cotizacion = new ECotizacionCab();
            Cliente = new ECliente();
            Moneda = new ETipoMoneda();
            Detalle = new ECotizacionDet();
            Documento = new ETipoDocumentoIdentidad();
            Producto = new EProducto();
        }

        public ECotizacionCab Cotizacion { get; set; }
        public ECliente Cliente { get; set; }
        public ETipoMoneda Moneda { get; set; }
        public ECotizacionDet Detalle { get; set; }
        public ETipoDocumentoIdentidad Documento { get; set; }
        public EProducto Producto { get; set; }
        public int Item { get; set; }

    }
}

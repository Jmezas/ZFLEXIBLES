using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EReporteCotizacion
    {
        public EReporteCotizacion()
        {
            CotizacionCab = new ECotizacionCab();
            CotizacionDet = new ECotizacionDet();
            Cliente = new ECliente();
            Documento = new ETipoDocumentoIdentidad();
            Producto = new EProducto();
            Moneda = new ETipoMoneda();
        }

        public ECotizacionCab CotizacionCab { get; set; }
        public ECotizacionDet CotizacionDet { get; set; }
        public ECliente Cliente { get; set; }
        public ETipoDocumentoIdentidad Documento { get; set; }
        public EProducto Producto { get; set; }
        public ETipoMoneda Moneda { get; set; }
        public int Item { get; set; }
    }
}

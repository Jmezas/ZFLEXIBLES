using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EReporteFactura
    {
        public EReporteFactura()
        {
            FacturaCab = new EFacturaCab();
            Detalle = new EFacturaDet();
            Cliente = new ECliente();
            Comprobante = new EComprobanteFac();
            Documento = new ETipoDocumentoIdentidad();
            Moneda = new ETipoMoneda();
            Producto = new EProducto();
        }
        public EFacturaCab FacturaCab { get; set; }
        public EFacturaDet Detalle { get; set; }
        public ECliente Cliente { get; set; }
        public EComprobanteFac Comprobante { get; set; }//factura - boleta
        public ETipoDocumentoIdentidad Documento { get; set; }      
        public ETipoMoneda Moneda { get; set; }
        public EProducto Producto { get; set; }
        public int Item { get; set; }
       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
   public class ECuentaPago
    {
        public ECuentaPago()
        {
            Comprobante = new EComprobanteFac();
            Cliente = new ECliente();
            Documento = new ETipoDocumentoIdentidad();
            Moneda = new ETipoMoneda();
            Factura = new EFacturaCab();
            Pago = new EPago();
        }
        public int IdBandeja { get; set; }
        public EComprobanteFac Comprobante { get; set; } //fac - bol
        public ECliente Cliente { get; set; }
        public ETipoDocumentoIdentidad Documento { get; set; }
        public ETipoMoneda Moneda { get; set; }
        public EFacturaCab Factura { get; set; }
        public EPago Pago { get; set; }
        public int Item { get; set; }
        public string Pagado { get; set; }
        public string Pendiente { get; set; }
        public string FechaEmision { get; set; }
        public string EstadoFactura { get; set; }

    }
}

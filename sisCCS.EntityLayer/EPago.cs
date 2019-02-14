using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EPago
    {
        public EPago()
        {
            Cliente = new ECliente();
            Factura = new EFacturaCab();
            Usuario = new EUsuario();
            Comprobante = new EComprobanteFac();
        }
        public int Item { get; set; }
        public int IdPago { get; set; }
        public string Hora { get; set; }
        public ECliente Cliente { get; set; }
        public EFacturaCab Factura { get; set; }
        public string Monto { get; set; }
        public string FechaPago { get; set; }
        public EUsuario Usuario { get; set; }
        public EComprobanteFac Comprobante { get; set; }
         

    }
}

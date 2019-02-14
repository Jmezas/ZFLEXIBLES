using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EFacturaCab
    {
        public EFacturaCab()
        {
            Cliente = new ECliente();
            Comprobante = new EComprobanteFac();
            Moneda = new ETipoMoneda();
            Usuario = new EUsuario();
        }

        public int IdVenta { get; set; }
        public ECliente Cliente { get; set; }
        public EComprobanteFac Comprobante { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string EstadoDocumento { get; set; }
        public ETipoMoneda Moneda { get; set; }
        public string Cantidad { get; set; }
        public string SubTotal { get; set; }
        public string IGV { get; set; }
        public string Total { get; set; }
        public EUsuario Usuario { get; set; }
        public string TipoSerieComprobante { get; set; }
        public string Mensaje { get; set; }
        public string Observacion { get; set; }
        public string Motivo { get; set; }
        public string FechaEmisio { get; set; }
        public int Item { get; set; }

    }
}

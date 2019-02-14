using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
   public class ECotizacionCab
    {
        public ECotizacionCab()
        {
            Documento = new ETipoDocumentoIdentidad();
            Cliente = new ECliente();
            Moneda = new ETipoMoneda();
            Usuario = new EUsuario();
           
        }

        public int IdCotizacion { get; set; }
        public ETipoDocumentoIdentidad Documento { get; set; }
        public EComprobanteFac ComprobanteFac { get; set; }
        public ECliente Cliente { get; set; }
        public ETipoMoneda Moneda { get; set; }
        public string FechaEmision { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string Mensaje { get; set; }
        public string Cantidad { get; set; }
        public string SubTotal { get; set; }
        public string IGV { get; set; }
        public string Total { get; set; }
        public EUsuario Usuario { get; set; }
        public string Asunto { get; set;}
        public int Item { get; set; }
        

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EOrdenCompraCab
    {
        public EOrdenCompraCab()
        {
            proveed = new EProveedor();
            Moneda = new ETipoMoneda();
        }
        public int Item { get; set; }
        public int IdOrden { get; set; }
        public EProveedor proveed { get; set; }
        public string Serie { get; set; }
        public string NumeroDoc { get; set; }
        public string Cantidad { get; set; }
        public string SutTotal { get; set; }
        public string IGv { get; set; }
        public string Total { get; set; }
        public string FechaRegistro { get; set;  }
        public ETipoMoneda Moneda { get; set; }
       
    }
}

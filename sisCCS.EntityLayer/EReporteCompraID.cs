using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EReporteCompraID
    {
        public EReporteCompraID()
        {
            OrdenCompraCab = new EOrdenCompraCab();
            Proveedor = new EProveedor();
            CompraDetalle = new EOrdenCompraDet();
            Producto = new EProducto();
            Ubigeo = new EUbigeo();
            Moneda = new ETipoMoneda();

        }

        public EOrdenCompraCab OrdenCompraCab { get; set; }
        public EProveedor Proveedor { get; set; }
        public EOrdenCompraDet CompraDetalle { get; set; }
        public EProducto Producto { get; set; }
        public EUbigeo Ubigeo { get; set; }
        public ETipoMoneda Moneda { get; set; }
        public int Item { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EFacturaDet
    {   
        public EFacturaDet()
        {
            FacturaCab = new EFacturaCab();
            Documento = new ETipoDocumentoIdentidad();
            Producto = new EProducto();
            Usuario = new EUsuario();
        }

        public EFacturaCab FacturaCab { get; set; }
        public ETipoDocumentoIdentidad Documento { get; set; }
        public EProducto Producto { get; set; }
        public string Cantidad { get; set; }
        public string Precio { get; set; }
        public string Importe { get; set; }
        public EUsuario Usuario { get; set; }
        public string fImportSnIGV { get; set; }
        
    }
}

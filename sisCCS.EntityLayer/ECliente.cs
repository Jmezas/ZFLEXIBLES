using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
   public class ECliente
    {
        public ECliente()
        {
            Documento = new ETipoDocumentoIdentidad();
            Usuario = new EUsuario();
        }
        public int IdCliente { get; set; }
        public string NroDocumento { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Estado { get; set; }
        public EUsuario Usuario { get; set; }
        public ETipoDocumentoIdentidad Documento { get; set; }

    }
}

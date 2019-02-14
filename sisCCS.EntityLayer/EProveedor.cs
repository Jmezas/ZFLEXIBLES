using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EProveedor
    {
        public EProveedor()
        {
            Ubigeo = new EUbigeo();
        }
        public int IdProveedor { get; set; }
        public string Nombre { get; set; }
        public string NroDocumento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public EUbigeo Ubigeo { get; set; }
        public string Direccion { get; set; }
    }
}

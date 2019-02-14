using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EEmpresa : EGeneral
    {
        public EEmpresa()
        {
            UsuarioCreador = new EUsuario();
            TipoDocumentoIdentidad = new ETipoDocumentoIdentidad();
        }

        public string EstadoContribuyente { get; set; }
        public string CondicionDomcilio { get; set; }
        public string Ubigeo { get; set; }
        public string TipoVia { get; set; }
        public string NombreVia { get; set; }
        public string TipoZona { get; set; }
        public string NombreZona { get; set; }
        public string Numero { get; set; }
        public string Interior { get; set; }
        public string Lote { get; set; }
        public string Departamento { get; set; }
        public string Manzana { get; set; }
        public string Kilometro { get; set; }
        public string RUC { get; set; }
        public string RazonSocial { get; set; }
        public string Correo { get; set; }
        public string Contrasenia { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Fecha { get; set; }
        public EUsuario UsuarioCreador { get; set; }
        public ETipoDocumentoIdentidad TipoDocumentoIdentidad { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string DireccionTexto { get; set; }
        public string Direccion { get;set;   }
    }
}

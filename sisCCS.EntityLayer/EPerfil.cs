using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EPerfil : EGeneral
    {
        public EEmpresaHolding EmpresaHolding { get; set; }
        public EUsuario UsuarioCreador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int Id { get; set; }
        public int IdEmpresaHolding { get; set; }
        public string NombrePerfil { get; set; }
        public int IdUsuarioReg { get; set; }
        public string FechaHoraReg { get; set; }
        public bool Estado { get; set; }
        public int Menu { get; set; }
        public int MenuPadre { get; set; }
        public string DescripcionMenu { get; set; }
        public bool TieneAcceso { get; set; }
    }
}

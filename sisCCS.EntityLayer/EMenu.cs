using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EMenu : EGeneral
    {
        public int Orden { get; set; }
        public string Controlador { get; set; }
        public string Vista { get; set; }
        public bool TieneAcceso { get; set; }
        public EMenu Padre { get; set; }
        public List<EMenu> Hijos { get; set; }
    }
}
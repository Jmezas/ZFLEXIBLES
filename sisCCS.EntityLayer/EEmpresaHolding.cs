using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EEmpresaHolding : EGeneral
    {
        public EEmpresaHolding()
        {
            Holding = new EHolding();
            Empresa = new EEmpresa();
        }
        public EHolding Holding { get; set; }
        public EEmpresa Empresa { get; set; }
        public bool AgenteRetencion { get; set; }
    }
}

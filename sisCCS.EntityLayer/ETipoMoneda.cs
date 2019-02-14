using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class ETipoMoneda: EGeneral
    {
        public int idMoneda { get; set; }
        public string Descripcion { get; set; }
    }
}

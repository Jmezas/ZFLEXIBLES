using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EGeneral
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool? Estado { get; set; }
        public string Text { get; set; }
    }

    public class EGeneralJson<T>
    {
        public long Total { get; set; }
        public int Visualizados { get; set; }
        public List<T> Datos { get; set; }
    }

}

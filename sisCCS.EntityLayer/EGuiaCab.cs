using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EGuiaCab
    {
        public EGuiaCab()
        {
            Ubigeo = new EUbigeo();
            Comprobante = new ECotizacionCab();
            Cliente = new ECliente();
            Usuario = new EUsuario();
           
            Producto = new EProducto();
        }
        public int IdGuia { get; set; }
        public string Serie { get; set; }
        public string Numero { get; set; }
        public string PuntoLlegada { get; set; }
        public EUbigeo Ubigeo { get; set; }
        public string Marca { get; set; }
        public string Placa { get; set; }
        public string Licencia { get; set; }
        public string NombreEmpresa { get; set; }
        public string RucEmpresa { get; set; }
        public string FechaEmision { get; set; }
        public string FechaOperacion { get; set; }
        public ECotizacionCab Comprobante { get; set; }
        public ECliente Cliente { get; set; }
        public string Motivo { get; set; }
        public EUsuario Usuario { get; set; }
        public string Asunto { get; set; }
        public string Cantidad { get; set; }
        public string Precio { get; set; }
        public string Mensaje { get; set; }
        public int Item { get; set; }
        public EGuiadet Detalle { get; set; }
        public EProducto Producto { get; set; }

    }
}

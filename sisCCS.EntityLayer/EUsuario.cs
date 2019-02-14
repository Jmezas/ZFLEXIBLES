using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sisCCS.EntityLayer
{
    public class EUsuario
    {
        public EUsuario()
        {
            Pais = new EPais();
            Ubigeo = new EUbigeo();
        }

        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public EGeneral TipoDocumento { get; set; }
        public string NroDocumento { get; set; }
        public string Direccion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public DateTime FechaUltimoAcceso { get; set; }
        public int TiempoVidaPassword { get; set; }
        public DateTime FechaCaducidad { get; set; }
        public bool CambiarPassword { get; set; }
        public bool PermitirCambio { get; set; }
        public bool TipoCliente { get; set; }
        public EPais Pais { get; set; }
        public EUbigeo Ubigeo { get; set; }
        public EPerfil Perfil { get; set; }
        //public ESucursal Sucursal { get; set; }
        //public EVendedorCaraTurno VendedorCaraTurno { get; set; }
        public EUsuario UsuarioCreador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<EMenu> Menu { get; set; }
        public string ultimafechaoperacion { get; set; }
        public string IndicadorDia { get; set; }
        public int Turno { get; set; }
        public string IndicadorTurno { get; set; }

        public string ultimafechaoperacionNC { get; set; }
        public string IndicadorDiaNC { get; set; }
        public int TurnoNC { get; set; }
        public string IndicadorTurnoNC { get; set; }
        public string TipoSistema { get; set; }
        public string AplicaPlaca { get; set; }
        public string MensajeTicket { get; set; }
        

        public string NombreCompleto
        {
            get
            {
                return Nombre + (string.IsNullOrEmpty(ApellidoPaterno) ? "" : " " + ApellidoPaterno) + (string.IsNullOrEmpty(ApellidoMaterno) ? "" : " " + ApellidoMaterno);
            }
        }

        public string Nombres { get; set; }
    }
}

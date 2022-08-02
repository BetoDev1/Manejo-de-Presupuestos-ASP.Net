using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Models
{
    public class TransaccionModel
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        [Display(Name ="Fecha Transaccion")]
        [DataType(DataType.Date)]
        public DateTime FechaTransaccion { get; set; } = DateTime.Today;
        public decimal Monto { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage ="Debe seleccionar una categoria")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }
        [StringLength(maximumLength:1000, ErrorMessage = "La nota no puede pasar de 1000 caracteres")]
        public string Nota { get; set; }
        [Range(1, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta")]
        [Display(Name = "Cuenta")]
        public int CuentaId { get; set; }
        public TipoOperacion TipoOperacionId { get; set; } = TipoOperacion.Ingreso; //Cargo por defecto el primer Tipo de Operacion
        public string Cuenta { get; set; }
        public string Categoria { get; set; }

    }
}

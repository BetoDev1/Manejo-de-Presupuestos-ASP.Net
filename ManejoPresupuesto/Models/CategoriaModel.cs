using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Models
{
    public class CategoriaModel
    {
        public int id { get; set; }
        [Required(ErrorMessage="El campo {0} es requerido")]
        [StringLength(maximumLength:50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        public string Nombre { get; set; }
        [Display(Name = "Tipo Operacion")]
        public TipoOperacion TipoOperacionId { get; set; }
        public int UsuarioId { get; set; }


    }
}

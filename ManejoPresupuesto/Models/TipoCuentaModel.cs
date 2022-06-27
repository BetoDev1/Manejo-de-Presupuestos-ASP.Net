

using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuentaModel
    {
        public int Id { get; set; }        
        [Required(ErrorMessage ="El campo Nombre es obligatorio")]
        [PrimeraLetraMayuscula]
        [Remote(action:"VerificarExisteTipoCuenta", controller:"TiposCuentas")]
        public string Nombre { get; set; }
        public int UsuarioId { get; set; }
        public int Orden { get; set; }


        //pruebas de validaciones
        //[EmailAddress(ErrorMessage ="El campo debe ser un correo")]
        //public string Email { get; set; }

        //[Range(minimum: 18, maximum:130, ErrorMessage = "El valor debe estar entre {1} y {2}")]
        //public int Edad { get; set; }

        //[Url(ErrorMessage ="El campo debe ser una direccion de internet")]
        //public string URL { get; set; }

        //[CreditCard(ErrorMessage ="La tarjeta de credito no es valida")]
        //public string TarjetaDeCredito { get; set; }





    }
}
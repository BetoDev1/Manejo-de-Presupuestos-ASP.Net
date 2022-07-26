using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Models
{
    public class CuentaCreacionViewModel : CuentaModel
    {
        public IEnumerable<SelectListItem> TiposCuentas { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Models
{
    public class IndiceCuentasViewModel
    {
        public string TipoCuenta { get; set; }
        public IEnumerable<CuentaModel> Cuentas { get; set; }
        public decimal Balance => Cuentas.Sum(x => x.Balance);
        // Establezco que "Balance" va ser igual a la suma de los Balances de la "Cuenta" perteneciente a "TipoCuenta"
    }
}

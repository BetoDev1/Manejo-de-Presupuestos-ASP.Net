using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Servicios
{

    public interface IRepositorioCuentas
    {
        Task<IEnumerable<CuentaModel>> Buscar(int usuarioID);
        Task Crear(CuentaModel cuenta);
    }
    public class RepositorioCuentas : IRepositorioCuentas
    {

        private readonly string connectionString;
        public RepositorioCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(CuentaModel cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Cuentas (Nombre, TipoCuentaId, Descripcion, Balance)
                                                          VALUES (@Nombre, @TipoCuentaId, @Descripcion, @Balance);
                                                                SELECT SCOPE_IDENTITY()", cuenta);
            cuenta.Id = id;
        }

        public async Task<IEnumerable<CuentaModel>> Buscar(int usuarioID)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<CuentaModel>(@"SELECT Cuentas.Id, Cuentas.Nombre, Balance, TiposCuentas.Nombre as TipoCuenta
                                                            FROM Cuentas
                                                            INNER JOIN TiposCuentas
                                                            ON TiposCuentas.Id = Cuentas.TipoCuentaId
                                                            WHERE TiposCuentas.UsuarioId = @UsuarioId", new { usuarioID });
        }
    }
}

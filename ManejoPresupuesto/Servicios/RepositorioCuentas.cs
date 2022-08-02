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
        Task Actualizar(CuentaCreacionViewModel cuenta);
        Task Borrar(int id);
        Task<IEnumerable<CuentaModel>> Buscar(int usuarioID);
        Task Crear(CuentaModel cuenta);
        Task<CuentaModel> ObtenerPorId(int id, int usuarioId);
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

        public async Task<CuentaModel>ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<CuentaModel>(@"SELECT Cuentas.Id, Cuentas.Nombre, Balance, Descripcion, TipoCuentaId
                                                            FROM Cuentas
                                                            INNER JOIN TiposCuentas
                                                            ON TiposCuentas.Id = Cuentas.TipoCuentaId
                                                            WHERE TiposCuentas.UsuarioID = @UsuarioId AND Cuentas.Id = @Id", new { id, usuarioId });
        }

        public async Task Actualizar(CuentaCreacionViewModel cuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Cuentas SET Nombre = @Nombre,
                                                               Balance = @Balance,
                                                               Descripcion = @Descripcion,
                                                               TipoCuentaId = @TipoCuentaId
                                                               WHERE Id = @Id", cuenta);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Cuentas WHERE Id = @Id", new { id });
        }
    }
}

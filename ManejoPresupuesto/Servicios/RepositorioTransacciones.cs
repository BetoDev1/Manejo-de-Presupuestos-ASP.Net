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
    public interface IRepositorioTransacciones
    {
        Task Actualizar(TransaccionModel transaccion, decimal montoAnterior, int cuentaAnterior);
        Task Borrar(int id);
        Task Crear(TransaccionModel transaccion);
        Task<IEnumerable<TransaccionModel>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo);
        Task<TransaccionModel> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioTransacciones : IRepositorioTransacciones
    {
        private readonly string connectionString;

        public RepositorioTransacciones(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(TransaccionModel transaccion)
        {
            using var conncection = new SqlConnection(connectionString);
            var id = await conncection.QuerySingleAsync<int>("Transacciones_Insertar",
                new
                {
                    transaccion.UsuarioId,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota
                }, commandType: System.Data.CommandType.StoredProcedure);

            transaccion.Id = id;
        }

        public async Task<IEnumerable<TransaccionModel>> ObtenerPorCuentaId(ObtenerTransaccionesPorCuenta modelo)
        {
            using var conncection = new SqlConnection(connectionString);
            return await conncection.QueryAsync<TransaccionModel>(@"SELECT t.id, t.Monto, t.FechaTransaccion, c.Nombre as Categoria, 
                                                                    cu.Nombre as Cuenta, c.TipoOperacionId
                                                                    FROM Transacciones t
                                                                    INNER JOIN Categorias c
                                                                    ON c.Id = t.CategoriaId
                                                                    INNER JOIN Cuentas cu
                                                                    ON cu.Id = t.CuentaId
                                                                    WHERE t.CuentaId = @CuentaId AND t.UsuarioId = @UsuarioId
                                                                    AND FechaTransaccion BETWEEN @FechaInicio AND @FechaFin", modelo);
        }

        public async Task Actualizar(TransaccionModel transaccion, decimal montoAnterior, int cuentaAnteriorId)
        {
            using var conncection = new SqlConnection(connectionString);
            await conncection.ExecuteAsync("Transacciones_Actualizar", 
                new
                {
                    transaccion.Id,
                    transaccion.FechaTransaccion,
                    transaccion.Monto,
                    transaccion.CategoriaId,
                    transaccion.CuentaId,
                    transaccion.Nota,
                    montoAnterior,
                    cuentaAnteriorId

                }, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<TransaccionModel> ObtenerPorId(int id, int usuarioId)
        {
            using var conncection = new SqlConnection(connectionString);
            return await conncection.QueryFirstOrDefaultAsync<TransaccionModel>(@"SELECT Transacciones.*, cat.TipoOperacionId
                                                                            FROM Transacciones
                                                                            INNER JOIN Categorias cat
                                                                            ON cat.Id = Transacciones.CategoriaId
                                                                            WHERE Transacciones.id = @Id
                                                                            AND Transacciones.UsuarioId = @UsuarioId",
                                                                            new { id, usuarioId });
        }

        public async Task Borrar(int id)
        {
            using var conncection = new SqlConnection(connectionString);
            await conncection.ExecuteAsync("Transacciones_Borrar",
                new {id}, commandType: System.Data.CommandType.StoredProcedure);

        }
    }
}

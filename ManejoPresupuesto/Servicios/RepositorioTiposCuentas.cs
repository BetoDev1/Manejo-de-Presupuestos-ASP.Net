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

    public interface IRepositorioTiposCuentas
    {

        Task Actualizar(TipoCuentaModel tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuentaModel tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuentaModel>> Obtener(int usuarioId);
        Task<TipoCuentaModel> ObtenerPorId(int id, int usuarioId);
        Task Ordenar(IEnumerable<TipoCuentaModel> tipoCuentasOrdenados);
    }
    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Models.TipoCuentaModel tipocuenta)
        {
            using var connection = new SqlConnection(connectionString) ;
            var id = await connection.QuerySingleAsync<int>($@"insert into TiposCuentas (Nombre, UsuarioId, Orden)
                                                 values (@Nombre, @UsuarioId, 0);
                                                    SELECT SCOPE_IDENTITY()", tipocuenta);
            tipocuenta.Id = id;
        }

        public async Task<bool> Existe(string Nombre, int UsuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"select 1 from TiposCuentas where Nombre = @Nombre
                                                                           AND UsuarioID = @UsuarioId;", new { Nombre, UsuarioId });
            return existe == 1;

        }

        public async Task<IEnumerable<TipoCuentaModel>>Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<TipoCuentaModel>($"Select Id, Nombre, UsuarioId, Orden " +
                $"FROM TiposCuentas WHERE UsuarioId = @UsuarioId",
                new { usuarioId });

        }

        public async Task Actualizar(TipoCuentaModel tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE TiposCuentas SET Nombre = @Nombre WHERE Id = @Id", tipoCuenta);
        }

        public async Task<Models.TipoCuentaModel>ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuentaModel>(@"
                                                                SELECT Id, Nombre, Orden
                                                                FROM TiposCuentas
                                                                WHERE Id = @Id AND UsuarioId = @UsuarioId",
                                                                new { id, usuarioId });
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE TiposCuentas WHERE id = @Id", new { id });
        }

        public Task Ordenar(IEnumerable<TipoCuentaModel> tipoCuentasOrdenados)
        {
            throw new NotImplementedException();
        }
    }
}

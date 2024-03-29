﻿using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioCategorias
    {
        Task Actualizar(CategoriaModel categoria);
        Task Borrar(int id);
        Task Crear(CategoriaModel categoria);
        Task<IEnumerable<CategoriaModel>> Obtener(int usuarioId);
        Task<IEnumerable<CategoriaModel>> Obtener(int usuarioId, TipoOperacion tipoOperacionId);
        Task<CategoriaModel> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioCategorias : IRepositorioCategorias
    {
        private readonly string connectionString;
        public RepositorioCategorias(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Crear(Models.CategoriaModel categoria)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO Categorias (Nombre, TipoOperacionId, UsuarioId)
                                                                Values(@Nombre, @TipoOperacionId, @UsuarioId)
                                                                SELECT SCOPE_IDENTITY()", categoria);

            categoria.id = id;
        }

        //Obtenemos todas las categorias de un usuario determinado
        public async Task<IEnumerable<CategoriaModel>>Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<CategoriaModel>("SELECT *FROM Categorias WHERE UsuarioId = @usuarioId", new { usuarioId });
        }

        //Este segundo metodo Obtener es para utilizarlo en TransaccionesController para obtener el Tipo de Operacion
        public async Task<IEnumerable<CategoriaModel>> Obtener(int usuarioId, TipoOperacion tipoOperacionId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<CategoriaModel>(@"SELECT *FROM Categorias " +
                                                             "WHERE UsuarioId = @usuarioId AND TipoOperacionId = @TipoOperacionId",
                                                             new { usuarioId, tipoOperacionId });
        }

        public async Task<CategoriaModel> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<CategoriaModel>(@"SELECT *
                                                            FROM Categorias                                                           
                                                            WHERE Id = @Id AND UsuarioId = @UsuarioId", 
                                                            new { id, usuarioId });
        }
        public async Task Actualizar(CategoriaModel categoria)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Categorias SET Nombre = @Nombre, TipoOperacionId = @TipoOperacionId
                                           WHERE Id = @Id", categoria);
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Categorias WHERE Id = @Id", new { id });
        }
    }

}


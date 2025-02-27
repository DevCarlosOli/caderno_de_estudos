using CaderDeEstudosAPI.Domain.Models;
using CaderDeEstudosAPI.Infra.DbContext;
using CaderDeEstudosAPI.Infra.Repositories.Interfaces;
using System.Data;

namespace CaderDeEstudosAPI.Infra.Repositories {
    public class CadernoRepository : ICadernoRepository {
        private readonly DbPostegreSql _dbPostegreSql;

        public CadernoRepository(DbPostegreSql dbPostegreSql) {
            _dbPostegreSql = dbPostegreSql;
        }

        public async Task<List<Caderno>> FindAllCadernosAsync() {
            var cadernos = new List<Caderno>();

            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        SELECT
                                                CADERNO_ID,
                                                NOME,
                                                DESCRICAO,
                                                DATA_CRIACAO
                                         FROM   CADERNO
                    ";

                    using (var dr = await cmd.ExecuteReaderAsync()) {
                        while (await dr.ReadAsync()) { 
                            var caderno = new Caderno();
                            caderno.CadernoId = dr.GetInt32("CADERNO_ID");
                            caderno.Nome = dr.GetString("NOME");
                            caderno.Descricao = dr.IsDBNull("DESCRICAO") ? null : dr.GetString("DESCRICAO");
                            caderno.DataCriacao = dr.GetDateTime("DATA_CRIACAO");

                            cadernos.Add(caderno);
                        }
                    }
                }
            }
            return cadernos;
        }

        public async Task<Caderno> AddCadernoAsync(Caderno caderno) {
            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        INSERT INTO CADERNO(
                                                        NOME,
                                                        DESCRICAO,
                                                        DATA_CRIACAO
                                                    )VALUES(
                                                        @NOME,
                                                        @DESCRICAO,
                                                        @DATA_CRIACAO
                                                    )RETURNING CADERNO_ID
                    ";

                    cmd.Parameters.AddWithValue("@NOME", caderno.Nome);
                    cmd.Parameters.AddWithValue("@DESCRICAO", string.IsNullOrEmpty(caderno.Descricao) ? "" : caderno.Descricao);
                    cmd.Parameters.AddWithValue("@DATA_CRIACAO", caderno.DataCriacao);

                    var cadernoId = await cmd.ExecuteScalarAsync();
                    caderno.CadernoId = (int)cadernoId;

                    return caderno;
                }
            }
        }
    }
}

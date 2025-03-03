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

        public async Task<Caderno> FindCadernoByIdAsync(int cadernoId) {
            var caderno = new Caderno();
            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        SELECT
                                                CADERNO_ID,
                                                NOME,
                                                DESCRICAO,
                                                DATA_CRIACAO
                                          FROM  CADERNO
                                         WHERE  CADERNO_ID = @CADERNO_ID
                    ";

                    cmd.Parameters.AddWithValue("@CADERNO_ID", cadernoId);

                    using (var dr = await cmd.ExecuteReaderAsync()) {
                        if (await dr.ReadAsync()) {
                            caderno.CadernoId = dr.GetInt32("CADERNO_ID");
                            caderno.Nome = dr.GetString("NOME");
                            caderno.Descricao = dr.IsDBNull("DESCRICAO") ? "" : dr.GetString("DESCRICAO");
                            caderno.DataCriacao = dr.GetDateTime("DATA_CRIACAO");

                            return caderno;
                        }
                    }
                }
            }
            return null;
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

        public async Task<Caderno> UpdateCadernoAsync(int cadernoId, Caderno caderno) {
            using (var conn = _dbPostegreSql.GetConnection()) { 
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        UPDATE 
                                                CADERNO 
                                           SET 
                                                NOME = COALESCE(@NOME, NOME),
                                                DESCRICAO = COALESCE(@DESCRICAO, DESCRICAO)
                                         WHERE  CADERNO_ID = @CADERNO_ID
                                     RETURNING  CADERNO_ID
                    ";

                    cmd.Parameters.AddWithValue("@NOME", caderno.Nome ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DESCRICAO", caderno.Descricao ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CADERNO_ID", cadernoId);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result != null && int.TryParse(result.ToString(), out int id))
                        return await FindCadernoByIdAsync(id);
                }
            }
            return null;
        }

        public async Task<int> DeleteCadernoAsync(int cadernoId) {
            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        DELETE FROM CADERNO
                                              WHERE CADERNO_ID = @CADERNO_ID
                    ";

                    cmd.Parameters.AddWithValue("@CADERNO_ID", cadernoId);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

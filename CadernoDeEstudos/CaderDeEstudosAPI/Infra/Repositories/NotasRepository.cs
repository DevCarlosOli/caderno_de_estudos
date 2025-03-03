using CaderDeEstudosAPI.Domain.Models;
using CaderDeEstudosAPI.Infra.DbContext;
using CaderDeEstudosAPI.Infra.Repositories.Interfaces;
using System.Data;

namespace CaderDeEstudosAPI.Infra.Repositories {
    public class NotasRepository : INotasRepository {
        private readonly DbPostegreSql _dbPostegreSql;

        public NotasRepository(DbPostegreSql dbPostegreSql) {
            _dbPostegreSql = dbPostegreSql;
        }

        public async Task<List<Notas>> FindAllNotasAsync() {
            var notas = new List<Notas>();
            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        SELECT
                                                NOTAS_ID,
                                                TITULO,
                                                CONTEUDO,
                                                DATA_CRIACAO,
                                                CADERNO_ID
                                         FROM   NOTAS
                    ";

                    using (var dr = await cmd.ExecuteReaderAsync()) {
                        while (await dr.ReadAsync()) {
                            var nota = new Notas();
                            nota.NotasId = dr.GetInt32("NOTAS_ID");
                            nota.Titulo = dr.GetString("TITULO");
                            nota.Conteudo = dr.GetString("CONTEUDO");
                            nota.DataCriacao = dr.GetDateTime("DATA_CRIACAO");
                            nota.Caderno.CadernoId = dr.GetInt32("CADERNO_ID");

                            notas.Add(nota);
                        }
                    }
                }
            }
            return notas;
        }

        public async Task<Notas> FindNotasByIdAsync(int notasId) {
            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        SELECT
                                                NOTAS_ID,
                                                TITULO,
                                                CONTEUDO,
                                                DATA_CRIACAO,
                                                CADERNO_ID
                                         FROM   NOTAS
                                        WHERE   NOTAS_ID = @NOTAS_ID
                    ";

                    cmd.Parameters.AddWithValue("@NOTAS_ID", notasId);

                    using (var dr = await cmd.ExecuteReaderAsync()) {
                        if (await dr.ReadAsync()) {
                            var notas = new Notas();
                            notas.NotasId = dr.GetInt32("NOTAS_ID");
                            notas.Titulo = dr.GetString("TITULO");
                            notas.Conteudo = dr.GetString("CONTEUDO");
                            notas.DataCriacao = dr.GetDateTime("DATA_CRIACAO");
                            notas.Caderno.CadernoId = dr.GetInt32("CADERNO_ID");

                            return notas;
                        }
                    }
                }
            }
            return null;
        }

        public async Task<Notas> AddNotasAsync(Notas notas) {
            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        SELECT
                                                CADERNO_ID
                                          FROM  CADERNO
                                         WHERE  NOME ILIKE '%' || @NOME || '%'
                                         LIMIT  1
                    ";

                    cmd.Parameters.AddWithValue("@NOME", notas.Caderno.Nome);

                    var cadernoId = await cmd.ExecuteScalarAsync();

                    if (cadernoId == null)
                        throw new Exception("Caderno não encontrado");

                    notas.Caderno.CadernoId = (int)cadernoId;
                }

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        INSERT INTO NOTAS(
                                                        TITULO,
                                                        CONTEUDO,
                                                        DATA_CRIACAO,
                                                        CADERNO_ID
                                                    )
                                                    VALUES(
                                                        @TITULO,
                                                        @CONTEUDO,
                                                        @DATA_CRIACAO,
                                                        @CADERNO_ID
                                                    )RETURNING NOTAS_ID
                    ";

                    cmd.Parameters.AddWithValue("@TITULO", notas.Titulo);
                    cmd.Parameters.AddWithValue("@CONTEUDO", notas.Conteudo);
                    cmd.Parameters.AddWithValue("@DATA_CRIACAO", notas.DataCriacao);
                    cmd.Parameters.AddWithValue("@CADERNO_ID", notas.Caderno.CadernoId);

                    var notasId = await cmd.ExecuteScalarAsync();
                    notas.NotasId = (int)notasId;

                    return notas;
                }
            }
        }

        public async Task<Notas> UpdateNotasAsync(int notasId, Notas notas) {
            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        UPDATE
                                                NOTAS
                                           SET
                                                TITULO = COALESCE(@TITULO, TITULO),
                                                CONTEUDO = COALESCE(@CONTEUDO, CONTEUDO)
                                         WHERE  NOTAS_ID = @NOTAS_ID
                                     RETURNING  NOTAS_ID
                    ";

                    cmd.Parameters.AddWithValue("@TITULO", notas.Titulo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CONTEUDO", notas.Conteudo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NOTAS_ID", notasId);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result != null && int.TryParse(result.ToString(), out int id)) {
                        return await FindNotasByIdAsync(notasId);
                    }
                }
            }
            return new Notas();
        }

        public async Task<int> DeleteNotasAsync(int notasId) {
            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        DELETE FROM NOTAS
                                              WHERE NOTAS_ID = @NOTAS_ID
                    ";

                    cmd.Parameters.AddWithValue("@NOTAS_ID", notasId);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

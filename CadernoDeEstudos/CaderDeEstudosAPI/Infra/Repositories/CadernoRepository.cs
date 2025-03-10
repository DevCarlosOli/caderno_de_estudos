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
            var cadernos = new Dictionary<int, Caderno>();

            using (var conn = _dbPostegreSql.GetConnection()) {
                await conn.OpenAsync();

                using (var cmd = conn.CreateCommand()) {
                    cmd.CommandText = @"
                                        SELECT 
		                                        C.CADERNO_ID,
		                                        C.NOME,
		                                        C.DESCRICAO,
		                                        C.DATA_CRIACAO AS CRIACAO_CADERNO,
		                                        N.NOTAS_ID,
		                                        N.TITULO,
		                                        N.CONTEUDO,
		                                        N.DATA_CRIACAO AS CRIACAO_NOTA
	                                      FROM  CADERNO C
	                                 LEFT JOIN  NOTAS N ON N.CADERNO_ID = C.CADERNO_ID
                    ";

                    using (var dr = await cmd.ExecuteReaderAsync()) {
                        while (await dr.ReadAsync()) {
                            int cadernoId = dr.GetInt32("CADERNO_ID");

                            if (!cadernos.TryGetValue(cadernoId, out var caderno)) {
                                caderno = new Caderno();
                                caderno.CadernoId = cadernoId;
                                caderno.Nome = dr.GetString("NOME");
                                caderno.Descricao = dr.IsDBNull("DESCRICAO") ? null : dr.GetString("DESCRICAO");
                                caderno.DataCriacao = dr.GetDateTime("CRIACAO_CADERNO");
                                caderno.Notas = new List<Notas>();

                                cadernos.Add(cadernoId, caderno);
                            }

                            if (!dr.IsDBNull("NOTAS_ID")) {
                                var nota = new Notas();
                                nota.NotasId = dr.GetInt32("NOTAS_ID");
                                nota.Titulo = dr.GetString("TITULO");
                                nota.Conteudo = dr.IsDBNull("CONTEUDO") ? null : dr.GetString("CONTEUDO");
                                nota.DataCriacao = dr.GetDateTime("CRIACAO_NOTA");

                                caderno.Notas.Add(nota);
                            }
                        }
                    }
                }
            }
            return cadernos.Values.ToList();
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

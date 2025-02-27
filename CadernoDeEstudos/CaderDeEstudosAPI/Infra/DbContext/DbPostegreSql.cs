using Npgsql;

namespace CaderDeEstudosAPI.Infra.DbContext {
    public class DbPostegreSql {
        private readonly string _connectionString;

        public DbPostegreSql(IConfiguration connectionString) {
            _connectionString = connectionString.GetConnectionString("DbPostegreSql");
        }

        public NpgsqlConnection GetConnection() { 
            return new NpgsqlConnection(_connectionString);
        }
    }
}

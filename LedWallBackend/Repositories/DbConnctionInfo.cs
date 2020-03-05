namespace LedWallBackend.Repositories
{
    public class DbConnctionInfo
    {
        public DbConnctionInfo(string connectionString)
        {
            ConnectionString = !string.IsNullOrEmpty(connectionString) ? connectionString : "mongodb://localhost:27017/";
        }

        public string ConnectionString { get; }
    }
}
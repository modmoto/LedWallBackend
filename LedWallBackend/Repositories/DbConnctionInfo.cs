namespace LedWallBackend.Repositories
{
    public class DbConnctionInfo
    {
        public DbConnctionInfo(string connectionString)
        {
            ConnectionString = !string.IsNullOrEmpty(connectionString) ? connectionString : "mongodb://h2865571.stratoserver.net:4001/?readPreference=primary&ssl=false";
        }

        public string ConnectionString { get; }
    }
}
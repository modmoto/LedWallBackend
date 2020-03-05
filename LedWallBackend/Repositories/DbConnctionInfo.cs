namespace LedWallBackend.Repositories
{
    public class DbConnctionInfo
    {
        public DbConnctionInfo(string connectionString)
        {
            ConnectionString = !string.IsNullOrEmpty(connectionString) ? connectionString : "mongodb+srv://mongoDbTestUser:meinTestPw@cluster0-xhbcb.azure.mongodb.net/test?retryWrites=true&w=majority";
        }

        public string ConnectionString { get; }
    }
}
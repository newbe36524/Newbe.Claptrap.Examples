namespace HelloClaptrap.Repository
{
    public class DbOptions
    {
        public const string DefaultDbFilename = "d:/orderdb.db";

        public string OrderDbConnectionString { get; set; } =
            $"Data Source={DefaultDbFilename};Cache Size=5000;Journal Mode=WAL;Pooling=True;Default IsolationLevel=ReadCommitted";
    }
}
namespace Newbe.Claptrap.Auth.Models
{
    public class ClaptrapClusteringOptions
    {
        public string DefaultConnectionString { get; set; }
        public ClaptrapOrleansOptions Orleans { get; set; } = new ClaptrapOrleansOptions();
    }
}
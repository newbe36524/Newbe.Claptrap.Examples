namespace Newbe.Claptrap.Auth.Models
{
    public class ClaptrapOrleansOptions
    {
        public string Hostname { get; set; }
        public int? GatewayPort { get; set; }
        public int? SiloPort { get; set; }
        public bool EnableDashboard { get; set; }
        public ClaptrapOrleansClusteringOptions Clustering { get; set; } = new ClaptrapOrleansClusteringOptions();
    }
}
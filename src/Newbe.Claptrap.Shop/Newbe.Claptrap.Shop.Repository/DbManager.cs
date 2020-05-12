using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Logging;

namespace Newbe.Claptrap.Shop.Repository
{
    public class DbManager : IDbManager
    {
        private readonly ILogger<DbManager> _logger;
        private readonly IDbConnectionStringFactory _dbConnectionStringFactory;
        private readonly IDbFactory _dbFactory;

        public DbManager(
            ILogger<DbManager> logger,
            IDbConnectionStringFactory dbConnectionStringFactory,
            IDbFactory dbFactory)
        {
            _logger = logger;
            _dbConnectionStringFactory = dbConnectionStringFactory;
            _dbFactory = dbFactory;
        }

        public Task InitAsync()
        {
            var dbMigration =
                DeployChanges.To
                    .PostgresqlDatabase(_dbConnectionStringFactory.GetMainDb())
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToAutodetectedLog()
                    .LogToConsole()
                    .WithVariablesEnabled()
                    .Build();

            var result = dbMigration.PerformUpgrade();

            if (!result.Successful)
            {
                throw new Exception("db migration failed", result.Error);
            }

            if (result.Scripts.Any())
            {
                _logger.LogInformation("db migration is success.");
            }
            else
            {
                _logger.LogDebug("db schema is latest, do nothing to migration");
            }

            _logger.LogDebug("db migration log:{log}", WriteExecutedScriptsToOctopusTaskSummary(result));
            return Task.CompletedTask;
        }


        private static string WriteExecutedScriptsToOctopusTaskSummary(DatabaseUpgradeResult result)
        {
            var sb = new StringBuilder();
            sb.AppendLine("##octopus[stdout-highlight]");
            sb.AppendLine($"Ran {result.Scripts.Count()} script{(result.Scripts.Count() == 1 ? "" : "s")}");
            foreach (var script in result.Scripts)
            {
                sb.AppendLine(script.Name);
                sb.AppendLine(script.Contents);
            }

            sb.AppendLine("##octopus[stdout-default]");
            return sb.ToString();
        }

        public Task RemoveAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
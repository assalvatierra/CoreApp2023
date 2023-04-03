using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Reporting.Data {
    public class SqlDataConnectionDescription : DataConnection { }
    public class JsonDataConnectionDescription : DataConnection { }
    public abstract class DataConnection {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ConnectionString { get; set; }
    }

    public class ReportItem {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public byte[] LayoutData { get; set; }
    }

    public class ReportDbContext : DbContext {
        public DbSet<JsonDataConnectionDescription> JsonDataConnections { get; set; }
        public DbSet<SqlDataConnectionDescription> SqlDataConnections { get; set; }
        public DbSet<ReportItem> Reports { get; set; }
        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options) {
        }
        public void InitializeDatabase() {
            Database.EnsureCreated();

            var nwindJsonDataConnectionName = "NWindProductsJson";
            if(!JsonDataConnections.Any(x => x.Name == nwindJsonDataConnectionName)) {
                var newData = new JsonDataConnectionDescription {
                    Name = nwindJsonDataConnectionName,
                    DisplayName = "Northwind Products (JSON)",
                    ConnectionString = "Uri=Data/nwind.json"
                };
                JsonDataConnections.Add(newData);
            }


            var nwindSqlDataConnectionName = "NWindConnectionString";
            if(!SqlDataConnections.Any(x => x.Name == nwindSqlDataConnectionName)) {
                var newData = new SqlDataConnectionDescription {
                    Name = nwindSqlDataConnectionName,
                    DisplayName = "Northwind Data Connection",
                    ConnectionString = "XpoProvider=SQLite;Data Source=|DataDirectory|/Data/nwind.db"
                };
                SqlDataConnections.Add(newData);
            }

            var reportsDataConnectionName = "ReportsDataSqlite";
            if(!SqlDataConnections.Any(x => x.Name == reportsDataConnectionName)) {
                var newData = new SqlDataConnectionDescription {
                    Name = reportsDataConnectionName,
                    DisplayName = "Reports Data (Demo)",
                    ConnectionString = "XpoProvider=SQLite;Data Source=|DataDirectory|/Data/reportsData.db"
                };
                SqlDataConnections.Add(newData);
            }

            var mssql_reports = "ReportsMssqlServer";
            if (!SqlDataConnections.Any(x => x.Name == reportsDataConnectionName))
            {
                var newData = new SqlDataConnectionDescription
                {
                    Name = mssql_reports,
                    DisplayName = "Reports MsSQL Server",
                    ConnectionString = "Data Source=SQL5063.site4now.net;Initial Catalog=db_a0a0ae_inventorydev;User Id=db_a0a0ae_inventorydev_admin;Password=inventorydev123!;"
                };
                SqlDataConnections.Add(newData);
            }

            SaveChanges();
        }
    }
}
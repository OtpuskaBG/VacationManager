using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
//using VacationManager.Data.Entities; -- To be added.
using VacationManager.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VacationManager.Data
{
    public class ApplicationDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            if (args.Length != 1) throw new InvalidOperationException
                ("You need to pass the connection string to use the only argument!");
            string connectionString = args[0];

            DbContextOptionsBuilder<ApplicationDbContext> optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            ApplicationDbContext dbContext = new ApplicationDbContext(optionsBuilder.Options);

            return dbContext;
        }
    }
}
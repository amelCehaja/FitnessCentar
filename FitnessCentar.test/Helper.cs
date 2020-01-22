using FitnessCentar.data.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitnessCentar.test
{
    class Helper
    {
        public static MyContext GetDLWMSDbContext()
        {
            DbContextOptionsBuilder<MyContext> builder =
                new DbContextOptionsBuilder<MyContext>().UseSqlServer("Server = localhost; Database = RS1seminarski; Trusted_Connection = true; MultipleActiveResultSets = true; User ID =; Password = ");
            return new MyContext(builder.Options);
        }
    }
}

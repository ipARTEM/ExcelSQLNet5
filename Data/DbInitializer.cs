using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSQLNet5.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ExcelTableContext context)
        {
            context.Database.EnsureCreated();

            if (context.ListExcelsViewModels.Any())
            {
                return;
            }

            

        }

        


    }
}

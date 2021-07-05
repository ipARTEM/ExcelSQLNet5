using ExcelSQLNet5.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSQLNet5.Controllers
{
    public class SQLTableController : Controller
    {
        private readonly ExcelTableContext _context;

        public SQLTableController(ExcelTableContext context)
        {
            _context = context;

        }


        public async Task <IActionResult> SQLTable()
        {
            return View(await _context.ListExcelsViewModels.ToListAsync());
        }
    }
}

using ClosedXML.Excel;
using ExcelSQLNet5.Data;
using ExcelSQLNet5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSQLNet5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ExcelTableContext _context;


        public HomeController(ILogger<HomeController> logger, ExcelTableContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IndexExcel()
        {
            return View( await _context.ListExcelsViewModels.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                ListExcelsViewModel viewModel = new ListExcelsViewModel();
                using (XLWorkbook workBook = new XLWorkbook(fileExcel.OpenReadStream(), XLEventTracking.Disabled))
                {
                    foreach (IXLWorksheet worksheet in workBook.Worksheets)
                    {
                        ListExcel oneList = new ListExcel();
                        oneList.Title = worksheet.Name;

                        foreach (IXLColumn column in worksheet.ColumnsUsed())
                        {
                            ColumnName withoutFirstColumn = new ColumnName();
                            withoutFirstColumn.Title = column.Cell(1).Value.ToString();

                            foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                            {
                                try
                                {
                                    CellPosition firstColumn = new CellPosition();
                                    firstColumn.FirstName = row.Cell(1).Value.ToString();
                                    firstColumn.Cell = row.Cell(column.ColumnNumber()).Value.ToString();
                                    withoutFirstColumn.ColumnPositions.Add(firstColumn);

                                }
                                catch (Exception e)
                                {
                                    //logging
                                    viewModel.ErrorsTotal++;
                                }
                            }

                            oneList.CollectionOfSheets.Add(withoutFirstColumn);
                        }
                        viewModel.ListExcels.Add(oneList);
                    }
                }
                //например, здесь сохраняем все таблицу в БД

                _context.Add(viewModel);
                _context.SaveChanges();

                return View(viewModel);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Export()
        {
            List<ListExcel> phoneBrands = new List<ListExcel>();
            phoneBrands.Add(new ListExcel()
            {
                Title = "Apple",
                CollectionOfSheets = new List<ColumnName>()
            {
                new ColumnName() { Title = "iPhone 7"},
                new ColumnName() { Title = "iPhone 7 Plus"}
            }
            });
            phoneBrands.Add(new ListExcel()
            {
                Title = "Samsung",
                CollectionOfSheets = new List<ColumnName>()
            {
                new ColumnName() { Title = "A3"},
                new ColumnName() { Title = "A3 2016"},
                new ColumnName() { Title = "A3 2017"}
            }
            });

            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var worksheet = workbook.Worksheets.Add("Brands");

                worksheet.Cell("A1").Value = "Бренд";
                worksheet.Cell("B1").Value = "Модели";
                worksheet.Row(1).Style.Font.Bold = true;

                //нумерация строк/столбцов начинается с индекса 1 (не 0)
                for (int i = 0; i < phoneBrands.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = phoneBrands[i].Title;
                    worksheet.Cell(i + 2, 2).Value = string.Join(", ", phoneBrands[i].CollectionOfSheets.Select(x => x.Title));
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"brands_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}

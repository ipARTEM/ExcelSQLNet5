using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSQLNet5.Models
{
    public class ListExcelsViewModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        public ListExcelsViewModel()
        {
            ListExcels = new List<ListExcel>();
        }

        public List<ListExcel> ListExcels { get; set; }

        //кол-во ошибок при импорте
        public int ErrorsTotal { get; set; }
    }
}

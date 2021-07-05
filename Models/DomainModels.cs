using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSQLNet5.Models
{
    public class CellPosition
    {
        
        public int ID { get; set; }

        public string FirstName { get; set; }


        public string Cell { get; set; }
    }

    public class ColumnName
    {
        public ColumnName()
        {
            ColumnPositions = new List<CellPosition>();
        }

       
        public int ID { get; set; }


        public string Title { get; set; }

        public List<CellPosition> ColumnPositions { get; set; }
    }

    public class ListExcel
    {
        
        public int ID { get; set; }

        public ListExcel()
        {
            CollectionOfSheets = new List<ColumnName>();
        }

        //Название листа
        public string Title { get; set; }

        public List<ColumnName> CollectionOfSheets { get; set; }
    }
}
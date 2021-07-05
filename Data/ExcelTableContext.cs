using ExcelSQLNet5.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExcelSQLNet5.Data
{
    public class ExcelTableContext : DbContext
    {
        public ExcelTableContext(DbContextOptions<ExcelTableContext> options):base( options)
        {

        } 

        public DbSet<ListExcelsViewModel> ListExcelsViewModels { get; set; }
        public DbSet<CellPosition> CellPositions { get; set; }
        public DbSet<ColumnName> ColumnNames { get; set; }
        public DbSet<ListExcel> ListExcels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ListExcelsViewModel>().ToTable("Excel");
            modelBuilder.Entity<CellPosition>().ToTable("CellPosition");
            modelBuilder.Entity<ColumnName>().ToTable("ColumnName");
            modelBuilder.Entity<ListExcel>().ToTable("ListExcel");
        }



    }
}

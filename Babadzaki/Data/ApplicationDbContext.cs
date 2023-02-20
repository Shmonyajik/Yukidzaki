
using Microsoft.EntityFrameworkCore;
using Babadzaki.Models;
using System.Reflection.Metadata;


namespace Babadzaki.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>()
                .ToTable(tb => tb.HasTrigger("UpdateTotalTokensNumAfterUpdate"));//костыль
        }
        // TODO: Подумать что использовать триггеры или хранимые процедуры и переименовать UpdateTotalTokensNumAfterUpdate
        //https://learn.microsoft.com/ru-ru/ef/core/what-is-new/ef-core-7.0/breaking-changes#sqlserver-tables-with-triggers
        //https://powerusers.microsoft.com/t5/Building-Power-Apps/Error-when-submitting-quot-new-quot-form-to-SQL/m-p/394338
        //https://habr.com/ru/post/310040/

        public DbSet<Token> Tokens { get; set; }

        public DbSet<SeasonCollection> SeasonCollections { get; set; }
    }
}
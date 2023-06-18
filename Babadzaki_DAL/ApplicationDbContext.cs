
using Microsoft.EntityFrameworkCore;
using Babadzaki_Domain.Models;
using System.Reflection.Metadata;


namespace Babadzaki_DAL
{

    public class ApplicationDbContext:DbContext /*: IdentityDbContext*/
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //The entity type 'IdentityUserLogin<string>' requires a primary key to be defined.
            //If you intended to use a keyless entity type, call 'HasNoKey' in 'OnModelCreating'.
            //For more information on keyless entity types, see https://go.microsoft.com/fwlink/?linkid=2141943
            
        }
        // TODO: Подумать что использовать триггеры или хранимые процедуры и переименовать UpdateTotalTokensNumAfterUpdate
        //https://learn.microsoft.com/ru-ru/ef/core/what-is-new/ef-core-7.0/breaking-changes#sqlserver-tables-with-triggers
        //https://powerusers.microsoft.com/t5/Building-Power-Apps/Error-when-submitting-quot-new-quot-form-to-SQL/m-p/394338
        //https://habr.com/ru/post/310040/

        public DbSet<Token> Tokens { get; set; }

        public DbSet<SeasonCollection> SeasonCollections { get; set; }


        public DbSet<Email> Emails { get; set; }

        public DbSet<Filter> Filters { get; set; }
        public DbSet<TokensFilters> TokensFilters { get; set; }

    }
}
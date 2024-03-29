﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sgart.Net.ConsoleApp.Data;

namespace Sgart.Net.ConsoleApp.Data
{
    /// <summary>
    /// serve per creare le migration in un progetto differente da quello principale (Sgart.Net.ConsoleApp)
    /// </summary>
    public class SgartDbContextFactory : IDesignTimeDbContextFactory<SgartDbContext>
    {
        public SgartDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SgartDbContext>();
            optionsBuilder.UseSqlServer("Data Source=...impostare la stringa di connessione per fare il remove delle migration...");

            return new SgartDbContext(optionsBuilder.Options);
        }
    }
}

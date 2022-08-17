using System;
using Microsoft.EntityFrameworkCore;
using QrCodeValidator.Models;

namespace QrCodeValidator.Data
{
	public class AppDbContext:DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
		}
		public DbSet<Utxc> GetUtxcs { get; set; }
	}
}


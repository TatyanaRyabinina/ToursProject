using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DAL.Models
{
	public class UserContext : DbContext
	{
		public UserContext() :
			base("DefaultConnection")
		{  }
		public DbSet<Client> Clients { get; set; }
		public DbSet<OrderedTour> OrderedTours { get; set; }
		public DbSet<Excursion> Excursions { get; set; }

	}
}
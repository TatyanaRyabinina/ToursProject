using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DAL.Models;

namespace DAL.Models
{
	public class TourProjectDBInitializer : DropCreateDatabaseAlways<UserContext>
	{
		protected override void Seed(UserContext db)
		{
			db.Clients.Add(new Client { Email = "tatyana@gmail.com", Password = "123123", FirstName = "Tatyana", LastName = "Ryabinina", PhotoPath = "tatyana@gmail.com.png" });
			db.Clients.Add(new Client { Email = "jgreen@gmail.com", Password = "Qwerty", FirstName = "John", LastName = "Green", PhotoPath = "jgreen@gmail.com.png" });
			db.Excursions.Add(new Excursion { ExcursionName = "China" });
			db.Excursions.Add(new Excursion { ExcursionName = "England" });
			db.Excursions.Add(new Excursion { ExcursionName = "Czech Republic" });

			db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 10, 23), ExcursionId = 2, ClientId = 2 });
			db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 05, 2), ExcursionId = 1, ClientId = 1});
			db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 04, 14), ExcursionId = 3, ClientId = 1 });

			base.Seed(db);
		}
	}
}

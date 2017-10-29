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
            db.Clients.Add(new Client { Email = "qwerty@gmail.com", Password = "159753zx", FirstName = "Wye", LastName = "Oak", PhotoPath = "qwerty@gmail.com.png" });
            db.Clients.Add(new Client { Email = "moss@gmail.com", Password = "mr.robot", FirstName = "Angela", LastName = "Moss", PhotoPath = "moss@gmail.com.png" });

            db.Excursions.Add(new Excursion { ExcursionName = "China" });
			db.Excursions.Add(new Excursion { ExcursionName = "England" });
			db.Excursions.Add(new Excursion { ExcursionName = "Czech Republic" });

            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 1, ExcursionSightName = "Meridian Gate" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 1, ExcursionSightName = "Grittleton House" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 1, ExcursionSightName = "Panda Breeding Center" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 1, ExcursionSightName = "Zhouzhuang Water Village" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 1, ExcursionSightName = "Macau" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 3, ExcursionSightName = "Prague" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 3, ExcursionSightName = "Bohemian and Saxon Switzerland National Park" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 3, ExcursionSightName = "Cesky Krumlov" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 3, ExcursionSightName = "Terezin Concentration Camp" });
            db.ExcursionSights.Add(new ExcursionSight { ExcursionId = 3, ExcursionSightName = "Kutna Hora" });

            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 10, 23), ExcursionId = 2, ClientId = 2 });
            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 10, 23), ExcursionId = 2, ClientId = 1 });
            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 10, 27), ExcursionId = 2, ClientId = 3 });
            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 11, 21), ExcursionId = 1, ClientId = 3 });
            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 12, 21), ExcursionId = 1, ClientId = 4 });
            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 12, 27), ExcursionId = 3, ClientId = 4 });
            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2017, 12, 25), ExcursionId = 1, ClientId = 2 });
            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2018, 02, 11), ExcursionId = 3, ClientId = 3 });
            db.OrderedTours.Add(new OrderedTour { Date = new DateTime(2018, 02, 11), ExcursionId = 3, ClientId = 2 });

            db.OrderedTour_ExcursionSights.Add(new OrderedTour_ExcursionSight { OrderedTourId = 4, ExcursionSightId = 2, OrdinalNumber = 0 });
            db.OrderedTour_ExcursionSights.Add(new OrderedTour_ExcursionSight { OrderedTourId = 4, ExcursionSightId = 1, OrdinalNumber = 1 });
            db.OrderedTour_ExcursionSights.Add(new OrderedTour_ExcursionSight { OrderedTourId = 5, ExcursionSightId = 6, OrdinalNumber = 0 });
            db.OrderedTour_ExcursionSights.Add(new OrderedTour_ExcursionSight { OrderedTourId = 5, ExcursionSightId = 8, OrdinalNumber = 1 });
            db.OrderedTour_ExcursionSights.Add(new OrderedTour_ExcursionSight { OrderedTourId = 5, ExcursionSightId = 10, OrdinalNumber = 2 });





            base.Seed(db);
		}
	}
}

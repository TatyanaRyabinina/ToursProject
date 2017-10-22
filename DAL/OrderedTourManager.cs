using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
	public class OrderedTourManager
	{
		public List<OrderedTour> GetAllTours()
		{
			using (UserContext db = new UserContext())
			{
				return db.OrderedTours.ToList();
			}
		}

        public OrderedTour GetTourInfo(int id)
        {
            using (UserContext db = new UserContext())
            {
                return db.OrderedTours.Include(e => e.Excursion).Include(c => c.Client).FirstOrDefault(u => u.OrderedTourId == id);
            }
        }

		public static Boolean AddNewTour(OrderedTour objTour)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					db.OrderedTours.Add(objTour);
					db.SaveChanges();
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
        public static Boolean EditTourInfo(int Id, DateTime Date, int ClientId, int ExcursionId)
        {
            try
            {
                using (UserContext db = new UserContext())
                {
                    OrderedTour tour = db.OrderedTours.Find(Id);
                    tour.Date = Date;
                    tour.ClientId = ClientId;
                    tour.ExcursionId = ExcursionId;
                    //   db.Entry(prod).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

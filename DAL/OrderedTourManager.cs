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

		public static int AddNewTour(OrderedTour objTour)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					db.OrderedTours.Add(objTour);
					db.SaveChanges();
				}
				return objTour.OrderedTourId;
			}
			catch
			{
				return -1;
			}
		}

		public static OrderedTour_ExcursionSight AddNewExcursionSightToOrederedTour(OrderedTour_ExcursionSight objExcursionSightToOrderedTour)
		{
			using (UserContext db = new UserContext())
			{
				db.OrderedTour_ExcursionSights.Add(objExcursionSightToOrderedTour);
				db.SaveChanges();
			}
			return objExcursionSightToOrderedTour;
		}

		public static OrderedTour_ExcursionSight GetExcursionSightToOrder(int OrderedTourId, int ExcursionSightId)
		{
			using (UserContext db = new UserContext())
			{
				return db.OrderedTour_ExcursionSights.FirstOrDefault(u => u.OrderedTourId == OrderedTourId && u.ExcursionSightId == ExcursionSightId);
			}
		}

		public static Boolean EditOrdinalNumberExcursionSight(OrderedTour_ExcursionSight ExcursionSightToOrderedTour, int OrdinalNumber)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					ExcursionSightToOrderedTour.OrdinalNumber = OrdinalNumber;
					db.Entry(ExcursionSightToOrderedTour).State = EntityState.Modified;
					db.SaveChanges();
				}
				return true;
			}
			catch
			{
				return false;
			}
		}
		public static Boolean EditExcursionIdinOrderedTour(OrderedTour_ExcursionSight ExcursionSightToOrderedTour, int ExcursionSightId)
		{
			using (UserContext db = new UserContext())
			{
				ExcursionSightToOrderedTour.ExcursionSightId = ExcursionSightId;
				db.Entry(ExcursionSightToOrderedTour).State = EntityState.Modified;
				db.SaveChanges();
			}
			return true;
		}

		public static OrderedTour_ExcursionSight GetExcursionSightId(int OrdinalNumber, int OrderedTourId)
		{
			using (UserContext db = new UserContext())
			{
			   return db.OrderedTour_ExcursionSights.FirstOrDefault(u => u.OrderedTourId == OrderedTourId && u.OrdinalNumber == OrdinalNumber);
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

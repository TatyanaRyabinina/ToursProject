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
	}

}

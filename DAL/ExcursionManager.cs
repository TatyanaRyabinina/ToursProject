using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL
{
	public class ExcursionManager
	{
		public List<Excursion> GetAllExcursions()
		{
			using (UserContext db = new UserContext())
			{
				return db.Excursions.ToList();
			}
		}

		public static int AddNewExcursion(Excursion objExcursion)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					db.Excursions.Add(objExcursion);
					db.SaveChanges();
					return objExcursion.ExcursionId;
				}
			}
			catch
			{
				return -1;
			}
		}

		public static Boolean EditExcursionInfo(int id, string name)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					Excursion exc = db.Excursions.Find(id);
					exc.ExcursionName = name;
					db.SaveChanges();
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public static Excursion GetExcursionExist(string ExcursionName)
		{
			using (UserContext db = new UserContext())
			{
				return db.Excursions.FirstOrDefault(u => u.ExcursionName == ExcursionName);
			}
		}

		public List<ExcursionSight> GetAllExcursionSight(string excursionValue)
		{
			List<ExcursionSight> GetAllExcursionSight = null;
			using (UserContext db = new UserContext())
			{
				Excursion excursion = db.Excursions.FirstOrDefault(u => u.ExcursionName == excursionValue);
				if (excursion != null)
					GetAllExcursionSight = db.ExcursionSights.Where(u => u.ExcursionId == excursion.ExcursionId).ToList();
				return GetAllExcursionSight;
			}
		}

		public static ExcursionSight GetExcursionSightExist(string ExcursionSightName, int ExcursionId)
		{
			using (UserContext db = new UserContext())
			{
				return db.ExcursionSights.FirstOrDefault(u => u.ExcursionSightName == ExcursionSightName && u.ExcursionId == ExcursionId);
			}
		}

		public static int AddExcursionSight(ExcursionSight objSight)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					db.ExcursionSights.Add(objSight);
					db.SaveChanges();
					return objSight.ExcursionSightId;
				}
			}
			catch
			{
				return -1;
			}

		}
		public List<OrderedTour_ExcursionSight> GetExcursionSightInfo(int id)
		{
			using (UserContext db = new UserContext())
			{
				return db.OrderedTour_ExcursionSights.Where(u => u.OrderedTourId == id).ToList();
			}
		}

		public static ExcursionSight GetExcursionSightName(int id)
		{
			using (UserContext db = new UserContext())
			{
				return db.ExcursionSights.FirstOrDefault(u => u.ExcursionSightId == id);
			}
		}
	}
}

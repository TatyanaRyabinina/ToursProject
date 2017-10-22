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

        public List<ExcursionSight> GetAllExcursionSight(int id)
        {
            using (UserContext db = new UserContext())
            {
                return db.ExcursionSights.Where(u=> u.ExcursionId == id).ToList();
            }
        }

    }
}

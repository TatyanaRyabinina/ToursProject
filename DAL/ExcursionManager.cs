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
	}
}

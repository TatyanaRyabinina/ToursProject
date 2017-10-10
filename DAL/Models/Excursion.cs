using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class Excursion
	{
		[Key]
		public int ExcursionId { get; set; }
		public string ExcursionName { get; set; }
		
		public virtual ICollection<OrderedTour> OrderedTour { get; set; }
	}
}

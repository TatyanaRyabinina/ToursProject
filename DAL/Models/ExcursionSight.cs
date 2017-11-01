using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
	public class ExcursionSight
	{
		[Key]
		public int ExcursionSightId { get; set; }

		public string ExcursionSightName { get; set; }

		[ForeignKey("Excursion")]
		public int ExcursionId { get; set; }

		public virtual Excursion Excursion { get; set; }
		public virtual ICollection<OrderedTour_ExcursionSight> OrderedTour_ExcursionSight { get; set; }
	}
}

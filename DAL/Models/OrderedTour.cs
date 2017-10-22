using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Models
{
	public class OrderedTour
	{
		[Key]
		public int OrderedTourId { get; set; }
		[DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
		public DateTime Date { get; set; }

		[ForeignKey("Excursion")]
		public int ExcursionId { get; set; }

		[ForeignKey("Client")]
		public int ClientId { get; set; }

		public virtual Client Client { get; set; }
		public virtual Excursion Excursion { get; set; }
        public virtual ICollection<OrderedTour_ExcursionSight> OrderedTour_ExcursionSight { get; set; }

    }
}

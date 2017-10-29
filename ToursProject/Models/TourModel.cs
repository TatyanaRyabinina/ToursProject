using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToursProject.Models
{
	public class TourModel
	{
		public int OrderedTourId { get; set; }

		[DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
		public DateTime Date { get; set; }

		[Required]
		[Display(Name = "Excursion Name")]
		public string ExcursionName { get; set; }

		[Required]
		[Display(Name = "Client Name")]
		public string ClientName { get; set; }

		[Required]
		public List<string> ExcursionSight { get; set; }

	}
}
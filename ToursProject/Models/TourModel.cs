using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToursProject.Models
{
	public class TourModel
	{
		//[DisplayFormat(DataFormatString = "{0:MM'/'dd'/'yyyy}", ApplyFormatInEditMode = true)]
		public DateTime Date { get; set; }
		public int? ExcursionId { get; set; }
		public string ExcursionName { get; set; }
		public int? ClientId { get; set; }
		public string ClientName { get; set; }
	}
}
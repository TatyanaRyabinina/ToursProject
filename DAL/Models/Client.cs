using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAL.Models
{
	public class Client
	{
		public Client()
		{
			this.OrderedTour = new HashSet<OrderedTour>();
		}
		[Key]
		public int ClientId { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhotoPath { get; set; }

		public virtual ICollection<OrderedTour> OrderedTour { get; set; }

	}
}
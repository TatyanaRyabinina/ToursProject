using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToursProject.Models
{
	public class LoginModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}

	public class RegisterModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		public string ConfirmPassword { get; set; }

	}
}
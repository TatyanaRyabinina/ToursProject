﻿using DAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ToursProject.Models;

namespace ToursProject.Controllers
{
	public class AccountController : Controller
	{
		ClientManager clientMan = new ClientManager();

		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(LoginModel model)
		{
			string error = "";
			if (ModelState.IsValid)
			{
				Boolean RegisterUserBoolean = ClientManager.FindRegisteredCustomer(model.Email, model.Password);

				if (RegisterUserBoolean)
				{
					FormsAuthentication.SetAuthCookie(model.Email, true);
					return Json(new { status = true });
				}
				else
				{
					Boolean EmailExistBoolean = ClientManager.FindEmailClient(model.Email);
					if (EmailExistBoolean)
					{
						error = "The password is incorrect.";
					}
					else
					{
						error = "User with email '" + model.Email + "' does not exist.";
					}
				}
			}
			else
			{
				error = "Form is incorrect.";
			}

			return Json(new { status = false, error });
		}

		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Register(RegisterModel model, HttpPostedFileBase Photo)
		{
			string error = "";
			if (ModelState.IsValid && Photo != null)
			{
				Boolean EmailExistBoolean = ClientManager.FindEmailClient(model.Email);
				if (!EmailExistBoolean)
				{
					string fileName = System.IO.Path.GetFileName(Photo.FileName);
					Photo.SaveAs(Server.MapPath("~/Files/" + fileName));

					Client objClient = new Client
					{
						Email = model.Email,
						Password = model.Password,
						FirstName = model.FirstName,
						LastName = model.LastName,
						PhotoPath = fileName
					};

					Boolean RegisterBoolean = ClientManager.RegisterNewClient(objClient);

					if (RegisterBoolean)
					{
						FormsAuthentication.SetAuthCookie(model.Email, true);
						return Json(new { status = true });
					}
					else
					{
						error = "Unexpected error. Please try again.";
					}
				}
				else
				{
					error = "User with email '" + model.Email + "' already exists.";
				}
			}
			else
			{
				error = Photo != null ? "Form is incorrect." : "Photo has not been uploaded.";
			}
			return Json(new { status = false, error });
		}

		public ActionResult Logoff()
		{
			FormsAuthentication.SignOut();
			return Redirect("/Account/Login");
		}
	}
}
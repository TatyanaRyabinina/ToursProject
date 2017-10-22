using DAL;
using DAL.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToursProject.Models;
using System.Linq.Expressions;

using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Web.Script.Serialization;

namespace ToursProject.Controllers
{
	public class TourController : BaseController
	{
		OrderedTourManager tourMan = new OrderedTourManager();
		ClientManager clientMan = new ClientManager();
		ExcursionManager excMan = new ExcursionManager();

		[Authorize]
		public ActionResult Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				var client = ClientManager.GetClient(User.Identity.Name);
				ViewBag.clientName = client.FirstName + " " + client.LastName;
				ViewBag.PhotoPath = client.PhotoPath;
			}
			return View();
		}

		public JsonResult GetTours(string sidx, string sord, Int32 page, int rows)
		{
			List<OrderedTour> allTours = tourMan.GetAllTours();
			List<Client> allClients = clientMan.GetAllClients();
			List<Excursion> allExcursions = excMan.GetAllExcursions();
			Int32 totalRows = allTours.Count;
			Int32 totalPages = (Int32)Math.Ceiling((float)totalRows / (float)rows);
			JsonResult result = new JsonResult()
			{
				Data = new
				{
					page = page,
					total = totalPages,
					records = totalRows,
					rows = (from o in allTours
							join c in allClients on o.ClientId equals c.ClientId
							join e in allExcursions on o.ExcursionId equals e.ExcursionId
							select new
							{
								OrderedTourId = o.OrderedTourId,
								Date = o.Date.ToString("MM'/'dd'/'yyyy"),
								ClientName = c.FirstName + " " + c.LastName,
								ExcursionName = e.ExcursionName
							}).OrderBy(u => u.OrderedTourId).Skip((page-1)*rows).Take((int)rows).ToArray()
				}
	};

			return result;
		}


		public JsonResult GetAllExcursions()
		{
			List<Excursion> allExcursions = excMan.GetAllExcursions();
			JsonResult result = new JsonResult()
			{
				Data = new
				{
					selectedData = (from o in allExcursions
									select new
									{
										id = o.ExcursionId,
										value = o.ExcursionName
									})
				}
			};

			return Json(new { status = "success", result }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAllClients()
		{
			List<Client> allClients = clientMan.GetAllClients();
			JsonResult result = new JsonResult()
			{
				Data = new
				{
					selectedData = (from o in allClients
								  select new
								  {
									  id = o.ClientId,
									  value = o.FirstName + " " + o.LastName
								  })
				}
			};
			return Json(new { status = "success", result }, JsonRequestBehavior.AllowGet );
		}

		public JsonResult GetAllExcursionSights(int id)
		{
			List<DAL.Models.ExcursionSight> allExcursionSight = excMan.GetAllExcursionSight(1);
			JsonResult result = new JsonResult()
			{
				Data = new
				{
					selectedData = (from o in allExcursionSight
								  select new
								  {
									  id = o.ExcursionSightId,
									  value = o.ExcursionSightName 
								  })
				}
			};
			return Json(new { status = "success", result },  JsonRequestBehavior.AllowGet );
		}

		public JsonResult AddTour(TourModel Model)
		{
			string error = "";

			Boolean TourBoolean = false;
			if (ModelState.IsValid)
			{
				int clientId = 0;
				int excursionId = 0;
				if (Model.ClientId == null)
				{
					var names = Model.ClientName.Split(' ');
					string firstName = names[0];
					string lastName = names[1];

					Client objClient = new Client
					{
						FirstName = firstName,
						LastName = lastName

					};
					Model.ClientId = ClientManager.AddNewClient(objClient);
				}
				if (Model.ExcursionId == null)
				{
					Excursion objExcursion = new Excursion
					{
						ExcursionName = Model.ExcursionName
					};
					Model.ExcursionId = ExcursionManager.AddNewExcursion(objExcursion);
				}
				if (excursionId == -1 || clientId == -1)
				{
					error = "Unexpected error. Please try again.";
					return Json(new { status = false, error }, JsonRequestBehavior.AllowGet);
				}
				OrderedTour objTour = new OrderedTour
				{
					Date = Model.Date,
					ClientId = (int)Model.ClientId,
					ExcursionId = (int)Model.ExcursionId
				};

				TourBoolean = OrderedTourManager.AddNewTour(objTour);
			}
			else
			{
				error = "Form is incorrect.";
			}
			return Json(new { status = TourBoolean, error }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult EditTour(int id)
		{
			OrderedTour tour = tourMan.GetTourInfo(id);

			TourModel tourM = new TourModel
			{
				OrderedTourId = tour.OrderedTourId,
				Date = tour.Date,
				ExcursionId = tour.ExcursionId,
				ExcursionName = tour.Excursion.ExcursionName,
				ClientId = tour.ClientId,
				ClientName = tour.Client.FirstName + " " + tour.Client.LastName
			};

			if (tour != null)
			{
				return Json(RenderPartialViewToString("EditTour", tourM), JsonRequestBehavior.AllowGet);
			}
			return View("Index");
		}

		[HttpPost]
		public ActionResult Edit(TourModel Model)
		{
			string error = "";
			
			try
			{
				Boolean EditExcursionBoolean = false;
				Boolean EditClientBoolean = false;
				Boolean EditBoolean = false;

				if (ModelState.IsValid)
				{
					
					if (Model.ClientId == null)
					{
						var names = Model.ClientName.Split(' ');
						string firstName = names[0];
						string lastName = names[1];

						Client objClient = new Client
						{
							FirstName = firstName,
							LastName = lastName
						};
						Model.ClientId = ClientManager.AddNewClient(objClient);
					}
					if (Model.ExcursionId == null)
					{
						Excursion objExcursion = new Excursion
						{
							ExcursionName = Model.ExcursionName
						};
						Model.ExcursionId = ExcursionManager.AddNewExcursion(objExcursion);
					}
					if (Model.ExcursionId != -1)
					{
						EditExcursionBoolean = ExcursionManager.EditExcursionInfo((int)Model.ExcursionId, Model.ExcursionName);
					}
					if (Model.ClientId != -1)
					{
						EditClientBoolean = ClientManager.EditClientInfo((int)Model.ClientId, Model.ClientName);
					}
					if (EditExcursionBoolean && EditClientBoolean)
					{
						EditBoolean = OrderedTourManager.EditTourInfo(Model.OrderedTourId, Model.Date, (int)Model.ClientId, (int)Model.ExcursionId);
						if (!EditBoolean)
						{
							error = "Unexpected error. Please try again.";
						}
					}
					else
					{
						error = "Unexpected error. Please try again.";
					}
				}
				else
				{
					error = "Form is incorrect.";
				}
				return Json(new { status = EditBoolean, error }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				error = "Error occured:" + ex.Message;
				return Json(new { status = false, error }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}
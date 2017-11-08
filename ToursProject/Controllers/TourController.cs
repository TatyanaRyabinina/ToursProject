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
            return Json(RenderPartialViewToString("Index"), JsonRequestBehavior.AllowGet);
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
							}).OrderBy(u => u.OrderedTourId).Skip((page - 1) * rows).Take((int)rows).ToArray()
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
										value = o.FirstName + " " + o.LastName
									})
				}
			};
			return Json(new { status = "success", result }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAllExcursionSights(string excursionValue)
		{
			List<DAL.Models.ExcursionSight> allExcursionSight = excMan.GetAllExcursionSight(excursionValue);
			if (allExcursionSight != null)
			{
				JsonResult result = new JsonResult()
				{
					Data = new
					{
						selectedData = (from o in allExcursionSight
										select new
										{
											value = o.ExcursionSightName
										})
					}
				};
				return Json(new { status = "success", result }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { status = "fail" }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult AddTour(TourModel Model)
		{
			string error = "";

			if (ModelState.IsValid)
			{
				int ClientId = GetClientId(Model.ClientName);
				int ExcursionId = GetExcursionId(Model.ExcursionName);

				OrderedTour objTour = new OrderedTour
				{
					Date = Model.Date,
					ClientId = ClientId,
					ExcursionId = ExcursionId
				};
				int orderedTourId = OrderedTourManager.AddNewTour(objTour);
				if (orderedTourId > -1)
				{
					for (int i = 0; i < Model.ExcursionSight.Count; i++)
					{
						var excursionSightId = GetExcursionSightId(Model.ExcursionSight[i], ExcursionId);

						AddExcursionSight(orderedTourId, excursionSightId, i);
					}
				}
			}
			else
			{
				error = "Form is incorrect.";
				return Json(new { status = false, error }, JsonRequestBehavior.AllowGet);
			}
			return Json(new { status = true, error }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult EditTour(int id)
		{
			OrderedTour tour = tourMan.GetTourInfo(id);
			List<OrderedTour_ExcursionSight> excursionSight = excMan.GetExcursionSightInfo(id);

			List<string> excursionSightArray = new List<string>();
			for (int i = 0; i < excursionSight.Count; i++)
			{
				foreach (var x in excursionSight)
				{
					if (x.OrdinalNumber == i && x.ExcursionSightId.HasValue)
					{
						excursionSightArray.Add((ExcursionManager.GetExcursionSightName((int)x.ExcursionSightId)).ExcursionSightName);
					}
				}
			}

			TourModel tourM = new TourModel
			{
				OrderedTourId = tour.OrderedTourId,
				Date = tour.Date,
				ExcursionName = tour.Excursion.ExcursionName,
				ClientName = tour.Client.FirstName + " " + tour.Client.LastName,
				ExcursionSight = excursionSightArray
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
				if (ModelState.IsValid)
				{
					int ClientId = GetClientId(Model.ClientName);
					int ExcursionId = GetExcursionId(Model.ExcursionName);

					OrderedTourManager.EditTourInfo(Model.OrderedTourId, Model.Date, ClientId, ExcursionId);
					List<OrderedTour_ExcursionSight> excursionSight = excMan.GetExcursionSightInfo(Model.OrderedTourId);

					var Count = excursionSight.Count > Model.ExcursionSight.Count ? excursionSight.Count : Model.ExcursionSight.Count;

					int? excursionSightId;
					for (int i = 0; i < Count; i++)
					{
						try
						{
							excursionSightId = (int)GetExcursionSightId(Model.ExcursionSight[i], ExcursionId);
						}
						catch
						{
							excursionSightId = null;
						}


						OrderedTour_ExcursionSight ExcursionSightToOrderedTour = OrderedTourManager.GetExcursionSightId(i, Model.OrderedTourId);
						if (ExcursionSightToOrderedTour != null)
						{
							OrderedTourManager.EditExcursionIdinOrderedTour(ExcursionSightToOrderedTour, excursionSightId);
						}
						else
						{
							AddExcursionSight(Model.OrderedTourId, excursionSightId, i);
						}
					}
				}
				else
				{
					error = "Form is incorrect.";
					return Json(new { status = false, error }, JsonRequestBehavior.AllowGet);
				}
				return Json(new { status = true, error }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				error = "Error occured:" + ex.Message;
				return Json(new { status = false, error }, JsonRequestBehavior.AllowGet);
			}
		}

		public static int GetClientId(string ClientName)
		{
			int ClientId = -1;

			var names = ClientName.Split(' ');
			string firstName = names[0];
			string lastName = names[1];
			Client ClientExist = ClientManager.GetClientExist(firstName, lastName);

			if (ClientExist == null)
			{
				Client objClient = new Client
				{
					FirstName = firstName,
					LastName = lastName
				};
				ClientId = ClientManager.AddNewClient(objClient);
			}
			return ClientExist != null ? ClientExist.ClientId : ClientId;
		}

		public static int GetExcursionId(string ExcursionName)
		{
			int ExcursionId = -1;
			Excursion ExcursionExist = ExcursionManager.GetExcursionExist(ExcursionName);

			if (ExcursionExist == null)
			{
				Excursion objExcursion = new Excursion
				{
					ExcursionName = ExcursionName
				};
				ExcursionId = ExcursionManager.AddNewExcursion(objExcursion);
			}
			return ExcursionExist != null ? ExcursionExist.ExcursionId : ExcursionId;
		}

		public static int GetExcursionSightId(string ExcursionSightName, int ExcursionId)
		{
			int ExcursionSightId = -1;
			ExcursionSight ExcursionSightExist;

			ExcursionSightExist = ExcursionManager.GetExcursionSightExist(ExcursionSightName, ExcursionId);
			if (ExcursionSightExist == null)
			{
				ExcursionSight objSight = new ExcursionSight
				{
					ExcursionSightName = ExcursionSightName,
					ExcursionId = ExcursionId
				};
				ExcursionSightId = ExcursionManager.AddExcursionSight(objSight);
			}
			return ExcursionSightExist != null ? ExcursionSightExist.ExcursionSightId : ExcursionSightId;
		}

		public static OrderedTour_ExcursionSight AddExcursionSight(int orderedTourId, int? ExcursionSightId, int OrdinalNumber)
		{
			OrderedTour_ExcursionSight objExcursionSightToOrderedTour = new OrderedTour_ExcursionSight
			{
				OrderedTourId = orderedTourId,
				ExcursionSightId = ExcursionSightId,
				OrdinalNumber = OrdinalNumber
			};
			return OrderedTourManager.AddNewExcursionSightToOrederedTour(objExcursionSightToOrderedTour);
		}
	}
}
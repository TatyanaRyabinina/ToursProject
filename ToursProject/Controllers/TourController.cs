using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ToursProject.Models;

namespace ToursProject.Controllers
{
	public class TourController : Controller
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
							})//.OrderBy(String.Format("{0} {1}", sidx, sord)).Skip((page - 1) * rows).Take(rows).ToArray()
				}
			};
			return result;
		}

		public JsonResult AddTourForm(TourModel Model)
		{
			//date not valid
			Boolean TourBoolean = false;
			if (ModelState.IsValid)
			{
				OrderedTour objTour = new OrderedTour
				{

				};
				TourBoolean = OrderedTourManager.AddNewTour(objTour);
			}
			return Json(new { isSuccess = TourBoolean }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAllExcursions()
		{
			List<Excursion> allExcursions = excMan.GetAllExcursions();
			JsonResult result = new JsonResult()
			{
				Data = new
				{
					allExcursions = (from o in allExcursions
									select new
									{
										ExcursionId = o.ExcursionId,
										ExcursionName = o.ExcursionName
									})
				}
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public JsonResult GetAllClients()
		{
			List<Client> allClients = clientMan.GetAllClients();
			JsonResult result = new JsonResult()
			{
				Data = new
				{
					allClients = (from o in allClients
								select new
								{
									ClientId = o.ClientId,
									ClientName = o.FirstName + " " + o.LastName
								})
				}
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public ActionResult AddNewTour(TourModel mpdel)
		{
			return View();
		}
	}
}
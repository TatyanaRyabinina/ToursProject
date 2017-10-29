using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
	public class ClientManager
	{
		public static Boolean FindRegisteredCustomer(string Email, string Password)
		{
			Client client = null;
			using (UserContext db = new UserContext())
			{
				client = db.Clients.FirstOrDefault(u => u.Email == Email && u.Password == Password);
			}
			return client != null ? true : false;
		}

		public static Boolean FindEmailClient(string Email)
		{
			Client client = null;

			using (UserContext db = new UserContext())
			{
				client = db.Clients.FirstOrDefault(u => u.Email == Email);
			}

			return client != null ? true : false;
		}

		public static Boolean RegisterNewClient(Client objClient)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					db.Clients.Add(objClient);
					db.SaveChanges();
					return true;
				}
			}
			catch
			{
				return false;
			}
		}

		public static int AddNewClient(Client objClient)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					db.Clients.Add(objClient);
					db.SaveChanges();
					return objClient.ClientId;
				}
			}
			catch
			{
				return -1;
			}
		}
		public static Client GetClient(string Email)
		{
			Client client = null;
			using (UserContext db = new UserContext())
			{
				client = db.Clients.FirstOrDefault(u => u.Email == Email);
			}
			return client;
		}

		public List<Client> GetAllClients()
		{
			using (UserContext db = new UserContext())
			{
				return db.Clients.ToList();
			}
		}
		public static Client GetClientExist(string FirstName, string LastName)
		{
			Client client = null;
			using (UserContext db = new UserContext())
			{
				client = db.Clients.FirstOrDefault(u => u.FirstName == FirstName && u.LastName == LastName);
			}
			return client;
		}
		public static Boolean EditClientInfo(int id, string name)
		{
			try
			{
				using (UserContext db = new UserContext())
				{
					Client client = db.Clients.Find(id);

					var names = name.Split(' ');
					string firstName = names[0];
					string lastName = names[1];

					client.FirstName = firstName;
					client.LastName = lastName;
					db.SaveChanges();
					return true;
				}
			}
			catch
			{
				return false;
			}
		}
	}
}

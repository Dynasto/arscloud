using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Diagnostics;

namespace ArsCloudWeb.Data
{
	public class Chirp
	{
		public Chirp()
		{
		}

		public Chirp(String username, String text, DateTime timestamp)
		{
			Username = username;
			Text = text;
			Timestamp = timestamp;
		}

		public String Username { get; set; }
		public String Text { get; set; }
		public DateTime Timestamp { get; set; }
	}

	public class ChirpEntity : TableServiceEntity
	{
		public ChirpEntity()
		{
		}

		public ChirpEntity(String username, String text, DateTime timestamp) : base("Chirps", String.Format("{0:10}-{1}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks, username.GetHashCode()))
		{
			Username = username;
			Text = text;
			TimeStamp = timestamp;
		}

		public String Username { get; set; }
		public String Text { get; set; }
		public DateTime TimeStamp { get; set; }
	}

	public class ChirpContext : TableServiceContext
	{
		private static string TABLE_NAME = "Chirps";

		public ChirpContext(string baseAddress, StorageCredentials credentials)
			: base(baseAddress, credentials)
		{
		}

		public IQueryable<ChirpEntity> ChirpEntities
		{
			get
			{
				return CreateQuery<ChirpEntity>(TABLE_NAME);
			}
		}

		public String AddChirp(string username, string text, DateTime timestamp)
		{
			TableServiceEntity entity = new ChirpEntity(username, text, timestamp);
			AddObject(TABLE_NAME, entity);
			SaveChanges();
			return entity.RowKey;
		}

		public void DelChirp(String rowKey)
		{
			ChirpEntity res = (from c in ChirpEntities where c.RowKey == rowKey select c).First();
			DeleteObject(res);
			SaveChanges();
		}
	}

	public class ChirpManager
	{
		private static string TABLE_NAME = "Chirps";

		public static void Initialize()
		{
			CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			CloudTableClient ctc = account.CreateCloudTableClient();
			ctc.CreateTableIfNotExist(TABLE_NAME);
		}

		public static Chirp Add(string username, string text)
		{
			CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			ChirpContext context = new ChirpContext(account.TableEndpoint.ToString(), account.Credentials);
			DateTime now = DateTime.UtcNow;
			context.AddChirp(username, text, now);
			return new Chirp(username, text, now);
		}

		protected static IQueryable<ChirpEntity> Find()
		{
			CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			ChirpContext context = new ChirpContext(account.TableEndpoint.ToString(), account.Credentials);
			return context.ChirpEntities;
		}

		public static IList<Chirp> FindAll()
		{
			// sadly, LINQ-to-Azure Tables won't let me say e.g.
			// from c in Find() select new Chirp(c.Username, c.Text, c.Timestamp);
			return (from c in Find() select c).ToList().Select((c) => new Chirp(c.Username, c.Text, c.Timestamp)).ToList();
		}

		public static IList<Chirp> FindMine(string username)
		{
			return (from c in Find() where c.Username == username select c).ToList().Select((c) => new Chirp(c.Username, c.Text, c.Timestamp)).ToList();
		}

		//public static IList<Chirp> FindReplies(string username)
		//{
		//    return (from c in Find() where c.Text.Contains(username) select c).ToList().Select((c) => new Chirp(c.Username, c.Text, c.Timestamp)).ToList();
		//}
	}
}

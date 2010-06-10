using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace ArsCloud.Azure
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

	public class ResizeRequestManager
	{
		private static string QUEUE_NAME = "resizes";

		public static void Initialize()
		{
			CloudQueue queue = GetQueue();
			queue.CreateIfNotExist();
		}

		public static void AddResizeRequest(Uri imageToResize)
		{
			CloudQueue queue = GetQueue();
			CloudQueueMessage message = new CloudQueueMessage(imageToResize.ToString());
			queue.AddMessage(message);
		}

		public class MessageKey
		{
			public MessageKey()
			{
			}

			public MessageKey(string messageId, string popReceipt)
			{
				MessageId = messageId;
				PopReceipt = popReceipt;
			}

			internal string MessageId { get; set; }
			internal string PopReceipt { get; set; }
		}

		public static Uri GetResizeRequest(out MessageKey key)
		{
			CloudQueue queue = GetQueue();
			if(queue.RetrieveApproximateMessageCount() == 0)
			{
				key = null;
				return null;
			}
			CloudQueueMessage message = queue.GetMessage();
			Uri result = new Uri(message.AsString);
			key = new MessageKey(message.Id, message.PopReceipt);
			return result;
		}

		public static void DeleteResizeRequest(MessageKey key)
		{
			CloudQueue queue = GetQueue();
			queue.DeleteMessage(key.MessageId, key.PopReceipt);
		}

		private static CloudQueue GetQueue()
		{
			CloudQueueClient cloudQueueClient = GetClient();
			CloudQueue queue = cloudQueueClient.GetQueueReference(QUEUE_NAME);
			return queue;
		}

		private static CloudQueueClient GetClient()
		{
			CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			CloudQueueClient cloudQueueClient = account.CreateCloudQueueClient();
			return cloudQueueClient;
		}
	}

	public class AvatarManager
	{
		private static string CONTAINER_NAME = "avatars";

		private static string GetHexBytes(byte[] bytes)
		{
			StringBuilder sb = new StringBuilder(bytes.Length * 2);
			foreach(byte b in bytes)
			{
				sb.Append(b.ToString("X2"));
			}
			return sb.ToString();
		}

		private static string GetBlobName(string username)
		{
			return GetHexBytes(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(username)));
		}

		public static void Initialize()
		{
			CloudBlobContainer cloudBlobContainer = GetContainer();
			
			if(cloudBlobContainer.CreateIfNotExist())
			{
				BlobContainerPermissions bcp = new BlobContainerPermissions();
				bcp.PublicAccess = BlobContainerPublicAccessType.Blob;
				cloudBlobContainer.SetPermissions(bcp);
			}
		}

		private static CloudBlobClient GetClient()
		{
			CloudStorageAccount account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
			CloudBlobClient cloudBlobClient = account.CreateCloudBlobClient();
			return cloudBlobClient;
		}

		public static Uri Save(string username, Stream input)
		{
			CloudBlobContainer cloudBlobContainer = GetContainer();
			CloudBlob cloudBlob = cloudBlobContainer.GetBlobReference(GetBlobName(username));
			using(BlobStream blobStream = cloudBlob.OpenWrite())
			{
				input.CopyTo(blobStream); // oh wow, we finally don't have to manually copy between streams.
			}
			return cloudBlob.Uri;
		}

		public static Uri GetUri(string username)
		{
			CloudBlobClient cloudBlobClient = GetClient();
			CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(CONTAINER_NAME);
			return cloudBlobContainer.GetBlobReference(GetBlobName(username)).Uri;
		}

		public static Uri GetUri(string username, int size)
		{
			CloudBlobContainer cloudBlobContainer = GetContainer();
			return cloudBlobContainer.GetBlobReference(GetBlobName(username) + "/" + size.ToString()).Uri;
		}

		public static bool HasUri(string username)
		{
			CloudBlobContainer cloudBlobContainer = GetContainer();
			CloudBlob blob = cloudBlobContainer.GetBlobReference(GetBlobName(username));
			return CheckExists(blob);
		}

		private static bool CheckExists(CloudBlob blob)
		{
			try
			{
				blob.FetchAttributes();
				return true;
			}
			catch(StorageClientException e)
			{
				if(e.ErrorCode == StorageErrorCode.ResourceNotFound)
				{
					return false;
				}
				else
				{
					throw;
				}
			}
		}

		public static bool HasUri(string username, int size)
		{
			CloudBlobContainer cloudBlobContainer = GetContainer();
			CloudBlob blob = cloudBlobContainer.GetBlobReference(GetBlobName(username) + "/" + size.ToString());
			return CheckExists(blob);
		}

		private static CloudBlobContainer GetContainer()
		{
			CloudBlobClient cloudBlobClient = GetClient();
			CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(CONTAINER_NAME);
			return cloudBlobContainer;
		}

		public static Stream GetReadStream(Uri geller)
		{
			CloudBlobClient cloudBlobClient = GetClient();
			CloudBlob cb = new CloudBlob(geller.ToString(), cloudBlobClient);
			return cb.OpenRead();
		}

		public static Stream GetWriteStream(Uri geller)
		{
			CloudBlobClient cloudBlobClient = GetClient();
			CloudBlob cb = new CloudBlob(geller.ToString(), cloudBlobClient);
			return cb.OpenWrite();
		}

		public static Stream GetWriteStream(Uri geller, int size)
		{
			CloudBlobClient cloudBlobClient = GetClient();
			CloudBlob cb = new CloudBlob(geller.ToString() + "/" + size.ToString(), cloudBlobClient);
			return cb.OpenWrite();
		}
	}
}

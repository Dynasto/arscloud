using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ArsCloud.Web.Models
{
	public class ChirpModel
	{
		[Required]
		[DataType(DataType.MultilineText)]
		[DisplayName("What are you doing?")]
		public string Chirp { get; set; }
	}
}

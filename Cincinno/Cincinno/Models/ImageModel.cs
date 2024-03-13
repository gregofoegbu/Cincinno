using System;
namespace Cincinno.Models
{
	public class ImageModel
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }
		public string Filename { get; set; }
		public byte[] Data { get; set; }
		public string Name { get; set; }

		public ImageModel()
		{
		}
	}
}


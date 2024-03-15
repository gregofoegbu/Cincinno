using System;
namespace CincinnoView.Models
{
	public class ImageViewModel
	{
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string Filename { get; set; }
        public byte[] Data { get; set; }
        public string Name { get; set; }
	}
}


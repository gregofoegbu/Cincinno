using System;
namespace CincinnoView.Models
{
	public class ActivityViewModel
	{
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string MemberName { get; set; }
        public DateTime AccessTime { get; set; }
        public LogStatus Status { get; set; }

    }

    public enum LogStatus
    {
        AccessGranted,
        AccessDenied
    }
}


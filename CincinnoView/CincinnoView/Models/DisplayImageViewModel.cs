using System;
namespace CincinnoView.Models
{
	public class DisplayImageViewModel
	{
		public List<HouseholdMemberInfo> HouseholdMembers { get; set; }

		public DisplayImageViewModel(List<HouseholdMemberInfo> householdMembers)
		{
			HouseholdMembers = householdMembers;
		}

		public DisplayImageViewModel()
		{
			HouseholdMembers = new List<HouseholdMemberInfo>();
		}
	}

	public class HouseholdMemberInfo
	{
		public string MemberName { get; set; }
		public List<ImageViewModel> Images { get; set; }

		public HouseholdMemberInfo(string memberName, List<ImageViewModel> images)
		{
			MemberName = memberName;
			Images = images;
		}
		public HouseholdMemberInfo()
		{
			Images = new List<ImageViewModel>();
		}
	}
}


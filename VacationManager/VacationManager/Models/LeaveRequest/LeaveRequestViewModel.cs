    using VacationManager.Data.Enums;
using VacationManager.Data.Models;

namespace VacationManager.Models.LeaveRequest
{
    public class LeaveRequestViewModel
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RequestDate { get; set; }

        public bool HalfDay { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public LeaveType Type { get; set; }

        public string? AttachmentPath { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
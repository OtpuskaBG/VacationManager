using VacationManager.Data.Models;

namespace VacationManager.Models.Team
{
    public class TeamViewModel
    {
        public string Name { get; set; } = "";


        //public Guid ProjectId { get; set; }
        public Project Project { get; set; }


        public Guid Id { get; set; }
        public List<ApplicationUser> Developers { get; set; } = new();

        public ApplicationUser TeamLead { get; set; }
        public string TeamLeadId { get; set; } = "";
        
        
        public string? UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}

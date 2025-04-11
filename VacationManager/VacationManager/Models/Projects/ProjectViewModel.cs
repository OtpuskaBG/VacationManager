using VacationManager.Data.Models;
using VacationManager.Models.Team;


namespace VacationManager.Models.Project
{
    public class ProjectViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public Guid? TeamId { get; set; }
        public VacationManager.Data.Models.Team? Team { get; set; } // Now correctly references the Team type

        public string? UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }
    }
}

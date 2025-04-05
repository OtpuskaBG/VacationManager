using AutoMapper;
using VacationManager.Core.Prototypes;
using VacationManager.Data.Models;
using VacationManager.Models.Team;
namespace VacationManager.Profiles
{
    public class TeamProfile : Profile
    {
        public TeamProfile()
        {
            this.CreateMap<TeamViewModel, Team>();
            this.CreateMap<TeamViewModel, TeamPrototype>();
            this.CreateMap<Team, TeamViewModel>();
            this.CreateMap<Team, TeamPrototype>();
            this.CreateMap<VacationManager.Data.Models.Team, VacationManager.Components.Pages.Team.Team>();
        }
    }
}

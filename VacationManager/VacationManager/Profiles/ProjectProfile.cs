using AutoMapper;
using VacationManager.Core.Prototypes;
using VacationManager.Data.Models;
using VacationManager.Models.Project;

namespace VacationManager.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            this.CreateMap<Project,ProjectPrototype>();
            this.CreateMap<Project, ProjectViewModel>();
            this.CreateMap<ProjectViewModel, Project>();
            this.CreateMap<ProjectViewModel,ProjectPrototype>();
        }
    }
}

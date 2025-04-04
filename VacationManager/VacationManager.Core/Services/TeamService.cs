using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VacationManager.Core.Authentication.Abstractions;
using VacationManager.Core.Authentication.Extensions;
using VacationManager.Core.Prototypes;
using VacationManager.Core.Services.Abstractions;
using VacationManager.Data.Models;
using VacationManager.Data.Repositories.Abstractions;

namespace VacationManager.Core.Services
{
    class TeamService(IRepository<Team> repository, ITeamService teamService, IProjectService projectService, IAuthenticationContext authContext) : BaseService<Team, TeamPrototype>(repository), ITeamService
    {
        private readonly ITeamService _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        private readonly IProjectService _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        private readonly IAuthenticationContext _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));

        protected override async Task ApplyAsync(Team entity, TeamPrototype prototype, CancellationToken cancellationToken)
        {
            entity.Name = prototype.Name;

            entity.User = this._authContext.GetCurrentUserRequired();

            if (prototype.ProjectId.HasValue)
            {
                Project project = await this._projectService.GetByIdRequiredAsync(prototype.ProjectId.Value, cancellationToken);

                entity.ProjectId = project.Id;
                entity.Project = project;   
            }
            else
            {
                entity.Project = null;
                entity.ProjectId = default;
            }


            //maybe users?

            //if (prototype.Developers != null)
            //{
            //    entity.Developers = new List<ApplicationUser>();
            //    foreach (var developer in prototype.Developers)
            //    {
            //        var user = await this._authContext.GetUserByIdAsync(developer.Id, cancellationToken);
            //        if (user == null)
            //        {
            //            throw new ArgumentException($"User with id {developer.Id} not found");
            //        }
            //        entity.Developers.Add(user);
            //    }
            //}
            //else
            //{
            //    entity.Developers = new List<ApplicationUser>();
            //}


        }
        protected override IEnumerable<Expression<Func<Team, bool>>> BuildAdditionalFilters()
        {
            ApplicationUser currentUser = this._authContext.GetCurrentUserRequired();
            return [ti => ti.User == currentUser];
        }
    }
}

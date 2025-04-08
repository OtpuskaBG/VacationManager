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
using VacationManager.Data.Enums;
using VacationManager.Data.Models;
using VacationManager.Data.Repositories.Abstractions;

namespace VacationManager.Core.Services
{
    public class TeamService(IRepository<Team> repository, IAuthenticationContext authContext) : BaseService<Team, TeamPrototype>(repository), ITeamService
    {
        //private readonly ITeamService _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        //private readonly IProjectService _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        private readonly IAuthenticationContext _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        protected override Task<Team> InitializeAsync(TeamPrototype prototype, CancellationToken cancellationToken)
         => Task.FromResult(new Team());


        protected override async Task ApplyAsync(Team entity, TeamPrototype prototype, CancellationToken cancellationToken)
        {

            entity.Name = prototype.Name;

            entity.User = this._authContext.GetCurrentUserRequired();

            if (!string.IsNullOrWhiteSpace(prototype.TeamLeadId))
            {
                var teamLeadGuid = Guid.Parse(prototype.TeamLeadId);
                var teamLeadUser = await this._authContext.GetUserByIdAsync(teamLeadGuid, cancellationToken);

                if (teamLeadUser == null)
                {
                    throw new ArgumentException($"Team lead with id {prototype.TeamLeadId} not found");
                }

                entity.TeamLeadId = prototype.TeamLeadId;
                entity.TeamLead = teamLeadUser;
            }

            //if (prototype.ProjectId.HasValue)
            //{
            //    Project project = await this._projectService.GetByIdRequiredAsync(prototype.ProjectId.Value, cancellationToken);

            //    entity.ProjectId = project.Id;
            //    entity.Project = project;
            //}
            //else
            //{
            //    entity.Project = null;
            //    entity.ProjectId = default;
            //}


            //maybe users?

            if (prototype.Developers != null)
            {
                entity.Developers = new List<ApplicationUser>();
                foreach (var developer in prototype.Developers)
                {
                    var userId = Guid.Parse(developer.Id);
                    var user = await this._authContext.GetUserByIdAsync(userId, cancellationToken);

                    if (user == null)
                    {
                        throw new ArgumentException($"User with id {developer.Id} not found");
                    }
                    entity.Developers.Add(user);
                }
            }
            else
            {
                entity.Developers = new List<ApplicationUser>();
            }


        }
        public async Task<Team[]> GetAllAsync(CancellationToken cancellationToken)
        {
            // Включваме нужните навигации
            var includes = new[]
            {
                nameof(Team.Developers),
                nameof(Team.TeamLead),
                nameof(Team.Project),
                nameof(Team.User)
            };

            return await repository.GetManyWithNavigationsAsync([], includes, cancellationToken);
        }
        protected override IEnumerable<Expression<Func<Team, bool>>> BuildAdditionalFilters()
        {
            ApplicationUser currentUser = this._authContext.GetCurrentUserRequired();
            return [ti => ti.User == currentUser];
        }
        //public async Task SoftDeleteAsync(Guid teamId, CancellationToken cancellationToken)
        //{
        //    var team = await this.Repository.GetCompleteAsync([t => t.Id == teamId], cancellationToken);
        //    if (team == null) throw new ArgumentException("Team not found");

        //    foreach (var dev in team.Developers)
        //    {
        //        await devService.ChangeRoleAsync(dev, Role.Unassigned, cancellationToken);
        //    }

        //    if (team.TeamLead is not null)
        //    {
        //        await devService.ChangeRoleAsync(team.TeamLead, Role.Unassigned, cancellationToken);
        //    }

        //    await this.Repository.DeleteAsync(team, cancellationToken);
        //}
    }
}





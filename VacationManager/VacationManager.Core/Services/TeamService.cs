using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using VacationManager.Data.Repositories;
using VacationManager.Data.Repositories.Abstractions;

namespace VacationManager.Core.Services
{
    public class TeamService(IRepository<Team> repository, IAuthenticationContext authContext, IUserService userService) : BaseService<Team, TeamPrototype>(repository), ITeamService
    {
        //private readonly ITeamService _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        //private readonly IProjectService _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        private readonly IAuthenticationContext _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
        private readonly IUserService _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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
        public async Task<Team?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var includes = new[]
            {
                nameof(Team.Developers),
                nameof(Team.TeamLead),
                nameof(Team.Project),
                nameof(Team.User)
            };

            var filters = new Expression<Func<Team, bool>>[]
            {
                t => t.Id == id
            };

            return await repository.GetWithNavigationsAsync(filters, includes, cancellationToken);
        }
        public async Task SoftDeleteAsync(Guid teamId, CancellationToken cancellationToken)
        {
            var team = await repository.GetCompleteAsync([t => t.Id == teamId], cancellationToken);
            if (team == null) throw new ArgumentException("Team not found");

            foreach (var developer in team.Developers)
            {
                await userService.ChangeRoleAsync(developer, Role.Unassigned, cancellationToken);
            }

            if (team.TeamLead != null)
            {
                await userService.ChangeRoleAsync(team.TeamLead, Role.Unassigned, cancellationToken);
            }

            team.Developers.Clear();

            await repository.DeleteAsync(team, cancellationToken);
        }
        //public async Task AddDevelopersToTeamAsync(Guid teamId, List<string> developerIds, CancellationToken cancellationToken)
        //{
        //    var team = await repository.GetWithNavigationsAsync(
        //        new Expression<Func<Team, bool>>[] { t => t.Id == teamId },
        //        new[] { nameof(Team.Developers) },
        //        cancellationToken);

        //    if (team == null)
        //    {
        //        throw new ArgumentException("Team not found");
        //    }

        //    foreach (var developerId in developerIds)
        //    {
        //        var user = await _authContext.GetUserByIdAsync(Guid.Parse(developerId), cancellationToken);

        //        if (user != null && !team.Developers.Contains(user))
        //        {
        //            await repository.AddAsync(new TeamDeveloper
        //            {
        //                TeamId = teamId,
        //                DeveloperId = Guid.Parse(developerId)
        //            }, cancellationToken);

        //            team.Developers.Add(user);
        //        }
        //    }

        //    await repository.SaveChangesAsync(cancellationToken);
        //}

        //public async Task RemoveDeveloperFromTeamAsync(Guid teamId, string developerId, CancellationToken cancellationToken)
        //{
        //    var team = await repository.GetWithNavigationsAsync(
        //        new Expression<Func<Team, bool>>[] { t => t.Id == teamId },
        //        new[] { nameof(Team.Developers) },
        //        cancellationToken);

        //    if (team == null)
        //    {
        //        throw new ArgumentException("Team not found");
        //    }

        //    var user = team.Developers.FirstOrDefault(d => d.Id == Guid.Parse(developerId));

        //    if (user != null)
        //    {
        //        team.Developers.Remove(user);

        //        await repository.DeleteAsync(new TeamDeveloper
        //        {
        //            TeamId = teamId,
        //            DeveloperId = Guid.Parse(developerId)
        //        }, cancellationToken);

        //        await repository.SaveChangesAsync(cancellationToken);
        //    }
        //}
    }
}





using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;
using VacationManager.Core.Authentication.Abstractions;
using VacationManager.Core.Authentication.Extensions;
using VacationManager.Core.Prototypes;
using VacationManager.Core.Services.Abstractions;
using VacationManager.Data.Models;
using VacationManager.Data.Repositories.Abstractions;

namespace VacationManager.Core.Services
{
    public class ProjectService(
        IRepository<Project> repository,
        ITeamService teamService,
        IAuthenticationContext authContext
    ) : BaseService<Project, ProjectPrototype>(repository), IProjectService
    {
        private readonly ITeamService _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        private readonly IAuthenticationContext _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));

        protected override async Task ApplyAsync(Project entity, ProjectPrototype prototype, CancellationToken cancellationToken)
        {
            entity.Name = prototype.Name;
            entity.Description = prototype.Description;
            entity.User = _authContext.GetCurrentUserRequired();

            entity.Teams.Clear();

            foreach (var team in prototype.Teams)
            {
                var existingTeam = await _teamService.GetByIdRequiredAsync(team.Id, cancellationToken);


                if (existingTeam.User.Id != entity.User.Id)
                {
                    throw new UnauthorizedAccessException($"User does not own the team with id {team.Id}");
                }

                entity.Teams.Add(existingTeam);
            }
        }

        protected override IEnumerable<Expression<Func<Project, bool>>> BuildAdditionalFilters()
        {
            var currentUser = _authContext.GetCurrentUserRequired();
            return [project => project.User == currentUser];
        }
    }
}

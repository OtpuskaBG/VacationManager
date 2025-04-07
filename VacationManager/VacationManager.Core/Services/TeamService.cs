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
    public class TeamService(IRepository<Team> repository, IAuthenticationContext authContext) : BaseService<Team, TeamPrototype>(repository), ITeamService
    {
        //private readonly ITeamService _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        //private readonly IProjectService _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        private readonly IAuthenticationContext _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));

        protected override async Task ApplyAsync(Team entity, TeamPrototype prototype, CancellationToken cancellationToken)
        {

            entity.Name = prototype.Name;

            entity.User = this._authContext.GetCurrentUserRequired();

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
        protected override IEnumerable<Expression<Func<Team, bool>>> BuildAdditionalFilters()
        {
            ApplicationUser currentUser = this._authContext.GetCurrentUserRequired();
            return [ti => ti.User == currentUser];
        }
    }
}



//using Microsoft.AspNetCore.Identity;
//using System.Linq.Expressions;
//using VacationManager.Core.Authentication.Abstractions;
//using VacationManager.Core.Authentication.Extensions;
//using VacationManager.Core.Prototypes;
//using VacationManager.Core.Services.Abstractions;
//using VacationManager.Data.Models;
//using VacationManager.Data.Repositories.Abstractions;

//namespace VacationManager.Core.Services
//{
//    public class TeamService : BaseService<Team, TeamPrototype>, ITeamService
//    {
//        private readonly Lazy<IProjectService> _projectService;
//        private readonly IAuthenticationContext _authContext;

//        public TeamService(
//            IRepository<Team> repository,
//            Lazy<IProjectService> projectService,
//            IAuthenticationContext authContext
//        ) : base(repository)
//        {
//            _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
//            _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));
//        }

//        protected override async Task ApplyAsync(Team entity, TeamPrototype prototype, CancellationToken cancellationToken)
//        {
//            entity.Name = prototype.Name;
//            entity.User = _authContext.GetCurrentUserRequired();

//            if (prototype.ProjectId.HasValue)
//            {
//                var project = await _projectService.Value.GetByIdRequiredAsync(prototype.ProjectId.Value, cancellationToken);
//                entity.ProjectId = project.Id;
//                entity.Project = project;
//            }
//            else
//            {
//                entity.Project = null;
//                entity.ProjectId = default;
//            }

//            entity.Developers = new List<ApplicationUser>();
//            if (prototype.Developers != null)
//            {
//                foreach (var developer in prototype.Developers)
//                {
//                    var userId = Guid.Parse(developer.Id);
//                    var user = await _authContext.GetUserByIdAsync(userId, cancellationToken);
//                    if (user == null)
//                    {
//                        throw new ArgumentException($"User with id {developer.Id} not found");
//                    }
//                    entity.Developers.Add(user);
//                }
//            }
//        }

//        protected override IEnumerable<Expression<Func<Team, bool>>> BuildAdditionalFilters()
//        {
//            var currentUser = _authContext.GetCurrentUserRequired();
//            return [team => team.User == currentUser];
//        }
//    }
//}

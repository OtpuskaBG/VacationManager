using System.Linq.Expressions;
using VacationManager.Core.Authentication.Abstractions;
using VacationManager.Core.Authentication.Extensions;
using VacationManager.Core.Prototypes;
using VacationManager.Core.Services.Abstractions;
using VacationManager.Data.Models;
using VacationManager.Data.Repositories.Abstractions;

namespace VacationManager.Core.Services
{
    public class LeaveRequestService(
        IRepository<LeaveRequest> repository,
        IAuthenticationContext authContext
    ) : BaseService<LeaveRequest, LeaveRequestPrototype>(repository), ILeaveRequestService
    {
        private readonly IAuthenticationContext _authContext = authContext ?? throw new ArgumentNullException(nameof(authContext));

        protected override async Task ApplyAsync(LeaveRequest entity, LeaveRequestPrototype prototype, CancellationToken cancellationToken)
        {
            entity.User = _authContext.GetCurrentUserRequired();
            entity.StartDate = prototype.StartDate;
            entity.EndDate = prototype.EndDate;
            entity.RequestDate = prototype.RequestDate;
            entity.HalfDay = prototype.HalfDay;
            entity.Type = prototype.Type;
            entity.AttachmentPath = prototype.AttachmentPath;
            entity.Approved = prototype.Approved;
        }

        protected override IEnumerable<Expression<Func<LeaveRequest, bool>>> BuildAdditionalFilters()
        {
            var currentUser = _authContext.GetCurrentUserRequired();
            return [lr => lr.User == currentUser];
        }
    }
}

﻿using System.Linq.Expressions;
using VacationManager.Core.Authentication.Abstractions;
using VacationManager.Core.Authentication.Extensions;
using VacationManager.Core.Prototypes;
using VacationManager.Core.Services.Abstractions;
using VacationManager.Data.Enums;
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

        public async Task ApproveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filters = new List<Expression<Func<LeaveRequest, bool>>>
    {
        r => r.Id == id
    };

            var request = await repository.GetAsync(filters, cancellationToken);
            if (request == null)
                throw new ArgumentException("Leave request not found");

            request.ApprovalStatus = ApprovalStatus.Approved;

            await repository.UpdateAsync(request, cancellationToken);
        }

        public async Task DenyAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filters = new List<Expression<Func<LeaveRequest, bool>>>
    {
        r => r.Id == id
    };

            var request = await repository.GetAsync(filters, cancellationToken);
            if (request == null)
                throw new ArgumentException("Leave request not found");

            request.ApprovalStatus = ApprovalStatus.Denied;

            await repository.UpdateAsync(request, cancellationToken);
        }



        protected override async Task ApplyAsync(LeaveRequest entity, LeaveRequestPrototype prototype, CancellationToken cancellationToken)
        {
            entity.User = _authContext.GetCurrentUserRequired();
            entity.StartDate = prototype.StartDate;
            entity.EndDate = prototype.EndDate;
            entity.RequestDate = prototype.RequestDate;
            entity.HalfDay = prototype.HalfDay;
            entity.Type = prototype.Type;
            entity.AttachmentPath = prototype.AttachmentPath;
            entity.ApprovalStatus = prototype.ApprovalStatus;
        }

        public async Task<LeaveRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filters = BuildAdditionalFilters().ToList();

            filters.Add(lr => lr.Id == id);

            return await repository.GetWithNavigationsAsync(
                filters,
                new[] { "User" },
                cancellationToken
            );
        }

        public async Task<List<LeaveRequest>> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            // Add the current user's role-based filters (CEO or TeamLead can see all leave requests)
            var filters = BuildAdditionalFilters().ToList();

            // Add a filter for the user ID to fetch only leave requests for the specific user
            filters.Add(lr => lr.User.Id == userId.ToString());

            // Fetch leave requests for the given user with navigation for related entities like 'User'
            var leaveRequests = await repository.GetManyWithNavigationsAsync(
                filters,
                new[] { "User" }, // Load related user data
                cancellationToken
            );

            return leaveRequests.ToList(); // Return the list of leave requests
        }

        
        public async Task<LeaveRequest[]> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var filters = BuildAdditionalFilters();
            return await repository.GetManyWithNavigationsAsync(
                filters,
                new[] { "User" },
                cancellationToken
            );
        }

        protected override IEnumerable<Expression<Func<LeaveRequest, bool>>> BuildAdditionalFilters()
        {
            var currentUser = _authContext.GetCurrentUserRequired();

            if (currentUser.Role is Role.CEO or Role.TeamLead)
            {
                return []; // няма филтри, виждат всичко
            }

            return [lr => lr.User == currentUser]; // само собствените заявки
        }

    }
}

using System.Reflection;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Moq;
using MudBlazor.Services;
using VacationManager.Core.Authentication;
using VacationManager.Core.Authentication.Abstractions;
using VacationManager.Core.Services.Abstractions;
using VacationManager.Data;
using VacationManager.Data.Repositories;
using VacationManager.Data.Repositories.Abstractions;
using VacationManager.Profiles;
using VacationManager.Components.Pages.Leave_Requests;
using VacationManager.Data.Models;
using Xunit;
using Bunit.TestDoubles;
using Microsoft.Extensions.DependencyInjection;
using VacationManager.Core.Services;
using VacationManager.Data.Enums;
using Assert = Xunit.Assert;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace VacationManager.Test;

public class CreateAndEditLeaveRequestAuthorizationTests : Bunit.TestContext
{
    private readonly TestAuthorizationContext _authContext;

    public CreateAndEditLeaveRequestAuthorizationTests()
    {
        Services.AddMudServices();
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        Services.AddAutoMapper(currentAssembly);
        Services.AddAutoMapper(typeof(LeaveRequestProfile));
        Services.AddScoped<IAuthenticationContext, AuthenticationContext>();
        Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        Services.AddDbContext<ApplicationDbContext>();

        // Mock leave request service
        var mockLeaveRequestService = new Mock<ILeaveRequestService>();
        mockLeaveRequestService
            .Setup(service => service.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VacationManager.Data.Models.LeaveRequest?)new()); // Simulate "leave request not found"
        Services.AddScoped(_ => mockLeaveRequestService.Object);
        Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
        Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        Services.AddDbContext<ApplicationDbContext>();

        _authContext = this.AddTestAuthorization();
        _authContext.SetAuthorized("testuser");
    }

    [Fact]
    public void DeveloperCanAccessCreateLeaveRequest()
    {
        _authContext.SetRoles(Role.Developer.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Leave_Requests.Create>();

        Assert.NotEqual("", cut.Markup);
        Assert.Contains("Create Leave Request", cut.Markup);
    }

    [Fact]
    public void TeamLeadCanAccessCreateLeaveRequest()
    {
        _authContext.SetRoles(Role.TeamLead.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Leave_Requests.Create>();

        Assert.NotEqual("", cut.Markup);
        Assert.Contains("Create Leave Request", cut.Markup);
    }

    [Fact]
    public void CEOCannotAccessCreateLeaveRequest()
    {
        _authContext.SetRoles(Role.CEO.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Leave_Requests.Create>();

        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void DeveloperCanAccessEditOwnLeaveRequest()
    {
        _authContext.SetRoles(Role.Developer.ToString());

        var leaveRequestId = Guid.NewGuid();
        var testUserId = "testuser";

        var mockLeaveRequestService = Services.GetRequiredService<Mock<ILeaveRequestService>>();
        mockLeaveRequestService
            .Setup(s => s.GetByIdAsync(leaveRequestId, default))
            .ReturnsAsync(new VacationManager.Data.Models.LeaveRequest
            {
                Id = leaveRequestId,
                UserId = testUserId,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(2),
                Type = LeaveType.Paid,
                ApprovalStatus = ApprovalStatus.Pending
            });

        var cut = RenderComponent<VacationManager.Components.Pages.Leave_Requests.Edit>(parameters => parameters.Add(p => p.leaveRequestId, leaveRequestId));

        Assert.NotEqual("", cut.Markup);
        Assert.Contains("Edit Leave Request", cut.Markup);
    }

    [Fact]
    public void CEOCannotAccessEditLeaveRequest()
    {
        _authContext.SetRoles(Role.CEO.ToString());

        var leaveRequestId = Guid.NewGuid();

        var cut = RenderComponent<VacationManager.Components.Pages.Leave_Requests.Edit>(parameters => parameters.Add(p => p.leaveRequestId, leaveRequestId));

        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void UnassignedCannotAccessCreateLeaveRequest()
    {
        _authContext.SetRoles(Role.Unassigned.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Leave_Requests.Create>();

        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void UnassignedCannotAccessEditLeaveRequest()
    {
        _authContext.SetRoles(Role.Unassigned.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Leave_Requests.Edit>();

        Assert.Equal("", cut.Markup);
    }
}
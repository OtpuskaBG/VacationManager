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

public class CreateAndEditProjectAuthorizationTests : Bunit.TestContext
{
    private readonly TestAuthorizationContext _authContext;

    private CreateAndEditProjectAuthorizationTests()
    {
        Services.AddMudServices();
        Assembly currentAssembly = Assembly.GetExecutingAssembly();
        Services.AddAutoMapper(currentAssembly);
        Services.AddAutoMapper(typeof(ProjectProfile));
        Services.AddAutoMapper(typeof(TeamProfile));
        Services.AddScoped<IAuthenticationContext, AuthenticationContext>();
        Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        var mockCourseService = new Mock<IProjectService>();
        mockCourseService
            .Setup(service => service.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((VacationManager.Data.Models.Project?)new()); // Simulate "course not found"
        Services.AddScoped(_ => mockCourseService.Object);

        var mockModuleService = new Mock<ITeamService>();
        mockModuleService
            .Setup(service => service.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Team?)new()); // Simulate "module not found"
        Services.AddScoped(_ => mockModuleService.Object);

        Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        Services.AddDbContext<ApplicationDbContext>();

        _authContext = this.AddTestAuthorization();
        _authContext.SetAuthorized("testuser");
    }

    [Fact]
    public void CEOCanAccessCreateProject()
    {
        _authContext.SetRoles(Role.CEO.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Projects.CreateProject>();

        Assert.NotEqual("", cut.Markup);
        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void CEOCanAccessEditProject()
    {
        _authContext.SetRoles(Role.CEO.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Projects.EditProject>();

        Assert.NotEqual("", cut.Markup);
        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void TeamLeadCannotAccessCreateProject()
    {
        _authContext.SetRoles(Role.TeamLead.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Projects.CreateProject>();

        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void TeamLeadCannotAccessEditProject()
    {
        _authContext.SetRoles(Role.TeamLead.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Projects.EditProject>();

        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void DeveloperCannotAccessCreateProject()
    {
        _authContext.SetRoles(Role.Developer.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Projects.CreateProject>();

        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void DeveloperCannotAccessEditProject()
    {
        _authContext.SetRoles(Role.Developer.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Projects.EditProject>();

        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void UnassignedCannotAccessCreateProject()
    {
        _authContext.SetRoles(Role.Unassigned.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Projects.CreateProject>();

        Assert.Equal("", cut.Markup);
    }

    [Fact]
    public void UnassignedCannotAccessEditProject()
    {
        _authContext.SetRoles(Role.Unassigned.ToString());

        var cut = RenderComponent<VacationManager.Components.Pages.Projects.EditProject>();

        Assert.Equal("", cut.Markup);
    }
}
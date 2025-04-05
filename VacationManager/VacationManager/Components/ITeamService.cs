using VacationManager.Data.Models;

public interface ITeamService
{
    Task<List<Team>> GetAllTeamsAsync();
    Task<Team?> GetTeamByIdAsync(Guid id);
    Task CreateTeamAsync(Team team);
}

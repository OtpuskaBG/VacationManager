using VacationManager.Data.Models;
using VacationManager.Data;
using Microsoft.EntityFrameworkCore;


public class TeamService : ITeamService
{
    private readonly ApplicationDbContext _context;

    public TeamService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        return await _context.Teams
            .Include(t => t.Developers)
            .Include(t => t.Project)
            .ToListAsync();
    }

    public async Task<Team?> GetTeamByIdAsync(Guid id)
    {
        return await _context.Teams
            .Include(t => t.Developers)
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task CreateTeamAsync(Team team)
    {
        _context.Teams.Add(team);
        await _context.SaveChangesAsync();
    }
}

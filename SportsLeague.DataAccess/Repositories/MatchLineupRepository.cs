using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories
{
    public class MatchLineupRepository : GenericRepository<MatchLineup>, IMatchLineupRepository
    {
        public MatchLineupRepository(LeagueDbContext context) : base(context) { }
        public async Task<bool> ExistsByMatchAndPlayerAsync(int matchId, int playerId)
        {
            // Busca si existe al menos un registro
            // en donde el MatchId y PlayerId coincidan.
            return await _context.MatchLinesUp
                .AnyAsync(ml =>
                    ml.MatchId == matchId &&
                    ml.PlayerId == playerId);
        }

        public async Task<int> CountStartersInLineUpAsync(int matchId, int teamId)
        {
            // Cuenta cuántos jugadores titulares tiene un equipo en un partido.
            return await _context.MatchLinesUp
             .CountAsync(ml => ml.MatchId == matchId &&
                         ml.Player.TeamId == teamId &&
                         ml.IsStarter); // Solo cuenta titulares.
        }

        public async Task<List<MatchLineup>> GetByMatchAndTeamAsync(int matchId, int teamId)
        {
            // Trae la alineación de un partido específico para un equipo específico.
            return await _context.MatchLinesUp
                .Where(ml => ml.MatchId == matchId &&
                             ml.Player.TeamId == teamId)
                .Include(ml => ml.Player)
                .ThenInclude(p => p.Team) // Incluye detalles del jugador.
                .ToListAsync();
        }
        public async Task<List<MatchLineup>> GetByMatchAsync(int matchId)
        {
            // Trae la alineación de un partido específico.
            return await _context.MatchLinesUp
            .Where(ml => ml.MatchId == matchId)
            .Include(ml => ml.Player)
            .ThenInclude(p => p.Team)
            .ToListAsync();
        }
    }
}

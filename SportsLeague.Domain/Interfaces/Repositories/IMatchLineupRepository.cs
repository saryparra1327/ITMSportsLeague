using SportsLeague.Domain.Entities;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IMatchLineupRepository :IGenericRepository<MatchLineup>
    {

        Task<bool> ExistsByMatchAndPlayerAsync(int matchId, int playerId);//No permite registrar un jugador dos veces en la misma alineación
        Task<int> CountStartersInLineUpAsync(int matchId, int teamId);//Valida que el máximo de jugadores titulares por equipo sean 11
        Task<List<MatchLineup>> GetByMatchAndTeamAsync(int matchId,int teamId);//Traer la alineación de un partido específico para un equipo específico (HomeTeam o AwayTeam)
        Task<List<MatchLineup>> GetByMatchAsync(int matchId);//Traer la alineación de un partido específico

    }
}

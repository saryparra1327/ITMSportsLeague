using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Services;
public interface IMatchLineUpService

{
    Task<MatchLineup> AddPlayerToLineUpAsync(int PlayerId, int MatchId, bool IsStarter, string Position);
    Task<MatchLineup?> GetLineUpByMatchIdAsync(int MatchId);
    Task<List<MatchLineup>> GetLineUpByMatchAndTeamIdAsync(int matchId, int TeamId);
    Task DeletePlayerFromLineUpAsync(int lineupId);

}


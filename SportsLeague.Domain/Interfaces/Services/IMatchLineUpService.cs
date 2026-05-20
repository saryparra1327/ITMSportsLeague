using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Services;
public interface IMatchLineUpService

{
    Task addPlayerToLineUp(int PlayerId, int MatchId);
    Task<MatchLineup?> GetLineUpByIdAsync(int Id);
    Task<MatchLineup?> GetLineUpByIdAsync(int Id, int TeamId);
    Task deletePlayerToLineUp(int PlayerId, int MatchId);

}


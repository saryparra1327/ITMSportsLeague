using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities;

public class MatchLineup: AuditBase
{
    public bool IsStarter { get; set; }
    public Position Position { get; set; }

    //Foreign Key
    public int MatchId { get; set; }
    public int PlayerId { get; set; }

    //Navigation Property 
    public Match Match { get; set; } = null!;
    public Player Player { get; set; } = null!;
}


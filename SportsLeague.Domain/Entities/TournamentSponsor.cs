namespace SportsLeague.Domain.Entities;


public class TournamentSponsor : AuditBase

{
   public decimal ContractAmount { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

   //Foreign Key
    public int TournamentId { get; set; }

    public int SponsorId { get; set; }


    // Navigation Properties

    public Tournament Tournament { get; set; } = null!;

    public Sponsor Sponsor { get; set; } = null!;
    
}

using SportsLeague.Domain.Enums;

namespace SportsLeague.Domain.Entities
{
    public class Sponsor : AuditBase
    {
        public string Name { get; set; } = string.Empty;

        public string ContactEmail { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? WebSiteUrl { get; set; }

        public SponsorCategory Category { get; set; }

        //Relacion N:M

        public ICollection<TournamentSponsor> TournamentSponsors { get; set; } = new List<TournamentSponsor>();
    }
}

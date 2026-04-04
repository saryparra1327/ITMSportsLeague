using SportsLeague.Domain.Enums;

namespace SportsLeague.API.DTOs.Request
{
    public class SponsorRequestDTO
    {

        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? WebSiteUrl { get; set; }
        public SponsorCategory Category { get; set; }
    }
}

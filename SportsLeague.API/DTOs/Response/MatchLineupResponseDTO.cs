namespace SportsLeague.API.DTOs.Response
{
    public class MatchLineupResponseDTO
    {
        public int MatchId { get; set; }
        public int MatchLineupId { get; set; }
        public int PlayerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public bool IsStarter { get; set; }

    }
}

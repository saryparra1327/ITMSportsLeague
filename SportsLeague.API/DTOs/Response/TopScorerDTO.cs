namespace SportsLeague.API.DTOs.Response;

public class TopScorerDTO
{
    //Este DTOs no tiene CreateMap en AutoMapper porque no se mapea desde una entidad.
    //Se construye manualmente en el Service con LINQ. SoEs un DTOs de reporte/estadística. 
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = string.Empty;
    public string TeamName { get; set; } = string.Empty;
    public int Goals { get; set; }
    public int Penalties { get; set; }
    public int MatchesWithGoals { get; set; }
}

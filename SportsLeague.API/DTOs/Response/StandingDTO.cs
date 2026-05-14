namespace SportsLeague.API.DTOs.Response;

public class StandingDTO
{
    //Este DTOs no tiene CreateMap en AutoMapper porque no se mapea desde una entidad.
    //Se construye manualmente en el Service con LINQ. SoEs un DTOs de reporte/estadística. 
    public int Position { get; set; }
    public int TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public int MatchesPlayed { get; set; }     // Partidos Jugados 
    public int Wins { get; set; }              // Partidos Ganados 
    public int Draws { get; set; }             // Partidos Empatados 
    public int Losses { get; set; }            // Partidos Perdidos 
    public int GoalsFor { get; set; }          // Goles a Favor 
    public int GoalsAgainst { get; set; }      // Goles en Contra 
    public int GoalDifference { get; set; }    // Diferencia de Goles 
    public int Points { get; set; }            // Puntos 

}

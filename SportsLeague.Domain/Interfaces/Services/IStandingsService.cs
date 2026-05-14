namespace SportsLeague.Domain.Interfaces.Services;

public interface IStandingsService
{
    Task<object> GetStandingsAsync(int tournamentId);//Obtener la tabla de posiciones para un torneo especifico
    Task<object> GetTopScorersAsync(int tournamentId);//Obtener la lista de los máximos goleadores pra un torneo
    Task<object> GetCardStatsAsync(int tournamentId);//Obtener la s estadisticas de tarjetas para un torneo
}
//¿Por qué retorna object?
//El Domain no conoce los DTOs de la capa API. Usamos object como tipo de retorno genérico(puede devolver cualquier dato).
//Otra opción sería crear clases de resultado en Domain, pero para simplificar usamos object.
//En la práctica, el Service construirá objetos anónimos o listas de objetos anónimos que el Controller enviará como JSON.
//Este object me sirve como un genericos sin tener que hacer una clase
//generica (como la clase en la que el generico lo llamamos T)
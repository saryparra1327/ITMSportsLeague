using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services;

public class MatchLineUpService : IMatchLineUpService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IMatchLineupRepository _matchLineupRepository;
    private readonly ILogger<MatchLineUpService> _logger;

    public MatchLineUpService(
        IPlayerRepository playerRepository,
        IMatchRepository matchRepository,
        IMatchLineupRepository matchLineupRepository,
        ILogger<MatchLineUpService> logger)
    {
        _playerRepository = playerRepository;
        _matchRepository = matchRepository;
        _matchLineupRepository = matchLineupRepository;
        _logger = logger;
    }

    public async Task<MatchLineup> AddPlayerToLineUpAsync(int playerId, int matchId, bool isStarter, string position)
    {
        // 1. Validar que el jugador exista
        var player = await _playerRepository.GetByIdWithTeamAsync(playerId);
        if (player == null)
        {
            _logger.LogWarning("Player with ID {PlayerId} not found", playerId);
            throw new KeyNotFoundException(
                $"No se encontró el jugador con ID {playerId}");
        }

        // 2. Validar que el partido exista
        var existingMatch = await _matchRepository.GetByIdAsync(matchId);
        if (existingMatch == null)
        {
            _logger.LogWarning("Match with ID {matchId} not found", matchId);
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");
        }

        // 6. Validar que el partido esté en estado Scheduled
        if (existingMatch.Status != MatchStatus.Scheduled)
        {
            _logger.LogWarning("Match should be Scheduled");
            throw new InvalidOperationException(
            "Solo se pueden editar partidos con estado Scheduled");
        }

        // 3. Validar que el jugador pertenezca al HomeTeam o AwayTeam del partido
        if (player.TeamId != existingMatch.HomeTeamId && player.TeamId != existingMatch.AwayTeamId)
        {
            _logger.LogWarning("The player does not belong to the HomeTeam or AwayTeam " +
                "of the match {matchId}", matchId);
            throw new KeyNotFoundException($"El jugador no pertenece al Equipo local o al equipo visitante" +
                $" del partido {matchId}");
        }

        // 4. Validar que el jugador no esté registrado dos veces en la misma alineación
        var playerExists = await _matchLineupRepository
    .ExistsByMatchAndPlayerAsync(matchId, playerId);//Busca si existe al menos un registro en donde
                                                    //el MatchId y PlayerId coincidan.

        if (playerExists)
        {
            _logger.LogWarning(
                "Player {PlayerId} is already registered in match {matchId}", playerId, matchId);
            throw new InvalidOperationException(
                "El jugador ya está registrado en la alineación");
        }

        // 5. Validar que el máximo de jugadores titulares por equipo sean 11

        if (isStarter)
        {
            var startersCount = await _matchLineupRepository
                .CountStartersInLineUpAsync(matchId, player.TeamId);//Cuenta cuántos jugadores titulares tiene un equipo en un partido.
            if (startersCount >= 11)
            {
                _logger.LogWarning(
                    "Team {TeamId} already has 11 starters in match {matchId}", player.TeamId, matchId);
                throw new InvalidOperationException(
                    "El equipo ya tiene 11 jugadores titulares en la alineación");
            }
        }

        var lineup = new MatchLineup //Guarda la alineación del jugador en el partido
        {
            MatchId = matchId,
            PlayerId = playerId,
            IsStarter = isStarter,
            Position = position
        };


        await _matchLineupRepository.CreateAsync(lineup);
        return lineup;
    }


    public async Task<MatchLineup?> GetLineUpByMatchIdAsync(int matchId)//Trae la alineación por ID de partido
    {
        _logger.LogInformation(
            "Retrieving lineup with Match ID: {matchId}",
            matchId);

        var lineup = await _matchLineupRepository
            .GetByIdAsync(matchId);

        if (lineup == null)
        {
            _logger.LogWarning(
                "Lineup with Match ID {matchId} not found",
                matchId);
        }

        return lineup;

    }

    public async Task<List<MatchLineup>> GetLineUpByMatchAndTeamIdAsync(int matchId, int TeamId)//Trae la alineación de un partido para un equipo específico
    {
        _logger.LogInformation(
          "Retrieving lineup for Team {TeamId} in Match {matchId}",
          TeamId,
          matchId);

        return await _matchLineupRepository
            .GetByMatchAndTeamAsync(matchId, TeamId);
    }
    public async Task DeletePlayerFromLineUpAsync(int lineupId)//Elimina un jugador de la alineación de un partido específico
    {
        var lineup = await _matchLineupRepository.GetByIdAsync(lineupId);

        if (lineup == null)
        {
            _logger.LogWarning("Lineup with Id {LineupId} not found", lineupId);
            throw new KeyNotFoundException($"No se encontró la alineación con ID {lineupId}");
        }

        // Elimina la alineación.
        await _matchLineupRepository.DeleteAsync(lineupId);

        _logger.LogInformation(
            "Lineup {LineupId} deleted successfully",
            lineupId);
    }

}
using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Services;

public class MatchLineUpService : IMatchLineUpService
{
    private readonly IPlayerRepository _playerRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly ILogger<MatchLineUpService> _logger;

    public MatchLineUpService(
        IPlayerRepository playerRepository,
        IMatchRepository matchRepository,
        ILogger<MatchLineUpService> logger)
    {
        _playerRepository = playerRepository;
        _matchRepository = matchRepository;
        _logger = logger;
    }

    public async Task addPlayerToLineUp(int playerId, int matchId)
    {
        // 1. Validar que el jugador exista
        var player = await _playerRepository.GetByIdWithTeamAsync(playerId);
        if (player == null)
        {
            _logger.LogWarning("Player with ID {PlayerId} not found", playerId);
            throw new KeyNotFoundException(
                $"No se encontró el jugadpr con ID {playerId}");
        }
        // 2. Validar que el partido exista
        var existingMatch = await _matchRepository.GetByIdAsync(matchId);
        if (existingMatch == null)
        {
            _logger.LogWarning("Match with ID {MatchId} not found", matchId);
            throw new KeyNotFoundException($"No se encontró el partido con ID {matchId}");
        }
        // 3. Validar que el jugador pertenezca al HomeTeam o AwayTeam del partido
        if (player.TeamId != existingMatch.HomeTeamId && player.TeamId != existingMatch.AwayTeamId)
        {
            _logger.LogWarning("The player does not belong to the HomeTeam or AwayTeam " +
                "of the match {MatchId}", matchId);
            throw new KeyNotFoundException($"El jugador no pertenece al Equipo local o al equipo visitante" +
                $" del partido {matchId}");
        }

        // 4. Validar que el jugador no esté registrado dos veces en la misma alineación

        // 5. Validar que el máximo de jugadores titulares por equipo sean 11

        // 6. Validar que el partido esté en estado Scheduled
        if (existingMatch.Status != MatchStatus.Scheduled)
        {
            _logger.LogWarning("Match should be Scheduled");
            throw new InvalidOperationException(
            "Solo se pueden editar partidos con estado Scheduled");
        }

        throw new NotImplementedException();
    }

    public Task deletePlayerToLineUp(int PlayerId, int MatchId)
    {
        throw new NotImplementedException();
    }

    public Task<MatchLineup?> GetLineUpByIdAsync(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<MatchLineup?> GetLineUpByIdAsync(int Id, int TeamId)
    {
        throw new NotImplementedException();
    }

    public async Task<Player?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Retrieving player with ID: {PlayerId}", id);
        var player = await _playerRepository.GetByIdWithTeamAsync(id);

        if (player == null)
        {
            _logger.LogWarning("Player with ID {PlayerId} not found", id);
        }

        return player;
    }
}


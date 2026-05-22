using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;
using SportsLeague.Domain.Services;


namespace SportsLeague.API.Controllers
{
    
        [ApiController]
        [Route("api/match/{matchId}/lineup")]
    public class MatchLineUpController : ControllerBase
        {
            private readonly IMatchLineUpService _matchLineupService;
            private readonly IMapper _mapper;
            public MatchLineUpController(
                IMatchLineUpService matchLineUpService, IMapper mapper)
            {
                _matchLineupService = matchLineUpService;
                _mapper = mapper;
            }

        [HttpPost]
        public async Task<ActionResult<MatchLineupResponseDTO>>AddPlayerToLineUpAsync(int matchId,MatchLineupRequestDTO dto)
        {
            try
            {
                var lineup = _mapper.Map<MatchLineup>(dto);

                var createdLineup = await _matchLineupService
                    .AddPlayerToLineUpAsync(
                        lineup.PlayerId,
                        matchId,
                        lineup.IsStarter,
                        lineup.Position);

                var responseDto = _mapper.Map<MatchLineupResponseDTO>(
                    createdLineup);

                return StatusCode(201, responseDto);
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }

            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<MatchLineupResponseDTO>>> GetLineUpByMatchIdAsync(int matchId)
        {
            try
            {
                var lineup = await _matchLineupService
                    .GetLineUpByMatchIdAsync(matchId);

                var response =
                    _mapper.Map<MatchLineupResponseDTO>(lineup);

                return Ok(response);
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
        }

        [HttpGet("team/{teamId}")]
        public async Task<ActionResult<List<MatchLineupResponseDTO>>> GetLineUpByMatchAndTeamIdAsync(int matchId, int teamId)
        {
            var lineup = await _matchLineupService
                .GetLineUpByMatchAndTeamIdAsync(matchId, teamId);

            var response = _mapper.Map<List<MatchLineupResponseDTO>>(lineup);

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayerFromLineUpAsync(int lineupId)
        {
            try
            {
                await _matchLineupService
                    .DeletePlayerFromLineUpAsync(lineupId);

                return Ok(new
                {
                    message = "Jugador eliminado correctamente"
                });
            }

                catch (KeyNotFoundException ex)
                {
                     return NotFound(new{message = ex.Message});
                }
            }
         }
}

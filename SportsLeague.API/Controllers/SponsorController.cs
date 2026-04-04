using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SponsorController : ControllerBase
    {
        private readonly ISponsorService _sponsorService;
        private readonly IMapper _mapper;

        public SponsorController(ISponsorService sponsorService, IMapper mapper)
        {
            _sponsorService = sponsorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SponsorResponseDTO>>> GetAll()
        {
            var sponsors = await _sponsorService.GetAllAsync();
            var sponsorDto = _mapper.Map<IEnumerable<SponsorResponseDTO>>(sponsors);

            return Ok(sponsorDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SponsorResponseDTO>> GetById(int id)
        {
            var sponsor = await _sponsorService.GetByIdAsync(id);

            if (sponsor == null)
            {
                return NotFound($"No se encontró el sponsor con id {id}");
            }

            var sponsorDto = _mapper.Map<SponsorResponseDTO>(sponsor);
            return Ok(sponsorDto);
        }

        [HttpPost]
        public async Task<ActionResult<SponsorResponseDTO>> CreateAsync(SponsorRequestDTO request)
        {
            var sponsor = _mapper.Map<Sponsor>(request);
            var createdSponsor = await _sponsorService.CreateAsync(sponsor);

            var sponsorDto = _mapper.Map<SponsorResponseDTO>(createdSponsor);

            return CreatedAtAction(
                nameof(GetById),
                new { id = sponsorDto.Id },
                sponsorDto);
        }

        //ASociar un Patrocinador con un Torneo
        [HttpPost("{id}/tournaments")]
        public async Task<IActionResult> LinkSponsorAsync(int id, TournamentSponsorRequestDTO request)
        {
            // validar que el id del sponsor coincide
            if (id != request.SponsorId)
                return BadRequest("El ID del sponsor no coincide");

            var tournamentSponsor = await _sponsorService.LinkSponsorAsync(
                _mapper.Map<TournamentSponsor>(request));

            var response = _mapper.Map<TournamentSponsorResponseDTO>(tournamentSponsor);

            return Ok(response);
        }

        //Mostrar todos los torneos de un Sponsor
        [HttpGet("{id}/tournaments")]
        public async Task<IActionResult> GetTournamentsBySponsor(int id)
        {
            var tournamentSponsors = await _sponsorService.GetTournamentsBySponsorAsync(id);

            var response = _mapper.Map<IEnumerable<TournamentSponsorResponseDTO>>(tournamentSponsors);

            return Ok(response);
        }

        [HttpPut("{id}")]//Actualizar Un Sponsor con Id
        public async Task<IActionResult> Update(int id, SponsorRequestDTO request)
        {
            var sponsor = _mapper.Map<Sponsor>(request);

            try
            {
                await _sponsorService.UpdateAsync(id, sponsor);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        //Desvincular un sponsor de un torneo
        [HttpDelete("{id}/tournaments/{tournamentId}")]
        public async Task<IActionResult> UnLinkSponsorAsync(int SponsorId, int tournamentId)
        {
            await _sponsorService.UnLinkSponsorAsync(tournamentId, SponsorId);
            return NoContent();
        }

        [HttpDelete("{id}")]//Eliminar un Sponsor
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sponsorService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}



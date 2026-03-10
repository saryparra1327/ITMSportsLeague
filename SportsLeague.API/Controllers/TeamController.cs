using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportsLeague.API.DTOs.Request;
using SportsLeague.API.DTOs.Response;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.API.Controllers
{
    [ApiController] //DataAnnottations para validar el modelo, si el modelo no es valido, se devuelve un 400 Bad Request con los errores de validación
    //Valida que por ejemplo en un int no se mande otro tipo de dato.
    [Route("api/[controller]")] // para enrutar las url
    public class TeamController : ControllerBase // esta clase controllerBase me ayuda a definir los me´todo de la api con el protocolo http
    {
        private readonly ITeamService _teamService;
        private readonly IMapper _mapper;
        private readonly ILogger<TeamController> _logger;

        public TeamController(
            ITeamService teamService,
            IMapper mapper,
            ILogger<TeamController> logger)
        {
            _teamService = teamService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamResponseDTO>>> GetAll()
            //Obtiene todos los equipos, el TeamResponse es porque esto se va a exponer directamente al usuario.
        {
            var teams = await _teamService.GetAllAsync();
            var teamsDto = _mapper.Map<IEnumerable<TeamResponseDTO>>(teams);// El autoMapper es para poder ir del
                                                                            // Dto a la entidad team, me convierte un objeto en otro

            return Ok(teamsDto);
        }

        [HttpGet("{id}")] //DattaAnotation, get para obtener, le agrega el id al route
        public async Task<ActionResult<TeamResponseDTO>> GetById(int id)
        {
            var team = await _teamService.GetByIdAsync(id);

            if (team == null)
                return NotFound(new { message = $"Equipo con ID {id} no encontrado" });

            var teamDto = _mapper.Map<TeamResponseDTO>(team);
            return Ok(teamDto);
        }

        [HttpPost] // Post para crear
        public async Task<ActionResult<TeamResponseDTO>> Create(TeamRequestDTO dto)
        {
            try
            {
                var team = _mapper.Map<Team>(dto);
                var createdTeam = await _teamService.CreateAsync(team);
                var responseDto = _mapper.Map<TeamResponseDTO>(createdTeam);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = responseDto.Id },
                    responseDto);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")] // Put para actualizar
        public async Task<ActionResult> Update(int id, TeamRequestDTO dto)
        {
            try
            {
                var team = _mapper.Map<Team>(dto);
                await _teamService.UpdateAsync(id, team);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")] // Eliminar
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _teamService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            } // Aquí no tenemso que mappear nada porque estemétodo es solo eliminar con el id
        }
    }
}

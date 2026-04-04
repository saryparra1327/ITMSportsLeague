using Microsoft.Extensions.Logging;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using SportsLeague.Domain.Interfaces.Services;

namespace SportsLeague.Domain.Services
{
    public class SponsorService : ISponsorService
    {
        private readonly ISponsorRepository _sponsorRepository;
        private readonly ILogger<SponsorService> _logger;

        private readonly ITournamentRepository _tournamentRepository;
        private readonly ITournamentSponsorRepository _tournamentSponsorRepository;

        public SponsorService(ISponsorRepository sponsorRepository,
            ITournamentRepository tournamentRepository,
            ITournamentSponsorRepository tournamentSponsorRepository,
            ILogger<SponsorService> logger)
        {
            _sponsorRepository = sponsorRepository;
            _tournamentSponsorRepository = tournamentSponsorRepository;
            _tournamentRepository = tournamentRepository;
            _logger = logger;


        }
        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            _logger.LogInformation("Retrieving all Sponsors");
            return await _sponsorRepository.GetAllAsync();
        }

        public async Task<Sponsor?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Retrieving sponsor with ID: {SponsorId}", id);
            var sponsor = await _sponsorRepository.GetByIdAsync(id);

            if (sponsor == null)
                _logger.LogWarning("sponsor with ID {SponsorId} not found", id);

            return sponsor;
        }
        public async Task<Sponsor> GetByNameAsync(string name)
        {
            var sponsor = await _sponsorRepository.GetByNameAsync(name);

            if (sponsor == null)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con nombre {name}");
            }

            return sponsor;
        }


        public async Task<Sponsor> CreateAsync(Sponsor sponsor)
        {
            //Validar email 
            try
            {
                var addr = new System.Net.Mail.MailAddress(sponsor.ContactEmail);
            }
            catch
            {
                throw new InvalidOperationException("El email no tiene un formato válido");
            }

            // Validación de negocio: nombre único
            var existingSponsor = await _sponsorRepository.GetByNameAsync(sponsor.Name);
            if (existingSponsor != null)
            {
                _logger.LogWarning("Sponsor with name '{SponsorName}' already exists", sponsor.Name);
                throw new InvalidOperationException(
                    $"Ya existe un patrocinador con el nombre '{sponsor.Name}'");
            }

            _logger.LogInformation("Creating sponsor: {SponsorName}", sponsor.Name);
            return await _sponsorRepository.CreateAsync(sponsor);



        }


        public async Task UpdateAsync(int id, Sponsor sponsor)
        {
            var existingSponsor = await _sponsorRepository.GetByIdAsync(id);
            if (existingSponsor == null)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found for update", id);
                throw new KeyNotFoundException(
                    $"No se encontró el Patrocinador con ID {id}");
            }

            // Validar nombre único
            if (existingSponsor.Name != sponsor.Name)
            {
                var SponsorWithSameName = await _sponsorRepository.GetByNameAsync(sponsor.Name);
                if (SponsorWithSameName != null)
                {
                    throw new InvalidOperationException(
                        $"Ya existe un patrocinador  con el nombre '{sponsor.Name}'");
                }
            }

            existingSponsor.Name = sponsor.Name;
            existingSponsor.ContactEmail = sponsor.ContactEmail;
            existingSponsor.Phone = sponsor.Phone;
            existingSponsor.WebSiteUrl = sponsor.WebSiteUrl;
            existingSponsor.Category = sponsor.Category;

            _logger.LogInformation("Updating Sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.UpdateAsync(existingSponsor);
        }

        public async Task DeleteAsync(int id)
        {
            var exists = await _sponsorRepository.ExistsAsync(id);
            if (!exists)
            {
                _logger.LogWarning("Sponsor with ID {SponsorId} not found for deletion", id);
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {id}");
            }

            _logger.LogInformation("Deleting sponsor with ID: {SponsorId}", id);
            await _sponsorRepository.DeleteAsync(id);
        }

        public async Task<TournamentSponsor> LinkSponsorAsync(TournamentSponsor tournamentSponsor)
        {
            if (tournamentSponsor.ContractAmount <= 0)
            {
                throw new InvalidOperationException("El monto del contrato debe ser mayor a 0");
            }

            var sponsor = await _sponsorRepository.GetByIdAsync(tournamentSponsor.SponsorId);
            if (sponsor == null)
            {
                throw new KeyNotFoundException("El sponsor no existe");
            }

            var tournament = await _tournamentRepository.GetByIdAsync(tournamentSponsor.TournamentId);
            if (tournament == null)
            {
                throw new KeyNotFoundException("El torneo no existe");
            }

            var existing = await _tournamentSponsorRepository
                .GetRelationAsync(tournamentSponsor.TournamentId, tournamentSponsor.SponsorId);

            if (existing != null)
            {
                throw new InvalidOperationException("La relación ya existe");
            }

            return await _tournamentSponsorRepository.LinkSponsorAsync(tournamentSponsor);
        }

        public async Task UnLinkSponsorAsync(int tournamentId, int sponsorId)
        {
            var relation = await _tournamentSponsorRepository
                .GetRelationAsync(tournamentId, sponsorId);

            if (relation == null)
            {
                throw new KeyNotFoundException("La relación no existe");
            }

            await _tournamentSponsorRepository.UnLinkSponsorAsync(relation.TournamentId, relation.SponsorId);
        }
        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            var sponsor = await _sponsorRepository.GetByIdAsync(sponsorId);

            if (sponsor == null)
            {
                throw new KeyNotFoundException(
                    $"No se encontró el patrocinador con ID {sponsorId}");
            }

            var tournamentSponsors = await _tournamentSponsorRepository
                .GetBySponsorIdAsync(sponsorId);

            return tournamentSponsors;
        }
    }
}

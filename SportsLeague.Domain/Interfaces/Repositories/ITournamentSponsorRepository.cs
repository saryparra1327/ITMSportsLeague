using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface ITournamentSponsorRepository
    {
        Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId);//sponsors de un torneo
        Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId);//accede a la db
        Task<TournamentSponsor?> GetRelationAsync(int tournamentId, int sponsorId);//para evitar la duplicacion en vinculación
        Task<TournamentSponsor> LinkSponsorAsync(TournamentSponsor tournamentSponsor);//vincula un sponsor a un torneo
        Task UnLinkSponsorAsync(int tournamentId, int sponsorId);//desvincula de un torneo
        Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId);//obtiene los torneos de un sponsor
    }
}

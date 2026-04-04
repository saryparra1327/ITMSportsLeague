using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.DataAccess.Repositories
{
    public class TournamentSponsorRepository
    : GenericRepository<TournamentSponsor>, ITournamentSponsorRepository
    {
        public TournamentSponsorRepository(LeagueDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TournamentSponsor>> GetByTournamentIdAsync(int tournamentId)
        {
            return await _dbSet
                .Where(ts => ts.TournamentId == tournamentId)
                .Include(ts => ts.Sponsor) 
                .ToListAsync();
        }

        public async Task<IEnumerable<TournamentSponsor>> GetBySponsorIdAsync(int sponsorId)
        {
            return await _dbSet
                .Where(ts => ts.SponsorId == sponsorId)
                .Include(ts => ts.Tournament)
                .ToListAsync();
        }

        public async Task<TournamentSponsor?> GetRelationAsync(int tournamentId, int sponsorId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(ts =>
                    ts.TournamentId == tournamentId &&
                    ts.SponsorId == sponsorId);
        }//Devuelve la relación en donde el sponsorId y TorurnamentId Coincida
        public async Task<TournamentSponsor> LinkSponsorAsync(TournamentSponsor tournamentSponsor)
        {
            // Agrega la relación a la base de datos
            await _dbSet.AddAsync(tournamentSponsor);
            await _context.SaveChangesAsync();
            return tournamentSponsor;
        }

        public async Task UnLinkSponsorAsync(int tournamentId, int sponsorId)
        {
            // Busca la relación
            var relation = await GetRelationAsync(tournamentId, sponsorId);
            if (relation != null)
            {
                _dbSet.Remove(relation);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<TournamentSponsor>> GetTournamentsBySponsorAsync(int sponsorId)
        {
            return await _dbSet
                .Where(ts => ts.SponsorId == sponsorId)
                .Include(ts => ts.Tournament)
                .Include(ts => ts.Sponsor)
                .ToListAsync();
        }
    }
}

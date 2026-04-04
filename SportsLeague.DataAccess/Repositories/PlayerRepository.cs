using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.DataAccess.Repositories
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository

    {

        public PlayerRepository(LeagueDbContext context) : base(context)
        {
            //Método constructor donde le inyeccto la dependencia del contexto de la base de datos (context)
        }
        public async Task<IEnumerable<Player>> GetByTeamAsync(int teamId)

        {

            return await _dbSet
            .Where(p => p.TeamId == teamId)//El id que el paso en el parámetro
                                           //debe ser igualalid de la tabla player,me permite filtrar solo los jugadores del equipo especificado con id
            .Include(p => p.Team)//Es un Inner Join para incluir la información del equipo en el resultado
            .ToListAsync();//Se devuelve una lista de jugadores

        }
        public async Task<Player?> GetByTeamAndNumberAsync(int teamId, int number)
        {

            return await _dbSet
            .FirstOrDefaultAsync(p => p.TeamId == teamId && p.Number == number);//Retorna un jugador con oda su información
            //Linq es la forma simplificada de escribir consultas a bases de datos
        }


        public async Task<IEnumerable<Player>> GetAllWithTeamAsync()
        {
            return await _dbSet
            .Include(p => p.Team)
            .ToListAsync();

        }
        public async Task<Player?> GetByIdWithTeamAsync(int id)
        {

            return await _dbSet
            .Where(p => p.Id == id)
            .Include(p => p.Team)
            .FirstOrDefaultAsync();


            /*.Include(p => p.Team)
            .FirstOrDefaultAsync(p => p.Id == id);Esta es otra forma de hacerlo y funciona exactamente igual*/
        }
    }
}
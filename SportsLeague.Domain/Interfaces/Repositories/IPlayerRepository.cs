using SportsLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SportsLeague.Domain.Interfaces.Repositories
{
    public interface IPlayerRepository : IGenericRepository<Player>/*Todas las clases Repository
                                                                Deben tener herecia de la clase genérica*/

    {

        Task<IEnumerable<Player>> GetByTeamAsync(int teamId);//Obtener todos los jugadores por equipo

        Task<Player?> GetByTeamAndNumberAsync(int teamId, int number);//Obtener el jugador por id del equipo
                                                                      //y numero de camiseta

        Task<IEnumerable<Player>> GetAllWithTeamAsync();//Obtiene todos los jugadores de todos los equipos,
                                               //no le mando parámetros por que no es importante ningún id 

        Task<Player?> GetByIdWithTeamAsync(int id);//Obtiene jugador por id del jugador
        /*Estos métodos son abstractos la lógica de estos estará en la clase PlayerRepository*/
    }
}

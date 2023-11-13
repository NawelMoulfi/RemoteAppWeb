using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;
using System.Reflection;

namespace RemoteAppApi.Repositories
{
    public class RapportInterventionRepository : IRapportInterventionRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public RapportInterventionRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<RapportInterventionDto> AddRapportIntervention(RapportInterventionDto RapportIntervention)
        {
            var RapportIntervention1 = mapper.Map<RapportInterventionDto, RapportIntervention>(RapportIntervention);
            var addedEntity = await _appDbContext.RapportInterventions.AddAsync(RapportIntervention1);
            await _appDbContext.SaveChangesAsync();
            var RapportInterventionDTO = mapper.Map<RapportIntervention, RapportInterventionDto>(addedEntity.Entity);
            return RapportInterventionDTO;
          
        }

        public async Task DeleteRapportIntervention(long RapportInterventionId)
        {
            var foundRapportIntervention = await _appDbContext.RapportInterventions.FirstOrDefaultAsync(e => e.RapportId == RapportInterventionId);
            if (foundRapportIntervention == null) return;

            _appDbContext.RapportInterventions.Remove(foundRapportIntervention);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RapportInterventionDto>> GetAllRapportInterventions()
        {
            var RapportInterventions = new List<RapportIntervention>();
            RapportInterventions = await this._appDbContext.RapportInterventions.Include(p => p.CreatedByUser).ToListAsync();//.ToListAsync();
        
        List <RapportInterventionDto> RapportInterventionDtos = new List<RapportInterventionDto>();
            foreach (var RapportIntervention in RapportInterventions)
            {
                RapportInterventionDtos.Add(mapper.Map<RapportIntervention, RapportInterventionDto>(RapportIntervention));
            }
            return RapportInterventionDtos;
         
                }

        public async Task<RapportInterventionDto> GetRapportInterventionrById(long RapportInterventionId)
        {
            var RapportIntervention = await _appDbContext.RapportInterventions.FirstOrDefaultAsync(c => c.RapportId == RapportInterventionId);
            var RapportInterventionDTO = mapper.Map<RapportIntervention, RapportInterventionDto>(RapportIntervention);
            return RapportInterventionDTO;

         
        }

        public async Task<RapportInterventionDto> UpdateRapportIntervention(RapportInterventionDto RapportIntervention)
        {
            var RapportIntervention1 = mapper.Map<RapportInterventionDto, RapportIntervention>(RapportIntervention);
            var foundRapportIntervention = await _appDbContext.RapportInterventions.FirstOrDefaultAsync(e => e.RapportId == RapportIntervention1.RapportId);

            if (foundRapportIntervention != null)
            {
                foundRapportIntervention.CreatedDate = RapportIntervention1.CreatedDate;
                foundRapportIntervention.Operation = RapportIntervention1.Operation;
                foundRapportIntervention.CommentaireTraveaux = RapportIntervention1.CommentaireTraveaux;
                foundRapportIntervention.AutreInformation = RapportIntervention1.AutreInformation;
                foundRapportIntervention.Num = RapportIntervention1.Num;
                foundRapportIntervention.Logiciel = RapportIntervention1.Logiciel;
                //foundRapportIntervention.ClientId = RapportIntervention.ClientId;


                 await _appDbContext.SaveChangesAsync();
                var RapportInterventionDTO = mapper.Map<RapportIntervention, RapportInterventionDto>(foundRapportIntervention);

                return RapportInterventionDTO;

                
            }

            return null;
        }
    }
}

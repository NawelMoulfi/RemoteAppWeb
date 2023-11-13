using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using System.Reflection;

namespace RemoteAppApi.Repositories
{
    public class RapportInterventionRepository : IRapportInterventionRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public RapportInterventionRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<RapportIntervention> AddRapportIntervention(RapportIntervention RapportIntervention)
        {
            var addedEntity = await _appDbContext.RapportInterventions.AddAsync(RapportIntervention);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteRapportIntervention(long RapportInterventionId)
        {
            var foundRapportIntervention = await _appDbContext.RapportInterventions.FirstOrDefaultAsync(e => e.RapportId == RapportInterventionId);
            if (foundRapportIntervention == null) return;

            _appDbContext.RapportInterventions.Remove(foundRapportIntervention);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<RapportIntervention>> GetAllRapportInterventions()
        {
            return await this._appDbContext.RapportInterventions.Include(p => p.CreatedByUser).ToListAsync();//.ToListAsync();
        }

        public async Task<RapportIntervention> GetRapportInterventionrById(long RapportInterventionId)
        {
            return await _appDbContext.RapportInterventions.FirstOrDefaultAsync(c => c.RapportId == RapportInterventionId);
        }

        public async Task<RapportIntervention> UpdateRapportIntervention(RapportIntervention RapportIntervention)
        {
            var foundRapportIntervention = await _appDbContext.RapportInterventions.FirstOrDefaultAsync(e => e.RapportId == RapportIntervention.RapportId);

            if (foundRapportIntervention != null)
            {
                foundRapportIntervention.CreatedDate = RapportIntervention.CreatedDate;
                foundRapportIntervention.Operation = RapportIntervention.Operation;
                foundRapportIntervention.CommentaireTraveaux = RapportIntervention.CommentaireTraveaux;
                foundRapportIntervention.AutreInformation = RapportIntervention.AutreInformation;
                foundRapportIntervention.Num = RapportIntervention.Num;
                foundRapportIntervention.Logiciel = RapportIntervention.Logiciel;
                //foundRapportIntervention.ClientId = RapportIntervention.ClientId;


                 await _appDbContext.SaveChangesAsync();

                return foundRapportIntervention;
            }

            return null;
        }
    }
}

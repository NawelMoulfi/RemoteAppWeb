using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using System;

namespace RemoteAppApi.Repositories
{
    public class MaterielRapportRepository : IMaterielRapportRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public MaterielRapportRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<MaterielRapport> AddMaterielRapport(MaterielRapport materielRapport)
        {
            var addedEntity = await _appDbContext.MaterielRapports.AddAsync(materielRapport);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<IEnumerable<MaterielRapport>> GetMateriels(long RapportId)
        {
            return await _appDbContext.MaterielRapports
          .Include(m => m.RapportIntervention)
              .ThenInclude(r => r.CreatedByUser)
           .Include(m=>m.Materiel)
          .Where(m => m.RapportInterventionId == RapportId)
          .ToListAsync();
            //_appContext.SaveChanges();

        }
    }
}

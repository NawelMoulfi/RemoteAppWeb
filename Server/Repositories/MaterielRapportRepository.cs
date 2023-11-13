using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;
using System;

namespace RemoteAppApi.Repositories
{
    public class MaterielRapportRepository : IMaterielRapportRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public MaterielRapportRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<MaterielRapportDto> AddMaterielRapport(MaterielRapportDto materielRapport)
        {
            var MaterielRapport = mapper.Map<MaterielRapportDto, MaterielRapport>(materielRapport);
            var addedEntity = await _appDbContext.MaterielRapports.AddAsync(MaterielRapport);
            await _appDbContext.SaveChangesAsync();
            var MaterielRapportDTO = mapper.Map<MaterielRapport, MaterielRapportDto>(addedEntity.Entity);
            return MaterielRapportDTO;
        }

        public async Task<IEnumerable<MaterielRapportDto>> GetMateriels(long RapportId)
        {
            var MaterielRapports = new List<MaterielRapport>();
            MaterielRapports = await _appDbContext.MaterielRapports
          .Include(m => m.RapportIntervention)
              .ThenInclude(r => r.CreatedByUser)
           .Include(m => m.Materiel)
          .Where(m => m.RapportInterventionId == RapportId)
          .ToListAsync();
            List<MaterielRapportDto> MaterielRapportDtos = new List<MaterielRapportDto>();
            foreach (var MaterielRapport in MaterielRapports)
            {
                MaterielRapportDtos.Add(mapper.Map<MaterielRapport, MaterielRapportDto>(MaterielRapport));
            }
            return MaterielRapportDtos;
           // return 
            //_appContext.SaveChanges();

        }
    }
}

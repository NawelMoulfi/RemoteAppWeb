using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using System;

namespace RemoteAppApi.Repositories
{
    public class MaterielRepository:IMaterielRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public MaterielRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Materiel> AddMateriel(Materiel materiel)
        {
            var addedEntity = await _appDbContext.Materiels.AddAsync(materiel);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteMateriel(long materielId)
        {
            var foundMateriel = await _appDbContext.Materiels.FirstOrDefaultAsync(e => e.MaterielId == materielId);
            if (foundMateriel == null) return;

            _appDbContext.Materiels.Remove(foundMateriel);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Materiel>> GetAllMateriels()
        {
            return await this._appDbContext.Materiels.ToListAsync();//.Include(p => p.RapportIntervention)
        }

        public async Task<Materiel> GetMaterielById(long MaterielId)
        {
            return await _appDbContext.Materiels.FirstOrDefaultAsync(c => c.MaterielId == MaterielId);
        }

        public async Task<Materiel> UpdateMateriel(Materiel materiel)
        {
            var foundMateriel = await _appDbContext.Materiels.FirstOrDefaultAsync(e => e.MaterielId == materiel.MaterielId);

            if (foundMateriel != null)
            {
                foundMateriel.Code = materiel.Code;
                foundMateriel.Piece = materiel.Piece;
           

                await _appDbContext.SaveChangesAsync();

                return foundMateriel;
            }

            return null;
        }
     
        }
    }

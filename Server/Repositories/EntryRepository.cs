using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;
using Server;
using Shared.Dto;

namespace RemoteAppApi.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        Mapper mapper = MapperConfig.InitializeAutomapper();
        public EntryRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<EntryDto> AddEntry(EntryDto entry)
        {
            var Entry  = mapper.Map<EntryDto, Entry>(entry);
            var addedEntity = await _appDbContext.Entries.AddAsync(Entry);
            await _appDbContext.SaveChangesAsync();
            var EntryDTO = mapper.Map<Entry, EntryDto>(addedEntity.Entity);
            return EntryDTO;
        }

        public async Task DeleteEntry(long entryId)
        {

            var foundEntry = await _appDbContext.Entries.FirstOrDefaultAsync(e => e.EntryId == entryId);
            if (foundEntry == null) return;

            _appDbContext.Entries.Remove(foundEntry);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<EntryDto>> GetAllEntrys()
        {
            var Entries = new List<Entry>();


            Entries = await this._appDbContext.Entries.Include(p => p.Folder).Include(p => p.CreatedByUser).ToListAsync();
            List<EntryDto> EntryDtos = new List<EntryDto>();
            foreach (var entry in Entries)
            {
                EntryDtos.Add(mapper.Map<Entry, EntryDto>(entry));
            }
            return EntryDtos;
        }

        public async Task<EntryDto> GetEntryById(long EntryId)
        {
            var Entry = await _appDbContext.Entries.FirstOrDefaultAsync(c => c.EntryId == EntryId);
            var EntryDTO = mapper.Map<Entry, EntryDto>(Entry);
            return EntryDTO;

        }

        public async Task<EntryDto> UpdateEntry(EntryDto entry)
        {
            var Entry = mapper.Map<EntryDto, Entry>(entry);
            var foundEntry = await _appDbContext.Entries.FirstOrDefaultAsync(e => e.EntryId == Entry.EntryId);

            if (foundEntry != null)
            {
                foundEntry.EntryName = Entry.EntryName;
                foundEntry.Address = Entry.Address;
                foundEntry.EntryType = Entry.EntryType;
                foundEntry.ID = Entry.ID;
                foundEntry.Password = Entry.Password;
                foundEntry.CreatedDate = Entry.CreatedDate;
                foundEntry.CreatedByUserId = Entry.CreatedByUserId;
                foundEntry.URL= Entry.URL;
                foundEntry.FolderId = Entry.FolderId;
                foundEntry.Description= Entry.Description;
                foundEntry.EntryStatus= Entry.EntryStatus;
                foundEntry.Command = Entry.Command;


                await _appDbContext.SaveChangesAsync();
                var EntryDTO = mapper.Map<Entry, EntryDto>(foundEntry);
                return EntryDTO;
               // return ;
            }

            return null;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RemoteApp.Data;
using RemoteApp.Data.Models;
using RemoteAppApi.Repositories.Contracts;

namespace RemoteAppApi.Repositories
{
    public class EntryRepository : IEntryRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public EntryRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Entry> AddEntry(Entry entry)
        {
            var addedEntity = await _appDbContext.Entries.AddAsync(entry);
            await _appDbContext.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task DeleteEntry(long entryId)
        {

            var foundEntry = await _appDbContext.Entries.FirstOrDefaultAsync(e => e.EntryId == entryId);
            if (foundEntry == null) return;

            _appDbContext.Entries.Remove(foundEntry);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Entry>> GetAllEntrys()
        {
            return await this._appDbContext.Entries.ToListAsync();
        }

        public async Task<Entry> GetEntryById(long EntryId)
        {
            return await _appDbContext.Entries.FirstOrDefaultAsync(c => c.EntryId== EntryId);
        }

        public async Task<Entry> UpdateEntry(Entry entry)
        {
            var foundEntry = await _appDbContext.Entries.FirstOrDefaultAsync(e => e.EntryId == entry.EntryId);

            if (foundEntry != null)
            {
                foundEntry.EntryName = entry.EntryName;
                foundEntry.Address = entry.Address;
                foundEntry.EntryType = entry.EntryType;
                foundEntry.ID = entry.ID;
                foundEntry.Password = entry.Password;
                foundEntry.CreatedDate = entry.CreatedDate;
                foundEntry.CreatedByUserId = entry.CreatedByUserId;
                foundEntry.URL= entry.URL;
                foundEntry.FolderId = entry.FolderId;
                foundEntry.Description= entry.Description;
                foundEntry.EntryStatus= entry.EntryStatus;
                foundEntry.Command = entry.Command;


                await _appDbContext.SaveChangesAsync();

                return foundEntry;
            }

            return null;
        }
    }
}

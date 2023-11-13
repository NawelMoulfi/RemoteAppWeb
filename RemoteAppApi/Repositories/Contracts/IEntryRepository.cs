using RemoteApp.Data.Models;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IEntryRepository
    {
        Task<IEnumerable<Entry>> GetAllEntrys();
        Task<Entry> GetEntryById(long EntryId);
        Task<Entry> AddEntry(Entry entry);
        // Task<Event> AddEvent(Event event);
        Task<Entry> UpdateEntry(Entry entry);
        Task DeleteEntry(long entryId);
    }
}

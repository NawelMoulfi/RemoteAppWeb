using RemoteApp.Data.Models;
using Shared.Dto;

namespace RemoteAppApi.Repositories.Contracts
{
    public interface IEntryRepository
    {
        Task<IEnumerable<EntryDto>> GetAllEntrys();
        Task<EntryDto> GetEntryById(long EntryId);
        Task<EntryDto> AddEntry(EntryDto entry);
        // Task<Event> AddEvent(Event event);
        Task<EntryDto> UpdateEntry(EntryDto entry);
        Task DeleteEntry(long entryId);
    }
}

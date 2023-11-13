using RemoteApp.Data.Models;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IEntryDataService
    {
        Task<IEnumerable<Entry>> GetAllEntries();
        // Task<Client> GetEmployeeDetails(int employeeId);
        Task<Entry> AddEntry(Entry entry);
        Task UpdateEntry(Entry entry);
        Task DeleteEntry(long entryId);
        Task<Entry> GetEntry(long EntryId);
    }
}

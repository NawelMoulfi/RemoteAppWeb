

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IEntryDataService
    {
        Task<IEnumerable<EntryDto>> GetAllEntries();
        // Task<Client> GetEmployeeDetails(int employeeId);
        Task<EntryDto> AddEntry(EntryDto entry);
        Task UpdateEntry(EntryDto entry);
        Task DeleteEntry(long entryId);
        Task<EntryDto> GetEntry(long EntryId);
    }
}

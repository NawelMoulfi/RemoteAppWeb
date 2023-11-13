using RemoteApp.Data.Models;

namespace RemoteApp.Services.Contracts
{
    public interface IMaterielDataService
    {
        Task<IEnumerable<Materiel>> GetAllMateriels();
    }
}

using RemoteApp.Data.Models;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IMaterielDataService
    {
        Task<IEnumerable<Materiel>> GetAllMateriels();
    }
}

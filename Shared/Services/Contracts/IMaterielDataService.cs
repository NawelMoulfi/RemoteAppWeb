

using Shared.Dto;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IMaterielDataService
    {
        Task<IEnumerable<MaterielDto>> GetAllMateriels();
    }
}

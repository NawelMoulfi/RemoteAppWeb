﻿using RemoteApp.Data.Models;

namespace RemoteAppWeb.Services.Contracts
{
    public interface IRapportDataService
    {
        Task<IEnumerable<RapportIntervention>> GetAllRepports() ;
        Task AddRapport(RapportIntervention rapportIntervention);
        Task UpdateRapport(RapportIntervention rapportIntervention);
        Task DeleteRapport(RapportIntervention rapportIntervention);
    }
}

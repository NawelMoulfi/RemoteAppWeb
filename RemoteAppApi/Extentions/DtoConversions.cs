using RemoteApp.Data.Models;
using RemoteAppDtos;

namespace RemoteAppApi.Extentions
{
    public  static class DtoConversions
    {
        public static IEnumerable<ModuleDto> ToDto(IEnumerable<Module> modules, IEnumerable<ModuleAction> moduleActions)
        {
            var moduleDtos = from module in modules
                             select new ModuleDto
                             {
                                 // Map Module properties
                                 ModuleID = module.ModuleID,
                                 ModuleNom = module.ModuleNom,
                                 ModuleGroup = module.ModuleGroup,
                                 // ...

                                 // Map related ModuleActions
                                 ModuleActions = moduleActions
                                     .Where(ma => ma.ModuleID == module.ModuleID)
                                     .Select(ma => new ModuleActionDto
                                     {
                                         ModuleActionID = ma.ModuleActionID,
                                         ModuleID = ma.ModuleID,
                                         ModuleNom = module.ModuleNom,
                                         ModuleGroup = module.ModuleGroup,
                                         Resource = (int)ma.Resource,
                                         Action = (int)ma.Action,
                                         ModuleActionNom = ma.ModuleActionNom
                                     })
                                     .ToList()
                             };

            return moduleDtos;
        }


    }
}

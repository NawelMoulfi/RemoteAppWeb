using AutoMapper;
using RemoteApp.Data.Models;
using Shared.Dto;
namespace Server
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
            //Provide all the Mapping Configuration
            var config = new MapperConfiguration(cfg =>
            {
                
                cfg.CreateMap<ModuleAction, ModuleActionDto>();
                cfg.CreateMap<ModuleActionDto, ModuleAction>();
                cfg.CreateMap<Module, ModuleDto>();
                cfg.CreateMap<ModuleDto, Module>();
                cfg.CreateMap<Role, RoleDto>();
                cfg.CreateMap<RoleDto, Role>();
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<Folder, FolderDto>();
                cfg.CreateMap<FolderDto, Folder>();
                cfg.CreateMap<Entry, EntryDto>();
                cfg.CreateMap<EntryDto, Entry>();
                cfg.CreateMap<Client, ClientDto>();
                 cfg.CreateMap<ClientDto, Client>();
                cfg.CreateMap<RapportIntervention, RapportInterventionDto>();
                cfg.CreateMap<RapportInterventionDto, RapportIntervention>();
                cfg.CreateMap<MaterielRapport, MaterielRapportDto>();
                cfg.CreateMap<MaterielRapportDto, MaterielRapport>();
                cfg.CreateMap<ModuleActionRole, ModuleActionRoleDto>();
                cfg.CreateMap<ModuleActionRoleDto, ModuleActionRole>();
                //Any Other Mapping Configuration ....
            });
            //Create an Instance of Mapper and return that Instance
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}

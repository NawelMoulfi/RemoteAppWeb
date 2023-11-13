using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Action = Shared.Dto.Action;
using Shared.Dto;



namespace RemoteAppWeb.Services
{
    public class CheckPermissionService
    {
        //private static IHttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        public static bool CheckAutorisation(Action action, Resource resource)
        {
            var isSuperUser = true;
         
           bool hasPermission = true;
            return hasPermission;

          
        }

        internal static bool CheckAutorisation(object utilisateurs, Resource parametrage)
        {
            throw new System.NotImplementedException();
        }



        public static void CheckModulesActionsConfiguration(Action action, Resource resource)
        {
            //if (module == "AllResource")
           /* //    return;
            var db = new ApplicationDbContext();
            //var edited = false;
            //var ml = db.Modules.FirstOrDefault(m => m.ModuleNom == module)
            //?? new Module { ModuleNom = module, ModuleActions = new List<ModuleAction>() };
            //if (ml.ModuleID == 0)
            //{
            //    db.Modules.Add(ml);
            //    db.SaveChanges();
            //    edited = true;
            //}
            var ma = db.ModuleActions.Where(a => a.Resource == resource && a.Action == action).FirstOrDefault();
            if (ma != null)
                return;
            ma = db.ModuleActions.Add(new ModuleAction { Resource = resource, Action = action }).Entity;
            db.SaveChanges();
            var ignoreIf = false;
            /* ma.Module.ModuleNom == Modules.Dashboard
            || ma.Module.ModuleNom == Modules.Demandes_Labo && ma.ModuleActionNom == ModuleActions.DateDemandeUltérieur
            || ma.Module.ModuleNom == Modules.ValidationBio && ma.ModuleActionNom == ModuleActions.EditValidatedResult
            || ma.Module.ModuleNom == Modules.Patient && ma.ModuleActionNom == ModuleActions.EditAfterBioValidation
            || ma.Module.ModuleNom == Modules.Analyse && ma.ModuleActionNom == ModuleActions.changeStateDate
            ;*/
           /* if (!ignoreIf)
            {
                var moduleActionRoles = db.Roles.ToList()
                .Select(r => new ModuleActionRole { RoleId = r.RoleId, ModuleActionId = ma.ModuleActionID }).ToList();
                db.ModuleActionRoles.AddRange(moduleActionRoles);
                db.SaveChanges();
            }*/
            // LoadModule_Actions();

        }


  
    }
}

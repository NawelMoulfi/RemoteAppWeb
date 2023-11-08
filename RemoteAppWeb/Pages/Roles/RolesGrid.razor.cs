using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RemoteApp.Data.Models;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

using Action = RemoteApp.Data.Models.Action;
using RemoteAppWeb.Services;
using RemoteAppWeb.Helpers;
using DevExpress.Blazor;
using RemoteAppWeb.Services.Contracts;
using RemoteAppAp.Data;



namespace RemoteAppWeb.Pages.Roles
{
    public partial class RolesGrid
    {

        [Inject] private CheckPermissionService CheckPermissionService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IRoleDataService RoleService { get; set; }
        [Inject] private IModuleActionDataService ModuleActionService { get; set; }
        [Inject] private IModuleActionRoleDataService ModuleActionRoleDataService { get; set; }

        private List<Resource> ResourceList { get; set; }
        private IEnumerable<Role> ListRoles { get; set; }
       
        private Role Role = new Role();
        private bool PopupVisible { get; set; }
        private bool IsTotalControlChecked { get; set; }
        private int Total { get; set; }
        private Dictionary<Resource, bool> IsResourceCheckedDict = new Dictionary<Resource, bool>();
        private Dictionary<Resource, Dictionary<Action, bool>> IsActionCheckedDict = new Dictionary<Resource, Dictionary<Action, bool>>();

        List<Action> actions = new List<Action>();
        private DxDataGrid<Role> grid;
        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }

        protected override void OnInitialized()
        {

            CanRead = true;// CheckPermissionService.CheckAutorisation(Action.Lecture, Resource.Folders);
            CanCreate = true;//CheckPermissionService.CheckAutorisation(Action.Creation, Resource.Folders);
            CanUpdate = true;//CheckPermissionService.CheckAutorisation(Action.Modification, Resource.Folders);
            CanDelete = true; //CheckPermissionService.CheckAutorisation(Action.Suppression, Resource.Folders);

            if (CanRead)
            {
                RefreshGrid();
                //ResourceList = Enum.GetValues(typeof(Resource)).Cast<Resource>().ToList();
                ResourceList = GetModuleEnumList();
                actions = GetActionEnumList();
            }
            else
            {
                JSRuntime.InvokeVoidAsync("open", "/NoAccess/", "_self");
            }
        }

        public List<Resource> GetModuleEnumList()
        {
            var list = new List<Resource>();

            foreach (Resource e in Enum.GetValues(typeof(Resource)))
            {

                list.Add(e);
            }
            return list;
        }

        public List<Action> GetActionEnumList()
        {
            var list = new List<Action>();

            foreach (Action e in Enum.GetValues(typeof(Action)))
            {

                list.Add(e);
            }
            return list;
        }

        private async Task RefreshGrid()
        {
            ListRoles = await RoleService.GetRolesList();
        }

        private async Task OnRowRemoving(Role x)
        {
           // var response = await JSRuntime.InvokeAsync<bool>("confirmDelete", "Êtes-vous sûr de vouloir supprimer cet élément?");
          // if (response)
                try
                {
                    Console.WriteLine($"The Role Id that should be removed  : {x.RoleId}");
                    await RoleService.DeleteRole(x.RoleId);
                    ListRoles = ListRoles.Where(v => v != x).ToArray();
                    OnInitialized();
                    StateHasChanged();
                   
                }
                catch (Exception e)
                {

                    await JSRuntime.InvokeVoidAsync("alert", "Vous ne pouvez pas supprimer Cet élémént, car il possède des references");

                }
        }

        private void OnRowEditStarting(Role x)
        {
            PopupVisible = true;
            Role = x;
            InitCheckboxTable();
        }

        private void OnRowNewStarting()
        {
            Role = new Role();
            PopupVisible = true;
            InitCheckboxTable();
        }

        private async Task HandleValidSubmit(EditContext editContext)
        {
            var p = (Role)editContext.Model;
            try
            {
                if (p.RoleId == 0)
                {
                   await RoleService.AddRole(p);
                    ListRoles = new[] { p }.Concat(ListRoles);
                }
                else
                {
                   await RoleService.UpdateRole(p);
                }
                await grid.CancelRowEdit();
                OnInitialized();
                StateHasChanged();
            }
            catch (Exception ex)
            {

                await JSRuntime.InvokeVoidAsync("alert", "Le nom existe déjà");

            }
            SaveChanges(IsActionCheckedDict);
            await InvokeAsync(StateHasChanged);
        }

        private async Task InitCheckboxTable()
        {
            Total = 0;
            int moduleActionListCount = 0;
            foreach (var resource in ResourceList)
            {
                var moduleActionList = await  ModuleActionService.GetListModuleActionsByResource(resource); //GetResourceActionList(resource);
                Total += moduleActionList.Count();
                IsResourceCheckedDict[resource] = moduleActionList.All(moduleAction => Role.ModuleActions?.Contains(moduleAction) ?? false);//True if the selected "Role" has access to all the "Actions" of the specified "Resource" 
                foreach (var moduleAction in moduleActionList)
                {
                    if (!IsActionCheckedDict.ContainsKey(resource))
                    {
                        IsActionCheckedDict[resource] = new Dictionary<Action, bool>();
                    }
                    IsActionCheckedDict[resource][moduleAction.Action] = Role.ModuleActions?.Contains(moduleAction) ?? false;
                }
                moduleActionListCount = moduleActionList.Count();
                if (moduleActionListCount == 0)
                {
                    IsActionCheckedDict[resource] = new Dictionary<Action, bool>();
                }
            }
            IsTotalControlChecked = Role.ModuleActions?.Count() == Total;
        }

        private void IsActionCheckboxChanged(Action action, Resource resource, bool value)
        {
            IsActionCheckedDict[resource][action] = value;
            if (IsActionCheckedDict[resource].Values.Distinct().Count() == 1)
            {
                IsResourceCheckedDict[resource] = value;
            }
            else
            {
                IsResourceCheckedDict[resource] = false;
            }
            // IsTotalControlChecked = IsResourceCheckedDict.Values.Distinct().Count() == 1 && IsResourceCheckedDict[resource];
        }

        private void IsResourceCheckboxChanged(Resource resource, bool value)
        {
            IsResourceCheckedDict[resource] = value;
            foreach (var key in IsActionCheckedDict[resource].Keys.ToList())
            {
                IsActionCheckedDict[resource][key] = value;
            }
            IsTotalControlChecked = IsResourceCheckedDict.Values.Distinct().Count() == 1 && value;
        }

        private void IsTotalControlCheckboxChanged(bool value)
        {
            IsTotalControlChecked = value;
            foreach (var keyResource in IsResourceCheckedDict.Keys.ToList())
            {
                IsResourceCheckedDict[keyResource] = value;
                foreach (var keyAction in IsActionCheckedDict[keyResource].Keys.ToList())
                {
                    IsActionCheckedDict[keyResource][keyAction] = value;
                }
            }
        }

        private async Task SaveChanges(Dictionary<Resource, Dictionary<Action, bool>> isActionCheckedDict)
        {
            ResourceActionParameters parameters = new ResourceActionParameters();
           
            var role = RoleService.GetRole(Role.RoleId);//ModuleService.FindRole(Role.RoleId);
            foreach (var keyResource in isActionCheckedDict.Keys.ToList())
            {
                foreach (var keyAction in isActionCheckedDict[keyResource].Keys.ToList())
                {
                    parameters.Action = keyAction;
                    parameters.Resource = keyResource;
                    var resourceActionList = await ModuleActionService.GetResourceAction(parameters)  ;//ModuleService.GetResourceAction(keyResource, keyAction);
                    var resourceAction = resourceActionList.ElementAt(1); 
                    if (resourceAction == null)
                    {
                        var moduleAction = new ModuleAction();
                        moduleAction.Resource = keyResource;
                        moduleAction.Action = keyAction;
                        resourceAction =  await ModuleActionService.AddModuleAction(moduleAction) ;//AddModuleAction(moduleAction);
                    }
                    var moduleActionInRole = Role.ModuleActions?.Find(moduleAction => moduleAction == resourceAction);
                    var isChecked = IsActionCheckedDict[keyResource][keyAction];
                    if (isChecked && moduleActionInRole == null)
                    {
                        var newResourceAction = new ModuleActionRole { RoleId = role.Id, ModuleActionId = resourceAction.ModuleActionID };
                        //  role.ModuleActionRoles.Add(newResourceAction);
                        ModuleActionRoleDataService.AddModuleActionRole(newResourceAction);
                    }
                    else if (!isChecked && moduleActionInRole != null)
                    {
                        var resourceActionToDelete = await ModuleActionRoleDataService.GetModuleActionRoleByRoleAndModuleActionId(role.Id, resourceAction.ModuleActionID);
                        // resourceActionToDelete = role.ModuleActionRoles.FirstOrDefault(e => e.RoleId == role.RoleId && e.ModuleActionId == resourceAction.ModuleActionID);
                        ModuleActionRoleDataService.DeleteModuleActionRole(resourceActionToDelete.ModuleActionRoleId);
                       // role.ModuleActionRoles.Remove(resourceActionToDelete);
                    }

                }
            }
            //ModuleService.SaveAllChanges();
        }

        private async Task ClosePopup()
        {
            PopupVisible = false;
            await grid.CancelRowEdit();
        }

        private String GetEnumDisplay(Resource resource)
        {
            return resource.GetAttribute<DisplayAttribute>()?.Name ?? resource.ToString();
        }

        private async void ResetPermissions()
        {
            //var result = await CheckPermissionService.ResetPermissions();
           // if (result) JSRuntime.InvokeVoidAsync("alert", "Les permissions ont été réinitialisé.");
           // else JSRuntime.InvokeVoidAsync("alert", "Les permissions n'ont pas été réinitialisé.");
        }
    }
}

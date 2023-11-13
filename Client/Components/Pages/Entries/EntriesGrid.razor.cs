using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RemoteAppWeb.Helpers;


using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RemoteAppWeb.Services;
using Microsoft.JSInterop;
using Action = Shared.Dto.Action;
using Microsoft.AspNetCore.Http;
using Shared.Dto;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using DevExpress.Blazor;
using RemoteAppWeb.Services.Contracts;


namespace RemoteApp.Pages.Entries
{
    public partial class EntriesGrid
    {

        [Inject]
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        [Inject]
        private IFolderDataService FolderService { get; set; }

        [Inject]
        private IUserDataService UserService { get; set; }

        [Inject]
        private IEntryDataService EntryService { get; set; }

      

        public List<EntryDto> Entrieslist { get; set; }
        public List<FolderDto> Folderslist { get; set; }
        public List<UserDto> UsersList { get; set; }
       
        public long entryId { get; set; } = 0;
        private bool PopupVisible { get; set; }

        public EntryDto entry { get; set; }


        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        List<KeyValuePair<string, int>> EntryTypes { get; set; }
        List<KeyValuePair<string, int>> EntryStatuses { get; set; }
        /*protected async override void OnInitialized()
        {
        
          
        }*/

        public List<KeyValuePair<string, int>> GetEnumListEntryType<T>()
        {
            var list = new List<KeyValuePair<string, int>>();
            string display = null;
            foreach (EntryType e in Enum.GetValues(typeof(T)))
            {
                display = e.GetType()
                 .GetMember(e.ToString())
                 .FirstOrDefault()?
                 .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString();
                list.Add(new KeyValuePair<string, int>(display, (int)e));
            }
            return list;
        }

        public List<KeyValuePair<string, int>> GetEnumListEntryStatus<T>()
        {
            var list = new List<KeyValuePair<string, int>>();
            string display = null;
            foreach (EntryStatus e in Enum.GetValues(typeof(T)))
            {
                display = e.GetType()
                 .GetMember(e.ToString())
                 .FirstOrDefault()?
                 .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString();
                list.Add(new KeyValuePair<string, int>(display, (int)e));
            }
            return list;
        }

        public async Task RefreshGrid()
        {
            CanRead = true;// CheckPermissionService.CheckAutorisation(Action.Lecture, Resource.Folders);
            CanCreate = true;//CheckPermissionService.CheckAutorisation(Action.Creation, Resource.Folders);
            CanUpdate = true;//CheckPermissionService.CheckAutorisation(Action.Modification, Resource.Folders);
            CanDelete = true; //CheckPermissionService.CheckAutorisation(Action.Suppression, Resource.Folders);

            if (CanRead)
            {
                Entrieslist = (List<EntryDto>)await EntryService.GetAllEntries();
                Console.WriteLine($"Entry List : {Entrieslist}");
                Folderslist = (List<FolderDto>)await FolderService.GetAllFolders();
                UsersList = (List<UserDto>)await UserService.GetAllUsers();

                EntryTypes = GetEnumListEntryType<EntryType>();
                EntryStatuses = GetEnumListEntryStatus<EntryStatus>();
                //base.OnInitialized();
            }
        }
        void OnRowUpdating(EntryDto x, Dictionary<string, object> newValue)
        {
            EntryService.UpdateEntry(x);
            RefreshGrid();
        }
       

        void OnRowRemoving(EntryDto x)
        {
            
            
                EntryService.DeleteEntry(x.EntryId);
                RefreshGrid();

            
          
        }
        void OnRowInserting(Dictionary<string, object> newValue)
        {
            var x = new EntryDto();

            EntryService.UpdateEntry(x);
            RefreshGrid();
        }
        public void ClosePopup()
        {
            RefreshGrid();
            PopupVisible = false;
        }


        void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            var model = e.DataItem as EntryDto;
            if (model == null)
            {
                model = new EntryDto();

            } 
            
            e.EditModel = model;
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
           var entry = (EntryDto)e.EditModel;
            if (e.IsNew)
            {
            
                EntryService.AddEntry(entry);
            }
             
            else
                EntryService.UpdateEntry(entry);

            RefreshGrid();

        }
        async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            var entry = (EntryDto)e.DataItem;
            
            
                EntryService.DeleteEntry(entry.EntryId);
                RefreshGrid();

        }

        public async Task OpenCommand(GridDataColumnCellDisplayTemplateContext context)
        {
          var _entry  =  await EntryService.GetEntry((long)context.Value);

            //object value = await JSRuntime.InvokeAsync<string>("OpenConnection", _entry.EntryType, _entry.ID, _entry.Password);
        }

    }
}

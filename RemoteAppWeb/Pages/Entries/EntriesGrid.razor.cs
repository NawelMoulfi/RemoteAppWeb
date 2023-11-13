using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RemoteAppWeb.Helpers;

using RemoteApp.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RemoteApp.Services;
using Microsoft.JSInterop;
using Action = RemoteApp.Data.Models.Action;
using Microsoft.AspNetCore.Http;
using RemoteApp.Data;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using DevExpress.Blazor;
using RemoteAppWeb.Services.Contracts;


namespace RemoteAppWeb.Pages.Entries
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

      

        public List<Entry> Entrieslist { get; set; }
        public List<Folder> Folderslist { get; set; }
        public List<User> UsersList { get; set; }
       
        public long entryId { get; set; } = 0;
        private bool PopupVisible { get; set; }

        public Entry entry { get; set; }


        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        List<KeyValuePair<string, int>> EntryTypes { get; set; }
        List<KeyValuePair<string, int>> EntryStatuses { get; set; }
        protected async override void OnInitialized()
        {
            RefreshGrid();
        }

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
                Entrieslist = (List<Entry>)await EntryService.GetAllEntries();
                Console.WriteLine($"Entry List : {Entrieslist}");
                Folderslist = (List<Folder>)await FolderService.GetAllFolders();
                UsersList = (List<User>)await UserService.GetAllUsers();

                EntryTypes = GetEnumListEntryType<EntryType>();
                EntryStatuses = GetEnumListEntryStatus<EntryStatus>();
                //base.OnInitialized();
            }
            else
            {
                JSRuntime.InvokeAsync<object>("open", "/NoAccess/", "_self");
            }
        }
        void OnRowUpdating(Entry x, Dictionary<string, object> newValue)
        {
            EntryService.UpdateEntry(x);
            RefreshGrid();
            StateHasChanged();
        }
       

        void OnRowRemoving(Entry x)
        {
            try
            {
                EntryService.DeleteEntry(x.EntryId);
                RefreshGrid();
                StateHasChanged();

            }
            catch (Exception e)
            {

                RefreshGrid();
                JSRuntime.InvokeAsync<object>("alert", "vous ne pouvez pas supprimer cet element !");
                // Console.WriteLine(e);
                // throw;
            }
        }
        void OnRowInserting(Dictionary<string, object> newValue)
        {
            var x = new Entry();

            EntryService.UpdateEntry(x);
            RefreshGrid();
            StateHasChanged();
        }
        public void ClosePopup()
        {
            RefreshGrid();
            PopupVisible = false;
        }


        void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            var model = e.DataItem as Entry;
            if (model == null)
            {
                model = new Entry();

            } 
            
            e.EditModel = model;
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
           var entry = (Entry)e.EditModel;
            if (e.IsNew)
            {
            
                EntryService.AddEntry(entry);
            }
             
            else
                EntryService.UpdateEntry(entry);

            RefreshGrid();
            StateHasChanged();
            
        }
        async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            var entry = (Entry)e.DataItem;
            try
            {
                EntryService.DeleteEntry(entry.EntryId);
                RefreshGrid();
                StateHasChanged();

            }
            catch (Exception ex)
            {

                RefreshGrid();
                JSRuntime.InvokeAsync<object>("alert", "vous ne pouvez pas supprimer cet element !");
                // Console.WriteLine(e);
                // throw;
            }
        }

        public async Task OpenCommand(GridDataColumnCellDisplayTemplateContext context)
        {
          var _entry  =  await EntryService.GetEntry((long)context.Value);

            await JSRuntime.InvokeAsync<string>("OpenConnection", _entry.EntryType, _entry.ID, _entry.Password);
        }

    }
}

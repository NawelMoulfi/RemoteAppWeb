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
using RemoteAppWeb.Services.Contracts;


namespace RemoteAppWeb.Pages.Folders
{
    public partial class FoldersGrid
    {
        [Inject]
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        

        [Inject]
        private IFolderDataService FolderService { get; set; }

  

        public List<Folder> Folderslist { get; set; }
        public List<Folder> ParentFolderslist { get; set; }
        public List<Folder> EditParentFolderslist { get; set; }
        public int folderId { get; set; } = 0;
        private bool PopupVisible { get; set; }

        public Folder folder { get; set; }


        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        List<KeyValuePair<string, int>> FolderStatuses { get; set; }
        protected async override void OnInitialized()
        {
            RefreshGrid();
        }

        public List<KeyValuePair<string, int>> GetEnumList<T>()
        {
            var list = new List<KeyValuePair<string, int>>();
            string display = null;
            foreach (FolderStatus e in Enum.GetValues(typeof(T)))
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
                Folderslist = (List<Folder>)await FolderService.GetAllFolders();
                ParentFolderslist = (List<Folder>)await FolderService.GetAllFolders();
                FolderStatuses = GetEnumList<FolderStatus>();
                //base.OnInitialized();
            }
            else
            {
                JSRuntime.InvokeAsync<object>("open", "/NoAccess/", "_self");
            }
        }
        async Task OnRowUpdating(Folder folder, Dictionary<string, object> newValue)
        {


            SetNewValue(folder, newValue);

            await FolderService.UpdateFolder(folder);
            RefreshGrid();
            StateHasChanged();
        }
        async Task OnRowEditStarting(Folder x)
        {
            // First, await the asynchronous GetAllFolders method
            var allFolders = await FolderService.GetAllFolders();

            // Then, perform the Except operation
            EditParentFolderslist = allFolders.Except(new List<Folder> { x }).ToList();


        }
        async Task OnRowNewStarting()
        {
            EditParentFolderslist = (List<Folder>)await FolderService.GetAllFolders();
        }

        void OnRowRemoving(Folder x)
        {
            try
            {
                FolderService.DeleteFolder(x.FolderId);
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
        async Task OnRowInserting(Dictionary<string, object> newValue)
        {
            var folder = new Folder();

          SetNewValue(folder, newValue);

            Console.WriteLine($"The new folder : {folder}");
            Console.WriteLine($"The new folder Id: {folder.FolderId}");
            Console.WriteLine($"The new folder Name: {folder.FolderName}");
            Console.WriteLine($"The new folder Description: {folder.FolderDescription}");
            Console.WriteLine($"The new folder Parent folder Id : {folder.ParentFolderId}");
            await FolderService.AddFolder(folder);
            RefreshGrid();
            StateHasChanged();
        }
        public void SetNewValue(Folder folder, Dictionary<string, object> newValue)
        {
            foreach (var field in newValue.Keys)
            {
                switch (field)
                {
                    case "FolderStatusInt":
                        folder.FolderStatus = (FolderStatus)newValue[field];
                        break;
                    case "ParentFolderId":
                        folder.ParentFolderId = (int?)newValue[field];
                        break;

                    case "FolderDescription":
                        folder.FolderDescription = (string)newValue[field];
                        break;

                    case "FolderName":
                        folder.FolderName = (string)newValue[field];
                        break;

                }
            }


        }
        public void ClosePopup()
        {
            RefreshGrid();
            PopupVisible = false;
        }
      

    }
}

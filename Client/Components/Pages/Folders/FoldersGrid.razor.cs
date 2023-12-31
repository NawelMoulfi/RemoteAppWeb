﻿using System;
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

using System.Reflection;
using System.ComponentModel.DataAnnotations;
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Forms;


namespace RemoteApp.Pages.Folders
{
    public partial class FoldersGrid
    {
        [Inject]
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        

        [Inject]
        private IFolderDataService FolderService { get; set; }

    

        public List<FolderDto> Folderslist { get; set; }
        public List<FolderDto> ParentFolderslist { get; set; }
        public List<FolderDto> EditParentFolderslist { get; set; }
        public int folderId { get; set; } = 0;
        private bool PopupVisible { get; set; }

        public FolderDto folder { get; set; }  


        public bool CanRead { get; set; }
        public bool CanCreate { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        List<KeyValuePair<string, int>> FolderStatuses { get; set; }
        private EditContext editContext;
        protected async override Task OnInitializedAsync()
        {
           
            await RefreshGrid();
            StateHasChanged(); // This might not be needed, see if removing it resolves the issue

            await base.OnInitializedAsync();
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



            Folderslist = (List<FolderDto>)await FolderService.GetAllFolders();
            ParentFolderslist = (List<FolderDto>)await FolderService.GetAllFolders();
            FolderStatuses = GetEnumList<FolderStatus>();
            StateHasChanged();
            //base.OnInitialized();
        }
       


        void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e)
        {
            var model = e.DataItem as FolderDto;
            if (model == null)
            {
                model = new FolderDto();

            }

            e.EditModel = model;
        }

        async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e)
        {
            var folder = (FolderDto)e.EditModel;
            if (e.IsNew)
            {
               
                await FolderService.AddFolder(folder);
            }

            else
                await FolderService.UpdateFolder(folder);


            await RefreshGrid();
            StateHasChanged();

        }
        async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e)
        {
            var folder = (FolderDto)e.DataItem;


            await FolderService.DeleteFolder(folder.FolderId);

            await RefreshGrid();
            StateHasChanged();

        }

          async Task OnRowUpdating(FolderDto folder, Dictionary<string, object> newValue)
          {


              SetNewValue(folder, newValue);

              await FolderService.UpdateFolder(folder);
              await RefreshGrid();
              StateHasChanged();
          }
          async Task OnRowEditStarting(FolderDto x)
          {
              // First, await the asynchronous GetAllFolders method
              var allFolders = await FolderService.GetAllFolders();

              // Then, perform the Except operation
              EditParentFolderslist = allFolders.Except(new List<FolderDto> { x }).ToList();


          }
          async Task OnRowNewStarting()
          {
              EditParentFolderslist = (List<FolderDto>)await FolderService.GetAllFolders();
          }

          async Task OnRowRemoving(FolderDto x)
          {

              await FolderService.DeleteFolder(x.FolderId);
              await RefreshGrid();
              StateHasChanged();



          }
          async Task OnRowInserting(Dictionary<string, object> newValue)
          {
              var folder = new FolderDto();

            SetNewValue(folder, newValue);

              Console.WriteLine($"The new folder : {folder}");
              Console.WriteLine($"The new folder Id: {folder.FolderId}");
              Console.WriteLine($"The new folder Name: {folder.FolderName}");
              Console.WriteLine($"The new folder Description: {folder.FolderDescription}");
              Console.WriteLine($"The new folder Parent folder Id : {folder.ParentFolderId}");
              await FolderService.AddFolder(folder);
              await RefreshGrid();
              StateHasChanged();

          }
          public void SetNewValue(FolderDto folder, Dictionary<string, object> newValue)
          {
              foreach (var field in newValue.Keys)
              {
                  switch (field)
                  {
                      case "FolderStatusInt":
                          folder.FolderStatus = (FolderStatus)newValue[field];
                          break;
                      case "ParentFolderId":
                          folder.ParentFolderId = (int)newValue[field];
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
        public async Task ClosePopup()
        {
            await RefreshGrid();

            PopupVisible = false;
        }
      

    }
}

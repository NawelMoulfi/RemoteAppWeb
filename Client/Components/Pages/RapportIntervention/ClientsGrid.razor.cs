using DevExpress.Blazor;
using DevExpress.Blazor.Base;
using Microsoft.AspNetCore.Components;

using RemoteAppWeb.Services;
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;

namespace RemoteApp.Pages.RapportIntervention;

public partial class ClientsGrid 
{
    [Inject]
    private IRapportDataService RapportService { get; set; }
    [Inject]
    private IClientDataService ClientService { get; set; }
    [Inject]
    private IMaterielDataService MaterielService { get; set; }
    public IEnumerable<ClientDto> Clients { set; get; }
    IGrid Grid;

   /* protected async override void OnInitialized()
    {
       // RefreshGrid();
    }*/
    void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e) {
        var client = e.DataItem as ClientDto;
        if (client == null)
        {
            client = new ClientDto();

        }
        e.EditModel = client;
    }
    async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e) {
        var client = (ClientDto)e.EditModel;
        if(e.IsNew)
        {
            ClientService.AddClient(client);
            RefreshGrid();
        }
        else
        {
            ClientService.UpdateClient(client);
            RefreshGrid();
        }
    }
    public async Task RefreshGrid()
    {

        Clients = await ClientService.GetAllClients();
    }
    async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e) {
        var client = (ClientDto)e.DataItem;
       await  ClientService.DeleteClient(client.ClientId);
        RefreshGrid();
      //  OnInitialized();
    }
}
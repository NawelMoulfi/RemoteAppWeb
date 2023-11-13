using DevExpress.Blazor;
using DevExpress.Blazor.Base;
using Microsoft.AspNetCore.Components;
using RemoteApp.Data.Models;
using RemoteApp.Services;
using RemoteApp.Services.Contracts;

namespace RemoteApp.Pages.RapportIntervention;

public partial class ClientsGrid 
{
    [Inject]
    private IRapportDataService RapportService { get; set; }
    [Inject]
    private IClientDataService ClientService { get; set; }
    [Inject]
    private IMaterielDataService MaterielService { get; set; }
    public IEnumerable<Client> Clients { set; get; }
    IGrid Grid;
    
    protected async override void OnInitialized()
    {
        RefreshGrid();
    }
    void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e) {
        var client = e.DataItem as Client;
        if (client == null)
        {
            client = new Client();

        }
        e.EditModel = client;
    }
    async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e) {
        var client = (Client)e.EditModel;
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
        var client = (Client)e.DataItem;
       await  ClientService.DeleteClient(client.ClientId);
        OnInitialized();
    }
}
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using RemoteAppWeb.Services;
using RemoteAppWeb.Services.Contracts;
using Shared.Dto;
namespace RemoteApp.Pages.RapportIntervention;
public partial class RapportGrid : ComponentBase
{
    [Inject]
    private IRapportDataService RapportService { get; set; }
    [Inject]
    private IUserDataService UserService { get; set; }
    [Inject]
    private IMaterielDataService MaterielService { get; set; }
    [Inject]
    private IMaterielRapportDataService MaterielRapportService { get; set; }
    public List<RapportInterventionDto> RapportInterventions { set; get; }
    public List<MaterielRapportDto> MaterielsRapport = new List<MaterielRapportDto>();
    public List<MaterielDto> Materiels = new List<MaterielDto>();
    IReadOnlyList<object> SelectedDataItems { get; set; }
    public List<UserDto> Users { set; get; }
    IGrid Grid;
    List<KeyValuePair<string, int>> Operations { get; set; }
    List<KeyValuePair<string, int>> Logiciels { get; set; }
    protected async override Task OnInitializedAsync()
    {

        await RefreshRapports();
        StateHasChanged(); // This might not be needed, see if removing it resolves the issue
        await base.OnInitializedAsync();
    }

    public List<KeyValuePair<string, int>> GetEnumListOperations<T>()
    {
        var list = new List<KeyValuePair<string, int>>();
        string display = null;
        foreach (Operation e in Enum.GetValues(typeof(T)))
        {
            display = e.GetType()
                .GetMember(e.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString();
            list.Add(new KeyValuePair<string, int>(display, (int)e));
        }
        return list;
    }
    
    public List<KeyValuePair<string, int>> GetEnumListLogiciels<T>()
    {
        var list = new List<KeyValuePair<string, int>>();
        string display = null;
        foreach (Logiciel e in Enum.GetValues(typeof(T)))
        {
            display = e.GetType()
                .GetMember(e.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString();
            list.Add(new KeyValuePair<string, int>(display, (int)e));
        }
        return list;
    }

    public bool AddMaterielPopup = false;
    public bool MaterielPopup = false;
    public bool UserPopup = false;
    public RapportInterventionDto CurrentRapport = new RapportInterventionDto();
    public MaterielRapportDto MaterielRapport1 = new MaterielRapportDto();
    void CloseAddMaterielPopup()
    {
        AddMaterielPopup = false;
    }
    async Task CloseUserPopup()
    {
        UserPopup = false;
        Users = (List<UserDto>)await UserService.GetAllUsers();
    }
    async Task OpenMaterielPopup(MouseEventArgs args, RapportInterventionDto rapportIntervention)
    {
        AddMaterielPopup = true;
        CurrentRapport = rapportIntervention;
        Console.WriteLine($"The Current Rapport Id  : {CurrentRapport.RapportId}");
        MaterielsRapport = (List<MaterielRapportDto>)await  MaterielRapportService.GetMateriels(CurrentRapport.RapportId);
        Materiels = (List<MaterielDto>)await MaterielService.GetAllMateriels();
    }
    void OpenUserPopup()
    {
        UserPopup = true;
    }
    void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e) {
        var model = e.DataItem as RapportInterventionDto;
        if (model == null)
        {
            model = new RapportInterventionDto();
        }
        e.EditModel = model;
    }
    async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e) {
        var rapport = (RapportInterventionDto)e.EditModel;
        if(e.IsNew)
        {
            await RapportService.AddRapport(rapport);
           
        }
        else
        {
            await RapportService.UpdateRapport(rapport);
          
        }
        await RefreshRapports();
        StateHasChanged();
    }
    async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e) {
        var rapport = e.DataItem as RapportInterventionDto;
        await RapportService.DeleteRapport(rapport);
        await RefreshRapports();
        StateHasChanged();

    }
    
    public async Task RefreshRapports() {
        RapportInterventions = (List<RapportInterventionDto>)await RapportService.GetAllRepports();
        Operations = GetEnumListOperations<Operation>();
        Logiciels = GetEnumListLogiciels<Logiciel>();
        Users = (List<UserDto>)await UserService.GetAllUsers();
        foreach (var user in Users)
        {
            Console.WriteLine($"User Id: {user.UserId}, User Name: {user.UserNom}");
        }

    }
    
    public async Task RefreshMaterielsRapport() {
        MaterielsRapport = (List<MaterielRapportDto>)await MaterielRapportService.GetMateriels(CurrentRapport.RapportId);
       
    }
    public void AddMaterilToRapport()
    {
        MaterielPopup = true;
    }
    private async Task HandleValidSubmit(EditContext editContext)
    {
        var p = (MaterielRapportDto)editContext.Model;
        p.RapportInterventionId = CurrentRapport.RapportId;
        
        await MaterielRapportService.AddMaterielRapport(p);
        await RefreshMaterielsRapport();
        StateHasChanged();
        MaterielPopup = false;
    }
    

}
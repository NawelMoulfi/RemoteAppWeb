using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using RemoteApp.Data.Models;
using RemoteApp.Services;
using RemoteApp.Services.Contracts;
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
    public List<Data.Models.RapportIntervention> RapportInterventions { set; get; }
    public List<MaterielRapport> MaterielsRapport = new List<MaterielRapport>();
    public List<Materiel> Materiels = new List<Materiel>();
    IReadOnlyList<object> SelectedDataItems { get; set; }
    public List<User> Users { set; get; }
    IGrid Grid;
    List<KeyValuePair<string, int>> Operations { get; set; }
    List<KeyValuePair<string, int>> Logiciels { get; set; }
    protected async override void OnInitialized()
    {
        RefreshRapports();
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
    public Data.Models.RapportIntervention CurrentRapport = new Data.Models.RapportIntervention();
    public MaterielRapport MaterielRapport1 = new MaterielRapport();
    void CloseAddMaterielPopup()
    {
        AddMaterielPopup = false;
    }
    async Task CloseUserPopup()
    {
        UserPopup = false;
        Users = (List<User>)await UserService.GetAllUsers();
    }
    async Task OpenMaterielPopup(MouseEventArgs args, Data.Models.RapportIntervention rapportIntervention)
    {
        AddMaterielPopup = true;
        CurrentRapport = rapportIntervention;
        Console.WriteLine($"The Current Rapport Id  : {CurrentRapport.RapportId}");
        MaterielsRapport = (List<MaterielRapport>)await  MaterielRapportService.GetMateriels(CurrentRapport.RapportId);
        Materiels = (List<Materiel>)await MaterielService.GetAllMateriels();
    }
    void OpenUserPopup()
    {
        UserPopup = true;
    }
    void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e) {
        var model = e.DataItem as Data.Models.RapportIntervention;
        if (model == null)
        {
            model = new Data.Models.RapportIntervention();
        }
        e.EditModel = model;
    }
    async Task Grid_EditModelSaving(GridEditModelSavingEventArgs e) {
        var rapport = (Data.Models.RapportIntervention)e.EditModel;
        if(e.IsNew)
        {
            RapportService.AddRapport(rapport);
            RefreshRapports();

        }
        else
        {
            RapportService.UpdateRapport(rapport);
            RefreshRapports();
        }
    }
    async Task Grid_DataItemDeleting(GridDataItemDeletingEventArgs e) {
        var rapport = e.DataItem as Data.Models.RapportIntervention;
        RapportService.DeleteRapport(rapport);
        RefreshRapports();

    }
    
    public async Task RefreshRapports() {
        RapportInterventions = (List<Data.Models.RapportIntervention>)await RapportService.GetAllRepports();
        Operations = GetEnumListOperations<Operation>();
        Logiciels = GetEnumListLogiciels<Logiciel>();
        Users = (List<User>)await UserService.GetAllUsers();
       
    }
    
    public async Task RefreshMaterielsRapport() {
        MaterielsRapport = (List<MaterielRapport>)await MaterielRapportService.GetMateriels(CurrentRapport.RapportId);
       
    }
    public void AddMaterilToRapport()
    {
        MaterielPopup = true;
    }
    private async Task HandleValidSubmit(EditContext editContext)
    {
        var p = (MaterielRapport)editContext.Model;
        p.RapportInterventionId = CurrentRapport.RapportId;
        
        await MaterielRapportService.AddMaterielRapport(p);
          RefreshMaterielsRapport();
        MaterielPopup = false;
    }
    

}
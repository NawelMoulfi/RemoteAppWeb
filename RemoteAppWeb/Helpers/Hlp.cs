#define Lotfi
using RemoteApp.Data;
using RemoteApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;


namespace RemoteAppWeb.Helpers
{
    //#define Hospita
    public static class Hlp
    {
        // private static NLog.Logger _logger = LogManager.GetCurrentClassLogger();
        public const double TarifNuit = 100; //100 DA


        //public static void Init()
        //{
        //    var efm = new ExeConfigurationFileMap {ExeConfigFilename = "Shared/App.config"};
        //    var configuration = ConfigurationManager.OpenMappedExeConfiguration(efm, ConfigurationUserLevel.None);
        //    if (!configuration.HasFile) return;
        //    var a = configuration.AppSettings.Settings["EtabShortName"];
        //    EtabShortName = a.Value;
        //}

        public static bool Access(Type entity, string action)
        {
            //if (entity == typeof (Fourn))
            //return Access(Modules.PharmAdmin, action);
            return true;
        }

        //public static bool Access(string resource, string action)
        //{

        //    //return true;
        //    try
        //    {
        //        using (ClientSecurityContext.CreateSecurityContext())
        //        {
        //            //ClaimsPrincipalPermission.CheckAccess(resource, action);
        //            return true;
        //        }

        //    }
        //    catch (SecurityException)
        //    {
        //        return false;
        //    }
        //}

        //public static bool Access(string resource, string action)
        //{
        //    var m = resource + " = " + action;
        //    try
        //    {
        //        ClaimsPrincipalPermission.CheckAccess(resource, action);
        //        //Logger.Log(m + " +");
        //        return true;
        //    }
        //    catch (SecurityException)
        //    {
        //        //Logger.Log(m + " -");
        //        return false;
        //    }
        //}

        public static string GetSha1(string userId, string type, string password)
        {
            var xx = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(userId + type + password));
            return xx.Aggregate("", (current, item) => current + item);
        }

        //public static void Imp_Click1(string name, string code, string ppa, uint fois)
        //{
        //    var sb = new StringBuilder();
        //    sb.AppendLine();
        //    sb.AppendLine("SIZE 40 mm, 25 mm");
        //    sb.AppendLine("GAP 3 mm,0");
        //    sb.AppendLine("BACKFEED " + Setting(Params.Bf));
        //    var tt = "DIRECTION 1\n" +
        //             "HOME\n" +
        //             "CLS\n" +
        //             "TEXT 10,10,\"2\",0,1,1,\"Code 128, switch co\"\n" +
        //             "BARCODE 10,50,\"128\",100,1,0,2,2,\"123456abcd123456\"\n" +
        //             "PRINT 1";
        //    sb.AppendLine(tt);
        //    RawPrinterHelper.SendStringToPrinter(Setting(Params.Printer), sb.ToString());
        //}

        //public static void Imp_Click(string name, string code, string ppa, uint fois)
        //{
        //    if (Setting(Params.PrintType) == "1")
        //    {
        //        PrintLab.OpenPort(Setting(Params.Printer));
        //        PrintLab.PTK_ClearBuffer();
        //        //PrintLab.PTK_EnableCircumgyrate(40);
        //        PrintLab.PTK_SetPrintSpeed(3);
        //        PrintLab.PTK_SetDarkness(8);
        //        PrintLab.PTK_SetLabelHeight(160, 16);
        //        PrintLab.PTK_SetLabelWidth(320);
        //        PrintLab.PTK_DrawTextTrueTypeW(65, 3, 30, 10, "1", 1, 900, false, false, false, "1", name);
        //        PrintLab.PTK_DrawBarcode(45, 35, 0, "1", 2, 2, 80, 'N', code); //1 maximum
        //        PrintLab.PTK_DrawTextTrueTypeW(25, 120, 30, 10, "1", 1, 700, false, false, false, "2", ppa);
        //        PrintLab.PTK_PrintLabel(fois, 1);
        //        PrintLab.ClosePort();
        //        return;
        //    }
        //    if (Setting(Params.PrintType) == "2") //todo test or remove
        //    {
        //        TscLab.openport(Setting(Params.Printer));
        //        PrintLab.PTK_ClearBuffer();
        //        //PrintLab.PTK_EnableCircumgyrate(40);
        //        PrintLab.PTK_SetPrintSpeed(3);
        //        PrintLab.PTK_SetDarkness(8);
        //        PrintLab.PTK_SetLabelHeight(160, 16);
        //        PrintLab.PTK_SetLabelWidth(320);
        //        PrintLab.PTK_DrawTextTrueTypeW(65, 3, 30, 10, "1", 1, 900, false, false, false, "1", name);
        //        PrintLab.PTK_DrawBarcode(45, 35, 0, "1", 2, 2, 80, 'N', code); //1 maximum
        //        PrintLab.PTK_DrawTextTrueTypeW(25, 120, 30, 10, "1", 1, 700, false, false, false, "2", ppa);
        //        PrintLab.PTK_PrintLabel(fois, 1);
        //        PrintLab.ClosePort();
        //        return;
        //    }
        //    var sb = new StringBuilder();
        //    sb.AppendLine();
        //    sb.AppendLine("SIZE 40 mm,25 mm");
        //    sb.AppendLine("GAP 3 mm,0");
        //    sb.AppendLine("BACKFEED 400");
        //    sb.AppendLine("HOME");
        //    sb.AppendLine("CLS");
        //    var tt = "TEXT 10,10,\"2\",0,1,1,\"Ahmed Bouarif\"\n" +
        //             "BARCODE 10,45,\"128\",80,1,0,2,2,\"123456789\"\n" +
        //             "TEXT 10,160,\"2\",0,1,1,\"EDTA\"\n";
        //    sb.AppendLine(tt);
        //    // BARCODE 5,35,"EAN13",80,0,0,3,3
        //    //sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "TEXT {1},0,1,1,\"{0}\"", name, Setting(Params.S1)));
        //    //sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "BARCODE {1},0,2,2,\"{0}\"", code, Setting(Params.S2)));
        //    //sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "BARCODE 10,50,\"128\",80,1,0,2,2,\"123456789\""));

        //    //sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "TEXT {1},0,1,1,\"{0}\"", ppa, Setting(Params.S3)));
        //    sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "PRINT {0},1", fois));
        //    //RawPrinterHelper.SendStringToPrinter(Setting(Params.Printer), sb.ToString());
        //}

        //public static string Setting(Params name)
        //{
        //    var db = new ApplicationDbContext();
        //    return db.ParamDicts.SingleOrDefault(x => x.ParamDictNom == name.ToString())?.ParamDictValeur;
        //}


        public static List<string> TypeAchat => new List<string> { "BC", "Fact" };
        public static int ScheduleConsMinDays => 7;
        public static int ScheduleConsMaxDays => 60;
        public static int MeanConsTime => 10; //todo 10 minutes depends on medecin

        //public static string GenerateBarcode(ClinModel db)
        //{
        //    var barcode = (++db.ParamDicts.Find(1).ParamDictNumeric).ToString();
        //    var b = new Barcode();
        //    b.Encode(TYPE.EAN13, barcode, 148, 60);
        //    return b.RawData;
        //}

        //public static string GenerateBarcode()
        //{
        //    var db = new ClinModel();
        //    var barcode = (++db.ParamDicts.Find(1).ParamDictNumeric).ToString();
        //    db.SaveChanges();
        //    var b = new Barcode();
        //    b.Encode(TYPE.EAN13, barcode, 148, 60);
        //    return b.RawData;
        //}




        //private static void RollBack(DbEntityEntry entry, bool rollback)
        //{
        //    ShowErrorMessage(entry);
        //    if (!rollback) return;
        //    switch (entry.State)
        //    {
        //        case EntityState.Added:
        //            entry.State = EntityState.Detached;
        //            break;
        //        case EntityState.Modified:
        //            entry.CurrentValues.SetValues(entry.OriginalValues);
        //            entry.State = EntityState.Unchanged;
        //            break;
        //        case EntityState.Deleted:
        //            entry.State = EntityState.Unchanged;
        //            break;
        //    }
        //}

        //private static string ShowErrorMessage(DbEntityEntry entry)
        //{
        //    var s = $"Elément {entry.Entity.GetType().Name} ";
        //    var msg = entry.State == EntityState.Deleted ? s + "lié à d'autres objets dans la BDD" : "non valide";
        //    return "Opération annulée\n" + msg ;
        //}


        //{
        //    var db = new ClinModel();
        //    return i.SejourID != null
        //        ? db.Sejours.Find(i.SejourID).Patient
        //        : db.Operations.Find(i.OperationID).Sejour.Patient;
        //}

        //public static void NotifyAll(Model.Domain.InfraModels.Service service, object entity, string type)
        //{
        //    foreach (var user in service.Users) Notify(user, entity, type);
        //}

        public static void NotifHandle()
        {
            /*            var db = new HospModel();
                        while (Application.Current!=null)
                        {
                            Thread.Sleep(5000);
                            var messages = db.Messages.Where(x => x.UserID == LoginManager.User.UserID).ToList();
                            if (messages.Count == 0) continue;
                            foreach (var m in messages)
                            {
                                Application.Current.Dispatcher.Invoke(() => new TaskbarIcon { Icon = SystemIcons.Information }.ShowCustomBalloon(new FancyBalloon { BalloonText = m.MessageTitle, Msg = m.MessageDescript }
                                    , PopupAnimation.Slide, 3600000));
                                db.Messages.Remove(m);
                            }
                            db.SaveChanges();
                        }*/
        }

        //public static IQueryable<MeasurementGroup> ListMesures(long id, int type, ClinModel db)
        //{
        //    return type == 0
        //        ? db.MeasurementGroups.Where(x => x.SejourID == id)
        //        : db.MeasurementGroups.Where(x => x.OperationID == id);
        //}

        //public static bool ConfirmMsg(string title, string msg)
        //{
        //    return MessageBox.Show(msg, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        //}

        //public static bool ConfirmDelete(string elt)
        //{
        //    return MessageBox.Show($"Voulez-vous vraiment supprimer cet élément '{elt}' ?",
        //        "Confirmation de Suppression", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        //}

        public static string GetAge(DateTime? dateTime)
        {
            var i = (DateTime.Today - dateTime)?.Days / 365;
            if (i < 1) return (DateTime.Today - dateTime)?.Days / 30 + " Mois";
            return i + " Ans";
        }

#if Lotfi
        //public static string EtabShortName = Setting(Params.EtabShortName);
        //public static string EtabName = Setting(Params.EtabName);
        //public static string EtabNameAr = Setting(Params.EtabName);
        //public static string EtabNameFr = Setting(Params.EtabNameFr);
        //public static string EtabVille = Setting(Params.EtabVille);
        //public static string EtabAdress = Setting(Params.EtabAdresse);
        //public static string EtabPhone = Setting(Params.EtabPhone);
        //public static string Icd10Chapter = Setting(Params.Icd10Chapter);
        //public static string ActionProcedureCategory = Setting(Params.ActionProcedureCategory);
#endif

#if Bahos
        public static string EtabShortName = Resources.EtabShortName3;

        public static string EtabName = Resources.EtabName3;

        public static string EtabNameAr = Resources.EtabName3;//Resources.EtabNameAr;

        public static string EtabNameFr = Resources.EtabNameFr3;
        
        public static string EtabVille = Resources.EtabVille3;
        public static string EtabAdress = Resources.EtabAdresse3;
        public static string EtabPhone = Resources.EtabPhone3;
#endif


    }
}

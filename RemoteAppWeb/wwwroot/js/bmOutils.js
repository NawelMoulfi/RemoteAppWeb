//Web Socket used
var _WebSocketPort = 32493; //max 65535
//var barCodePrint=false;
var scanId = 0;
var printerListString = "";

function GetPrintersList(id) {
    return window.printersNamesList[id];
}

function GetSelectedPrinterName() {
    return window.printerName;
}
/** * ***********************************************************
 *                      previews
****************************************************************/
function GetOriginUrl() {
   
    var pathname = window._ModulePathname !== undefined ? window._ModulePathname : window.location.pathname;
    if (pathname.toLowerCase().indexOf("request") >= 0)
        return "/request/";
    if (pathname.toLowerCase().indexOf("validation") >= 0)
        return "/validation/";
    if (pathname.toLowerCase().indexOf("plan") >= 0)
        return "/plan/";
    if (pathname.toLowerCase().indexOf("samples") >= 0)
        return "/samples/";
    if (pathname.toLowerCase().indexOf("convention") >= 0)
        return "/Convention/";
    if (pathname.toLowerCase().indexOf("rendezvous") >= 0)
        return "/RendezVous/";
    return "/request/";
}
//****************
//GetPrinters
//****************
function GetClientPrintersThirdPart(id, link, _this) {
    if (!("WebSocket" in window))
        alert("Ce navigateur ne supporte pas notre technologie d'impression");
    if (!window._this) window._this = _this;
    var ws = new WebSocket(link);
    ws.onopen = function () { ws.send(JSON.stringify({ command: "GetClientPrinters", params: {} })); };
    ws.onmessage = function (evt) { HandleSocketResponse(ws, evt, id) };
    ws.onclose = function () {};
    //ws.onerror = function (evt) { /*alert('impossible de se connecter à l\'imprimante !!');*/ };
}

//function HandleSocketResponse(ws, evt) {
//    var response = JSON.parse(evt.data);
//    var l = "";
//    for (var i = 0; i < response.content.length; i++) {
//        l = l + ";" + response.content[i];
//    }
//    return response.content;
//};
window.printersNamesList = {};
function HandleSocketResponse(ws, evt, id) {
    //console.log(new Date());
    var response = JSON.parse(evt.data);
    switch (response.type) {
        case "PrintersNamesList":
            for (var i = 0; i < response.content.length; i++) {
                printerListString = response.content[i] + ";" + printerListString;
            }
            window.printersNamesList[id] = printerListString;
            if (window._this) window._this.invokeMethodAsync('AddPrinters', id);
        //$(".PrinterListStrings").value = l;
        // alert(response.content);
        //s.AddItem("", "");
        //response.content.forEach(function (printerName) {
        //    s.AddItem(printerName, 'ws://127.0.0.1:32493;' + printerName);
        //});
        //s.SetEnabled(true);
        //InitComboBox(s);
        break;
    case "AppInfo":
        CheckBmToolsVersion(response.appVersion);
        break;
    case "success":
        break;
    case "error":
        //alert(response.content);
        break;
    }
    ws.close();
}
function SetPrintingSide(printerName, printType){
    localStorage.setItem(printType,printerName);
}
function GetLocalStorage(printType) {
   // alert(JSON.parse(localStorage.getItem(printerType)));
    var p = localStorage.getItem(printerType);
    var printerName = JSON.parse(p);
    return printerName.printer;
}
//function SetPrintingSide(s, side) {
//    var keys = s.name.split('_');
//    var sideType = keys[0];
//    var printerType = keys[1];
//    localStorage.setItem(printerType, JSON.stringify({ side: side, sideType: sideType, printer: s.GetValue() }));
//    var list = ["server", "client", "ClientShared"];
//    list.forEach(function (element) {
//        if (element !== sideType) {
//            try {
//                window[element + "_" + printerType].SetSelectedIndex(0);
//            } catch (e) { }
//        }
//    });
//}
function GetPrinterName(printerType) {
    try {
        var printerValue = JSON.parse(localStorage.getItem(printerType));
        if (!printerValue || !printerValue.printer || printerValue.printer === "") {
            Alert_Error({ status: "info", content: " Vous devez sélectionner l'imprimante associée avant d'imprimer !! " })
            ShowPrintersSettings();
            //window.open("/request/PrinterSettings");
        } else
            window.printerName = printerValue.printer;
    } catch (e) {
        Alert_Error({ status: "info", content: " Vous devez sélectionner l'imprimante associée avant d'imprimer !! " })
        ShowPrintersSettings();
        //window.open("/request/PrinterSettings");
    }
}
/****************************************************************
/* preview logic */
function GetPreview_Pdf(printParams) { //arId, previewType
    if (printParams.arId < 1)
        return;
    LoadingPanel.Show();
    $.post(
        GetOriginUrl() + "GetPdfPreview",
        { arId: printParams.arId, previewType: printParams.previewType, reportId: printParams.reportDefinitionId },
        function (data) {

            if (data.status === "success") {
                TryShowPdfReport(data.content.report, printParams);
                return;
            }
            LoadingPanel.Hide();
            window.Alert_Error(data);
        }
    );
}
function TryShowPdfReport(data, printParams) { //printParams { type: previewType}
    if ($("#div_ViewerPopup").html() === "") {
        $("#div_ViewerPopup").load(
            "/request/ViewerPopup",
            null,
            function () {
                ViewerPopup.Show();
                setTimeout(function () {
                    ReplacePdfJsFile(data, printParams);
                    LoadingPanel.Hide();
                },
                    1500);
            }
        );
        return;
    }
    ViewerPopup.Show();
    ReplacePdfJsFile(data, printParams);
    LoadingPanel.Hide();
}

function ToWordFromPdfPreview(printParams) {
    DownloadReportFromPreview(printParams, "Word");
}
function ToExcelFromPdfPreview(printParams) {
    DownloadReportFromPreview(printParams, "Excel");
}

function DownloadReportFromPreview(printParams, mimeType) {
    var previewType = printParams.previewType ? printParams.previewType : printParams.type;
    var entityId = previewType === "PriceListing" ? printParams.conventionId
        : previewType === "LaboratoryAll" || previewType === "GroupedByAnalysisType" || previewType === "soutraitanceDetail" || previewType === "CustomConvention" || previewType === "CompleteInvoice" ? printParams.priceConventionInvoiceId
            : printParams.arId;
    var parms = {
        Id: null,
        EntityId: entityId,
        ReportType: previewType,
        MimeType: mimeType,
        StartDate: printParams.startDate,
        EndDate: printParams.endDate,
        ReportDefinitionId: printParams.reportDefinitionId
    };
    window.open("/report/GetReportAsFile?" + new URLSearchParams(parms).toString(), "Iframe_Download_Target");
}
function PrintFromPdfPreview(printParams) {
    if (printParams.previewType === "DGSN" ||
        printParams.previewType === "Convention3" ||
        printParams.previewType === "CustomConvention" ||
        printParams.previewType === "Laboratory" ||
        printParams.previewType === "LaboratoryAll" ||
        printParams.previewType === "soutraitanceDetail" ||
        printParams.previewType === "GroupedByAnalysisType" ||
        printParams.previewType === "CompleteInvoice") {
        TryPrintInvoiceFacture(printParams);
        return;
    }
    if (printParams.previewType === "PlanGroupe" ||
        printParams.previewType === "listDenvoi" ||
        printParams.previewType === "oldSendList" ||
        printParams.previewType === "ToDoWorksheet" ||
        printParams.previewType === "AllPatientWorksheet" ||
        printParams.previewType === "ToDoWorksheet_Autre" ||
        printParams.previewType === "AllPatientWorksheet_Autre") {
        TryPrintGroupedWorksheet(printParams);
        return;
    }
    switch (printParams.previewType) {
        case "AnalysisRequestResult":
            TryPrint_AR_Result(printParams.arId);
            break;
        case "FedelCard":
            TryPrintFedelCard(printParams.arId);
            break;
        case "AboBadge":
            TryPrintAbo(printParams.arId, "Badge", printParams.reportDefinitionId);
            break;
        case "AboOldModel":
            TryPrintAbo(printParams.arId, "OldModel", printParams.reportDefinitionId);
            break;
        case "CustomModel":
            TryPrintAbo(printParams.arId, "CustomModel", printParams.reportDefinitionId);
            break;
        case "TicketRecu":
            TryPrintTicket(printParams.arId);
            break;
        case "Facture":
            TryPrintFacture(printParams.arId, printParams.reportDefinitionId);
            break;
        case "BacterioOnePatient":
            TryPrintBacterioReport(printParams);
            break;
        case "BacterioAllPatient":
            TryPrintBacterioReport(printParams);
            break;
        case "BacterioOthers":
            TryPrintBacterioReport(printParams);
            break;
        case "PreviewMaladieWithDeclaration":
            TryPrintPreviewMaladieWithDeclaration(printParams);
            break;
        case "ValidationToDoWorkSheet":
            TryPrintValidationToDoWorkSheet(printParams);
            break;
        case "PriceListing":
            TryPrintPriceListing(printParams);
            break;
        case "ResultListReport":
            TryPrintResultListReport(printParams);
            break;
        case "PreviewCovid19":
            TryPrintPreview_Covid19(printParams);
            break;
        default:
            AlertNotImplementedFunction();
            break;
    }
}
/** * ***********************************************************
 *                      Reports
****************************************************************/

function TryPrintPreview_Covid19(printParams) {
    var printer = GetPrinterName("Result");
    if (!printer)
        return;
    if (printer.side === "server")
        PrintPreview_Covid19(printParams, printer.printer);
    else if (printer.side === "client") {
        GetPreview_Covid19_PrnxReport(printParams, printer.printer);
    }
}

function PrintPreview_Covid19(printParams, printer) {

    AlertNotImplementedFunction();
}

function GetPreview_Covid19_PrnxReport(printParams, printerName) {
    printParams.command = "prnx";
    $.post(
        "/Analytic/Get_PreviewCovid19",
        //        "/Analytic/Get_PreviewMaladieWithDeclaration_Prnx",
        printParams,
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart(printParams.previewType, printerName, data.content);
                return;
            } else
                window.Alert_Error(data);
        }
    );
}

function TryPrint_AR_Result(arId, originUrl, ignorePrintWarning, showCachet) {
    var printer = GetPrinterName("Result");
    if (!printer)
        return;
    ignorePrintWarning = ignorePrintWarning === undefined ? false : ignorePrintWarning;
    if (printer.side === "server")
        PrintAnalysisRequestResult(arId, printer.printer, ignorePrintWarning, originUrl);
    else if (printer.side === "client") {
        Get_AR_ResultPrnxReport(arId, printer.printer, ignorePrintWarning, originUrl, showCachet);
    }
}
/*server print*/
function PrintAnalysisRequestResult(arId, printerName, ignorePrintWarning, originUrl) {

    if (arId < 1)
        return;
    originUrl = originUrl === undefined ? GetOriginUrl() : originUrl;
    var printType = "AnalysisRequestResult";
    var showOpenRequestLink = originUrl !== "/request/"/*undefined*/;
    $.post(
        originUrl + "PrintReportByType",
        {
            printType: printType,
            arId: arId,
            ignorePrintWarning: ignorePrintWarning,
            printer: printerName
        },
        function (data) {
            if (data.status === "success")
                return;
            if (data.status === "PrintWarning") {
                var printCommand = "TryPrint_AR_Result(" + arId + ", undefined, true);";
                OpenPrintWarningPopup(printCommand, arId, printType, printerName, data, showOpenRequestLink);
                //                $('#pop-up-div_container').load('/request/PrintWarningPopup',
                //                    {
                //                        arId: arId,
                //                        printerName: printerName,
                //                        message1: data.content.message1,
                //                        message2: data.content.message2,
                //                        message3: data.content.message3,
                //                        message4: data.content.message4,
                //                        showOpenRequestLink: showOpenRequestLink
                //                    },
                //                    function() {});
            } else
                window.Alert_Error(data);

        }
    );

}
/*client print*/
function Get_AR_ResultPrnxReport(arId, printerName, ignorePrintWarning, originUrl, showCachet) {

    if (arId < 1)
        return;
    originUrl = originUrl === undefined ? GetOriginUrl() : originUrl;
    var showOpenRequestLink = originUrl !== "/request/"/*undefined*/;
    var printType = "AnalysisRequestResult";
    $.post(
        originUrl + "GetPrnxReportByType",
        { printType: printType, arId: arId, ignorePrintWarning: ignorePrintWarning, showCachet: showCachet },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart(printType, printerName, data.content);
                return;
            }
            if (data.status === "PrintWarning") {
                var printCommand = "TryPrint_AR_Result(" + arId + ", undefined, true," + data.content.showCachet + ");";
                OpenPrintWarningPopup(printCommand, arId, printType, printerName, data, showOpenRequestLink);
                //                $('#pop-up-div_container').load('/request/PrintWarningPopup',
                //                    {
                //                        arId: arId,
                //                        printerName: printerName,
                //                        message1: data.content.message1,
                //                        message2: data.content.message2,
                //                        message3: data.content.message3,
                //                        message4: data.content.message4,
                //                        showOpenRequestLink: showOpenRequestLink
                //                    },
                //                    function() {});
            } else
                window.Alert_Error(data);

        }
    );

}

function OpenPrintWarningPopup(printCommand, arId, printType, printerName, data, showOpenRequestLink) {

    var d = document.createElement("div");
    $(d).load('/request/PrintWarningPopup',
        {
            arId: arId,
            printCommand: printCommand,
            printerName: printerName,
            printType: printType,
            message1: data.content.message1,
            message2: data.content.message2,
            message3: data.content.message3,
            message4: data.content.message4,
            showOpenRequestLink: showOpenRequestLink
        },
        function () { });
    $('#pop-up-div_container').append(d);
}

function TryPrintFedelCard(arId) {
    var printer = GetPrinterName("Fedel");
    if (!printer)
        return;
    if (printer.side === "server")
        PrintFideliteCard(arId, printer.printer);
    else if (printer.side === "client") {
        GetFedelCardPrnxReport(arId, printer.printer);
    }
}
/*server print*/
function PrintFideliteCard(arId, printerName) {

    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "PrintReportByType",
        { printType: "FedelCard", arId: arId, printer: printerName },
        function (data) {
            if (data.status !== "success") {
                window.Alert_Error(data);
            }
        }
    );
}
/*client print*/
function GetFedelCardPrnxReport(arId, printerName) {
    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "GetPrnxReportByType",
        { printType: "FedelCard", arId: arId },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart("FedelCard", printerName, data.content);
            } else {
                window.Alert_Error(data);
            }
        }
    );
}
/*####################################################################*/
function AlertNotImplementedFunction() {
    alert("not implemented function");
}

function TryPrint_EnvelopeReport(arId) {
    //var printer = GetPrinterName("Result");
    var printer = GetPrinterName("Envelope");

    if (!printer)
        return;
    if (printer.side === "server")
        AlertNotImplementedFunction();
    else if (printer.side === "client") {
        GetEnvelopePrnxReport(arId, printer.printer);
    }
}

function GetEnvelopePrnxReport(arId, printerName) {
    if (arId < 1)
        return;
    $.post("/Request/GetEnvelopeReport_Prnx",
        { arId: arId },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart("EnvelopeReport", printerName, data.content);
            } else {
                window.Alert_Error(data);
            }
        }
    );
}
/*####################################################################*/
function TryPrintAbo(arId, cardType, reportId, ignorePrintWarning) {
    if (arId < 1)
        return;
    if (cardType !== "Badge" && cardType !== "OldModel" && cardType !== "CustomModel")
        return;
    var printer =
        cardType === "OldModel" || (cardType === "CustomModel" &&
            aboReportTypeMapping.AboA4.indexOf(parseInt(reportId)) >= 0)
            ? GetPrinterName("AboOld")
            : GetPrinterName("AboBadge");
    if (!printer)
        return;
    var originUrl = GetOriginUrl();
    var showOpenRequestLink = originUrl !== "/request/"/*undefined*/;
    var printerName = (printer.side === "client" ? null : printer.printer);
    var printType = cardType === "Badge" ? "AboBadge"
        : cardType === "CustomModel" ? "CustomModel"
            : "AboOldModel";
    $.post(
        "/Request/PrintAbo",
        { printType: printType, arId: arId, printer: printerName, reportId: reportId, ignorePrintWarning: ignorePrintWarning },
        function (data) {
            if (data.status === "PrintWarning") {

                var printCommand = "TryPrintAbo(" + arId + ", \"" + cardType + "\", " + reportId + ",true);";
                return OpenPrintWarningPopup(printCommand, arId, printType, printerName, data, showOpenRequestLink);
                //                return OpenPrintWarningPopup(arId, printType, printerName, data, showOpenRequestLink);
            }
            HandlePrintStatus(printType, data, printer);
        }
    );

}

function PrintMasterBatchAbo(arIds, reportId) {

    var printer = GetPrinterName("AboOld");
    if (!printer)
        return;
    var name = (printer.side === "client" ? null : printer.printer);
    var printType = "AboOldModel";

    $.post(
        "/Request/PrintMasterBatchAbo",
        { arIds: arIds, printer: name, reportId: reportId },
        function (data) { HandlePrintStatus(printType, data, printer) }
    );

}

function TryPrintTicket(arId) {
    if (arId > 0) {
        var printer = GetPrinterName("TicketRecu");
        if (!printer)
            return;
        if (printer.side === "server")
            PrintPreviewInvoice(arId, false, printer.printer);
        else if (printer.side === "client") {
            GetInvoicePrnxReport(arId, false, printer.printer);
        }
    }
}
/*server print*/
function PrintPreviewInvoice(arId, ignorePrintWarning, printerName) {

    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "PrintReportByType",
        { printType: "TicketRecu", arId: arId, ignorePrintWarning: ignorePrintWarning, printer: printerName },
        function (data) {
            if (data.status === "PrintWarning") {
                if (confirm(data.content))
                    PrintPreviewInvoice(arId, true, printerName);
                return;
            }
            if (data.status === "success")
                return;
            window.Alert_Error(data);
        }
    );
}
/*client print*/
function GetInvoicePrnxReport(arId, ignorePrintWarning, printerName) {

    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "GetPrnxReportByType",
        { printType: "TicketRecu", arId: arId, ignorePrintWarning: ignorePrintWarning },
        function (data) {
            if (data.status === "PrintWarning") {
                if (confirm(data.content))
                    GetInvoicePrnxReport(arId, true, printerName);
                return;
            }
            if (data.status === "success") {
                PrintUsingThirdPart("TicketRecu", printerName, data.content);
            } else
                window.Alert_Error(data);
        }
    );
}

function TryPrintFacture(arId, reportId) {
    var printer = GetPrinterName("Facture");
    if (!printer)
        return;
    if (printer.side === "server")
        PrintFacture(arId, printer.printer);
    else if (printer.side === "client") {
        GetFacturePrnxReport(arId, printer.printer, reportId);
    }
}
/*server print*/
function PrintFacture(arId, printerName) {
    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "PrintReportByType",
        { printType: "Facture", arId: arId, printer: printerName },
        function (data) {
            if (data.status !== "success") {
                window.Alert_Error(data);
            }
        }
    );
}
/*client print*/
function GetFacturePrnxReport(arId, printerName, reportId) {
    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "GetPrnxReportByType",
        { printType: "Facture", arId: arId, reportId: reportId },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart("Facture", printerName, data.content);
            } else
                window.Alert_Error(data);
        }
    );
}

function TryPrintSampleBarCode1(arId, sampleId, analysisId) {
    var a = sampleId.split("-");
    TryPrintSampleBarCode(arId, a[0], analysisId, a[1]);
}

function TryPrintLaboBarCodes(laboId, start, end, reportId, arIds) {
    var printer = GetPrinterName("BarCode");
    if (!printer)
        return;
    var name = (printer.side === "client" ? null : printer.printer);
    $.post(
        GetOriginUrl() + "PrintLaboBarcodes",
        { labId: laboId, start: start, end: end, printer: name, reportId: reportId, "arIds[]": arIds },
        function (data) { HandlePrintStatus("BarCode", data, printer) }
    );
}

function PrintPrelevEnCous(startDate, endDate, sexeIds) {
    var printer = GetPrinterName("Result");
    if (!printer)
        return;
    var name = (printer.side === "client" ? null : printer.printer);
    $.post(
        GetOriginUrl() + "PrintEnCoursList",
        { startDate: startDate, endDate: endDate, sexeIds: sexeIds, printer: name },
        function (data) { HandlePrintStatus("Result", data, printer) }
    );
}

function HandlePrintStatus(type, data, printer, isPdf /* = false*/, hide /* = false*/, printParams /*= null*/) {

    isPdf = isPdf === undefined ? false : isPdf;
    hide = hide === undefined ? false : hide;
    printParams = printParams === undefined ? null : printParams;
    if (data.status !== "success") {
        window.Alert_Error(data);
    } else if (isPdf)
        TryShowPdfReport(data.content.report, printParams);
    else if (printer.side === "client")
        PrintUsingThirdPart(type, printer.printer, data.content);
    if (hide)
        LoadingPanel.Hide();
}

function TryPrintSampleBarCode(arId, sampleId, analysisId, tube) /*CurrentArId GetSelectedSampleId()*/ {
    var printer = GetPrinterName("BarCode");
    if (!printer)
        return;
    if (printer.side === "server")
        PrintSampleBarCode(arId, sampleId, analysisId, printer.printer, tube);
    else if (printer.side === "client") {
        GetSampleBarCodePrnxReport(CurrentArId, sampleId, analysisId, printer.printer, tube);
    }
}
/*server print*/
function PrintSampleBarCode(arId, sampleId, analysisId, printerName, tube) {
    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "PrintReportByType",
        {
            printType: "BarCode",
            arId: arId,
            sampleId: sampleId,
            printer: printerName,
            analysisId: analysisId,
            tube: tube
        },
        function (data) {
            if (data.status !== "success") {
                window.Alert_Error(data);
            }
        }
    );
}
/*client print*/
function GetSampleBarCodePrnxReport(arId, sampleId, analysisId, printerName, tube) {
    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "GetPrnxReportByType",
        { printType: "BarCode", arId: arId, sampleId: sampleId, analysisId: analysisId, tube: tube },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart("BarCode", printerName, data.content);
            } else {
                window.Alert_Error(data);
            }
        }
    );
}

function TryPrintAllSampleBarCode(arId) { /*CurrentArId*/
    var printer = GetPrinterName("BarCode");
    if (!printer)
        return;
    if (printer.side === "server")
        PrintAllSampleBarCode(arId, printer.printer);
    else if (printer.side === "client") {
        GetAllSampleBarCodePrnxReport(arId, printer.printer);
    }
}

function TryPrintLotBarCode(lotId) {
    var printer = GetPrinterName("BarCode");
    if (!printer)
        return;
    if (printer.side === "server")
        return;
    else if (printer.side === "client") {
        GetLotBarCodePrnxReport(lotId, printer.printer);
    }
}
/*client print*/
function GetLotBarCodePrnxReport(lotId, printerName) {
    if (lotId < 1)
        return;

    $.post("/Lots/GetBarCodePrnxReport",
        { lotId: lotId },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart("LotBarCode", printerName, data.content);

            } else {
                window.Alert_Error(data);

            }
        }
    );
}
/*server print*/
function PrintAllSampleBarCode(arId, printerName) {
    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "PrintReportByType",
        { printType: "AllBarCode", arId: arId, printer: printerName },
        function (data) {
            if (data.status !== "success") {
                window.Alert_Error(data);
            }
        }
    );
}
/*client print*/
function GetAllSampleBarCodePrnxReport(arId, printerName) {
    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "GetPrnxReportByType",
        { printType: "AllBarCode", arId: arId },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart("AllBarCode", printerName, data.content);
            } else {
                window.Alert_Error(data);

            }
        }
    );
}

function TryPrintPatientBarCode(patientId) {
    var printer = GetPrinterName("BarCode");
    if (!printer)
        return;
    if (printer.side === "server")
        PrintPatientBarCode(patientId, printer.printer);
    else if (printer.side === "client") {
        GetPatientBarCodePrnxReport(patientId, printer.printer);
    }
}

function PrintPatientBarCode(patientId, printerName) {

    $.post(
        GetOriginUrl() + "PrintReportByType",
        { printType: "PatientBarCode", patientId: patientId, printer: printerName },
        function (data) {
            if (data.status === "success")
                return;
            window.Alert_Error(data);
        }
    );
}

function GetPatientBarCodePrnxReport(patientId, printerName) {

    $.post(
        GetOriginUrl() + "GetPrnxReportByType",
        //"Request/GetPrnxReportByType",
        { printType: "PatientBarCode", patientId: patientId },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart("PatientBarCode", printerName, data.content);
            } else {
                window.Alert_Error(data);

            }
        }
    );
}
function PrintUsingThirdPart(printType, printer, prnxReport, entityId=null ) {
    if (!("WebSocket" in window))
        return alert("Ce navigateur ne supporte pas notre technologie d'impression !!");
    var link = "ws://" + printer.split(";")[0] + ":" + _WebSocketPort;
    if (/ws:\/\/.+;.+/.test(printer)) { a = printer.split(";"); link = a[0]; printer = a[1]; }
    var ws = new WebSocket(link);
    ws.onopen = function () { ws.send(JSON.stringify({ command: "Print", params: { printType: printType, printer: printer.split(";")[1], prnxReport: prnxReport } })); };
    ws.onclose = function () { };
    ws.onerror = function (evt) { alert('impossible de se connecter à l\'imprimante !!'); }
    ws.onmessage = function (evt) {
        var response = JSON.parse(evt.data);
        switch (response.type) {
           
            case "success":
                ws.close();
                if (scanId != 0) {
                    isRemis(scanId);
                    scanId = 0;
                }

                if (printType == "ImageRequestReportInterpretationReport") {
                    SetImageRequestPrintedDate(entityId);
                }
                
                break;
            case "error":
                ws.close();
                alert(response.content);
                break;
        }
    };
}

/******************************/

function CheckPrintPdfVersion() {
    var ws = new WebSocket("ws://127.0.0.1:" + _WebSocketPort);
    ws.onopen = function () { ws.send(JSON.stringify({ command: "GetInfo" })); };
    ws.onmessage = function (evt) {
        var response = JSON.parse(evt.data);
        if (response.type === "AppInfo" && cmpVersions(response.appVersion, "2.0.0.3") > 0)
            return;
        if (confirm(" BmTool need update !! want you to download it ??"))
            window.open("/home/GetTool?fileName=BMOutils.exe");
    };
    ws.onerror = function (evt) { alert('impossible de se connecter !!'); }
}

function GetClientSidePrinterName(printerType) {
    printerType = ToPrinterType(printerType);
    if (printerType === null) {
        alert("Type de rapport introuvable !!");
        return;
    }
    try {
        var printerValue = JSON.parse(localStorage.getItem(printerType));
        if (!printerValue || !printerValue.printer || printerValue.printer === "") {
            window.open("/request/PrinterSettings");
        } else
            return printerValue.printer;
    } catch (e) {
        window.open("/request/PrinterSettings");
    }
}
function ToPrinterType(pt) {
    if (resultTp.indexOf(pt) > -1) return "Result";
    if (barCodeTp.indexOf(pt) > -1) return "BarCode";
    if (fedelTp.indexOf(pt) > -1) return "Fedel";
    if (aboBadgeTp.indexOf(pt) > -1) return "AboBadge";
    if (aboOldTp.indexOf(pt) > -1) return "AboOld";
    if (ticketRecuTp.indexOf(pt) > -1) return "TicketRecu";
    if (factureTp.indexOf(pt) > -1) return "Facture";
    if (ticketWaitingLineTp.indexOf(pt) > -1) return "TicketWaitingLine";
    return null;
}
//*** ********************
//function GetPrinterName(printerType) {
//    try {
//        var printerValue = JSON.parse(localStorage.getItem(printerType));
//        if (!printerValue || !printerValue.printer || printerValue.printer === "") {
//            Alert_Error({ status: "info", content: " Vous devez sélectionner l'imprimante associée avant d'imprimer !! " })
//            ShowPrintersSettings();
//            //window.open("/request/PrinterSettings");
//        } else
//            return printerValue;
//    } catch (e) {
//        Alert_Error({ status: "info", content: " Vous devez sélectionner l'imprimante associée avant d'imprimer !! " })
//        ShowPrintersSettings();
//        //window.open("/request/PrinterSettings");
//    }
//}
function HandleBarCode(scanCode) {
    //var printer = GetPrinterName("Result");
    //if (!printer)
    //    return;
    //var name = (printer.side === "client" ? null : printer.printer);
    //$.post("Home/HandleBarCode",
    //    { scanCode: scanCode, printer: name },
    //    function (data) { HandlePrintStatus("AnalysisRequestResult", data, printer) }
    //);
    //barCodePrint=true;
    scanId = scanCode;
    TryPrint_AR_Result(scanCode, /*OriginUrl*/"/request/");
    //window.open("/Request/index/" + scanCode); //todo when refactor ok
}
function isRemis(arId) {
    $.post("/request/IsRemis",
        { arId: arId },
        function (data) {
            if (data.status === "success") {
                $("#isRemis-" + data.content.arId).toggle(data.content.isRemis);
            }
        }
    );
}

function SetImageRequestPrintedDate(imageRequestId)
{

    var imrId = imageRequestId.toString();
    DotNet.invokeMethodAsync('BM.RIS', 'SetImageRequestPrintedDate', imrId );
   
}

/** * ***********************************************************
 *                      plan Reports
****************************************************************/
/*bacterio*/
function GetBacterioPreview_Pdf(printParams) {
    TryPrintBacterioReport(printParams, true);
}

function TryPrintBacterioReport(printParams, isPdf /*=false*/) {
    isPdf = isPdf === undefined ? false : isPdf;
    LoadingPanel.Show();
    var printer = GetPrinterName("Paillasse");
    if (!printer)
        return;
    var name = (printer.side === "client" ? null : printer.printer);
    var cmd = isPdf ? "pdf" : (printer.side === "client" ? "prnx" : "print");
    var params;
    if (printParams.previewType === "BacterioOnePatient") {
        printParams.wspt = 0;
        printParams.command = cmd;
        printParams.printer = name;
        params = printParams;
        //        params = { wspt: 0, arId: printParams.arId, command: cmd, printer: name }
    }
    else if (printParams.previewType === "BacterioOthers") {
        printParams.wspt = 1;
        printParams.command = cmd;
        printParams.printer = name;
        params = printParams;
        //        params = { wspt: 2, analysisIds: JSON.stringify(printParams.analysisIds), command: cmd, printer: name }
    }
    else if (printParams.previewType === "BacterioAllPatient") {
        params = {
            wspt: 2,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            isNotPrinted: printParams.isNotPrinted,
            patialDone: printParams.patialDone,
            command: cmd,
            printer: name
        }
    }
    $.post("/Plan/GetDetailedWorksheet_Report",
        params,
        function (data) { HandlePrintStatus(printParams.previewType, data, printer, isPdf, true, printParams) });
}

/*Grouped work sheet*/
function GetGroupedWorksheet_pdf(printParams) {
    LoadingPanel.Show();
    var params;
    if (printParams.previewType === "PlanGroupe") {
        params = {
            gwst: 0,
            analysisIds: JSON.stringify(printParams.analysisIds),
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            command: "pdf"
        }
    }
    if (printParams.previewType === "listDenvoi" || printParams.previewType === "oldSendList") {
        params = {
            gwst: 2,
            analysisIds: JSON.stringify(printParams.analysisIds),
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            command: "pdf",
            sendListType: printParams.sendListType
        }
    }
    else if (printParams.previewType === "ToDoWorksheet") {
        params = {
            gwst: 3,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            command: "pdf"
        }
    }
    else if (printParams.previewType === "ToDoWorksheet_Autre") {
        params = {
            gwst: 5,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            command: "pdf"
        }
    }
    else if (printParams.previewType === "AllPatientWorksheet") {
        params = {
            gwst: 4,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            command: "pdf"
        }
    }
    else if (printParams.previewType === "AllPatientWorksheet_Autre") {
        params = {
            gwst: 6,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            SampleReceived: SampleReceivedCheckBox.GetValue(),
            command: "pdf"
        }
    }
    else if (printParams.previewType === "PlanGroupe2") {
        params = {
            gwst: 7,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            command: "pdf"
        }
    }
    $.post(
        GetOriginUrl() + "GetGroupedWorksheet_Report",
        params,
        function (data) {
            if (data.status === "success") {
                TryShowPdfReport(data.content.report, printParams);
                return;
            }
            LoadingPanel.Hide();
            window.Alert_Error(data);
        }
    );
}

function TryPrintPreviewMaladieWithDeclaration(printParams) {
    var printer = GetPrinterName("Result");
    if (!printer)
        return;
    if (printer.side === "server")
        PrintPreviewMaladieWithDeclaration(printParams, printer.printer);
    else if (printer.side === "client") {
        GetPreviewMaladieWithDeclaration_PrnxReport(printParams, printer.printer);
    }
}

function PrintPreviewMaladieWithDeclaration(printParams, printer) {

    AlertNotImplementedFunction();
}
function GetPreviewMaladieWithDeclaration_PrnxReport(printParams, printerName) {
    printParams.command = "prnx";
    $.post(
        "/Analytic/Get_PreviewMaladieWithDeclaration",
        //        "/Analytic/Get_PreviewMaladieWithDeclaration_Prnx",
        printParams,
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart(printParams.previewType, printerName, data.content);
                return;
            } else
                window.Alert_Error(data);

        }
    );
}

function TryPrintGroupedWorksheet(printParams) {
    var printer = GetPrinterName("Result");

    if (printParams.previewType == "ToDoWorksheet_Autre" || printParams.previewType == "AllPatientWorksheet_Autre")
        printer = GetPrinterName("Paillasse");

    if (!printer)
        return;
    if (printer.side === "server")
        PrintGroupedWorksheet(printParams, printer.printer);
    else if (printer.side === "client") {
        GetGroupedWorksheet_PrnxReport(printParams, printer.printer);
    }
}

function TryPrintPriceListing(printParams) {
    var printer = GetPrinterName("Result");
    if (!printer)
        return;
    if (printer.side === "server")
        AlertNotImplementedFunction();
    else if (printer.side === "client") {
        GetPrintPriceListing_PrnxReport(printParams, printer.printer);
    }
}

function GetPrintPriceListing_PrnxReport(printParams, printer) {
    printParams.command = "prnx";
    $.post(
        "/Convention/GetPriceListingReportAsPDF",
        printParams,
        function (data) {

            if (data.status === "success") {
                //                TryShowPdfReport(data.content.report, printParams);
                PrintUsingThirdPart(printParams.previewType, printer, data.content);
                return;
            }
            LoadingPanel.Hide();
            window.Alert_Error(data);
        }
    );
}
function PrintGroupedWorksheet(printParams, printer) {
    var params;
    if (printParams.previewType === "PlanGroupe") {
        params = {
            gwst: 0,
            analysisIds: JSON.stringify(printParams.analysisIds),
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            command: "print",
            printer: printer
        }
    }
    if (printParams.previewType === "listDenvoi") {
        params = {
            gwst: 2,
            analysisIds: JSON.stringify(printParams.analysisIds),
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            command: "print",
            printer: printer
        }
    } else if (printParams.previewType === "ToDoWorksheet") {
        params = {
            gwst: 3,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            command: "print",
            printer: printer
        }
    } else if (printParams.previewType === "AllPatientWorksheet") {
        params = {
            gwst: 4,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            SampleReceived: SampleReceivedCheckBox.GetValue(),
            command: "print",
            printer: printer
        }
    }
    $.post(
        GetOriginUrl() + "GetGroupedWorksheet_Report",
        params,
        function (data) {

            if (data.status === "success") {
                //                TryShowPdfReport(data.content.report, printParams);
                return;
            }
            LoadingPanel.Hide();
            window.Alert_Error(data);
        }
    );
}

function GetGroupedWorksheet_PrnxReport(printParams, printer) {
    var params;
    if (printParams.previewType === "PlanGroupe") {
        params = {
            gwst: 0,
            analysisIds: JSON.stringify(printParams.analysisIds),
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            command: "prnx"
        }
    }
    if (printParams.previewType === "listDenvoi" || printParams.previewType === "oldSendList") {
        params = {
            gwst: 2,
            analysisIds: JSON.stringify(printParams.analysisIds),
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            command: "prnx",
            sendListType: printParams.sendListType
        }
    } else if (printParams.previewType === "ToDoWorksheet") {
        params = {
            gwst: 3,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            command: "prnx"
        }
    } else if (printParams.previewType === "ToDoWorksheet_Autre") {
        params = {
            gwst: 5,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            command: "prnx"
        }

    } else if (printParams.previewType === "AllPatientWorksheet") {
        params = {
            gwst: 4,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            command: "prnx"
        }
    } else if (printParams.previewType === "AllPatientWorksheet_Autre") {
        params = {
            gwst: 6,
            startDate: printParams.startDate,
            endDate: printParams.endDate,
            unitsIds: printParams.unitsIds,
            analysisTypeIds: printParams.analysisTypeIds,
            SampleReceived: SampleReceivedCheckBox.GetValue(),
            command: "prnx"
        }
    }
    $.post(
        GetOriginUrl() + "GetGroupedWorksheet_Report",
        params,
        function (data) {

            if (data.status === "success") {
                //                TryShowPdfReport(data.content.report, printParams);
                PrintUsingThirdPart(printParams.previewType, printer, data.content);
                return;
            }
            LoadingPanel.Hide();
            window.Alert_Error(data);
        }
    );
}
/*Grouped work sheet*/
function TryPrintValidationToDoWorkSheet(printParams) {
    var printer = GetPrinterName("Result");
    if (!printer)
        return;
    if (printer.side === "server")
        AlertNotImplementedFunction();

    //        PrintValidationToDoWorkSheet(printParams, printer.printer);
    else if (printer.side === "client") {
        GetValidationToDoWorkSheet_PrnxReport(printParams, printer.printer);
    }
}


function GetValidationToDoWorkSheet_PrnxReport(printParams, printer) {
    printParams.isPdf = false;
    $.post(
        GetOriginUrl() + "ToDoWorksheet_PdfReport",
        printParams,
        function (data) {

            if (data.status === "success") {
                //                TryShowPdfReport(data.content.report, printParams);
                PrintUsingThirdPart(printParams.previewType, printer, data.content);
                return;
            }
            LoadingPanel.Hide();
            window.Alert_Error(data);
        }
    );
}
/*Grouped work sheet*/

function TryPrintInvoiceFacture(printParams) {
    var printer = GetPrinterName("Result");
    if (!printer)
        return;
    if (printer.side === "server")
        PrintInvoiceFacture(printParams);
    else if (printer.side === "client") {
        Get_InvoiceFacture_PrnxReport(printParams, printer.printer);
    }
}

/*server print*/
function PrintInvoiceFacture(printParams) {
    AlertNotImplementedFunction();
}

/*client print*/
function Get_InvoiceFacture_PrnxReport(printParams, printerName) {
    $.post(
        "/Convention/Get_InvoiceFacture_PrnxReport",
        printParams,
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart(printParams.previewType, printerName, data.content);
                return;
            } else
                window.Alert_Error(data);

        }
    );

}


function TryPrintScheduledTicket(arId) {
    if (arId > 0) {
        var printer = GetPrinterName("TicketRecu");
        if (!printer)
            return;
        if (printer.side === "server")
            AlertNotImplementedFunction();
        else if (printer.side === "client") {
            GetScheduledInvoicePrnxReport(arId, false, printer.printer);
        }
    }
}

/*client print*/
function GetScheduledInvoicePrnxReport(arId, ignorePrintWarning, printerName) {

    if (arId < 1)
        return;
    $.post(
        GetOriginUrl() + "GetScheduledTicket_PrnxReport",
        { printType: "TicketRecu", arId: arId, ignorePrintWarning: ignorePrintWarning },
        function (data) {
            if (data.status === "success") {
                PrintUsingThirdPart("TicketRecu", printerName, data.content);
            } else
                window.Alert_Error(data);
        }
    );
}


function TryPrintResultListReport(printParams) {
    var printer = GetPrinterName("Result");
    if (!printer)
        return;
    if (printer.side === "server")
        AlertNotImplementedFunction();
    else if (printer.side === "client") {
        GetPrintResultList_PrnxReport(printParams, printer.printer);
    }
}

function GetPrintResultList_PrnxReport(printParams, printer) {
    printParams.command = "prnx";
    $.post(
        "/Plan/ResultListReport",
        printParams,
        function (data) {

            if (data.status === "success") {
                //                TryShowPdfReport(data.content.report, printParams);
                PrintUsingThirdPart(printParams.previewType, printer, data.content);
                return;
            }
            LoadingPanel.Hide();
            window.Alert_Error(data);
        }
    );
}


function OpenConnection(hostTechnology, adId, adWP) {
    var link = "ws://127.0.0.1:" + 32493;
    var ws = new WebSocket(link);
    var script = '';
    if (hostTechnology === 'AnyDesk') {
        var anyDeskLocation = 'C:\\Program Files (x86)\\AnyDesk\\AnyDesk.exe';
        script = 'echo ' + adWP + ' | "' + anyDeskLocation + '" ' + adId + ' --with-password';
    }
    else if (hostTechnology === 'RustDesk') {
        var rustDeskLocation = '"C:\\Program Files\\RustDesk\\RustDesk.exe"';
        script = rustDeskLocation + ' --connect ' + adId;
    }
    //var anyDeskLocation = document.getElementById("anyDeskLocation").value;
    //anyDeskLocation = anyDeskLocation ? anyDeskLocation : "AnyDesk.exe";
    ws.onopen = function () {
        ws.send(JSON.stringify({
            command: "RCC2",
            params: { script: script }
        }));
    };
    ws.onclose = function () { };
    ws.onerror = function (evt) { alert('impossible de se connecter !!'); }
    ws.onmessage = function (evt) {
        var response = JSON.parse(evt.data);

        console.log(response);

        switch (response.type) {
            case "success": return ws.close();
            case "error": alert(response.content); return ws.close();
        }
    };
}






var ticketRecuTp = ["TicketRecu",];
var aboOldTp = ["AboOld", "AboOldModel",];
var aboBadgeTp = ["AboBadge",];
var fedelTp = ["Fedel", "FedelCard",];
var barCodeTp = ["BarCode",];
var factureTp = ["Facture",];
var ticketWaitingLineTp = ["TicketWaitingLine",];
var resultTp = ["Result",
    "AllPatientWorksheet",
    "AnalysisRequestResult",
    "AllPatientWorksheet_Autre",
    "BacterioOnePatient",
    "BacterioOthers",
    "BacterioAllPatient",
    "BacterioOnePatient",
    "BacterioAllPatient",
    "BacterioOthers",
    "CompleteInvoice",
    "Convention3",
    "CustomConvention",
    "CustomConvention",
    "CompleteInvoice",
    "CustomModel",
    "DGSN",
    "Entete",
    "GroupedByAnalysisType",
    "listDenvoi",
    "Laboratory",
    "LaboratoryAll",
    "PlanGroupe",
    "PriceListing",
    "PreviewMaladieWithDeclaration",
    "ResultListReport",
    "soutraitanceDetail",
    "ToDoWorksheet",
    "ToDoWorksheet_Autre",
    "ValidationToDoWorkSheet"];
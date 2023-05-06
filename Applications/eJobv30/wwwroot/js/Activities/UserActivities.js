﻿$(document).ready(Initialize());

function Initialize() {
    setDateDefault();
    setDefaultValues();

    //check table row count
    if ($('#Act-Main-Table tr').length == 1) {

        content = "<tr>";
        content += "<td colspan=10> <p class='text-center'> No Results Found </p> </td>";
        content += "</tr>";

        $(content).appendTo("#Act-Main-Table");
    }

}

function setDefaultValues() {
    $("#act-create-assigned").val("@ViewBag.User");
    $("#act-edit-assigned").val("@ViewBag.User");
}

//Default date
function setDateDefault() {
    //test
    console.log("moment date:" + moment().format("L"));
    console.log("js date:" + new Date());

    //Get URL Parameters
    var queryString = window.location.search;
    var urlParams = new URLSearchParams(queryString);
    var sDate = urlParams.get('sDate');
    var eDate = urlParams.get('eDate');
    var user = '@ViewBag.User';

    //update dates input fields from url parameters
    $('input[name="DtStart"]').val(sDate);
    $('input[name="DtEnd"]').val(eDate);
    ShowSalesReportMsg(sDate, eDate, " ");
    ShowTypeReportMsg(sDate, eDate, " ");

    //when url dates are empty, assign dates 1 week as default
    if (sDate == null || eDate == null) {
        var today = moment(new Date());
        let pastWeek = moment().add(-7, 'days');

        let oneMonthAgo = moment().add(-30, 'days');

        //convert to string
        var sDate = oneMonthAgo.format("MM/DD/YYYY");
        var eDate = today.format("MM/DD/YYYY");
        console.log("today: " + eDate);

        window.location.href = "UserActivities?user=" + user + "&sDate=" + sDate + "&eDate=" + eDate;
    }
}

function Search() {
    var sDate = $('input[name="DtStart"]').val();
    var eDate = $('input[name="DtEnd"]').val();
    var user = '@ViewBag.User';
    console.log("sDate : " + sDate + " - edate: " + eDate);
    window.location.href = "UserActivities?user=" + user + "&sDate=" + sDate + "&eDate=" + eDate;
}


/**
 *  Activity Type Summary
 **/
function FilterTypeSummary(type) {
    var user = '@ViewBag.User';
    var sDate = moment();
    var eDate = moment();

    $("#Act-Type-Filter-" + type).css("color", "black");
    $("#Act-Type-Filter-" + type).siblings("a").css("color", "#008ac9");

    switch (type) {
        case "Weekly":
            sDate = moment().add(-7, 'days').format('MM/DD/YYYY');
            break;
        case "Monthly":
            sDate = moment().startOf('month').format('MM/DD/YYYY');
            break;
        case "Yearly":
            sDate = moment().startOf('year').format('MM/DD/YYYY');
            break;
        default:
            break;
    }
    eDate = eDate.format('MM/DD/YYYY');

    //build json object
    var data = {
        user: user,
        sDate: sDate,
        eDate: eDate
    };

    //request data from server using ajax call
    $.ajax({
        url: '/Activities/GetUserTypeSummary/',
        //url: 'CustEntMains/TableResult',
        type: "GET",
        data: data,
        dataType: 'json',
        success: function (data) {
            console.log("ATTEMPT TO FILTER TYPE TABLE");
            ReLoadTypeSummary(data);
            ShowTypeReportMsg(sDate, eDate, type);
        },
        error: function (data) {
            console.log("ERROR");
            console.log(data);
            //LoadTable(data);
        }
    });
}

//populate data result to the type summary table
function ReLoadTypeSummary(data) {

    //clear table data
    $("#TypeSummaryTable").find("tr:gt(0)").remove();

    content = "<tr>";
    content += "<td> " + data["User"] + "</td>";
    content += "<td> " + data["Sales"] + "</td>";
    content += "<td> " + data["Quotation"] + "</td>";
    content += "<td> " + data["Meeting"] + "</td>";
    content += "<td> " + data["Procurement"] + "</td>";
    content += "<td> " + data["CallsAndEmail"] + "</td>";
    content += "</tr>";

    $(content).appendTo("#TypeSummaryTable");
}

function ShowTypeReportMsg(sDate, eDate, filter) {

    sDate = moment(sDate).format("MMM DD YYYY");
    eDate = moment(eDate).format("MMM DD YYYY");

    $("#Act-Type-Filter-Msg").show();
    $("#Act-Type-Filter-Msg").text("Showing " + filter + " Report from " + sDate + " to " + eDate + " ");
}


/**
 *  Activity Sales Summary
 **/
function FilterSalesSummary(type) {
    var user = '@ViewBag.User';
    var sDate = moment();
    var eDate = moment();

    $("#Act-Sales-Filter-" + type).css("color", "black");
    $("#Act-Sales-Filter-" + type).siblings("a").css("color", "#008ac9");

    switch (type) {
        case "Weekly":
            sDate = moment().add(-7, 'days').format('MM/DD/YYYY');
            break;
        case "Monthly":
            sDate = moment().startOf('month').format('MM/DD/YYYY');
            break;
        case "Yearly":
            sDate = moment().startOf('year').format('MM/DD/YYYY');
            break;
        default:
            break;
    }
    eDate = eDate.format('MM/DD/YYYY');

    //build json object
    var data = {
        user: user,
        sDate: sDate,
        eDate: eDate
    };

    //request data from server using ajax call
    $.ajax({
        url: '/Activities/GetUserSalesSummary/',
        //url: 'CustEntMains/TableResult',
        type: "GET",
        data: data,
        dataType: 'json',
        success: function (data) {
            console.log("ATTEMPT TO FILTER SALES TABLE");
            ReLoadSalesSummary(data);
            console.log(data);
            ShowSalesReportMsg(sDate, eDate, type);
        },
        error: function (data) {
            console.log("ERROR");
            console.log(data);
            //LoadTable(data);
        }
    });
}

//populate data result to the type summary table
function ReLoadSalesSummary(data) {

    //clear table data
    $("#SalesSummaryTable").find("tr:gt(0)").remove();

    content = "<tr>";
    content += "<td> " + data["User"].toLocaleString() + "</td>";
    content += "<td> " + data["TotalSales"].toLocaleString() + "</td>";
    content += "<td> " + data["TotalQuotation"].toLocaleString() + "</td>";
    content += "<td> " + data["TotalProcurement"].toLocaleString() + "</td>";
    content += "<td> " + data["TotalJobOrder"].toLocaleString() + "</td>";
    content += "</tr>";

    $(content).appendTo("#SalesSummaryTable");
}

function ShowSalesReportMsg(sDate, eDate, filter) {
    sDate = moment(sDate).format("MMM DD YYYY");
    eDate = moment(eDate).format("MMM DD YYYY");
    $("#Act-Sales-Filter-Msg").show();
    $("#Act-Sales-Filter-Msg").text("Showing " + filter + " Report from " + sDate + " to " + eDate + " ");
}


function CreateActivity() {
    $("#ActivityCreate").modal("show");
}

//DELETE
function DeleteActivity(id) {
    if (confirm("Do you want to remove this activity?")) {
        ajax_DeleteActivity(id);
    }
}

function ajax_DeleteActivity(id) {

    //build json object
    var data = {
        id: id
    };

    console.log(data);

    var url = '/Activities/DeleteActivityRecord';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'text',
        success: function (data) {
            console.log("ATTEMPTING DELETE");
            console.log(data);
            ReloadActivitySummary();
        },
        error: function (data) {
            console.log("ERROR");
            ReloadActivitySummary();
            console.log(data);
        }
    });
}



/**
  *  Activity Summary
  **/
function ReloadActivitySummary() {
    //Get URL Parameters
    var queryString = window.location.search;
    var urlParams = new URLSearchParams(queryString);
    var sDate = urlParams.get('sDate');
    var eDate = urlParams.get('eDate');
    var user = '@ViewBag.User';

    //eDate = moment(eDate).format('MM/DD/YYYY');

    //build json object
    var data = {
        user: user,
        sDate: sDate,
        eDate: eDate
    };

    //request data from server using ajax call
    $.ajax({
        url: '/Activities/GetUserActivitySummary',
        //url: 'CustEntMains/TableResult',
        type: "GET",
        data: data,
        dataType: 'json',
        success: function (data) {
            console.log("Reloading table");
            UpdateActivityTable(data);
        },
        error: function (data) {
            console.log("ERROR");
            console.log(data);
            //LoadTable(data);
        }
    });
}

//populate data result to the type summary table
function UpdateActivityTable(data) {
    var isAdmin = "@ViewBag.IsAdmin";
    var totalPoints = 0;

    //clear table data
    $("#Act-Main-Table").find("tr:gt(0)").remove();

    for (let i = 0; i < data.length; i++) {
        var id = data[i]["Id"];
        content = "<tr>";
        content += "<td> " + moment(data[i]["Date"]).format("MMM-DD YYYY") + "</td>";
        content += "<td> " + data[i]["ProjectName"] + "</td>";
        content += "<td> " + data[i]["SalesCode"] + "</td>";
        content += "<td> " + data[i]["Amount"].toLocaleString() + "</td>";
        content += "<td> " + data[i]["Status"] + "</td>";
        content += "<td> " + data[i]["Type"] + "</td>";
        content += "<td> " + data[i]["ActivityType"] + "</td>";
        content += "<td> " + data[i]["Company"] + "</td>";
        content += "<td> " + data[i]["Remarks"] + "</td>";
        content += "<td> " + data[i]["Assigned"] + "</td>";
        totalPoints += data[i]["Points"];

        //check if user is admin
        if (isAdmin == "True") {
            content += "<td> " + data[i]["Points"] + "</td>";
            content += "<td> ";
            content += " <a class='cursor-hand' onclick='EditActivity(" + id + ")'>Edit</a> | ";
            content += " <a class='cursor-hand' onclick='DeleteActivity(" + id + ")'>Delete</a> ";
            content += "</td> ";
        }


        content += "</tr>";
        $(content).appendTo("#Act-Main-Table");
    }


    //check if user is admin
    if (isAdmin == "True") {
        var totalPointsRow = "";
        totalPointsRow += "<tr>";
        totalPointsRow += "<td colspan='9'> </td>";
        totalPointsRow += "<td > Total Points: </td>";
        totalPointsRow += "<td> <b>" + totalPoints + "</b></td>";
        totalPointsRow += "</tr>";
        $(totalPointsRow).appendTo("#Act-Main-Table");
    }
}
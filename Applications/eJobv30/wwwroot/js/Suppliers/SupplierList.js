


//global variables
var status = "ALL";
var sort = "LATEST-DATE";
var viewType = "SIMPLE";

//load initial on page ready
$(document).ready( 
    initial()
    );

function initial() {
    status = "ALL";
    $('#ALL').css("color", "black");
    ajax_loadContent();
    // ajax_loadProduct()

    //trigger sorting on date of items
    $('#title a').trigger('click');

}    

//update status value on click
//change color of the text
$('#ACT').click(function () {
    status = "ACT";
    $('#ACT').css("color", "black");
    $('#ACT').siblings().css("color", "steelblue");
    $('#ACT').siblings().removeClass("Active");
    $('#ACT').addClass("Active");
    StatusRefresh(); //load active suppliers
});

$('#INC').click(function () {
    status = "INC";
    $('#INC').css("color", "black");
    $('#INC').siblings().css("color", "steelblue");
    $('#INC').siblings().removeClass("Active");
    $('#INC').addClass("Active");
    StatusRefresh(); // load inactive suppliers
});

$('#BAD').click(function () {
    status = "BAD";
    $('#BAD').css("color", "black");
    $('#BAD').siblings().css("color", "steelblue");
    $('#BAD').siblings().removeClass("Active");
    $('#BAD').addClass("Active");
    StatusRefresh(); // load bad suppliers
});

$('#ALL').click(function () {
    status = "ALL";
    $('#ALL').css("color", "black");
    $('#ALL').siblings().css("color", "steelblue");
    $('#ALL').siblings().removeClass("Active");
    $('#ALL').addClass("Active");
    StatusRefresh(); // load all suppliers
});

$('#AOP').click(function () {
    status = "AOP";
    $('#AOP').css("color", "black");
    $('#AOP').siblings().css("color", "steelblue");
    $('#AOP').siblings().removeClass("Active");
    $('#AOP').addClass("Active");
    StatusRefresh(); // load all suppliers
});

$('#ACC').click(function () {
    status = "ACC";
    $('#ACC').css("color", "black");
    $('#ACC').siblings().css("color", "steelblue");
    $('#ACC').siblings().removeClass("Active");
    $('#ACC').addClass("Active");
    StatusRefresh(); // load all suppliers
});

$('#simple-Table').click(function () {
    viewType = "SIMPLE";
    $('#simple-Table').css("color", "black");
    $('#simple-Table').siblings().css("color", "steelblue");
    $('#simple-Table').siblings().removeClass("Active");
    $('#simple-Table').addClass("Active");
    StatusRefresh();
});

$('#expanded-Table').click(function () {
    viewType = "EXPAND";
    $('#expanded-Table').css("color", "black");
    $('#expanded-Table').siblings().css("color", "steelblue");
    $('#expanded-Table').siblings().removeClass("Active");
    $('#expanded-Table').addClass("Active");
    StatusRefresh();
});



//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_loadContent() {
    var query = $('#srch-field').val();
    var srchCat = $('#srch-category').val();

    //build json object
    var data = {
        search: query.trim(),
        category: srchCat
    };

    //console.log(query);
    //request data from server using ajax call
    $.ajax({
        url: '/Suppliers/Suppliers/TableResult?search=' + query.trim() + '&category=' + srchCat + '&status=' + status,
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
            
        },
        error: function (data) {
           // console.log("ERROR");
           //console.log(data);
           switchViews(data)
        }
    });
}


//load table content on search btn click
function ajax_loadProduct() {
    var query = $('#srch-field').val();
    var srchCat = $('#srch-category').val();

    //build json object
    var data = {
        search: query.trim(),
        category: srchCat
    };

    console.log(query);
    //request data from server using ajax call
    $.ajax({
        url: '/Suppliers/Suppliers/TableResultProducts?search=' + query.trim() + '&category=' + srchCat + '&status=' + status,
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
            ProductsTable(data);

        },
        error: function (data) {
            //console.log("ERROR");
            //console.log(data);
            ProductsTable(data);
        }
    });

    //$.get('/Suppliers/TableResultProducts',
    //    {
    //        search: query.trim(),
    //        category: srchCat,
    //        status: status,
    //        sort : sort
    //    }
    //).done((data) => {
    //    ProductsTable(data);
    //}).fail(() => {
    //    LoadingTableError();
    //});
}



//param : data - json object containing 
//
function switchViews(data) {
    switch(viewType){
        case "SIMPLE":
            SimpleTable(data);
            break;
        case "EXPAND":
            ExpandedTable(data);
            break;
        default:
            SimpleTable(data);
    }

}

//display simple/limited information 
//of suppliers
function SimpleTable(data) {

    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    console.log(temp);

    //clear table contents except header
    $("#sup-Table").find("tr:gt(0)").remove();

    //populate table content
    for (var x = 0; x < temp.length; x++) {

        var country = temp[x]["Country"] != null ? temp[x]["Country"] : "--";
        var city = temp[x]["City"] != null ? temp[x]["City"] : "--";
        var category = temp[x]["Category"] != null ? temp[x]["Category"] : "--";
        var contact3 = temp[x]["ContactNumber"] != null ? temp[x]["ContactNumber"] : "--";
        var contactslength = temp[x]["ContactPerson"];
        var productslength = temp[x]["Product"];
        var status = temp[x]["Status"] != null ? temp[x]["Status"] : "--";
        var code = temp[x]["Code"] != null ? temp[x]["Code"] : "--";
       
        content =  "<tr>";
        content += "<td class='table-name-col' ><a href='/Suppliers/Details/" + temp[x]["Id"] +"' style='font-weight:700;'>"
            + temp[x]["Name"].toString() + "</span></td>";

        //content += "<td>" + code + "</td>";

        content += "<td>" + country  + " - " + city + "</td>";
        content += "<td>" + category + "</td>";
        content += "<td>" + FilteredStatus(status) + "</td>";

        content += "<td> ";

        //Products
        if (productslength["length"] > 0) {

            for (var prods = 0; prods < productslength["length"]; prods++) {
                if (typeof productslength[prods] === "undefined") {
                    console.log("Products is undefined");
                } else {
                    var product = productslength[prods].toString();
                    //console.log(productslength);
                    //console.log(name);
                    content += "<span style='font-weight:600;'> " + product + " <span> </br> ";
                }
            }
        }
        content += "</td>";

        // Contact Person
        content += "<td> ";
        for (var name = 0; name < contactslength["length"]; name++) {
            if (typeof contactslength[name] === "undefined") {
                console.log("Contact Person is undefined");
            } else {
                var contactName = contactslength[name].toString();
                //console.log(contactslength);
                //console.log(contactName);
                content += " " + contactName + "</br> </br>";
            }
        }
        content +=  "</td> ";

        content += "<td>";
        for (var contact = 0; contact < contact3["length"]; contact++) {
            if (typeof contact3[contact] === "undefined") {
                console.log("something is undefined");
            } else {
                var name = contact3[contact].toString();
                //console.log(contact3);
                //console.log(name);
                content += " " + name + "</br> ";
            }
        }

        content += "</td>";

        content += "<td>" +
            "<a href='/Suppliers/Suppliers/Details/" + temp[x]["Id"] + "' >Details</a>&nbsp;| " +
            "<a href='/Suppliers/Suppliers/InvItems/" + temp[x]["Id"] + "'>InvProduct</a>&nbsp;| " +
            "<a href='/Suppliers/SupplierActivities/Records/" + temp[x]["Id"] + "'>History</a>  " +
            "</td>";
        content += "</tr>";

        $(content).appendTo("#sup-Table");
        $("#sup-header-2").text("Category");
        $("#sup-header-3").text("Product");
    }

    $("#expireNotice").hide();
}

//display products
function ProductsTable(data) {
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);
    var today = moment(new Date());
    //console.log("products view");
    console.log(temp);
    //clear table contents except header
    $("#prod-Table").find("tr:gt(0)").remove();

    //populate table content
    for (var x = 0; x < temp.length; x++) {
        var product = temp[x]["Name"].toString();
        var particulars = temp[x]["Particulars"] != null ? temp[x]["Particulars"] : "--";
        var supplier = temp[x]["Supplier"] != null ? temp[x]["Supplier"] : "--";
        var supplierId = temp[x]["SupplierId"] != null ? temp[x]["SupplierId"] : "--";

        var rate = temp[x]["ItemRate"] != null ? temp[x]["ItemRate"] : "--";
        var unit = temp[x]["Unit"] != null ? temp[x]["Unit"] : "--";
        var dtEntered = temp[x]["DtEntered"] != null ? moment(temp[x]["DtEntered"]).format("MM/DD/YYYY") : "--";
        var dtValidFrom = temp[x]["DtValidFrom"] != null ? temp[x]["DtValidFrom"] : "--";
        var dtValidTo = temp[x]["DtValidTo"] != null ? temp[x]["DtValidTo"] : "--";
        var origin = temp[x]["Origin"] != null ? temp[x]["Origin"] : " ";
        var material = temp[x]["Material"] != null ? temp[x]["Material"] : " ";
        var remarks = temp[x]["Remarks"] != null ? temp[x]["Remarks"] : "--";
        var isValid = temp[x]["IsValid"] != null ? temp[x]["IsValid"] : "--";
        
        //exclude product without rate
        if (rate != "--") {
            content = " ";
            if (isValid == 0) {
                content += "<tr>";
            } else {
                content += "<tr style='color:gray'>";
            }

            content += "<td>" + product     + "</td>";
            content += "<td>" + supplier + "</td>";
            content += "<td>" + origin   + "</td>";
            content += "<td>" + rate + " " + unit + "</td>";
            content += "<td>" + material   + "</td>";
            content += "<td>" + particulars + "<br/> <span style='font-size:11px;'>Remarks: " + remarks +"</span> </td>";

            content += "<td>" +
                "<a href='/Suppliers/Suppliers/Details/" + supplierId + "'>Details</a> | " +
                "<a href='/Suppliers/Suppliers/InvItems/" + supplierId + "'>InvProduct</a>  " +
                "</td>";       
            content += "</tr>";

            //var validDate = moment(dtValidTo);
         
            $(content).appendTo("#prod-Table");
            
        }
        $("#expireNotice").show();
    }
}

//load table with ACTIVE suppliers
function StatusRefresh() {
    //clear search field
    $('#srch-field').val('');

    LoadingAnimation();

    //load table content
    ajax_loadContent();
}

//Filter Status Column 
function FilteredStatus(status) {
    switch(status){
        case "ACT":
            return "Active";
        case "INC":
            return "Inactive";
        case "BAD":
            return "Bad";
        case "ACC":
            return "Accredited";
        case "AOC":
            return "Acc. on Progress";
        default:
            return "Not Defined"
    }
}

//Search Filter
function searchSupplier() {
    //console.log("Searching");

    //get search-field
    var srch = $('#srch-field').val();
    //get search-category
    var srchCat = $('#srch-category').val();

    //search and load table
    switch (srchCat) {
        case "PRODUCT":
        case "MATERIAL":
        case "ORIGIN":
            //console.log("Product");
            $("#sup-Table").hide();
            $("#prod-Table").show();
            ajax_loadProduct();

            setTimeout(function () {
                //$("#sort-date-validity").click();
            }, 500);

            break;
        default:
            //console.log("Supplier");
            $("#prod-Table").hide();
            $("#sup-Table").show();

            LoadingAnimation();

            ajax_loadContent();
            break;
    }
}


//On Enter, Search and reload Table
$('#srch-field').on('keypress', function (e) {
    if (e.which === 13) {
        searchSupplier(); //Load Table
    }
});

//add loading animation
function LoadingAnimation() {

    //clear table contents except header
    $("#sup-Table").find("tr:gt(0)").remove();

    var loading = '<tr>'
        + '<td colspan="4"> </td>'
        + '<td colspan="4">'
        + ' <br />'
        + ' <img src="../Images/GIF/loading.gif" height="25" />'
        + ' Loading Companies, please wait...'
        + ' </td>'
        + ' <td></td>'
        + '</tr >';

    $(loading).appendTo("#sup-Table");
    $(loading).appendTo("#prod-Table");
}


//add loading error
function LoadingTableError() {

    //clear table contents except header
    $("#sup-Table").find("tr:gt(0)").remove();

    var loading = '<tr>'
        + '<td colspan="4"> </td>'
        + '<td colspan="4">'
        + ' <br />'
        + ' Unable to Load Suppliers.'
        + ' </td>'
        + ' <td></td>'
        + '</tr >';

    $(loading).appendTo("#sup-Table");
    $(loading).appendTo("#prod-Table");
}


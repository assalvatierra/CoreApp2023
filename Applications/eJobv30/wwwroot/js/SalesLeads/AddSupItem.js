
/**
 * Add Supplier Item  
 **/

var SLID = 0;
function addSupItemToView(supId, rateId, item, supplier, ratePerUnit) {
    content = "<span>";
    content += " " + item + " - " + ratePerUnit + " (" + supplier + ")";
    content += "</span><br>";
    $(content).appendTo("#SalesLeadItems-" + SLID);

    //close modal
    $('#selectSupplierItem-' + supId).modal('toggle');

    console.log("content:" + content);

    //add to db
    ajax_AddSupRate(SLID, rateId);
}

function addSupItem(salesLeadId) {
    SLID = salesLeadId;
    console.log("SLID:" + SLID);
}


function getItemSuppliers(itemId, item, itemLeadId) {
    $('#modal-supitems-title').text(item);

    //build json object
    var data = {
        id: itemId
    };
    console.log(item);
    //request data from server using ajax call
    $.ajax({
        url: '/SalesLeads/SalesLeads/getItemSuppliers/' + itemId,
        type: "GET",
        data: JSON.stringify(data),
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
        },
        error: function (data) {
            //console.log("ERROR");
            console.log(data);
            LoadTable(data, itemLeadId);
        }
    });
}

//display simple/limited information 
//of suppliers
function LoadTable(data, itemLeadId) {

    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    //remove existing items 
    removeSupItemsModal();

    //populate table content
    for (var x = 0; x < temp.length; x++) {

        content = '<a class="list-group-item skills" id="supItemDesc" data-toggle="modal" data-target="#selectSupplier" data-dismiss="modal" onclick="AddSupItemRate(' + temp[x]["Id"] + ',' + itemLeadId + ')" > '
            + temp[x]["SupplierName"] + ' - '
            + temp[x]["Rate"] + ' per '
            + temp[x]["Unit"] + ' <br /> Validity:  '
            + temp[x]["ValidStart"] + ' - '
            + temp[x]["ValidEnd"] + ' ' +
            ' </a> ';
        if (moment(temp[x]["ValidEnd"]).diff(moment(), 'days') > 0) {
            $(content).appendTo("#SupItems-modal-content");
        }
    }
}

//add supplier item quoted rate to the Sales Lead item
function AddSupItemRate(itemId, salesleadId) {
    //build json object
    var data = {
        SalesLeadId: salesleadId,
        ItemRateId: itemId
    };

    var url = '/SalesLeads/SalesLeads/AddSupItemRate';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function (data) {
            console.log(data);
            location.reload();
        }
    });
}

function removeSupItemsModal() {
    $("#SupItems-modal-content").empty();
}

function removeSupItems(id) {
    //build json object
    var data = {
        id: id
    };

    var url = '/SalesLeads/SalesLeads/RemoveSupItemRate';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function (data) {
            console.log(data);
            location.reload();
        }
    });
}

function displayNewItem(invId, itemId) {
    var content = $("#supItemDesc-" + itemId);
    $(content).appendTo("#supItem-" + invId);
}

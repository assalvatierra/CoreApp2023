/**
 *  Sales Leads
 */


var STATUS_APPROVED = 5;
var STATUS_AWARDED = 6;
var STATUS_ACCEPTED = 10;
var STATUS_APPROVED_CHCKER = 16;
var STATUS_APPROVED_OWNER = 15;
var STATUS_QUOTATION = 11;
var STATUS_REJECTED = 7;
var STATUS_CLOSE = 8;

// Filter Loading on click
$("#newLead, #acceptedLead, #closedLead, #ongoing, #rejectedLead, #allLead").click(() => {
    $("#overlay").show();
});


function setLeadId(id) {
    $("#supItem-supId").val(id);
}

function addLoadingBtn(e) {
    $(e).text("");
    var loadingImg = '<img src="/Images/GIF/loading.gif" width="20" > Loading ';
    $(e).append(loadingImg);
    $(e).attr('disabled', 'disabled');
}

//add item
function ajax_AddSalesLeadItem(e) {
    //show loading animation on button
    addLoadingBtn(e);

    //build json object
    //console.log("data: " + $("#supItem-Items option:selected").val());

    var data = {
        SalesLeadId: $("#supItem-supId").val(),
        ItemId: $("#supItem-Items option:selected").val(),
        price: $("#supItem-Rate").val(),
        remarks: $("#supItem-Remarks").val()
    };
    //console.log(data);
    var url = '/SalesLeads/SalesLeads/addSupItem';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function (data) {
            //console.log(data);
            location.reload(false);
        }
    });
}

//Remove Item
function ajax_RemoveItem(Id) {
    //build json object

    var data = {
        id: Id
    };

    var url = '/SalesLeads/SalesLeads/RemoveItem';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {

        },
        error: function (data) {
            //console.log(data);
            location.reload(false);
        }
    });
}

function EditLeadItem(id, invItemId, QuotedRate, Remarks, itemName) {
    $('#edit-supItem-Id').val(id);
    $('#edit-supItem-Items').val(invItemId);
    $('#edit-supItem-Rate').val(QuotedRate);
    $('#edit-supItem-Remarks').val(Remarks);

    $('#editItemName').text(itemName);
}

//Edit Item
function ajax_EditLeadItem() {
    //show loading animation on button
    addLoadingBtn(e);

    //build json object
    var Id = $('#edit-supItem-Id').val();
    var InvItemId = $('#edit-supItem-Items').val();
    var Rate = $('#edit-supItem-Rate').val();
    var Remarks = $('#edit-supItem-Remarks').val();
    var data = {
        id: Id,
        invItemId: InvItemId,
        rate: Rate,
        remarks: Remarks
    };

    var url = '/SalesLeads/SalesLeads/EditItem';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log(data);
        },
        error: function (data) {
            //console.log(data);
            location.reload(false);
        }
    });
}

//****************************
//*** Handle Invalid Input ***
//****************************

//prevent invalid inputs
function validate(evt) {
    var theEvent = evt || window.event;

    // Handle paste
    if (theEvent.type === 'paste') {
        key = event.clipboardData.getData('text/plain');
    } else {
        // Handle key press
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode(key);
    }
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

//handle numbers only on input
$('#edit-supItem-Rate').on('input', function () {
    //backspace
    if (isNaN($('#edit-supItem-Rate').val())) {
        var txt = $('#edit-supItem-Rate');
        txt.val(txt.val().slice(0, -1));
    }

    if ($('#edit-supItem-Rate').val() < 0) {
        $('#edit-supItem-Rate').val(0);
    }
});

//handle numbers only on input
$('#supItem-Rate').on('input', function () {
    //backspace
    if (isNaN($('#supItem-Rate').val())) {
        var txt = $('#supItem-Rate');
        txt.val(txt.val().slice(0, -1));
    }

    if ($('#supItem-Rate').val() < 0) {
        $('#supItem-Rate').val(0);
    }
});


//Update Activity Status
function UpdateLeadStatus(e, leadId, statusId) {
    $.post("/SalesLeads/SalesLeads/PostLeadStatus", { slId: leadId, StatusId: statusId }, (result) => {

        console.log(result);
        if (result == 'False') {
            alert("Unable to Update Status")
        }
    }).done((result) => {
        //on update success
        $(e).parent().removeClass("btn-primary");
        $(e).parent().addClass("btn-success");
        if (statusId == STATUS_APPROVED || statusId == STATUS_AWARDED || statusId == STATUS_QUOTATION
            || statusId == STATUS_REJECTED || statusId == STATUS_REJECTED || statusId == STATUS_CLOSE
            || statusId == STATUS_APPROVED_OWNER || statusId == STATUS_APPROVED_CHCKER) {
            window.location.reload(false);
        }
        //window.location.reload(false);
    }).fail((result) => {
        //on update failed
        //console.log("On Fail");
        //console.log(result);

        //display alert message
        alert("Unable to Update Status")
    });

}


//Update Activity Status
function UpdateLeadStatusRemarks(e, leadId, statusId) {
    $.post("/SalesLeads/SalesLeads/PostLeadStatus", { slId: leadId, StatusId: statusId }, (result) => {
        if (result == 'False') {
            alert("Unable to Update Status")
        }
    }).done((result) => {
        //on update success
        $(e).parent().removeClass("btn-primary");
        $(e).parent().addClass("btn-success");

        //show add Remarks 
        $("#SalesLeadRemarks-Id").val(leadId);

        $("#SalesLeadRemarksModal").modal('show');
    }).fail((result) => {
        //on update failed
        //console.log("On Fail");
        //console.log(result);

        //display alert message
        alert("Unable to Update Status")
    });

}


function SubmitSalesLeadRemarks() {
    var id = $("#SalesLeadRemarks-Id").val();
    var remarks = $("#SalesLeadRemarks-Remarks").val();

    $.post("/SalesLeads/SalesLeads/UpdateSalesLeadRemarks", { Id: id, Remarks: remarks })
        .done((res) => {
            $("#SalesLeadRemarksModal").modal('hide');
            window.location.reload(false);
        })
        .fail((err) => {
            alert("Unable to Update Sales Lead Remarks");
        });
}


function ShowUpdateActivityStatus_Modal(leadId) {

    $("#UpdateActivityStatus-LeadId").val(leadId);

    $("#UpdateActivityStatus").modal("show");
}

//Update Sales Lead Activity
function UpdateSalesLeadActivityStatus(status) {
    var id = $("#UpdateActivityStatus-LeadId").val();

    $("#UpdateActivityStatus").modal("hide");
    $.post("/SalesLeads/SalesLeads/UpdateLeadActivityStatus", { id: id, status: status }, (result) => {

    }).done((success) => {
        //console.log(success);
        if (success) {
            window.location.reload(false);
        } else {
            //console.log(err);
            //console.log("Unable to Update Activity Status");
        }
    }).fail((err) => {
        //console.log(err);
        //console.log("Unable to Update Activity Status");
    });
}

//Done Sales Lead Activity
function CustActivityDone(id) {
    $.post("/SalesLeads/SalesLeads/PostCustActivityDone", { id: id }, (result) => {
        //console.log(result);
        window.location.reload(false);
    }).done((success) => {
        //console.log(success);
        window.location.reload(false);
    }).fail((err) => {
        console.log(err);

        console.log("Unable to Close Activity");
    });
}

//Remove sales lead activity
function CustActivityRemove(id) {
    $.post("/SalesLeads/SalesLeads/PostCustActivityRemove", { id: id }, (result) => {
        //console.log(result);
        window.location.reload(false);
    }).done((success) => {
        //console.log(success);
        window.location.reload(false);
    }).fail((err) => {
        //console.log(err);

        console.log("Unable to Remove Activity");
    });
}

//confirmation for Revision
function RevisionConfirmation(id) {
    if (confirm("This will reset the status of the Sales Lead. Would you like to proceed?")) {
        window.location.href = "/SalesLeads/SalesLeads/Revision/" + id;
    }
}


function UpdateLeadItemStatus(leadItemId, statusId) {

    $.post('/Procurement/UpdateLeadItemStatus', { id: leadItemId, statusId: statusId })
        .done((result) => {
            console.log(result);
            if (result == "True") {
                window.location.reload(false);
            } else {
                alert("Unable to Update Sales Lead Item Status");
            }
        })
        .fail((err) => {
            alert("Unable to Update Sales Lead Item Status");
        });
}


function CustActivityEdit(activityId) {
    window.location.href = "/SalesLeads/EditCustActivityCode/" + activityId;
}

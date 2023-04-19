
/**
 *  Update SalesLead File Link
 * 
 * @param {int} leadId
 */

function ShowSalesLeadLink(leadId) {
    $("#AddLeadFile-LeadId").val(leadId);
    $("#AddLeadFileModal").modal("show");
}


function ShowSalesLeadEditLink(leadId) {
    $("#EditLeadFile-LeadId").val(leadId);
    $("#EditLeadFileModal").modal("show");

    //get sales item lead file link
    GetSalesLeadLink(leadId);
}

//Get Sales Leads Latest File Link 
function GetSalesLeadLink(leadId) {

    $.get("/SalesLeads/SalesLeads/GetLeadLastestLink", { id: leadId })
        .done((result) => {
            console.log(result);
            var leadLink = JSON.parse(result);
            $("#EditLeadFile-Link").val(leadLink.Link);
            $("#EditLeadFile-LeadFileId").val(leadLink.Id);
        })
        .fail(() => {
            console.log("Error: Unable to get link.");
            alert("Unable to get sales lead link");
        });
}



//CREATE
//Add new file link
function Submit_AddLeadFileLink() {
    var leadId = $("#AddLeadFile-LeadId").val();
    var input_link = $("#AddLeadFile-Link").val();

    console.log("leadID:" + leadId);
    console.log("input_link:" + input_link);

    $.post("/SalesLeads/SalesLeads/AddLeadLink", { id: leadId, link: input_link })
        .done((res) => {
            if (res == "OK") {
                console.log("Lead Link added");
                window.location.reload(false);
            }
        })
        .fail(() => { 
            console.log("Unable to add lead link");
            alert("Unable to add sales lead link");
        });
}



//Edit
//Add new file link
function Submit_EditLeadFileLink() {
    var leadFileId = $("#EditLeadFile-LeadFileId").val();
    var input_link = $("#EditLeadFile-Link").val();

    console.log("leadFileId:" + leadFileId);
    console.log("input_link:" + input_link);

    $.post("/SalesLeads/SalesLeads/EditLeadLink", { id: leadFileId, link: input_link })
        .done((res) => {
            if (res == "OK") {
                console.log("Lead Link update");
                window.location.reload(false);
            }
        })
        .fail(() => {
            console.log("Unable to update lead link");
            alert("Unable to update sales lead link");
        });
}


function RedirectLeadFileLink(leadId) {
    //Get latest Lead link from server
    $.get("/SalesLeads/SalesLeads/GetLeadLastestLink", { id: leadId })
        .done((result) => {
            var leadLink = JSON.parse(result);
            console.log(leadLink.Link);
            window.open(leadLink.Link);
            //window.location.href = result;
        })
        .fail((result) => {
            console.log(result);

            var leadLink = JSON.parse(result);
            console.log(leadLink.Link);
            window.open(leadLink.Link);

            console.log("Error: Unable to get link.");
            //alert("Unable to get sales lead link");
        });
}
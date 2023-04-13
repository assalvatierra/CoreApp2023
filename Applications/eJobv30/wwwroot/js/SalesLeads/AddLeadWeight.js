
/**
 *  Update SalesLead Weight
 * 
 * @param {any} leadId
 */


function ShowSalesLeadWeight(leadId) {
    $("#AddLeadWeight-LeadId").val(leadId);

    $("#AddLeadWeightModal").modal("show");

    //get sales item lead weight
    GetSalesLeadWeight(leadId);
}

function Submit_AddSalesLeadWeight() {
    var leadId = $("#AddLeadWeight-LeadId").val();
    var input_weight = $("#AddLeadWeight-Weight").val();

    console.log("leadID:" + leadId);
    console.log("input_weight:" + input_weight);

    $.post("/SalesLeads/SalesLeads/UpdateLeadWeight", { id: leadId, weight: input_weight })
        .done(() => {
            window.location.reload(false);
        })
        .fail(() => {
            console.log("Unable to update weight");
        });
}

function GetSalesLeadWeight(leadId) {
    //GetLeadWeight

    $.get("/SalesLeads/SalesLeads/GetLeadWeight", { id: leadId })
        .done((res) => {
            $("#AddLeadWeight-Weight").val(res);
        })
        .fail(() => {
            console.log("Unable to update weight");
        });
}

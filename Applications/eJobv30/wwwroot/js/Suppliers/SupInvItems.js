
/*
 *  Supplier Inventory Items 
 */ 

$(document).ready(new function () {
    var today = moment().format('MM/DD/YYYY');
    var endDate = moment(today).add(3, 'month').format('MM/DD/YYYY');

    $("#InvRate-ValidFrom").val(today);
    $("#InvRate-ValidTo").val(endDate);

});


function ShowAddItemModal() {
    $("#ItemAddModal").modal("show");
}


function ShowEditItemModal() {
    $("#editInvRate").modal("show");
}


//Item Add Item to Supplier
function ajax_addProduct(invId, supId) {

    //build json object
    var data = {
        invID: invId,
        supID: supId
    };

    console.log(data);

    var url = '/Suppliers/Suppliers/AddInvItems';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            // console.log("SUCCESS");
            ShowLoading()
            console.log(data);
            location.reload(false);
        },
        error: function (data) {
            // console.log("ERROR");
            ShowLoading()
            console.log(data);
            location.reload(false);
        }
    });
}

function ShowLoading() {
    $("#overlay").show();
}


function HideLoading() {
    $("#overlay").hide();
}

//Create Inventory Item Rate
function ajax_createInvRate() {

    //build json object
    var data = {
        id: $("#InvRate-ItemId").val(),
        Material: $("#InvRate-Material").val(),
        Particulars: $("#InvRate-Particulars").val(),
        Rate: $("#InvRate-Rate").val(),
        Unit: $("#InvRate-UnitType").find(":selected").val(),
        ValidFrom: $("#InvRate-ValidFrom").val(),
        ValidTo: $("#InvRate-ValidTo").val(),
        Remarks: $("#InvRate-Remarks").val(),
        TradeTerm: $("#InvRate-TradeTerm").val(),
        Tolerance: $("#InvRate-Tolerance").val(),
        Origin: $("#InvRate-Origin").val(),
        By: $("#InvRate-By").val(),
        ProcBy: $("#InvRate-ProcBy").val()
    };

    console.log(data);

    var url = '/Suppliers/Suppliers/AddRateInvItems';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            // console.log("SUCCESS");
            ShowLoading();
            console.log(data);
            location.reload(false);
        },
        error: function (data) {
            // console.log("ERROR");
            ShowLoading();
            console.log(data);
            location.reload(false);
        }
    });
}

function conformDelete(Id, desc) {
    var confirmRes = confirm("Do you want to REMOVE " + desc + " ?");
    if (confirmRes) {
        ajax_deleteInv(Id);
    }
}


function editInvRate(Id, Particulars, Material, Rate, UnitType, ValidFrom, ValidTo, Remarks, SupInvId, By, ProcBy, TradeTerm, Tolerance, Origin, DtEntered) {
    $("#EditInvRate-ItemId").val(Id);
    $("#EditInvRate-Particulars").val(Particulars);
    $("#EditInvRate-Material").val(Material);
    $("#EditInvRate-Rate").val(Rate);
    $("#EditInvRate-UnitType").val(UnitType);
    $("#EditInvRate-ValidFrom").val(ValidFrom);
    $("#EditInvRate-ValidTo").val(ValidTo);
    $("#EditInvRate-Remarks").val(Remarks);
    $("#EditInvRate-SupInvId").val(SupInvId);
    $("#EditInvRate-TradeTerm").val(TradeTerm);
    $("#EditInvRate-Tolerance").val(Tolerance);
    $("#EditInvRate-Origin").val(Origin);
    $("#EditInvRate-By").val(By);
    $("#EditInvRate-ProcBy").val(ProcBy);
    $("#EditInvRate-DtEntered").val(DtEntered);

    ShowEditItemModal();
}

//EDIT PRODUCT
function ajax_editInvRate() {

    //build json object
    var data = {
        id: $("#EditInvRate-ItemId").val(),
        Particulars: $("#EditInvRate-Particulars").val(),
        Material: $("#EditInvRate-Material").val(),
        Rate: $("#EditInvRate-Rate").val(),
        Unit: $("#EditInvRate-UnitType").find(":selected").val(),
        ValidFrom: $("#EditInvRate-ValidFrom").val(),
        ValidTo: $("#EditInvRate-ValidTo").val(),
        Remarks: $("#EditInvRate-Remarks").val(),
        TradeTerm: $("#EditInvRate-TradeTerm").val(),
        Tolerance: $("#EditInvRate-Tolerance").val(),
        Origin: $("#EditInvRate-Origin").val(),
        SupInvId: $("#EditInvRate-SupInvId").val(),
        By: $("#EditInvRate-By").val(),
        ProcBy: $("#EditInvRate-ProcBy").val(),
        DtEntered: $("#EditInvRate-DtEntered").val()
    };

    console.log(data);

    var url = '/Suppliers/Suppliers/EditRateInvItems';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
    }).done(() => {
        ShowLoading();
        location.reload(false);
    }).fail((err) => {
        //alert("Unable to Update Product");
        ShowLoading();
        console.log(err);
        location.reload(false);
    });
}

function conformDelete(Id, desc) {
    var confirmRes = confirm("Do you want to REMOVE " + desc + " ?");
    if (confirmRes) {
        ajax_deleteInv(Id);
    }
}

//Item Delete
function ajax_deleteInv(Id) {

    //build json object
    var data = {
        id: Id,
    };

    console.log(data);

    var url = '/Suppliers/Suppliers/RemoveInvItems';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            // console.log("SUCCESS");
            ShowLoading();
            console.log(data);
            location.reload(false);
        },
        error: function (data) {
            // console.log("ERROR");
            ShowLoading();
            console.log(data);
            location.reload(false);
        }
    });
}

function AddRateInitial(id, itemName) {
    $("#InvRate-ItemId").val(id);
    $("#InvRate-ItemName").text(itemName);
    //createInvRate

    $("#createInvRate").modal("show");
}

function confirmRateDelete(id, desc) {
    var res = confirm("Do you want to remove " + desc + "?")
    if (res) {
        ajax_deleteItemRate(id);
    }
}

//Item Delete
function ajax_deleteItemRate(Id) {

    //build json object
    var data = {
        id: Id,
    };

    console.log(data);

    var url = '/Suppliers/Suppliers/RemoveRateInvItem';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            // console.log("SUCCESS");
            ShowLoading();
            console.log(data);
            location.reload(false);
        },
        error: function (data) {
            // console.log("ERROR");
            ShowLoading();
            console.log(data);
            location.reload(false);
        }
    });
}

function ShowLoading() {
    $("#overlay").show();
}


function HideLoading() {
    $("#overlay").hide();
}

//Show hide edit / delete of address
$(document).ready(function () {
    AddressInitial();
    ClauseInitial();

    $("#status").text(parseStatusText(this.text));

});



//Show hide edit / delete of Clauses
function ClauseInitial() {
    $("div.comp-clauses").hover(
        function () { //on hover
            $(this).children("span.comp-clauses-actions").show();
        },
        function () { //on leave
            $(this).children("span.comp-clauses-actions").hide();
        }
    );
}

function AddressInitial() {
    $("p.comp-address").hover(
        function () { //on hover
            $(this).children("span.comp-address-actions").show();
        },
        function () { //on leave
            $(this).children("span.comp-address-actions").hide();
        }
    );
}

//ADDRESS EDIT - set address details to the address modal edit fields
function edit_setAddress(id, line1, line2, line3, line4, line5, isPrimary, isBilling) {
    $("#address-ID").val(id);
    $("#Address-1").val(line1);
    $("#Address-2").val(line2);
    $("#Address-3").val(line3);
    $("#Address-4").val(line4);
    $("#Address-5").val(line5);
    $("#EditPrim").attr("checked", isChecked(isPrimary));
    $("#EditBill").attr("checked", isChecked(isBilling));
}

//CLAUSE EDIT - ste clause details to the clause modal edit fields
function edit_setClause(id, title, stardate, enddate, desc1, desc2, desc3) {
    $("#Clause-ID").val(id);
    $("#Clause-Title").val(title);
    $("#Clause-StartDate").val(moment(stardate).format("MM/DD/YYYY"));
    $("#Clause-EndDate").val(moment(enddate).format("MM/DD/YYYY"));
    $("#Clause-Desc1").val(desc1);
    $("#Clause-Desc2").val(desc2);
    $("#Clause-Desc3").val(desc3);
}

// For isBilling and isPrimary checkboxes
// return True || False
function isChecked(input) {
    if (input == "onclick" || input == "on") {
        return true;
    } else {
        return false;
    }
}

function AddCompanyContact() {
    if (checkDuplicate() == true) {
        console.log("adding new contact ");
        ajax_AddContact();
    }

}


function InitialAddContactModal(companyId) {
    if (companyId != null) {
        $("#ac-companyId").val(companyId);
        $("#comContactAdd").modal("show");
    }
}

//GET : check the name of the contact on Add Contact Modal
//      when existing name is found, verify user,
//      if not, proceed to add new user
function checkDuplicate() {


    var custId = $("#ac-custId").val();

    console.log("checking duplicate name ");
    if (Check_AddContact()) {
    
        //New Customer
        if(custId == 1){
            console.log("adding new customer")
          
           return true;

        } else {
            console.log("add new contact ");
            return true;
        }
        return false;
    }
    return false;
}

function checkName() {
    var custId = $("#ac-custId").val();
    $("#add-contact-error").hide();

     $.get("/CustEntMains/CheckNameDuplicate", { custName: $('#ac-name').val() }, (result) => {
          console.log(result);
          if (result == 'True' && custId == 1) {
              $("#add-contact-error").show();
              isNameUnique = false;
          } 
     });
}


// Validate Add Contact
// Check if Name is not empty
function Check_AddContact() {
    name = $("#ac-name").val();

    if (name == '') {
        $("#ac-name-group").addClass("has-warning has-feedback");
        $("#ac-name-warning").show();
        return false;
    } else {
        return true;
    }
}


function PromptDupName(data) {
    var response =data["responseText"];

    if (response == "True") {
        var confirmResponse = confirm("The Customer Name : '" + $('#ac-name').val() + "' already exists, would you like to continue?");

        if (confirmResponse) {
            //Submit Data to create new customer
            ajax_AddContact();
        } else {
            //alert("Cancel");
            //focus on Name
        }
    } else {
        //create supplier and create new contact
        ajax_AddContact();
    }
}


//Add Company Contact
function ajax_AddContact() {
    //build json object
    var data = {
        companyId:  $("#ac-companyId").val(),
        customerId: $("#ac-custId").val(),
        name:       $("#ac-name").val(),
        position:   $("#ac-position").val(),
        email:      $("#ac-email").val(),
        tel:        $("#ac-tel").val(),
        mobile:     $("#ac-mobile").val(),
        social:     $("#ac-social").val(),
        status:     $("#ac-status").val()
    };

    $.post("/CustEntMains/AddContact", data, (result) => {
        if (result == 'True') {
            location.reload();
        } else {
            console.log(result);
            alert('Unable to add New Contact.');
        }
    });

}

//ADDRESS Create
function intialCreateAddress(custEntMainId) {
    if (custEntMainId != null) {
        //Prepare Fields
        $("#create-Address-1").val(""),
        $("#create-Address-2").val(""),
        $("#create-Address-3").val(""),
        $("#create-Address-4").val(""),
        $("#create-Address-5").val(""),

        //initial data & show modal
        $("#custEntMainId").val(custEntMainId);
        $("#AddressCreate").modal("show");
    }
}

function ajax_CreateAddress() {

    //build json object
    var data = {
        custEntMainId: $("#custEntMainId").val(),
        line1: $("#create-Address-1").val(),
        line2: $("#create-Address-2").val(),
        line3: $("#create-Address-3").val(),
        line4: $("#create-Address-4").val(),
        line5: $("#create-Address-5").val(),
        isPrimary: isChecked($("#create-isPrimary:checked").val()),
        isBilling: isChecked($("#create-isBilling:checked").val())
    };

    var url = '/CustEntMains/CreateAddress';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'text',
        success: function (data) {
            if (data == "True") {
                location.reload();
            }
        },
        error: function (data) {
            if (data == "True") {
                location.reload();
            } else {
                alert("Something went wrong while creaing new address for the Company. Please contact your administrator for assistance.");
            }
        }
    });
}

//ADDRESS EDIT
function ajax_EditAddress() {

    //build json object
    var data = {
        id: $("#address-ID").val(),
        line1: $("#Address-1").val(),
        line2: $("#Address-2").val(),
        line3: $("#Address-3").val(),
        line4: $("#Address-4").val(),
        line5: $("#Address-5").val(),
        isPrimary: isChecked($("#EditPrim:checked").val()),
        isBilling: isChecked($("#EditBill:checked").val())
    };


    var url = '/CustEntMains/EditAddress';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data == "200") {
                location.reload();
            }
        },
        error: function (data) {
            location.reload();
        }
    });
}

//CLAUSE EDIT
function ajax_EditClause() {

    //build json object
    var data = {
        id: $("#Clause-ID").val(),
        title: $("#Clause-Title").val(),
        startdate: $("#Clause-StartDate").val(),
        enddate: $("#Clause-EndDate").val(),
        desc1: $("#Clause-Desc1").val(),
        desc2: $("#Clause-Desc2").val(),
        desc3: $("#Clause-Desc3").val()
    };


    var url = '/CustEntMains/EditClause';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        error: function (data) {
            location.reload();
        }
    });
}

//REMOVE ADDRESS
function ajax_RemoveAddress(Id) {
    //build json object
    var data = {
        id: Id
    };

    var url = '/CustEntMains/DeleteAddress';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        error: function (data) {
            location.reload();
        }
    });
}

//CLAUSE DELETE
function ajax_RemoveClause(Id) {

    //build json object
    var data = {
        id: Id
    };

    var url = '/CustEntMains/DeleteClause';

    //Post data from server using ajax call
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        error: function (data) {
            location.reload();
        }
    });
}

    
function parseStatusText(status) {
    var data ;
    switch (status) {
        case 'ACT':
            data = 'ACTIVE';
            break;
        case 'INC':
            data = 'INACTIVE';
            break;
        case 'BAD':
            data = 'BAD ACCOUNT';
            break;
        case 'ACC':
            data = 'ACCREDITED';
            break;
        case 'PRIO':
            data = 'PRIORITY';
            break;
        case 'PRI':
            data = 'PRIORITY';
            break;
        default:
            data = 'NA'
            break;
    }

    $(this).val(data);

    return data;
}

function clickme(event){
    var data = $(event).text();
    $(this).text(parseStatus('ACT'));
}

    
function parseStatus(status) {
    switch (status) {
        case 'PRI':
            return 'PRIORITY';
            break;
        case 'ACC':
            return 'ACCREDITED';
            break;
        case 'ACP':
            return 'ACCREDITATION ON PROCESS';
            break;
        case 'BIL':
            return 'BILLING/TERMS';
            break;
        case 'ACT':
            return 'ACTIVE';
            break;
        case 'INC':
            return 'INACTIVE';
            break;
        case 'BAD':
            return 'BAD ACCOUNT';
            break;
        case 'SUS':
            return 'SUSPENDED';
            break;
        default:
            return 'NA'
            break;
    }
}

//Id 
function SelectCustomer(customerId) {
    var customer = $("#ac-custId option:selected").text();
    $('#ac-custId').val(customerId);
    $('#CustomersModal').modal('toggle');
    $('#customer-textfield').val(customer);
    ajax_getContactDetails();
}


$('#ac-custId').change(function(){
    ajax_getContactDetails();
});
    
//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_getContactDetails() {
    var Id = $("#ac-custId option:selected").val();
   
    //build json object
    var data = {
        id: Id
    };

    //request data from server using ajax call
    $.ajax({
        url: '/CustEntMains/getCustomerAccount',
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        error: function (data) {
            LoadContactFields(data);
        }
    });
}


//display simple/limited information 
//of suppliers
function LoadContactFields(data) {
    //parse data response to json object
    var temp = jQuery.parseJSON(data["responseText"]);

    //clear table contents except header
    $("#sup-Table").find("tr:gt(0)").remove();
    var jobcount = 0;
    var company  = "";
    var contact1 = "";
    var contact2 = "";

    clearAddContactFields()

    $('#ac-name').val(temp["Name"]);
    $('#ac-email').val(temp["Email"]);
    $('#ac-tel').val(temp["Telephone"]);
    $('#ac-mobile').val(temp["Mobile"]);
    $('#ac-social').val(temp["SocialMedia"]);
    $('#ac-position').val(temp["Position"]);
}

function clearAddContactFields(){
    $('#ac-name').val("");
    $('#ac-email').val("");
    $('#ac-tel').val("");
    $('#ac-mobile').val("");
    $('#ac-social').val("");
    $('#ac-position').val("");
}


function PromptRemoveDocument(docDesc, Id){
        
    var message = "Do you want to remove "+docDesc+" from the list?";

    if(confirm(message)){
        RemoveDocument(Id)
    }
}

//DELETE 
function RemoveDocument(Id){
   
    //build json object
    var data = {
        id: Id
    };

    //request data from server using ajax call
    $.ajax({
        url: '/CustEntMains/DeleteDocument',
        type: "POST",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
        },
        error: function (data) {
            location.reload();
        }
    });
}


// Arrow function test
// First arrow function, yay
// Check input changes on Add Contact Name Field
$("#ac-name").on("input", () => {
    $("#ac-name-group").removeClass("has-warning has-feedback");
    $("#ac-name-warning").hide();
});



/****  Company Documents Approval   ****/

function ApproveCompanyDocs(documentId) {
    $.post("/CustEntMains/ApproveDocument", { id: documentId }, (res) => {
        console.log(res);
        if (res == 'True') {
            window.location.reload();
        } else {
            alert("An error has occured. Unable to update document status.");
        }
    })
}
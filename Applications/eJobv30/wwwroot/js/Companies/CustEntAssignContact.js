//Show hide edit / delete of address
$(document).ready(function () {
    $("#add-contact-error").hide();
});



function AddCompanyContact() {
    if (checkDuplicate() == true) {
        console.log("adding new contact ");
        ajax_AddContact();
    }

}

function InitialAddAssignedAgentModal(companyId) {
    if (companyId != null) {
        $("#asc-companyId").val(companyId);
        $("#modalAddAssignedAgent").modal("show");
    }
}

//GET : check the name of the contact on Add Contact Modal
//      when existing name is found, verify user,
//      if not, proceed to add new user
function checkDuplicate() {


    var custId = $("#asc-custId").val();

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

    console.log("custId: " + custId);

    $("#asc-add-contact-error").hide();

     $.get("/CustEntMains/CheckNameDuplicate", { custName: $('#ac-name').val() }, (result) => {
          console.log(result);
         if (result == 'True' && custId == 1) {
             $("#add-contact-error").show();
             isNameUnique = false;
         } else {
             $("#add-contact-error").hide();
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

    console.log(data);

    $.post("/CustEntMains/AddContact", data, (result) => {
        if (result == 'True') {
            location.reload();
        } else {
            console.log(result);
            alert('Unable to add New Contact.');
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

    $("#add-contact-error").hide();
}


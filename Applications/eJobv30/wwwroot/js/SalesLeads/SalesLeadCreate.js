

function searchCustomer() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("SearchBarCustomer");
    filter = input.value.toUpperCase();
    ul = document.getElementById("SearchListCustomer");
    li = ul.getElementsByTagName("li");
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];
        if (a.innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}

function SearchCompany() {
    var input, filter, ul, li, a, i;
    input = document.getElementById("SearchBarCompany");
    filter = input.value.toUpperCase();
    ul = document.getElementById("SearchListCompany");
    li = ul.getElementsByTagName("li");
    for (i = 0; i < li.length; i++) {
        a = li[i].getElementsByTagName("a")[0];
        if (a.innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";
        }
    }
}

function SelectCustomer(customerId) {
    $('#CustomerId').val(customerId);
    $('#CustomersModal').modal('toggle');

    var customer = $("#CustomerId option:selected").text();
    //console.log("customer : " + customer);
    //$("#jobdesc").val(customer);
    getEmail();
    getNumber();
    getCompany();

    $("#CustName").val(customer);
}


function SelectCompany(companyId) {

    //console.log(companyId);
    $('#CompanyId').val(companyId);
    $('#CompanyModal').modal('toggle');

    var company = $("#CompanyId option:selected").text();

    GetCustomer(companyId);

    $("#company-textfield").val(company);
}


//get company using ajax post
function getCompany() {
    $.get("/Customers/GetCustomerCompanyOrDefault",
        {
            id: $("#CustomerId option:selected").val()
        },
        function (data, status) {
            //console.log("CompanyId: " + data);
            $('#CompanyId').val(data);
            $('#company-textfield').val($("#CompanyId option:selected").text());
        });
}


//get company using ajax post
function GetCustomer(companyId) {
    $.get("/Customers/GetCustomerByCompanyId",
        {
            id: companyId
        },
        function (data, status) {
            //console.log(data);
            //$('#CompanyId').val(data);
            //$('#company-textfield').val($("#CompanyId option:selected").text());
        }).done((customer) => {
            //console.log(customer);

            $("#CompanyId").val(companyId);
            $("#CustomerId").val(customer["Id"]);
            $("#CustName").val(customer["Name"]);
            $("#CustEmail").val(customer["Email"]);
            $("#CustPhone").val(customer["Contact2"]);

        }).fail(() => {
            alert("Unable to get Customer Details")
        });
}

//get email using ajax post
function getEmail() {
    $.post("/JobOrder/getCustomerEmail",
        {
            id: $("#CustomerId option:selected").val()
        },
        function (data, status) {
            $("#CustEmail").val(data);
            //  alert("Data: " + data + "\nStatus: " + status);
        });
}

//get number using ajax post
function getNumber() {
    $.post("/JobOrder/getCustomerNumber",
        {
            id: $("#CustomerId option:selected").val()
        },
        function (data, status) {
            $("#CustPhone").val(data);
        });
}


//Change job desription bsed on the cutomer name
$("#CustomerId").change(function () {
    var customer = $("#CustomerId option:selected").text();
    //console.log("customer : " + customer);
    //$("#jobdesc").val(customer);
    getEmail();
    getNumber();
    getCompany();

    $("#CustName").val(customer);
});

//on select of the customer
//$('#CustomerId').on('change', function () {
//    var custId = $('#CustomerId').val();
//    console.log('customerId: ' + custId);
//    ajax_loadContent(custId);
//});

//load table content on search btn click
//request data from server using ajax call
//then clear and add contents to the table
function ajax_loadContent(custId) {
    //build json object
    var data = {
        custId: custId
    };

    //request data from server using ajax call
    $.ajax({
        url: 'getCompanyId',
        type: "GET",
        data: data,
        dataType: 'application/json; charset=utf-8',
        success: function (data) {
            //console.log("SUCCESS");
            //console.log(data);
        },
        error: function (data) {
            //console.log("ERROR");
            //console.log(data);
            var comId = parseInt(data['responseText']);
            //console.log(comId);
            $('#CompanyId').val(comId);
        }
    });
}


//Check sales Code for duplicates
function CheckSalesCode() {
    var salesCode = $("#SalesCode").val();

    $.ajax({
        url: '/SalesLeads/CheckDuplicateSalesCode',
        type: "GET",
        data: {
            salesCode: salesCode
        },
        dataType: 'application/json; charset=utf-8',
        error: function (res) {
            //console.log("ERROR");
            console.log(res);

            if (res['responseText'] == "True") {
                console.log("Sales Code already exists. ");
                $("#salesCode-validation-msg").text("Sales Code already exists.")
                $("#Create-Btn").attr("disabled", true);
            } else {
                //clear message
                $("#salesCode-validation-msg").text("")
                $(":submit").removeAttr("disabled");
            }
        }
    });
}

//check input on SalesCode 
$('#SalesCode').focusout(function () {
    CheckSalesCode();
})

//check input on SalesCode 
$('#SalesCode').keyup(function () {
    CheckSalesCode();
})

function ShowCustomerSearchModal() {
    $("#CustomersModal").modal('show');
}

function ShowCompanySearchModal() {
    $("#CompanyModal").modal('show');
}
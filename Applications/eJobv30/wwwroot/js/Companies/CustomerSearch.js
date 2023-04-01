/**
 *  Job Create / Edit
 *  
 */ 

//Change job desription bsed on the cutomer name
$("#ac-custId").change(function () {
    var customer = $("#ac-custId option:selected").text();
    //console.log("customer : " + customer);
    //$("#jobdesc").val(customer);
    getEmail();
    getNumber();
    getCompany();
});

//get email using ajax post
function getEmail() {
    $.post("/JobOrder/getCustomerEmail",
        {
            id: $("#ac-custId option:selected").val()
        },
        function (data, status) {
            $("#CustContactEmail").val(data);
            //  alert("Data: " + data + "\nStatus: " + status);
        });
}

//get number using ajax post
function getNumber() {
    $.post("/JobOrder/getCustomerNumber",
        {
            id: $("#ac-custId option:selected").val()
        },
        function (data, status) {
            $("#CustContactNumber").val(data);
        });
}

//get company using ajax post
function getCompany() {
    $.post("/JobOrder/getCustomerCompany",
        {
            id: $("#ac-custId option:selected").val()
        },
        function (data, status) {
            console.log(data);
            $('#CompanyId').val(data);
            $('#company-textfield').val($("#CompanyId option:selected").text());
        });
}

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

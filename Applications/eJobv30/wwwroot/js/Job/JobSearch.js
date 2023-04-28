/**
 *  Job Create / Edit
 *  Job Search Company
 */

//Change job desription bsed on the cutomer name

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

function SelectCompany(companyId) {
    //console.log(value);
    $('#CompanyId').val(companyId);
    $('#company-textfield').val($("#CompanyId option:selected").text());

    //GetDefaultContact(companyId);

    //$('#customer-textfield').val($("#CompanyId option:selected").text());
    $('#CompanySearchModal').modal('hide');
}


//get company using ajax post
function GetDefaultContact(companyId) {
    $.get("/CustEntMains/GetDefaultContact",
        { id: companyId }, (data, status) => {
            console.log(data);
            $('#customerList').val(data);
            $('#customer-textfield').val($("#customerList option:selected").text());
        });
}

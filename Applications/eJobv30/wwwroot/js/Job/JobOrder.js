
/**
 *  Job Order 
 *  JobOrder/Index
 *  
 **/

function LoadOverlay() {
    $("#overlay2").css("display", "flex");
}


function disableLoadOverlay() {
    $("#overlay2").css("display", "none");
}

function InitialFilter(filter) {
    $("#filter-group").children("a").removeClass("active");
    switch (filter) {
        case 1:
            $("#ongoing").css("color", "black");
            $("#ongoing-bot").css("color", "black");

            $("#ongoing").addClass("active");
            break;
        case 2:
            $("#previous").css("color", "black");
            $("#previous-bot").css("color", "black");

            $("#previous").addClass("active");
            break;
        case 3:
            $("#closed").css("color", "black");
            $("#closed-bot").css("color", "black");

            $("#closed").addClass("active");
            break;
        case 4:
            $("#cancelled").css("color", "black");
            $("#cancelled-bot").css("color", "black");

            $("#cancelled").addClass("active");
            break;
        default:

            break;
    }
}



function updateval() {
    var iDiv = $('#categorylist').val();

    // Now create and append to iDiv
    var innerDiv = document.createElement('p');
    innerDiv.className = 'list-group-item';

    // The variable iDiv is still good... Just append to it.
    iDiv.appendChild(innerDiv);
}


function ajaxSendMail(jobid) {
    var data = {
        jobId: jobid,
        mailType: "CLIENT-INVOICE-SEND"
    }

    var serviceURL = '/JobOrder/SendEmail';

    //loading screen
    LoadOverlay()

    //send email using ajax
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function () {
            console.log('success');
            disableLoadOverlay();
        },
        error: function (jqXHR, textStatus, errorThrown) {

            alert(jqXHR.responseText);
            disableLoadOverlay();
        }
    });
}

/*Payments modal
 *Show Payment modal and retrieve the job payments history
 *and show actions with payments.
 */
function showPaymentModal(jobId) {
    $('#jobPayment').modal('show');
    getPaymentData(jobId);
}

/**
* Payment_Ajax
* Get Payments transactions of the job
* using the jobId
*/
function getPaymentData(jobId) {
    var data = {
        jobId: jobId,
    }

    var serviceURL = '/JobOrder/GetPaymentHistory';


    //send email using ajax
    $.ajax({
        type: "POST",
        url: serviceURL,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            jobPayments(data, jobId)
            //console.log('success');
            //console.log(data);
            //disableLoadOverlay();
        },
        error: function (jqXHR) {

            alert(jqXHR.responseText);
            //disableLoadOverlay();
        }
    });
}



function jobPayments(data, jobId) {

    if (data["length"] != 0) {

        console.log(data["0"]["Amount"]);
        //parse data response to json object
        var temp = data;

        //clear table contents except header
        $("#paymentList").find("tr:gt(0)").remove();

        //populate table content
        for (var x = 0; x < temp.length; x++) {
            var content = "<tr>";
            content += "<td>" + moment(temp[x]["DtPayment"]).format("MMM DD YYYY h:m A") + "</td>";
            content += "<td>" + temp[x]["Type"] + "</td>";
            content += "<td>" + temp[x]["Amount"] + "</td>";

            content += "</tr>";

            console.log();
            $(content).appendTo("#paymentList");
        }
    }


    //remove all link from div actions
    $("#paymentActions").empty();

    var contentAction = "<a href='/JobPayments/Payments/" + jobId + "' class='list-group-item'>  Payment Transaction</a>";
    contentAction += "<a href='/JobPayments/Create?JobMainId=" + jobId + "&remarks=Partial Payment' class='list-group-item'> Partial Payment </a>";
    contentAction += "<a href='/JobPayments/Create?JobMainId=" + jobId + "&remarks=Full Payment' class='list-group-item'> Full Payment </a>";
    contentAction += "<a href='/JobPayments/CreatePG?JobMainId=" + jobId + "&remarks=Personal Guarentee' class='list-group-item'> Personal Guarantee </a>";

    //<a href="Url.Action("PaymentCreate", "JobOrder", new { JobMainId = item.Main.Id, remarks = "Partial Payment" }, null)" class="list-group-item"> Partial Payment</a>
    //<a href="Url.Action("PaymentCreate", "JobOrder", new { JobMainId = item.Main.Id, remarks = "Full Payment" }, null)" class="list-group-item"> Full Payment</a>
    //<a href="Url.Action("PaymentCreatePG", "JobOrder", new { JobMainId = item.Main.Id, remarks = "Personal Guarentee" }, null)" class="list-group-item"> Personal Guarantee</a>
    $(contentAction).appendTo("#paymentActions");
}

function ajaxSearch() {
    var data = {
        search: $("#src-field").val()
    }
    console.log("src: " + $("#src-field").val());
    var serviceURL = '/JobOrder';
    var searchString = $("#src-field").val();

    window.location.href = '/JobOrder/JobServices?JobMainId=' + searchString;

    //window.location.href = '/JobOrder?search='+searchString;
}



function initialize() {
    $(".clickitem").click(fClick_item);
}

function fClick_item() {
    var svcid = $(this).attr("data_svcid");
    var actionid = $(this).attr("id");

    //alert(".clickitem - svcid : " + svcid + " : actionid : " + actionid);
    // var r = confirm("Mark '" + $(this).attr("Name") + "' as done?");

    // if (r) {
    fAjax_updateAction(svcid, actionid);
    //  }

    $(this).children("span").removeClass("btn-primary");
    $(this).children("span").addClass("btn-success");

    $(this).css("color", "green");
    event.preventDefault();
}

//AJAX CALLS
function fAjax_updateAction(svcid, actionid) {
    var serviceURL = '/JobOrder/MarkDone';
    // alert("fAjax_updateAction - svcid : " + svcid + " : actionid : " + actionid);

    $.ajax({
        type: "GET",
        url: serviceURL,
        data: { SvcId: svcid, ActionId: actionid },
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: successFunc(svcid, actionid),
        error: errorFunc()
    });

}

function successFunc(svcid, actionid) {
    // alert("Successfully Done!");
    // $("#" + actionid).hide();
    $("#" + actionid).css("color", "green");
    // $("#" + actionid).css("background-color", "gray");

    //location.reload();
}

function errorFunc() {
    //alert('error');
}
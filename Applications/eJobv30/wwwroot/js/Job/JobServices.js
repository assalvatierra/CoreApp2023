/*  
 *  JobServices 
 *  Send Quotation, Send reservation, send invoice
 *  load sending overlay animation / gif
 */

var ADMINPASS = "Admin123!"

function jobActionDisplay(action) {
    alert(action);
}

//SEND Quotation to client in email
function ajaxSendQuotation(jobId) {
    var data = {
        jobId: jobId,
        docType: "QUOTATION"
    }

    console.log(jobId);
    var serviceURL = '/JobOrder/SendEmailBooking';
    console.log("sending email");

    //loading screen
    LoadOverlay();

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
            console.log("email sent");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            $("#overlay2-text").text(jqXHR.responseText);
            disableLoadOverlay();
            console.log("Email Sent");
        }
    });
}

//SEND RESERVATION DETAILS IN EMAIL TO CLIENT
function ajaxSendReservation(jobId) {
    var data = {
        jobId: jobId,
        docType: "RESERVATION"
    }

    console.log(jobId);
    var serviceURL = '/JobOrder/SendEmailBooking';
    console.log("sending email");

    //loading screen
    LoadOverlay();

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
            console.log("email sent");
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(jqXHR.responseText);
            $("#overlay2-text").text(jqXHR.responseText);
            disableLoadOverlay();
            console.log("Email Sent");
        }
    });
}

function LoadOverlay() {
    console.log("load overlay");
    $("#overlay2").css("display", "flex");
}

function disableLoadOverlay() {
    console.log("disable overlay");
    $("#overlay2").css("display", "none");
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




//Set Payment Id for edit
function SetEditJobPermissionId(id) {
    $("#EditJob-Pass-warning").hide();
    $("#EditJob-Pass-Id").val(id);
    $("#EditJob-Pass-Input").val("");
}

//Check admin pass to edit
function CheckEditJobPermission() {
    //reset
    $("#EditJob-Pass-warning").hide();

    var passInput = $("#EditJob-Pass-Input").val();
    console.log(passInput);

    //check permission
    var res = $.get("/JobOrder/CheckAdminPermission", { pass: passInput }, (result) => {
        console.log(result);
        if (result) {
            var paymentId = $("#EditJob-Pass-Id").val();
            //redirect to Edit
            window.location.href = "/JobOrder/JobDetails?jobid=" + paymentId;
        } else {
            $("#EditJob-Pass-warning").show();
        }
    })
    console.log(res);
    //if ($("#EditJob-Pass-Input").val() == ADMINPASS) {
    //    var paymentId = $("#EditJob-Pass-Id").val();
    //    //redirect to Edit
    //    window.location.href = "/JobOrder/JobDetails?jobid=" + paymentId;
    //} else {
    //    $("#EditJob-Pass-warning").show();
    //}
}




//Set Payment Id for edit
function SetEditPermissionId(id) {
    $("#Edit-Pass-Id").val(id);
    $("#Edit-Pass-Input").val("");
    $("#Edit-Pass-warning").hide();
    //$("#Edit-Pass-Input").focus();
}

//Check admin pass to edit
function CheckEditPermission() {
    //reset
    $("#Edit-Pass-warning").hide();

    //check permission
    //$.get("/JobOrder/CheckAdminPermission", { pass: $("#Edit-Pass-Input").val() }, (result) => {
    //    console.log(result);
    //    if (result == 'True') {
    //        var paymentId = $("#Edit-Pass-Id").val();
    //        //redirect to Edit
    //        window.location.href = "/JobOrder/JobServiceEdit/" + paymentId;
    //    } else {
    //        $("#Edit-Pass-warning").show();
    //    }
    //})

    if ($("#Edit-Pass-Input").val() == ADMINPASS) {
        var paymentId = $("#Edit-Pass-Id").val();
        //redirect to Edit
        window.location.href = "/JobOrder/JobServiceEdit/" + paymentId;
    } else {
        $("#Edit-Pass-warning").show();
    }

}


//Set Payment Id for delete
function SetDeletePermissionId(id) {
    $("#Delete-Pass-Id").val(id);
    $("#Delete-Pass-Input").val("");
    $("#Delete-Pass-warning").hide();
    //$("#Delete-Pass-Input").focus();
}

//Check admin pass to delete
function CheckDeletePermission() {
    //reset
    $("#Delete-Pass-warning").hide();

    //check permission
    //$.get("/JobOrder/CheckAdminPermission", { pass: $("#Delete-Pass-Input").val() }, (result) => {
    //    console.log(result);
    //    if (result == 'True') {

    //        if (confirm("Do you want to delete this service?")) {

    //            var svcId = $("#Delete-Pass-Id").val();

    //            $.post("/JobOrder/ConfirmJobSvcDelete", { id : svcId }, (resDelete) => {
    //                if (resDelete == 'True') {
    //                    //redirect to Edit
    //                    window.location.href = "/JobOrder/JobServices?JobMainId=" + svcId;
    //                } else {
    //                    $("#Delete-Pass-warning").show();
    //                }
    //            });
    //        }


    //    } else {
    //        $("#Delete-Pass-warning").show();
    //    }

    //})

    if ($("#Delete-Pass-Input").val() == ADMINPASS) {
        if (confirm("Do you want to delete this service?")) {

            var svcId = $("#Delete-Pass-Id").val();

            $.post("/JobOrder/ConfirmJobSvcDelete", { id: svcId }, (resDelete) => {
                if (resDelete == 'True') {
                    //redirect to Edit
                    window.location.href = "/JobOrder/JobServices?JobMainId=" + svcId;
                } else {
                    $("#Delete-Pass-warning").show();
                }
            });
        }
    } else {
        $("#Delete-Pass-warning").show();
    }
}




//Check admin pass to delete
function DeleteConfirmation(id) {

    //check permission
    if (confirm("Do you want to delete this service?")) {

        var svcId = parseInt(id);

        $.post("/JobOrder/ConfirmJobSvcDelete", { id: svcId }, (resDelete) => {
            if (resDelete == 'True') {
                //redirect to Edit
                window.location.href = "/JobOrder/JobServices?JobMainId=" + svcId;
            } else {
                $("#Delete-Pass-warning").show();
            }
        });
    }
}


//On Enter submit
$('#EditJob-Pass-Input').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        $('#EditJob-Proceed').click();
        return false;
    }
});

//On Enter submit
$('#Edit-Pass-Input').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        $('#Edit-Proceed').click();
        return false;
    }
});


//On Enter submit
$('#Delete-Pass-Input').keypress(function (e) {
    var key = e.which;
    if (key == 13)  // the enter key code
    {
        $('#Delete-Proceed').click();
        return false;
    }
});


function ResetWarning() {
    $("#Edit-Pass-warning").hide();
    $("#EditJob-Pass-warning").hide();
    $("#Delete-Pass-warning").hide();
}


//Close and Print Job 
function CloseAndPrintService(Id) {
    $.post("/JobOrder/AjaxCloseJobStatus", { id: Id }, (res) => {
        //console.log(res);
        if (res == 'True') {

            var url = $("#ServiceBillingUrl").val();
            url = url.replace('_id', Id);

            //open service billing
            //window.open(url);


            //reload current page
            window.location.href = url;
        } else {
            alert('Unable to close the job. Please try editing the job instead.');
        }
    });

    //console.log(response);

}
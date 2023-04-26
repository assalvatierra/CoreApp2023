
/**
 *  Add Vehicle to the Job
 */
function AddJobVehicle(jobMainId) {

    var vehicleId = parseInt($('#addVehicle-Options option:selected').val());
    var input_mileage = parseInt($("#addVehicle-Mileage").val());

    if (isNaN(input_mileage)) {
        alert("Milieage is not a number. Please enter a valid mileage.");
        $("#addVehicle-Mileage").val(0);
    } else {
        //get previous milieage
        $.get("/JobOrder/GetVehiclePrevOdo", { vehicleId: vehicleId }, (resOdo) => {

            console.log("PrevOdo : " + resOdo);

            var prev_milieage = resOdo;

            var condition = (input_mileage < prev_milieage);

            //console.log("input: " + input_mileage);
            //console.log("prev: " + prev_milieage);
            //console.log(condition);

            //check input mileage from previous mileage
            if (input_mileage < prev_milieage) {

                $("#input-mileage").text(input_mileage);
                $("#prev-mileage").text(prev_milieage);

                $("#AddVehicleConfirmModal").modal('show');
                $("#AddVehicleModal").modal("hide");

            } else {
                submit_VehicleAndMileage(jobMainId);
            }
        })

    }
}

function submit_VehicleAndMileage(jobMainId) {
    var data = {
        jobMainId: jobMainId,
        vehicleId: parseInt($('#addVehicle-Options option:selected').val()),
        mileage: parseInt($("#addVehicle-Mileage").val())
    };

    var response = $.post("/JobOrder/AddJobVehicle", data, (result) => {
        if (result == 'True') {
            //close modal
            $("#AddVehicleModal").modal('hide');
            $("#AddVehicleConfirmModal").modal('hide');

            //add vehicle to view
            let selectedVehicle = "<br /> Description: " + $('#addVehicle-Options option:selected').text() + " Mileage: " + parseInt($("#addVehicle-Mileage").val());
            $("#jobVehicleText").text("");
            $("#jobVehicleText").append(selectedVehicle);
        } else {
            alert('Error! Unable to add vehicle.');
        }
    });

    //console.log(response);
}

function ShowAddVehicleModal() {

    $("#AddVehicleModal").modal("show");
}

async function GetVehiclePrevMileage() {

    var vehicleId = parseInt($('#addVehicle-Options option:selected').val());
    $.get("/JobOrder/GetVehiclePrevOdo", { vehicleId: vehicleId }, (resOdo) => {
        //console.log("PrevOdo : " + resOdo);
        return Promise.resolve(resOdo);
    })
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function searchVehicle() {
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

function SetVehicle(value) {
    $('#addVehicle-Options').val(value);
    $('#SearchVehicleModal').modal('hide');

}

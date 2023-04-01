
/*  Company Vehicles
 *  Create / Edit / Delete Vehicles for Customers under the Company
 *  path: Customer/Details
 *  7/1/2020
 **/ 

//Add Vehicle
function ShowAddVehicleModal(companyId) {
    Clear_CreateVehicle();
    $("#addVehicle-CompanyId").val(companyId);
}

//Submit Add Vehicle
function Submit_AddVehicle() {
    var vehicle = {
        vehicleModelId: $("#addVehicle-Model :selected").val(),
        yearModel: $("#addVehicle-YearModel").val(),
        plateNo: $("#addVehicle-PlateNo").val(),
        conduction: $("#addVehicle-Conduction").val(),
        engineNo: $("#addVehicle-EngineNo").val(),
        chassisNo: $("#addVehicle-ChassisNo").val(),
        color: $("#addVehicle-Color").val(),
        customerId: $("#addVehicle-ContactId :selected").val(),
        custEntMainId: parseInt($("#addVehicle-CompanyId").val()),
        remarks: $("#addVehicle-Remarks").val(),
    };
    //console.log(vehicle);
    //console.log("validation: " + Create_ValidateVehicleInput());

    if (Create_ValidateVehicleInput()) {

        var res = $.post("/CustEntMains/AddCustomerVehicle", vehicle, (result) => {
            //console.log(result);
            if (result == 'True') {
                $("#AddVehicleModal").modal('hide');
                window.location.reload();
            } else {
                alert("Error! Unable to add vehicle. ");
            }
        });

        //console.log(res);
    }
}

//Edit Vehicle

function ShowEditVehicleModal(id) {
    Clear_EditVehicle();
    $("#editVehicle-CompanyId").val(id);
    EditVehicle_GetDetails(id);
}

//Get Vehicle Details
function EditVehicle_GetDetails(id) {
    $.get("/CustEntMains/GetVehicleDetails", { id: id }, function (data, status) {
        $("#editVehicle-VehicleId").val(data[0]['Id']);
        $("#editVehicle-Model").val(data[0]['VehicleModelId']);
        $("#editVehicle-YearModel").val(data[0]['YearModel']);
        $("#editVehicle-PlateNo").val(data[0]['PlateNo']);
        $("#editVehicle-Conduction").val(data[0]['Conduction']);
        $("#editVehicle-EngineNo").val(data[0]['EngineNo']);
        $("#editVehicle-ChassisNo").val(data[0]['ChassisNo']);
        $("#editVehicle-Color").val(data[0]['Color']);
        $("#editVehicle-CompanyId").val(data[0]['CustEntMainId']);
        $("#editVehicle-ContactId").val(data[0]['CustomerId']);
        $("#editVehicle-Remarks").val(data[0]['Remarks']);;
    });
}

//save Edit Change
function Submit_EditVehicle() {
    var vehicle = {
        Id: parseInt($("#editVehicle-VehicleId").val()),
        vehicleModelId: parseInt($("#editVehicle-Model :selected").val()),
        yearModel: $("#editVehicle-YearModel").val(),
        plateNo: $("#editVehicle-PlateNo").val(),
        conduction: $("#editVehicle-Conduction").val(),
        engineNo: $("#editVehicle-EngineNo").val(),
        chassisNo: $("#editVehicle-ChassisNo").val(),
        color: $("#editVehicle-Color").val(),
        customerId: parseInt($("#editVehicle-ContactId :selected").val()),
        custEntMainId: parseInt($("#editVehicle-CompanyId").val()),
        remarks: $("#editVehicle-Remarks").val(),
    };

    //console.log(vehicle);
    if (Edit_ValidateVehicleInput()) {

        var res = $.post("/CustEntMains/EditCustomerVehicle", vehicle, (result) => {
            //console.log(result);
            if (result == 'True') {
                $("#EditVehicleModal").modal('hide');
                window.location.reload();
            } else {
                alert("Error! Unable to edit vehicle to customer. ");
            }
        });
        //console.log(res);
    }
}

//delete Vehicle
function ShowDeleteVehicleModal(id) {
    DeleteVehicle_GetDetails(id);
}

function DeleteVehicle_GetDetails(id) {
    $.get("/CustEntMains/GetVehicleDetails", { id: id }, function (data, status) {
        //console.log(data);
        $("#deleteVehicle-Id").text(data[0]['Id']);
        $("#deleteVehicle-Model").text(data[0]['Brand'] + ' ' + data[0]['Make'] + ' ' + data[0]['Type']);
        $("#deleteVehicle-YearModel").text(data[0]['YearModel']);
        $("#deleteVehicle-PlateNo").text(data[0]['PlateNo']);
        $("#deleteVehicle-Conduction").text(data[0]['Conduction']);
        $("#deleteVehicle-EngineNo").text(data[0]['EngineNo']);
        $("#deleteVehicle-ChassisNo").text(data[0]['ChassisNo']);
        $("#deleteVehicle-Color").text(data[0]['Color']);
        $("#deleteVehicle-CompanyId").text(data[0]['CustEntMainId']);
        $("#deleteVehicle-Customer").text(data[0]['Name']);
        $("#deleteVehicle-Remarks").text(data[0]['Remarks']);
    });
}


//submit Delete
function Submit_DeleteVehicle() {
    var vehicle = {
        Id: parseInt($("#deleteVehicle-Id").text())
    };

    var res = $.post("/CustEntMains/DeleteCustomerVehicle", vehicle, (result) => {
        //console.log(result);
        if (result == 'True') {
            $("#DeleteVehicleModal").modal('hide');
            window.location.reload();
        } else {
            alert("Error! Unable to delete vehicle. This vehicle is used in a job. ");
        }
    });

}


function Create_ValidateVehicleInput() {
    var yearModel = $("#addVehicle-YearModel").val();
    var plateNo = $("#addVehicle-PlateNo").val();

    var isValid = true;

    $("#validate-createVehicle").hide();

    if (plateNo == "") {
        $("#validate-createVehicle").text("Invalid Vehicle Plate No");
        $("#validate-createVehicle").show();
        isValid = false;
    }

    if (yearModel == "") {
        $("#validate-createVehicle").text("Invalid Vehicle Year Model");
        $("#validate-createVehicle").show();
        isValid = false;
    }

    if (yearModel == "" && plateNo == "") {
        $("#validate-createVehicle").text("Invalid Vehicle Year Model and Plate No");
        $("#validate-createVehicle").show();
        isValid = false;
    }


    return isValid;
}

function Edit_ValidateVehicleInput() {
    var yearModel = $("#editVehicle-YearModel").val();
    var plateNo = $("#editVehicle-PlateNo").val();

    var isValid = true;

    $("#validate-editVehicle").hide();

    if (plateNo == "") {
        $("#validate-editVehicle").text("Invalid Vehicle Plate No");
        $("#validate-editVehicle").show();
        isValid = false;
    }

    if (yearModel == "") {
        $("#validate-editVehicle").text("Invalid Vehicle Year Model");
        $("#validate-editVehicle").show();
        isValid = false;
    }

    if (yearModel == "" && plateNo == "") {
        $("#validate-editVehicle").text("Invalid Vehicle Year Model and Plate No");
        $("#validate-editVehicle").show();
        isValid = false;
    }

    return isValid;
}


function Clear_CreateVehicle() {
    $("#addVehicle-Model").val(1);
    $("#addVehicle-YearModel").val("");
    $("#addVehicle-PlateNo").val("");
    $("#addVehicle-Conduction").val("");
    $("#addVehicle-EngineNo").val("");
    $("#addVehicle-ChassisNo").val("");
    $("#addVehicle-Color").val("");
    $("#addVehicle-Remarks").val("");
}

function Clear_EditVehicle() {
    $("#editVehicle-YearModel").val("");
    $("#editVehicle-PlateNo").val("");
    $("#editVehicle-Conduction").val("");
    $("#editVehicle-EngineNo").val("");
    $("#editVehicle-ChassisNo").val("");
    $("#editVehicle-Color").val("");
    $("#editVehicle-Remarks").val("");
}
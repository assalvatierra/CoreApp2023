/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */


$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker()
{
    //-------------Date Start------------//
    var ddd1 = $('input[name="Date"]').val();

    $('input[name="Date"]').daterangepicker(
    {
        timePicker: true,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY hh:mm A'
        }
    },
    function (start, end, label) {
        //check if date is greater than or equal to today
        
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
        //alert(today > datepicker);

            if (today > datepicker) {
                alert("Job date is past the date today. Do you want to continue?");

            }

        //alert(start.format('YYYY-MM-DD'));
        
        }
    );

    $('input[name="Date"]').val(ddd1);
    //$('input[name="JobDate"]').val(ddd1.substr(0, ddd1.indexOf(" ") ));

}


//Amount - Number Only
//$('input[name="Amount"]').inputFilter(function (evt) {
//        var charCode = (evt.which) ? evt.which : evt.keyCode
//        return !(charCode > 31 && (charCode < 48 || charCode > 57));
//    });


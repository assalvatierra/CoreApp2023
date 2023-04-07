/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */


$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    var ddd1 = $('input[name="DtActivity"]').val();

    $('input[name="DtActivity"]').daterangepicker(
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

        var today = moment().format('YYYY-MM-DD hh:mm A');
        var datepicker = start.format('YYYY-MM-DD hh:mm A');
        //alert(today > datepicker);

        //if (today > datepicker) {
        //    alert("Job date is past the date today. Do you want to continue?");

        //}

        //alert(start.format('YYYY-MM-DD'));

    }
    );

    $('input[name="DtActivity"]').val(ddd1);
    //$('input[name="JobDate"]').val(ddd1.substr(0, ddd1.indexOf(" ") ));

}

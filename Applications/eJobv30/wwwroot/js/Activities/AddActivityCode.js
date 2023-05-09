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
    var ddd1 = $('input[name="DtActivity"]').val();

    $('input[name="DtActivity"]').daterangepicker(
    {
        timePicker: false,
        timePickerIncrement: 1,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'MM/DD/YYYY'
        }
    },
        function (start, end, label) {
        }
        );

    $('input[name="DtActivity"]').val(ddd1);

    //$('input[name="JobDate"]').val(ddd1.substr(0, ddd1.indexOf(" ") ));

}

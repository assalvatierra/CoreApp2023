
/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */

$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    //Date 1
    var ddd1 = $('input[name="DtStart"]').val();

    $('input[name="DtStart"]').daterangepicker(
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
            // alert(start.format('YYYY-MM-DD h:mm A'));

        }
    );

    //$('input[name="DtStart"]').val(ddd1.substr(0, ddd1.indexOf(" ")));

    //Date 2
    var ddd2 = $('input[name="DtEnd"]').val();

    $('input[name="DtEnd"]').daterangepicker(
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
            // alert(start.format('YYYY-MM-DD h:mm A'));

        }
    );

    //Date Date
    var ddd3 = $('input[name="Date"]').val();

    $('input[name="Date"]').daterangepicker(
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
            // alert(start.format('YYYY-MM-DD h:mm A'));

        }
    );

    $('input[name="Date"]').val(ddd3);

    //Date DtActivity
    var ddd4 = $('input[name="DtActivity"]').val();

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
            // alert(start.format('YYYY-MM-DD h:mm A'));

        }
    );


    //Date .datePicker
    var ddd5 = $('.datePicker').val();

    $('.datePicker').daterangepicker(
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


    $('.datePicker').val(moment(ddd5).format("MM/DD/YYYY"));

    //$('.datePicker').daterangepicker();
}



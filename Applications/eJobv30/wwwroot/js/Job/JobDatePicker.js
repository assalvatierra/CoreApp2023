/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */


$(document).ready(function () {
    InitDatePicker();
})


function InitDatePicker() {
    var ddd1 = $('input[name="JobDate"]').val();

    $('input[name="JobDate"]').daterangepicker(
        {
            timePicker: true,
            timePickerIncrement: 1,
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'MM/DD/YYYY'
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

    $('input[name="JobDate"]').val(ddd1);
    //$('input[name="JobDate"]').val(ddd1.substr(0, ddd1.indexOf(" ") ));

}

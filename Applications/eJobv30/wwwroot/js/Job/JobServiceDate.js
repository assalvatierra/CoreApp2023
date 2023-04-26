/* ********************************************************
* By Abel S. Salvatierra
* @2017 - Real Breeze Travel & Tours
* 
*********************************************************** */

$(document).ready(function () {
    InitDatePicker();
    initFieldEvents();
})


function InitDatePicker() {
    var ddd1 = $('input[name="DtStart"]').val();
    var ddd2 = $('input[name="DtEnd"]').val();

    $('input[name="DtStart"]').daterangepicker(
        {
            timePicker: false,
            timePickerIncrement: 10,
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'MM/DD/YYYY'
            }
        },

        function (start, end, label) {
            //alert(start.format('YYYY-MM-DD h:mm A'));
            //check if date is greater than or equal to today
            ddd2 = $('input[name="DtEnd"]').val();
            var today = moment().format('YYYY-MM-DD');
            var datepicker = start.format('YYYY-MM-DD');
            var dateEnd = moment(ddd2).format('YYYY-MM-DD');
            //alert(today > datepicker);

            if (today > datepicker) {
                alert("Service Start date is less than the date today. Are you sure?");
            }

            //check if end date is greater than start date
            if (datepicker > dateEnd) {
                alert("JobService start date is greater than the JobService end date. Are you sure?");
            }

        }
    );


    $('input[name="DtEnd"]').daterangepicker(
        {
            timePicker: false,
            timePickerIncrement: 10,
            singleDatePicker: true,
            showDropdowns: true,
            locale: {
                format: 'MM/DD/YYYY'
            }
        },
        function (start, end, label) {
            //alert(start.format('YYYY-MM-DD h:mm A'));
            //check if date is greater than or equal to today
            ddd1 = $('input[name="DtEnd"]').val();
            var today = moment().format('YYYY-MM-DD');
            var datepicker = start.format('YYYY-MM-DD');
            var dateStart = moment(ddd1).format('YYYY-MM-DD');
            //alert(today > datepicker);

            if (today > datepicker) {
                alert("Service End date is less than the date today. Are you sure?");

            }

            //check if end date is greater than start date
            if (datepicker < dateStart) {
                alert("JobService end date is less than the JobService start date. Are you sure?");
            }

        }
    );


    var SDate = $('input[name="DtStart"]').val();
    var EDate = $('input[name="DtEnd"]').val();

    $('input[name="DtStart"]').val(moment(SDate).format("MM/DD/YYYY"));
    $('input[name="DtEnd"]').val(moment(EDate).format("MM/DD/YYYY"));


}

function initFieldEvents() {
    $('#QuotedAmt').on('input', function () {
        var sTmp = $('#QuotedAmt').val();
        $('#SupplierAmt').val(sTmp);
        $('#ActualAmt').val(sTmp);
    });

    $('#SupplierAmt').on('input', function () {
        var sTmp = $('#SupplierAmt').val();
        $('#ActualAmt').val(sTmp);
    });


}

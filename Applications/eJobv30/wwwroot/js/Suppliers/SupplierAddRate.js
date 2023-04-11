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
    //------------- Add Start Date ------------------//
    var ddd1 = $('#InvRate-ValidFrom').val();

    $('#InvRate-ValidFrom').daterangepicker(
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
        //check if date is greater than or equal to today
        
        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');
        //alert(today > datepicker);

            if (today > datepicker) {
                alert("Job date is past the date today. Do you want to continue?");
            }
        }
    );

    $('#InvRate-ValidFrom').val(ddd1);

    //------------- Add End Date ------------------//

    var ddd2 = $('#InvRate-ValidTo').val();

    $('#InvRate-ValidTo').daterangepicker(
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
        //check if date is greater than or equal to today

        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');

        if (today > datepicker) {
            alert("Job date is past the date today. Do you want to continue?");
        }
    });

    $('#InvRate-ValidTo').val(ddd2);

    //------------- Edit Start Date ------------------//

    //Start Date
    var ddd3 = $('#EditInvRate-ValidFrom').val();

    $('#EditInvRate-ValidFrom').daterangepicker(
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
        //check if date is greater than or equal to today

        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');

        if (today > datepicker) {
            //alert("Job date is past the date today. Do you want to continue?");
        }
    });

    $('#EditInvRate-ValidFrom').val(ddd3);

    //------------- Edit End Date ------------------//

    //End Date
    var ddd4 = $('#EditInvRate-ValidTo').val();

    $('#EditInvRate-ValidTo').daterangepicker(
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
        //check if date is greater than or equal to today

        var today = moment().format('YYYY-MM-DD');
        var datepicker = start.format('YYYY-MM-DD');

        if (today > datepicker) {
            //alert("Job date is past the date today. Do you want to continue?");
        }
    });

    $('#EditInvRate-ValidTo').val(ddd4);

}


/* Job Services Add
 *  Filter Input Amount
 */

//Accept only positive decimal for amount
$(function () {
    $("#js-Amount").bind("change keyup input", function () {
        var position = this.selectionStart - 1;
        //remove all but number and .
        var fixed = this.value.replace(/[^0-9\.]/g, '');
        if (fixed.charAt(0) === '.')                  //can't start with .
            fixed = fixed.slice(1);

        var pos = fixed.indexOf(".") + 1;
        if (pos >= 0)               //avoid more than one .
            fixed = fixed.substr(0, pos) + fixed.slice(pos).replace('.', '');

        if (this.value !== fixed) {
            this.value = fixed;
            this.selectionStart = position;
            this.selectionEnd = position;
        }
    });

    //$("#js-Amount").bind("focus blur", function () {
    //    $("#js-Amount").select();
    //});
});


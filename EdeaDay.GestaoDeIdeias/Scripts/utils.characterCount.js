function countChars(textbox, counter) {
    var count = $(textbox).attr('data-val-length-max') - ($(textbox).val() == undefined ? 0 : $(textbox).val().length);
    if (count < 0) { $(counter).html("<span style=\"color: red;\">" + count + "</span>"); }
    else { $(counter).html(count); }
}

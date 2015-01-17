$(document).ready(function(){
  $("#parentTextBox").on('keydown', '#textBox', function(e) {
    var keyCode = e.keyCode || e.which;
    //console.log('keypress.. ' + keyCode);
    if (keyCode == 9) {
      e.preventDefault();
      var caretPos = document.getElementById("textBox").selectionStart;
      var textAreaTxt = $('#textBox').val();
      $('#textBox').val(textAreaTxt.substring(0, caretPos) + "  " + textAreaTxt.substring(caretPos) );
      document.getElementById("textBox").selectionStart = caretPos+2;
      document.getElementById("textBox").selectionEnd =  caretPos+2;
    }
  });

  $("#shareUrl").on('click', function(e){
    this.setSelectionRange(0, this.value.length);

  })
});

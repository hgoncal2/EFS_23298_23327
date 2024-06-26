// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function deleteDialog(controller, action, id) {
    var token = $('[name=__RequestVerificationToken]').val()
    $(function () {
        $("#dialog-confirm").dialog({
            resizable: false,
            height: "auto",
            width: 400,
            modal: true,
            buttons: {
                "Delete all items": function () {
                    $.ajax({
                        type: 'POST',
                        url: '/' + controller + '/' + action + '/' + id,
                        data: {
                            __RequestVerificationToken: token

                        },
                    }); $(this).dialog("close");

                    
                },
                Cancel: function () {
                    $(this).dialog("close");
                }
            },
            success: function (data) {
                if (data.status == "true") {
                    var urlToRedirect = '@Url.Action("Index","Home")';
                    window.location.href = urlToRedirect; //Redirect here
                }
                else if (data.status == "false") {
                    alert(data.msg)
                }
            },
            error: function () {
                alert('fail');
                window.location.reload();
            }
        });
        });
    
}

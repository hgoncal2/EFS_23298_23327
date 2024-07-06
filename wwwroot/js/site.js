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

var connection = new signalR.HubConnectionBuilder().withUrl("/hub").build();

connection.on("tema", function (user, message) {

    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    console.log(message);
    var id = message.split(",")[0];
    var color = message.split(",")[1];
    var msg = message.split(",")[2];
    console.log(msg);
    console.log(id);


    $("#testeTema").html([
`<div class="alert alert-dismissible  text-center fade show border-${color} border-3" role="alert">`,
        `<h4 class="alert-heading text-${color}">${msg}</h4>`,
        `<h2 text-info>Vê  o novo tema disponível <a  class="text-decoration-none link-${color}" href='/TemasGeral/Reserva/${id}'>aqui</a></h2>`,
        `<button type="button" class="btn-close btn-close-white" data-bs-dismiss="alert"   aria-label="Close"></button>`,
    `</div>`

    ].join(''))
});



connection.start().then(function () {
    console.log("ligado")
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(function () {

    Fancybox.bind("[data-fancybox]", {
        // Your custom options
    });
});

// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.




function showModal(difStr, reservaId, nome, dataI, dataF, nPessoas, salaColor, sala, tema, totalPreco, str, cancelada) {

    var stringCanc = "";

    var stringCanc = "";
    if (cancelada == "True") {
        stringCanc = `<i class="bi bi-x text-danger fa-5x"></i><span class='text-danger h3'>Cancelada!</span></br>`

    }

    $("#rModal").html([
        `<div class="modal text-center" id="ex" tabindex="-1">`,
        `<div class="modal-dialog text-center">`,
        `<div class="modal-content border-${difStr} border-2">`,
        `<div class="modal-header text-center">`,
        `<h5 class="modal-title text-center">Reserva ${reservaId} - ${nome}</h5>`,
        `<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>`,
        `</div>`,
        stringCanc,
        `<div class="modal-body">`,
        `<div class="container-fluid text-center justify-content-center">`,
        `<div class="row">`,
        ` <div class="col ">`,
        `<div >`,
        `Ínicio :<span class="text-success ms-1">${dataI}</span>`,

        `</div>`,
        `<div >`,
        `Fim : <span class="text-danger ms-1">${dataF}</span>`,

        `</div>`,
        `</div> `,
        ` <div class="col">`,
        `<div class="text-info">`,
        ` Número de pessoas: ${nPessoas}`,
        `</div>`,
        `<div >`,
        `Sala: <span style="color:${salaColor}">${sala}</span>`,
        `</div>`,
        `</div> `,
        `</div>`,
        `<div class="mt-2 text-center  ">`,
        `Tema: <span class="text-${difStr}">${tema}</span>`,
        ` </div>`,
        `<div class="mt-2 text-center  ">`,
        ` Preço Total: <span class="">${totalPreco}€</span>`,
        ` </div>`,
        ` <div class="mt-5 text-center  ">`,
        `<span class="">Anfitriões</span></br>`,

        `<ul class="list-group text-break list-group-horizontal mt-2 mb-2">`,
        `${str}`,
        `</div>`,
        ` </div>`,
        ` </div>`,

        `<div class="modal-footer">`,

        `<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>`,
        `</div>`,
        `</div>`,
        `</div>`,
        `</div>`].join(""));

    $("#ex").modal("show");





}















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


connection.on("reserva", function (user, message) {

    
    console.log(message);
    var rId = message.split(",")[0];
    var rData = message.split(",")[1];
    var sala = message.split(",")[2];
    

    $("#toastResBody").html("Reserva nª " + rId + ", Em " + rData + ", Sala " + sala);

    var toast = new bootstrap.Toast($("#toastReservas"))
    toast.show()
});





connection.start().then(function () {
    console.log("ligado")
}).catch(function (err) {
    return console.error(err.toString());
});

$(document).ready(function () {

   
});

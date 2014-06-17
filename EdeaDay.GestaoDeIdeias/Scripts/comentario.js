///<reference path="~/Scripts/libs/jquery-1.8.2.js" />
///<reference path="~/Scripts/libs/handlebars-min.js" />
///<reference path="~/Scripts/script.js" />

var btComentar = $('#btComentar');
var ideiaId = $('#IdeiaID').val();
var textAreaComentario = $('#comentario');
var listaComentarios = $("#listaComentarios");
var btVerMaisComentarios = $("#btVerMaisComentarios");
var btRemoverComentario = $("a.removerComentario");

$(document).ready(function () {
    CarregarTodosComentarios(0, InserirTemplateListaComentario);
});

btVerMaisComentarios.click(function () {
    var preCarregados = listaComentarios.find("li[data-id]").length;

    CarregarTodosComentarios(preCarregados, InserirTemplateListaComentarioVerMais);
});

textAreaComentario.keypress(function (event) {
    if ($(this).val() == "")
        return;

    if (event.keyCode == 13) {
        btComentar.click();
    }
});

btComentar.click(function () {

    var textoComentario = textAreaComentario.val();
    var edicaoComentario = "#edicaoComentario";

    $.ajax({
        url: "/Comentario/ComentarioIdeia",
        type: "POST",
        data: {
            IdeiaID: ideiaId,
            textoComentario: textoComentario
        },
        dataType: "json",
        beforeSend: function (x, p) {
            LockContainer(edicaoComentario);
        },
        success: function (data) {
            InserirTemplateNovoComentario(data);
        },
        error: function () {
            alert('Ocorreram problemas para inserir o seu comentário. Por favor, tente novamente.');
        },
        complete: function (x, s) {
            UnlockContainer(edicaoComentario);
        }
    });
});

btRemoverComentario.live('click', function () {
    var parentContainer = $(this).parent();

    var comentarioId = parentContainer.attr('data-id');

    $.ajax({
        url: "/Comentario/ExcluirComentario",
        type: "POST",
        data: {
            IdeiaID: ideiaId,
            ComentarioID: comentarioId
        },
        dataType: "json",
        beforeSend: function (x, p) {
            var confirmacao = confirm('Você tem certeza que deseja excluir este comentário?');
            if (confirmacao) {
                LockContainer(parentContainer);
            }
            return confirmacao;
        },
        success: function (data) {
            parentContainer.animate({ opacity: 0.0 }).slideUp('slow');
        },
        error: function () {
            alert('Ocorreram problemas para excluir o seu comentário. Por favor, tente novamente.');
        },
        complete: function (x, s) {
            UnlockContainer(parentContainer);
        }
    });
});


function CarregarTodosComentarios(comentariosPreCarregados, successCallback) {

    $.ajax({
        url: "/Comentario/ComentariosEmIdeia",
        type: "GET",
        data: {
            IdeiaID: ideiaId,
            comentariosPreCarregados: comentariosPreCarregados
        },
        dataType: "json",
        cache: false,
        beforeSend: function (x, p) {
            LockContainer('#boxComentarios');
        },
        success: function (data) {
            successCallback(data);
        },
        error: function () {
            InserirErroAoCarregarComentarios();
        },
        complete: function (x, s) {
            UnlockContainer('#boxComentarios');
        }
    });
}

function CarregarTemplateComentarios(data) {

    var source = $("#comentario-template").html();
    var template = Handlebars.compile(source);

    return template(data);
}

function InserirTemplateListaComentario(data) {

    var htmlDoTemplate = CarregarTemplateComentarios(data);

    $(htmlDoTemplate).prependTo(listaComentarios);

    TratarBtVerMaisComentarios(data);
}


function InserirTemplateListaComentarioVerMais(data) {
    var htmlDoTemplate = CarregarTemplateComentarios(data);

    var ultimoComentarioCarregado = listaComentarios.find("li[data-id]").last();

    $(htmlDoTemplate).insertAfter(ultimoComentarioCarregado);

    TratarBtVerMaisComentarios(data);
}

function InserirTemplateNovoComentario(data) {
    var htmlDoTemplate = CarregarTemplateComentarios(data);

    try {
        $(htmlDoTemplate)
            .hide()
            .css('opacity', 0.0)
            .prependTo(listaComentarios)
            .slideDown('slow')
            .animate({ opacity: 1.0 });
    }
    catch (e) {
        $(htmlDoTemplate).prependTo(listaComentarios);
    }

    textAreaComentario.val('');
}

function TratarBtVerMaisComentarios(data) {

    btVerMaisComentarios.text('Ver Mais (' + data.ComentariosRestantes + ')');

    if (data.ComentariosRestantes == 0) {
        btVerMaisComentarios.parent().hide();
    } else {
        btVerMaisComentarios.parent().show();
    }
}

function InserirErroAoCarregarComentarios() {
    var erro = "<li><p class='msg'><span>Ocorreu um erro ao carregar os comentários.</span></p></li>";
    listaComentarios.prepend(erro);
}

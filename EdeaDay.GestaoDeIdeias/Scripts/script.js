///<reference path="~/Scripts/libs/jquery-1.8.2.js" />
///<reference path="~/Scripts/libs/jquery-ui-1.8.22.min.js" />

$(document).ready(function () {
    configurarModalDeIdealizar();
    inicializarBox();
});

function inicializarBox() {
    alternarAbas();
    inserirBrainstorm();
}

function LockScreen() {
    $.blockUI({
        message: "<div style='text-align: center; margin: 0 auto;'><h2 class='titulo'>Carregando...</h2><br /><img src='/Content/images/301.gif' alt='Aguarde' /></div>",
        overlayCSS: {
            backgroundColor: '#fff',
            opacity: 0.9
        },
        css: {
            background: "url('/Content/images/backgrounds/patternLista.gif')",
            border: '3px solid #e6e6e6',
            padding: '25px',
            backgroundColor: '#000',
            color: '#FFF'
        }
    });
}

function UnlockScreen() {
    $.unblockUI();
}

function LockContainer(container) {
    $(container).block({
        message: "<div style='text-align: center; margin: 0 auto;'><h2 class='titulo' style='float: right; position: relative; right: 20px; top: 15px;'>Carregando...</h2><br /><img src='/Content/images/301.gif' alt='Aguarde' style='width: 25px; height: 25px; top: -5px; position: relative; border: none;' /></div>",
        overlayCSS: {
            backgroundColor: '#fff',
            opacity: 0.9
        },
        css: {
               background: "url('/Content/images/backgrounds/patternLista.gif')",
               border: '3px solid #e6e6e6',
        }
    });
}

function UnlockContainer(container) {
    $(container).unblock();
}

function InicializarFiltrarPor() {
    $('#filtrarPor').change(function () {
        var selectedOption = $(this).val();
        CarregarListaDeIdeias(selectedOption);
    });

    var opcaoAtual = $('#filtrarPor option:selected').val();
    CarregarListaDeIdeias(opcaoAtual);
}

function CarregarListaDeIdeias(selectedOption) {
    $.ajax({
        url: "/Ideia/ListaDeIdeias",
        type: "POST",
        data: { id: selectedOption },
        dataType: "html",
        beforeSend: function (x, p) {
            LockScreen();
        },
        success: function (data) {
            $("#secaoListaIdeias").html(data);
        },
        error: function () {
            alert('Ocorreram problemas na busca. Tente novamente mais tarde.');
        },
        complete: function (x, s) {
            UnlockScreen();
        }
    });
}


function alternarAbas() {
    var abas = $('.boxLateral .legenda').children();
    var containers = $('.boxLateral .conteudo');
    abas.click(function (event) {
        event.preventDefault();
        event.stopPropagation();
        abas.removeClass('ativo');
        $(this).addClass('ativo');
        containers.removeClass('inativo');
        $(this).parent().closest('li').siblings().find('div.conteudo').addClass('inativo');
    });
}

function AbaToggle(umaAba) {
    var cssAbaInativa = 'desabilitado';

    umaAba = $(umaAba);

    $('section.conteudoAba').hide();
    var idDaSectionComConteudoDaAba = '#' + umaAba.attr('data-conteudo-aba');

    $(idDaSectionComConteudoDaAba).show();

    $('a.aba').removeClass(cssAbaInativa);
    umaAba.addClass(cssAbaInativa);
}

function ConfigurarContagemDeCaracteres(textbox, labelContagem) {
    textbox.ready(function () { countChars(textbox, labelContagem); });
    textbox.keyup(function () { countChars(textbox, labelContagem); });
    textbox.keydown(function () { countChars(textbox, labelContagem); });
}

function inserirBrainstorm() {
    var containerBrainstorm = $('.boxLateral .conteudo ul').last();
    var inputTexto = $('input[name="txtBrainstorm"]');

    containerBrainstorm.children('li:not(:last)').on('click', function (event) { tratarEdicaoBrainstorm($(this)); });

    inputTexto.keypress(function (e) {
        if (e.keyCode == 13) {
            var texto = jQuery.trim(inputTexto.val());
            if (texto !== '') {
                jQuery.ajax({
                    url: "/Usuario/CriarBrainstorm",
                    type: "POST",
                    data: { textoBrainstorm: texto },
                    dataType: "json",
                    success: function (data) {
                        var novaIdeia = $('<li/>');
                        novaIdeia.text(texto);
                        novaIdeia.attr('data-id', data.id);
                        novaIdeia.bind('click', function () { tratarEdicaoBrainstorm($(this)); });
                        inputTexto.parent().before(novaIdeia);
                        containerBrainstorm.scrollTop(containerBrainstorm[0].scrollHeight);
                        inputTexto.val('');
                    },
                    error: function () {
                        alert('Não foi possível adicionar salvar, tente novamente mais tarde');
                    }
                });
            }
        }
    });
}

function tratarEdicaoBrainstorm(liClicado) {
    if (!liClicado.has('input[type="text"]').length) {
        var inputTextEdicao = $('<input/>');
        var dataId = liClicado.attr('data-id');
        var texto = liClicado.text();
        inputTextEdicao.attr('type', 'text');
        inputTextEdicao.attr('data-id', dataId);
        inputTextEdicao.val(texto);
        liClicado.text('');
        inputTextEdicao.appendTo(liClicado);
        inputTextEdicao.focus();
        inputTextEdicao.keypress(function (event) {
            if (event.keyCode == 13) {
                var textoEditado = jQuery.trim($(this).val());
                if (textoEditado !== '') {
                    editarBrainstorm(dataId, textoEditado);
                    var inputTextEditado = liClicado.find('input[type="text"]');
                    liClicado.attr('data-id', inputTextEditado.attr("data-id"));
                    liClicado.html(inputTextEditado.val());
                } else {
                    deletarBrainstorm(dataId);
                    liClicado.remove();
                }
            }
        });
    } else {
        var inputText = liClicado.find('input[type="text"]');
        liClicado.attr('data-id', inputText.attr("data-id"));
        liClicado.html(inputText.val());
    }
}

function editarBrainstorm(idBrainstorm, textoBrainstorm) {
    var texto = jQuery.trim(textoBrainstorm);
    if (texto !== '') {
        jQuery.ajax({
            url: "/Usuario/EditarBrainstorm",
            type: "POST",
            data: { id: idBrainstorm, texto: texto },
            dataType: "json",
            success: function (data) {
            },
            error: function () {
                alert('Não foi possível editar, tente novamente mais tarde');
            }
        });
    }
}

function deletarBrainstorm(idBrainstorm) {
    jQuery.ajax({
        url: "/Usuario/DeletarBrainstorm",
        type: "POST",
        data: { id: idBrainstorm },
        dataType: "json",
        success: function (data) {
        },
        error: function () {
            alert('Não foi possível deletar, tente novamente mais tarde');
        }
    });
}

function configurarModalDeIdealizar() {
    $('#idealizarButton').colorbox({ inline: true, width: "710px" });

    $(document).bind('cbox_complete', function () {
        var fileInputBoxIdealizar = $(this).find('input[name="flFotoBoxIdealizar"]');
        var imagePreviewBoxIdealizar = $(this).find('img[id="imagemIdeiaBoxIdealizar"]');

        if (fileInputBoxIdealizar.data('events') != undefined) {
            return false;
        }

        fileInputBoxIdealizar.on('change', function () {
            configurarPreviewUploadImagens(fileInputBoxIdealizar[0], imagePreviewBoxIdealizar);
        });

        trataCliqueImagemCrossBrowser($('label[for="flFotoBoxIdealizar"]'), fileInputBoxIdealizar);
    });
}

function trataCliqueImagemCrossBrowser(label, inputFile) {
    if ($.browser.mozilla || $.browser.msie) {
        label.click(function () {
            inputFile.click();
        });
    }
}

function configurarPreviewUploadImagens(input, imagemPreview) {
    var inputPossuiArquivos = input.files && input.files[0];

    if (!inputPossuiArquivos) {
        imagemPreview.attr('src', '/Content/images/logos/logoPadrao.jpg');
        if ($.browser.msie) {
            var img = input.value;
            imagemPreview.attr('src', img).width(194).height(188);
        }
    }

    var reader = new FileReader();

    reader.onload = function (e) {
        imagemPreview
            .attr('src', e.target.result)
            .width(194)
            .height(188);
    };

    reader.readAsDataURL(input.files[0]);
}
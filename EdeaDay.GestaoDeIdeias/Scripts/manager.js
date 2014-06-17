///<reference path="~/Scripts/libs/jquery-1.8.2.js" />
///<reference path="~/Scripts/libs/jquery-ui-1.8.22.min.js" />
///<reference path="~/Scripts/script.js" />
///<reference path="~/Scripts/libs/jquery.colorbox.js"/>

$(document).ready(function () {
    InicializarElementosDoManager();

    InicializarUploadDeLogoDoManager();

    InicializarModalDeInscricoes();

    $('#abaInformacoes').click(function () {
        AbaToggle(this);
    });

    $('#abaVagas').click(function () {
        AbaToggle(this);
    });
    
    $('#abaEnvolvidos').click(function () {
        AbaToggle(this);
    });
});

function InicializarElementosDoManager() {
    ConfigurarContagemDeCaracteres($('#Descricao'), $("#counterDescricao"));
    ConfigurarContagemDeCaracteres($('#Titulo'), $("#counterTitulo"));

    $('#DataInicial').datepicker({ dateFormat: 'dd/mm/yy' });

    $('#StatusOptions').change(function () { $('#Status').val($(this).val()); });
}

function InicializarUploadDeLogoDoManager() {
    var fileInput = $('input[name="flFoto"]');
    var imagemPreviewManager = $('#imagemIdeia');

    fileInput.on('change', function () { configurarPreviewUploadImagens(fileInput[0], imagemPreviewManager); });

    trataCliqueImagemCrossBrowser($('label[for="flFoto"]'), fileInput);
}

function InicializarModalDeInscricoes() {
        $('.verInscricoes').colorbox({
            inline: true
            , width: "710px"
            , onOpen: function () {
                $('#modalInscricoes').html(ObterInscricoes($(this).attr('data-id')));
            }
        });
    };

function ObterInscricoes(vagaId) {
    var inscricoes = '';
    $.ajax({
        url: "/Vaga/ListSubscription",
        type: "POST",
        data: { id: vagaId },
        dataType: "html",
        async: false,
        beforeSend: function (x, p) {
            LockScreen();
        },
        success: function (data) {
            inscricoes = data;
        },
        error: function () {
            alert('Ocorreram problemas para listar as inscrições. Tente novamente mais tarde.');
        },
        complete: function (x, s) {
            UnlockScreen();
        }
    });

    return inscricoes;
}

function AprovacaoSucesso() {
    alert('O usuário foi aprovado para a vaga com sucesso!');
    location.reload();
}

function AprovacaoFalha() {
    alert('Ocorreram problemas com a aprovação do usuário. Tente novamente mais tarde.');
}
function toggleParcelas() {
    var checkbox = document.getElementById('Parcelado');
    var parcelasDiv = document.getElementById('parcelasDiv')

    if (checkbox.checked) {
        parcelasDiv.classList.remove('d-none');
    } else {
        parcelasDiv.classList.add('d-none');
    }
}

function atualizarParcelas() {
    var rangeInput = document.getElementById('customRange2');
    var quantidadeParcelas = document.getElementById('quantidadeParcelas');

    quantidadeParcelas.textContent = rangeInput.value;
}

$(document).ready(function () {
    getDatatable('#usuariosTable');
    getDatatable('#clientesTable');
    getDatatable('#vendasTable');
});

function getDatatable(id) {
    $(id).DataTable({
        "ordering": true,
        "paging": true,
        "searching": true,
        "oLanguage": {
            "sEmptyTable": "Nenhum registro encontrado na tabela",
            "sInfo": "Mostrar _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrar 0 até 0 de 0 Registros",
            "sInfoFiltered": "(Filtrar de _MAX_ total registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "Mostrar _MENU_ registros por pagina",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "Pesquisar",
            "oPaginate": {
                "sNext": ">",
                "sPrevious": "<",
                "sFirst": "<<",
                "sLast": ">>"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    })
}

$(document).ready(function () {
    $('#inputValor').inputmask('decimal', {
        radixPoint: ',',
        groupSeparator: '.',
        autoGroup: true,
        digits: 2,
        digitsOptional: false,
        rightAlign: false,
        removeMaskOnSubmit: true
    });
});

// Fecha o alerta ao clicar no botão de fechar
$('.close-alert').click(function (e) {
    e.preventDefault();
    $(this).closest('.alert').hide('slow');
});

document.getElementById('open_btn').addEventListener('click', function () {
    document.getElementById('sidebar').classList.toggle('open-sidebar');
});
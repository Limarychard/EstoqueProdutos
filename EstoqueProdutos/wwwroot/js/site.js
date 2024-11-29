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
    getDatatable('#ProdutosVendasTable', true)
});

function getDatatable(id, enableSearching = false) {
    $(id).DataTable({
        "ordering": true,
        "paging": true,
        "searching": enableSearching,
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

$('.close-alert').click(function (e) {
    e.preventDefault();
    $(this).closest('.alert').hide('slow');
});

document.getElementById('open_btn').addEventListener('click', function () {
    document.getElementById('sidebar').classList.toggle('open-sidebar');

    content.classList.toggle("with-sidebar");
});

document.addEventListener("DOMContentLoaded", function () {
    // Recupera o tema armazenado no localStorage
    const temaSalvo = localStorage.getItem('tema');

    // Se o tema salvo existir, aplica ele ao documento
    if (temaSalvo) {
        document.documentElement.setAttribute('data-theme', temaSalvo);
    } else {
        // Se não houver tema salvo, usa o tema da API (ObterPreferencias)
        fetch('./Configuracao/ObterPreferencias')
            .then(response => response.json())
            .then(data => {
                const tema = data.tema || 'light';  // Tema padrão caso não seja retornado da API
                document.documentElement.setAttribute('data-theme', tema);
                localStorage.setItem('tema', tema);  // Salva no localStorage
            })
            .catch(error => {
                console.error('Erro ao carregar as preferências:', error);
            });
    }

    console.log("Tema recuperado:", temaSalvo);
});

document.addEventListener("DOMContentLoaded", function () {
    const temaButton = document.getElementById('alterar-tema');

    // Verifica se o botão existe e então adiciona o evento de clique
    if (temaButton) {
        temaButton.addEventListener('click', function () {
            // Pega o valor do tema selecionado
            const temaSelecionado = document.getElementById('themeSelector').value;

            const tema = (temaSelecionado === '1') ? 'dark' : 'light';

            // Chama a função para mudar o tema
            mudarTema(tema);
        });
    }
});

function mudarTema(tema) {
    // Salva o tema escolhido no localStorage
    localStorage.setItem('tema', tema);

    // Aplica o tema no documento
    document.documentElement.setAttribute('data-theme', tema);

    console.log("Tentando salvar o tema no backend:", tema);

    // Chama o backend para salvar a alteração no banco de dados
    fetch('./Configuracao/SalvarPreferencias', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ tema: tema }) // Envia o tema escolhido para o backend
    })
        .then(response => response.json())
        .then(data => {
            console.log("Resposta do servidor:", data);
            if (data.sucesso) {
                console.log('Tema alterado com sucesso no backend');
                localStorage.setItem('tema', tema); // Garante que o localStorage esteja atualizado
                location.reload(); // Recarrega a página para aplicar as mudanças (opcional)
            } else {
                console.error('Erro ao alterar o tema:', data.erro);
            }
        })
        .catch(error => {
            console.error('Erro ao fazer a requisição para alterar o tema:', error);
        });
}

document.addEventListener("DOMContentLoaded", function () {
    const menuState = localStorage.getItem("menuState");
    const sidebar = document.getElementById("sidebar");
    const content = document.getElementById("contentMain");

    if (sidebar && content) {
        if (menuState === "closed") {
            sidebar.classList.remove("open-sidebar");
            sidebar.setAttribute("data-state", "closed");
            content.classList.remove("with-sidebar");
        } else {
            sidebar.classList.add("open-sidebar");
            sidebar.setAttribute("data-state", "open");
            content.classList.add("with-sidebar");
        }
    }

    const menuButton = document.getElementById("open_btn");
    if (menuButton) {
        menuButton.addEventListener("click", toggleMenuState);
    }
});

function toggleMenuState() {
    const sidebar = document.getElementById("sidebar");
    const content = document.getElementById("contentMain");
    const currentState = sidebar.getAttribute("data-state");

    if (currentState === "open") {
        sidebar.classList.remove("open-sidebar");
        sidebar.setAttribute("data-state", "closed");
        content.classList.remove("with-sidebar");
        localStorage.setItem("menuState", "closed");
        localStorage.setItem("contentState", "without-sidebar");
    } else {
        sidebar.classList.add("open-sidebar");
        sidebar.setAttribute("data-state", "open");
        content.classList.add("with-sidebar");
        localStorage.setItem("menuState", "open");
        localStorage.setItem("contentState", "without-sidebar");
    }
}

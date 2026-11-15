$(document).ready(function () {
    // Aplica em qualquer input que tenha id/name "CEP"
    $('input[name="CEP"], #CEP').on('blur', function () {
        const campoCEP = $(this);
        const cep = campoCEP.val().replace(/\D/g, ''); // Remove máscara

        // Só consulta se tiver 8 dígitos
        if (cep.length !== 8) {
            return;
        }

        // Feedback visual
        campoCEP.prop('disabled', true);

        $.ajax({
            url: `https://viacep.com.br/ws/${cep}/json/`,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data.erro) {
                    alert('CEP não encontrado.');
                    return;
                }

                // Preenche os campos do endereço
                $('input[name="Logradouro"], #Logradouro').val(data.logradouro);
                $('input[name="Bairro"], #Bairro').val(data.bairro);

                // Se existir campo Cidade/UF, preenche também
                $('input[name="Cidade"], #Cidade').val(data.localidade);
                $('input[name="UF"], #UF').val(data.uf);

                // Foco no campo Número
                $('input[name="Numero"], #Numero').focus();
            },
            error: function () {
                alert('Erro ao consultar o CEP. Verifique sua conexão.');
            },
            complete: function () {
                campoCEP.prop('disabled', false);
            }
        });
    });
});
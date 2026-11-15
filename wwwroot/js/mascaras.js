$(document).ready(function () {

    // ===== MÁSCARAS SIMPLES =====
    $('input[name="CPF"], #CPF').mask('000.000.000-00');
    $('input[name="Telefone"], #Telefone').mask('(00) 00000-0000');
    $('input[name="CEP"], #CEP').mask('00000-000');
    $('input[name="CNPJ"], #CNPJ').mask('00.000.000/0000-00');

    // ===== MÁSCARA MONETÁRIA =====
    function aplicarMascaraMoeda() {
        $('.mask-money').each(function () {
            var $campo = $(this);
            if ($campo.data('maskmoney-aplicado')) return;
            $campo.data('maskmoney-aplicado', true);

            $campo.maskMoney({
                prefix: 'R$ ',
                allowNegative: false,
                thousands: '.',
                decimal: ',',
                affixesStay: true
            });
        });
    }

    aplicarMascaraMoeda();

    $(document).on('shown.bs.modal', function () {
        aplicarMascaraMoeda();
    });
});
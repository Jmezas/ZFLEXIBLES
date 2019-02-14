var ListaGuia = {
    CargarDocumento: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaDocumento"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    $('#lstTipoDoc').append($('<option>', { value: 0, text: 'Seleccione' }));
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstTipoDoc"]').append($('<option>', { value: oDocumento["Codigo"], text: oDocumento["Descripcion"] }));
                    });
                }

                CargarGuia();
            }

        });
    },
    Inicializar: function () {
        $('input[name="txtFechaInicio"]').Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $('input[name="txtFechaFin"]').Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $('input[name="txtNumero"]').Validate({ type: 'numeric' })
        $('input[name="txtDocumento"]').Validate({ type: 'numeric' })
    }

}

$(function () {

    
    ListaGuia.CargarDocumento();
    ListaGuia.Inicializar();
  

    $('input[name="txtNumero"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarGuia();
        }
    });
    $('input[name="txtCliente"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarGuia();
        }
    });
    $('input[name="txtDocumento"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarGuia();
        }
    });
    $('input[name="txtFechaInicio"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarGuia();
        }
    });
    $('input[name="txtFechaFin"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarGuia();
        }
    });
    $("#btnFiltrar").click(function () {
        CargarGuia();
    })



    $('#txtFechaInicio').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>',

    });
    $('#txtFechaInicio').datepicker();

    $('#txtFechaFin').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>',

    });
    $('#txtFechaFin').datepicker();


    $("#divGuia").scroll(function () {
        var numero = $('input[name="txtNumero"]').val(),
            cliente = $('input[name="txtCliente"]').val(),
            TipoDoc = $('#lstTipoDoc').val(),
            documento = $('#txtDocumento').val(),
            Fechainicio = $("#txtFechaInicio").val(),
            Fechafin = $("#txtFechaFin").val(),
            Comienzo = $('#tbGuia').find('tbody tr').length;
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath('Reporte/ListaReportGuia'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { iComienzo: Comienzo, iMedia: 10, Numero: documento, Numdoc: numero, Cliente: cliente, Doc: TipoDoc, FechaInicio: Fechainicio, FechaFin: Fechafin },
                success: function (response) {
                    if (!response.hasOwnProperty('ErrorMessage')) {
                        var $td = $("#tbGuia")
                        if (response["Total"] == 0) {
                            return false;
                        }
                        if (response["Datos"].length == 0) {
                            $td.find('tbody').html('<tr><td colspan="10">No hay resultados para el filtro ingresado</td></tr>');
                          
                        } else {
                            $.grep(response["Datos"], function (item) {
                                $td.find('tbody').append(

                                                '<tr data-id="' + item["IdGuia"] + '">' +
                                                '<td>' + item["Item"] + '</td>' +
                                                '<td>' + item["Serie"] + '</td>' +
                                                '<td>' + item["FechaEmision"] + '</td>' +
                                                '<td>' + item["Comprobante"]["Serie"] + '</td>' +
                                                '<td>' + item["Cliente"]["NroDocumento"] + '</td>' +
                                                '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                                '<td>' + item["Motivo"] + '</td>' +
                                                '<td>' + '<button class="btn btn-danger btn-xs"><i class="fa fa-file-pdf-o" aria-hidden="true"></i> PDF</button>' + '</td>' +
                                                '</tr>'
                                    );
                            });
                            $('#pHelperProductos').html('Existe(n) ' + response["Total"] + ' resultado(s) para mostrar.' +
                                (response["Total"] > 0 ? ' Del&iacute;cese hacia abajo para visualizar m&aacute;s...' : ''));
                        }

                    }
                }
            });
        }
    });
    $("#tbGuia").find('tbody').on('click', '.btn-danger', function () {
        var IdGuia = $(this).closest('tr').attr('data-id');
        var Correo = 1;

        window.location.href = General.Utils.ContextPath('Reporte/ImprimirGuia?Codigo=' + IdGuia +"&Envio=" + Correo );

    });

    $("#btnPDf").click(function () {
        var numero = $('input[name="txtNumero"]').val(),
            cliente = $('input[name="txtCliente"]').val(),
            TipoDoc = $('#lstTipoDoc').val(),
            documento = $('#txtDocumento').val(),
            Fechainicio = $("#txtFechaInicio").val(),
            Fechafin = $("#txtFechaFin").val(),
            iComienzo = 0,
            media = 100;
        window.location.href = General.Utils.ContextPath('Reporte/ImprimirGuiaCab?iComienzo=' + iComienzo + "&iMedia=" + media + "&Numero=" + numero + "&Numdoc" + documento + "&Cliente=" + cliente + "&Doc=" + TipoDoc + "&FechaInicio=" + Fechainicio + "&FechaFin=" + Fechafin);

    });

});



function CargarGuia() {
    var numero = $('input[name="txtNumero"]').val(),
        cliente = $('input[name="txtCliente"]').val(),
        TipoDoc = $('#lstTipoDoc').val(),
        documento = $('#txtDocumento').val(),
        Fechainicio = $("#txtFechaInicio").val(),
        Fechafin = $("#txtFechaFin").val();
    console.log(TipoDoc);
    $.ajax({
        async: false,
        type: 'post',
        url: General.Utils.ContextPath('Reporte/ListaReportGuia'),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { iComienzo: 0, iMedia: 10, Numero: documento, Numdoc: numero, Cliente: cliente, Doc: TipoDoc, FechaInicio: Fechainicio, FechaFin: Fechafin },
        success: function (response) {
            console.log(response);

            if (!response.hasOwnProperty('ErrorMessage')) {
                var $td = $("#tbGuia")
                $td.find('tbody').empty();
                if (response["Datos"].length == 0) {
                    $td.find('tbody').html('<tr><td colspan="10">No hay resultados para el filtro ingresado</td></tr>');
                    $('#pHelperProductos').html('');
                } else {
                    $.grep(response["Datos"], function (item) {
                        $td.find('tbody').append(

                                        '<tr data-id="' + item["IdGuia"] + '">' +
                                        '<td>' + item["Item"] + '</td>' +
                                        '<td>' + item["Serie"] + '</td>' +
                                        '<td>' + item["FechaEmision"] + '</td>' +
                                        '<td>' + item["Comprobante"]["Serie"] + '</td>' +
                                        '<td>' + item["Cliente"]["NroDocumento"] + '</td>' +
                                        '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                        '<td>' + item["Motivo"] + '</td>' +
                                        '<td>' + '<button class="btn btn-danger btn-xs"><i class="fa fa-file-pdf-o" aria-hidden="true"></i> PDF</button>' + '</td>' +
                                        '</tr>'
                            );
                    });
                    $('#pHelperProductos').html('Existe(n) ' + response["Total"] + ' resultado(s) para mostrar.' +
                        (response["Total"] > 0 ? ' Del&iacute;cese hacia abajo para visualizar m&aacute;s...' : ''));
                }

            }
        }
    });

}
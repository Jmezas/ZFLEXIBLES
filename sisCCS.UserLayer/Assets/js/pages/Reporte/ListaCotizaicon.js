var listaCotizacion = {
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
                CargarCotizacion();
            }

        });
    },
    Inicializar: function () {
        $('input[name="txtFechaInicio"]').Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $('input[name="txtFechaFin"]').Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $('input[name="txtNumero"]').Validate({ type: 'numeric' })
    }



}
$(function () {
    listaCotizacion.CargarDocumento();
    listaCotizacion.Inicializar();
    $.datepicker.setDefaults($.datepicker.regional['es']);

    $("#btnPDf").click(function () {
        var numero = $('input[name="txtNumero"]').val(),
       cliente = $('input[name="txtCliente"]').val(),
       TipoDoc = $('#lstTipoDoc').val(),
       documento = $('#txtDocumento').val(),
       Fechainicio = $("#txtFechaInicio").val(),
       Fechafin = $("#txtFechaFin").val(),
       iComienzo = 0,
        media = 100;
        window.location.href = General.Utils.ContextPath('Reporte/ImprimirCot?iComienzo=' + iComienzo + "&iMedia=" + media + "&Numero=" + numero + "&Numdoc" + documento + "&Cliente=" + cliente + "&TipoDocumento=" + TipoDoc + "&FechaInicio=" + Fechainicio + "&FechaFin=" + Fechafin);

    });

    $('#txtFechaInicio').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>',
        onSelect: function (selectedDate) {
            // $('#fechaEmision').datepicker('option', 'minDate', selectedDate);
        }
    });
    $('#txtFechaInicio').datepicker();

    $.datepicker.setDefaults($.datepicker.regional['es']);

    $('#txtFechaFin').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>',
        onSelect: function (selectedDate) {
            // $('#fechaEmision').datepicker('option', 'minDate', selectedDate);
        }
    });
    $('#txtFechaFin').datepicker();

    $('input[name="txtCliente"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarCotizacion();
        }
    });

    $('input[name="txtNumero"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarCotizacion();
        }
    });

    $('input[name="txtDocumento"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarCotizacion();
        }
    });

    $('input[name="txtFechaInicio"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarCotizacion();
        }
    });

    $('input[name="txtFechaFin"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarCotizacion();
        }
    });

    $("#btnFiltrar").click(function () {
        $("#txtNumero").val();
        $("#txtCliente").val();
        $("#lstTipoDoc").val();
        $("#txtDocumento").val(),
        $("#txtFechaInicio").val();
        $("#txtFechaFin").val();
        CargarCotizacion();

    });
    $("#tbCotizacion").find('tbody').on('click', '.btn-danger', function () {
        var IdCotizacion = $(this).closest('tr').attr('data-id');
        var Correo = 1;
        window.location.href = General.Utils.ContextPath('Reporte/ImprimirCotizacion?IdCotizacion=' + IdCotizacion + "&Envio=" + Correo );

    });

    $("#idCotizacion").scroll(function () {
        var numero = $('input[name="txtNumero"]').val(),
       cliente = $('input[name="txtCliente"]').val(),
       TipoDoc = $('#lstTipoDoc').val(),
       documento = $('#txtDocumento').val(),
       Fechainicio = $("#txtFechaInicio").val(),
       Fechafin = $("#txtFechaFin").val(),
       iComienzo = $('#tbCotizacion').find('tbody tr').length;


        // Si el scroll se encuentra abajo de todo el DOM
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {

            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath('Reporte/ListaCotizacion'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { iComienzo: iComienzo, iMedia: 10, Numero: numero, Numdoc: documento, Cliente: cliente, TipoDocumento: TipoDoc, FechaInicio: Fechainicio, FechaFin: FechaFin },
                success: function (response) {
                    console.log(response);
                    if (!response.hasOwnProperty('ErrorMessage')) {

                        var $td = $("#tbCotizacion")
                        if (response["Total"] == 0) {
                            return false;
                        }
                        
                        if (response["Datos"].length == 0) {
                            General.Utils.ShowMessage(TypeMessage.Information, 'No existen m&aacute;s resultados para mostrar.');
                        } else {
                            $.grep(response["Datos"], function (item) {
                                $td.find('tbody').append(
                                                '<tr data-id="' + item["IdCotizacion"] + '">' +
                                                '<td>' + item["Item"] + '</td>' +
                                                '<td>' + item["Serie"] + '</td>' +
                                                '<td>' + item["Documento"]["Descripcion"] + '</td>' +
                                                '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                                '<td>' + item["Cliente"]["Direccion"] + '</td>' +
                                                '<td>' + item["FechaEmsion"] + '</td>' +
                                                '<td>' + item["SubTotal"] + '</td>' +
                                                '<td>' + item["IGV"] + '</td>' +
                                                '<td>' + item["Total"] + '</td>' +
                                                '<td>' + item["Moneda"]["Nombre"] + '</td>' +
                                                '<td>' + '<button class="btn btn-danger btn-xs"><i class="fa fa-file-pdf-o" aria-hidden="true"></i> PDF</button>' +
                                                '</td>' +
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

});

function CargarCotizacion() {
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
        url: General.Utils.ContextPath('Reporte/ListaCotizacion'),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { iComienzo: 0, iMedia: 10, Numero: numero, Numdoc: documento, Cliente: cliente, TipoDocumento: TipoDoc, FechaInicio: Fechainicio, FechaFin: Fechafin },
        success: function (response) {
            console.log(response);

            if (!response.hasOwnProperty('ErrorMessage')) {
                var $td = $("#tbCotizacion")
                $td.find('tbody').empty();
                if (response["Datos"].length == 0) {
                    $td.find('tbody').html('<tr><td colspan="10">No hay resultados para el filtro ingresado</td></tr>');
                    $('#pHelperProductos').html('');
                } else {
                    $.grep(response["Datos"], function (item) {
                        $td.find('tbody').append(

                                        '<tr data-id="' + item["IdCotizacion"] + '">' +
                                        '<td>' + item["Item"] + '</td>' +
                                        '<td>' + item["Serie"] + '</td>' +
                                        '<td>' + item["Documento"]["Descripcion"] + '</td>' +
                                        '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                        '<td>' + item["Cliente"]["Direccion"] + '</td>' +
                                        '<td>' + item["FechaEmision"] + '</td>' +
                                        '<td>' + item["SubTotal"] + '</td>' +
                                        '<td>' + item["IGV"] + '</td>' +
                                        '<td>' + item["Total"] + '</td>' +
                                        '<td>' + item["Moneda"]["Nombre"] + '</td>' +
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
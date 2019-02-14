var ListaFactura = {
    CargarDocumentoIdentitida: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("venta/ListaDocumento"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    $('#lstTipoDoc').append($('<option>', { value: 0, text: 'Seleccione' }));
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstTipoDoc"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                        console.log(oDocumento.text);
                    });
                }
                CargarFactura();
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

    ListaFactura.CargarDocumentoIdentitida();
    ListaFactura.Inicializar();


    CargarFactura();
    $("#btnFiltrar").click(function () {

        CargarFactura();
    })

    $("#btnFactura").click(function () {
        var numero = $('input[name="documento"]').val(),
       cliente = $('input[name="Cliente"]').val(),
       TipoDoc = $("#lstTipoDoc").val(),
       Fechainicio = $("#txtFechaInicio").val(),
       Fechafin = $("#txtFechaFin").val(),
       iComienzo = 0;
        media = 100;


        window.location.href = General.Utils.ContextPath('Reporte/Imprimir?iComienzo=' + iComienzo + "&iMedia=" + media + "&Numero=" + numero + "&Cliente=" + cliente + "&TipoDocumento=" + TipoDoc + "&FechaInicio=" + Fechainicio + "&FechaFin=" + Fechafin);
    });
    $("#tbProductos").find('tbody').on('click', '.btn-danger', function () {
        var idFactura = $(this).closest('tr').attr('data-id');
        var Correo = 1;
        window.location.href = General.Utils.ContextPath('Reporte/ImprimirFactura?Codigo=' + idFactura + "&Envio=" + Correo );

    });

    $('input[name="documento"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarFactura();
        }
    });

    $('input[name="Cliente"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarFactura();
        }
    });

    $('#txtFechaInicio').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarFactura();
        }
    });

    $('#txtFechaFin').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarFactura();
        }
    });

    $.datepicker.setDefaults($.datepicker.regional['es']);

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


    $("#dvProductos").scroll(function () {
        var numero = $('input[name="documento"]').val(),
            cliente = $('input[name="Cliente"]').val(),
            TipoDoc = $("#lstTipoDoc").val(),
            Fechainicio = $("#txtFechaInicio").val(),
            Fechafin = $("#txtFechaFin").val(),
            iComienzo = $('#tbProductos').find('tbody tr').length;
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath("Reporte/ListaReporteFac"),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { icomienzo: iComienzo, iMedia: 10, Numero: numero, Cliente: cliente, TipoDocumento: TipoDoc, FechaInicio: Fechainicio, FechaFin: Fechafin },
                success: function (response) {
                    if (!response.hasOwnProperty('ErrorMessage')) {
                        var $td = $("#tbProductos")
                        if (response["Total"] == 0) {
                            return false;
                        }
                        if (response["Datos"].length == 0) {
                            General.Utils.ShowMessage(TypeMessage.Information, 'No existen m&aacute;s resultados para mostrar.');
                        } else {
                            $.grep(response["Datos"], function (item) {
                                $td.find('tbody').append(

                                                '<tr data-id="' + item["IdVenta"] + '">' +
                                                '<td>' + item["Item"] + '</td>' +
                                                '<td>' + item["Comprobante"]["Nombre"] + '</td>' +
                                                '<td>' + item["Serie"] + '</td>' +
                                                '<td>' + item["FechaEmisio"] + '</td>' +
                                                '<td>' + item["Cliente"]["NroDocumento"] + '</td>' +
                                                '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                                '<td>' + item["Cliente"]["Telefono"] + '</td>' +
                                                '<td>' + item["Moneda"]["Nombre"] + '</td>' +
                                                '<td>' + item["Cantidad"] + '</td>' +
                                                '<td>' + item["SubTotal"] + '</td>' +
                                                '<td>' + item["IGV"] + '</td>' +
                                                '<td>' + item["Total"] + '</td>' +
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



function CargarFactura() {
    var numero = $('input[name="documento"]').val(),
        cliente = $('input[name="Cliente"]').val(),
        TipoDoc = $("#lstTipoDoc").val(),
        Fechainicio = $("#txtFechaInicio").val(),
        Fechafin = $("#txtFechaFin").val();

    $.ajax({
        async: false,
        type: 'post',
        url: General.Utils.ContextPath("Reporte/ListaReporteFac"),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { icomienzo: 0, iMedia: 10, Numero: numero, Cliente: cliente, TipoDocumento: TipoDoc, FechaInicio: Fechainicio, FechaFin: Fechafin },
        success: function (response) {
            if (!response.hasOwnProperty('ErrorMessage')) {
                var $td = $("#tbProductos")
                $td.find('tbody').empty();
                if (response["Datos"].length == 0) {
                    $td.find('tbody').html('<tr><td colspan="9">No hay resultados para el filtro ingresado</td></tr>');
                    $('#pHelperProductos').html('');
                } else {
                    $.grep(response["Datos"], function (item) {
                        $td.find('tbody').append(

                                         '<tr data-id="' + item["IdVenta"] + '">' +
                                         '<td>' + item["Item"] + '</td>' +
                                         '<td>' + item["Comprobante"]["Nombre"] + '</td>' +
                                         '<td>' + item["Serie"] + '</td>' +
                                         '<td>' + item["FechaEmisio"] + '</td>' +
                                         '<td>' + item["Cliente"]["NroDocumento"] + '</td>' +
                                         '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                         '<td>' + item["Cliente"]["Telefono"] + '</td>' +
                                         '<td>' + item["Moneda"]["Nombre"] + '</td>' +
                                         '<td>' + item["Cantidad"] + '</td>' +
                                         '<td>' + item["SubTotal"] + '</td>' +
                                         '<td>' + item["IGV"] + '</td>' +
                                         '<td>' + item["Total"] + '</td>' +
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
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
                    CargarFactura();
                }

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


        window.location.href = General.Utils.ContextPath('BandejaPago/ReportePagoCab?iComienzo=' + iComienzo + "&iMedia=" + media + "&Numero=" + numero + "&Cliente=" + cliente + "&Tipodocumento=" + TipoDoc + "&FechaInicio=" + Fechainicio + "&FechaFin=" + Fechafin);
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
            iComienzo = $('#tbPago').find('tbody tr').length;
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath("BandejaPago/ListaComprobante"),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { iComienzo: iComienzo, iMedia: 10, Numero: numero, Cliente: cliente, Tipodocumento: TipoDoc, FechaInicio: Fechainicio, FechaFin: Fechafin },
                success: function (response) {
                    if (!response.hasOwnProperty('ErrorMessage')) {
                        var $td = $("#tbPago")
                        if (response["Total"] == 0) {
                            return false;
                        }
                        if (response["Datos"].length == 0) {
                            General.Utils.ShowMessage(TypeMessage.Information, 'No existen m&aacute;s resultados para mostrar.');
                        } else {
                            $.grep(response["Datos"], function (item) {
                                $td.find('tbody').append(
                                       '<tr data-id="' + item["IdBandeja"] + '">' +
                                        '<td>' + item["Item"] + '</td>' +
                                        '<td>' + item["Comprobante"]["Nombre"] + '</td>' +
                                        '<td>' + item["Cliente"]["NroDocumento"] + '</td>' +
                                        '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                        '<td>' + item["Moneda"]["Nombre"] + '</td>' +
                                        '<td>' + item["Factura"]["Serie"] + '</td>' +
                                        '<td>' + item["Pago"]["FechaPago"] + '</td>' +
                                        '<td>' + item["Factura"]["Total"].replace(",", ".") + '</td>' +
                                        '<td>' + item["Pendiente"].replace(",", ".") + '</td>' +
                                        '<td>' + item["EstadoFactura"] + '</td>' +
                                        '<td class="text-center">' +
                                        '<a class="btn btn-warning btn-xs" href="' + General.Utils.ContextPath('BandejaPago/PagarDocumento?IdComprobante=' + item["IdComprobante"]) + '"><i class="fa fa-shopping-cart"> Pagar</i></a>' +
                                         '<button class="btn btn-info btn-xs"><i class="fa fa-binoculars" aria-hidden="true"></i> ver Pago</button>' + //para pagar
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

    $("#tbPago").find('tbody').on('click', '.btn-info', function () {
        var IdPago = $(this).closest('tr').attr('data-id');

        ListaDetalle(IdPago)
        $("#ModalNuevo").modal('show');
    });

    $("#tbPagoDetalle").find('tbody').on('click', '.btn-info', function () {
        var IdPago = $(this).closest('tr').attr('data-id');

        window.location.href = General.Utils.ContextPath('BandejaPago/ReportePago?Codigo=' + IdPago);
    });

    $("#vbDetalle").scroll(function () {

        iComienzo = $('#tbPagoDetalle').find('tbody tr').length;
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {

            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath("BandejaPago/ListarPagos"),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { iComienzo: iComienzo, iMedia: 20, tipoDoc: Id },
                success: function (response) {
                    console.log(response);

                    var $td = $("#tbPagoDetalle")
                    $td.find('tbody').empty();

                    $.grep(response["Datos"], function (item) {
                        $td.find('tbody').append(
                              '<tr data-id="' + item["IdPago"] + '">' +
                                '<td>' + item["Item"] + '</td>' +
                                '<td>' + item["Hora"] + '</td>' +
                                '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                '<td>' + item["sNumero"] + '</td>' +
                                '<td>' + item["Monto"].replace(",", ".") + '</td>' +
                                '<td>' + item["FechaPago"] + '</td>' +
                                '<td class="text-center">' +
                                '<button class="btn btn-info btn-xs"><i class="fa fa-download" aria-hidden="true"></i> ver Pago</button>' + //para pagar
                                '</td>' +
                               '</tr>'
                        )
                    });
                    $('#pHelperProductos').html('Existe(n) ' + response["Total"] + ' resultado(s) para mostrar.' +
                               (response["Total"] > 0 ? ' Del&iacute;cese hacia abajo para visualizar m&aacute;s...' : ''));
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
        url: General.Utils.ContextPath("BandejaPago/ListaComprobante"),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { iComienzo: 0, iMedia: 10, Numero: numero, Cliente: cliente, Tipodocumento: TipoDoc, FechaInicio: Fechainicio, FechaFin: Fechafin },
        success: function (response) {
              console.log(response);
            if (!response.hasOwnProperty('ErrorMessage')) {
                var $td = $("#tbPago")
                $td.find('tbody').empty();
                if (response["Datos"].length == 0) {
                    $td.find('tbody').html('<tr><td colspan="9">No hay resultados para el filtro ingresado</td></tr>');
                    $('#pHelperProductos').html('');
                } else {
                    $.grep(response["Datos"], function (item) {
                        $td.find('tbody').append(
                                        '<tr data-id="' + item["IdBandeja"] + '">' +
                                        '<td>' + item["Item"] + '</td>' +
                                        '<td>' + item["Comprobante"]["Nombre"] + '</td>' +
                                        '<td>' + item["Cliente"]["NroDocumento"] + '</td>' +
                                        '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                        '<td>' + item["Moneda"]["Nombre"] + '</td>' +
                                        '<td>' + item["Factura"]["Serie"] + '</td>' +
                                        '<td>' + item["Pago"]["FechaPago"] + '</td>' +
                                        '<td>' + item["Factura"]["Total"].replace(",", ".") + '</td>' +
                                        '<td>' + item["Pendiente"].replace(",", ".") + '</td>' +
                                        '<td>' + item["EstadoFactura"] + '</td>' +
                                        '<td class="text-center">' +
                                        '<a class="btn btn-warning btn-xs" href="' + General.Utils.ContextPath('BandejaPago/PagarDocumento?IdComprobante=' + item["IdBandeja"]) + '"><i class="fa fa-shopping-cart"> Pagar</i></a>' +
                                         '<button class="btn btn-info btn-xs"><i class="fa fa-binoculars" aria-hidden="true"></i> ver Pago</button>' + //para pagar
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

function ListaDetalle(Id) {

    $.ajax({
        async: false,
        type: 'post',
        url: General.Utils.ContextPath("BandejaPago/ListarPagos"),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { iComienzo: 0, iMedia: 20, tipoDoc: Id },
        success: function (response) {
            console.log(response);

            var $td = $("#tbPagoDetalle")
            $td.find('tbody').empty();

            $.grep(response["Datos"], function (item) {
                $td.find('tbody').append(
                      '<tr data-id="' + item["IdPago"] + '">' +
                        '<td>' + item["Item"] + '</td>' +
                        '<td>' + item["Hora"] + '</td>' +
                        '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                        '<td>' + item["Factura"]["Serie"] + '</td>' +
                        '<td>' + item["Monto"].replace(",", ".") + '</td>' +
                        '<td>' + item["FechaPago"] + '</td>' +
                        '<td class="text-center">' +
                        '<button class="btn btn-info btn-xs"><i class="fa fa-download" aria-hidden="true"></i> ver Pago</button>' + //para pagar
                        '</td>' +
                       '</tr>'
                )
            });
            $('#pHelperProductos').html('Existe(n) ' + response["Total"] + ' resultado(s) para mostrar.' +
                               (response["Total"] > 0 ? ' Del&iacute;cese hacia abajo para visualizar m&aacute;s...' : ''));
        }


    });

}
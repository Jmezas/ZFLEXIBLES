var ListaOrden = {
    CargarConteniedo: function () {
        var Fecha = $("#txtCodigoNew").val();

    },
    Inicializar: function () {
        $('input[name="txtFechaInicio"]').Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $('input[name="txtFechaFin"]').Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $('input[name="txtNumero"]').Validate({ type: 'numeric' })
    }
}


$(function () {

    CargarListaOrden();
    ListaOrden.Inicializar();

    $("#btnOrden").click(function () {

        var documento = $('#txtNumero').val(),
            Fechainicio = $("#txtFechaInicio").val(),
            Fechafin = $("#txtFechaFin").val(),
           iComienzo = 0,
        media = 100;

        window.location.href = General.Utils.ContextPath('GestionActivos/ImprimirOrden?iComienzo=' + iComienzo + "&iMedia=" + media + "&FechaInicio=" + Fechainicio + "&FechaFin=" + Fechafin + "&Numero=" + documento);
    });

    $('#txtNumero').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarListaOrden();
        }
    });
    $('#txtFechaInicio').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarListaOrden();
        }
    });
    $('#txtFechaFin').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarListaOrden();
        }
    });
    $("#btnFiltrar").click(function () {

        CargarListaOrden();
    });
    $('#tbOrdenCompra').find('tbody').on('click', '.btn-danger', function () {
        var idCompra = $(this).closest('tr').attr('data-id');
        console.log(idCompra);
        window.location.href = General.Utils.ContextPath('GestionActivos/ImpresionOrdenCompra?IdDoc=' + idCompra)

    });
    $("#dvProductos").scroll(function () {
        var documento = $('#txtNumero').val(),
       Fechainicio = $("#txtFechaInicio").val(),
       Fechafin = $("#txtFechaFin").val();
        iComienzo = $('#tbOrdenCompra').find('tbody tr').length;
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath('GestionActivos/ListaOrdenCompra'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { iComienzo: iComienzo, iMedia: 10, fechaInicio: Fechainicio, fechafin: Fechafin, numero: documento },
                success: function (response) {
                    console.log(response);
                    if (!response.hasOwnProperty('ErrorMessage')) {
                        var $td = $("#tbOrdenCompra")
                        if (response["Total"] == 0) {
                            return false;
                        }

                        if (response["Datos"].length == 0) {
                            General.Utils.ShowMessage(TypeMessage.Information, 'No existen m&aacute;s resultados para mostrar.');
                        } else {
                            $.grep(response["Datos"], function (item) {
                                $td.find('tbody').append(

                                                '<tr data-id="' + item["IdOrden"] + '">' +
                                                '<td>' + item["Item"] + '</td>' +
                                                '<td>' + item["Serie"] + '</td>' +
                                                '<td>' + item["FechaRegistro"] + '</td>' +
                                                '<td>' + item["proveed"]["Nombre"] + '</td>' +
                                                '<td>' + item["Cantidad"] + '</td>' +
                                                '<td>' + item["SutTotal"] + '</td>' +
                                                '<td>' + item["IGv"] + '</td>' +
                                                '<td>' + item["Total"] + '</td>' +
                                                '<td class="text-center">' +
                                                '<button class="btn btn-danger btn-xs"><i class="fa fa-file-pdf-o" aria-hidden="true"></i> PDF</button>' +
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



function CargarListaOrden() {
    var
       documento = $('#txtNumero').val(),
       Fechainicio = $("#txtFechaInicio").val(),
       Fechafin = $("#txtFechaFin").val();

    $.ajax({
        async: false,
        type: 'post',
        url: General.Utils.ContextPath('GestionActivos/ListaOrdenCompra'),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { iComienzo: 0, iMedia: 15, fechaInicio: Fechainicio, fechafin: Fechafin, numero: documento },
        success: function (response) {
            console.log(response);
            if (!response.hasOwnProperty('ErrorMessage')) {
                var $td = $("#tbOrdenCompra")
                $td.find('tbody').empty();
                if (response["Datos"].length == 0) {
                    $td.find('tbody').html('<tr><td colspan="10">No hay resultados para el filtro ingresado</td></tr>');
                    $('#pHelperProductos').html('');
                } else {
                    $.grep(response["Datos"], function (item) {
                        $td.find('tbody').append(

                                        '<tr data-id="' + item["IdOrden"] + '">' +
                                        '<td>' + item["Item"] + '</td>' +
                                        '<td>' + item["Serie"] + '</td>' +
                                        '<td>' + item["FechaRegistro"] + '</td>' +
                                        '<td>' + item["proveed"]["Nombre"] + '</td>' +
                                        '<td>' + item["Cantidad"] + '</td>' +
                                        '<td>' + item["SutTotal"] + '</td>' +
                                        '<td>' + item["IGv"] + '</td>' +
                                        '<td>' + item["Total"] + '</td>' +
                                        '<td class="text-center">' +
                                        '<button class="btn btn-danger btn-xs"><i class="fa fa-file-pdf-o" aria-hidden="true"></i> PDF</button>' +
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
var ListaGuia = {
    CargarUbigeo: function () {
        $.ajax({
            async: true,
            type: 'POST',
            url: General.Utils.ContextPath("Shared/ListaUbigeo"),
            dataType: 'json',
            data: { Tipo: 'D', Dep: '', Prov: '' },
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) {
                    $("#ListaDepartamento").empty();
                    $("#ListaDepartamento").append($('<option>', { value: 0, text: 'Seleccione' }));
                    $.grep(response, function (oUbigeo) {
                        $("#ListaDepartamento").append($('<option>', { value: oUbigeo["CodigoDepartamento"], text: oUbigeo["Nombre"] }))
                    });
                }
            }
        });
    },
    CargarDoc: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaDocumento"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    $.grep(response, function (oDocumento) {
                        $('select[name="ltDocumento"]').append($('<option>', { value: oDocumento["Codigo"], text: oDocumento["Descripcion"] }));

                    });
                    CargarListaCotizacion();
                }
            }

        });
    },
    CargarSerieDoc: function () {
        var doc = 2;
        var cmp = 12;
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("venta/ListaSerieDoc"),
            dataType: 'json',
            data: { tipo: doc, comprobante: cmp },
            success: function (response) {
                console.log(response);
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error                   
                    $("#txtSerie").val(response.Serie);
                    $("#txtNumero").val(response.Numero);
                }
            }
        });
    },
    Inicializar: function () {
        $("#txtFechaEmision").Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $("#txtFechaInicio").Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $("#txtFechaFin").Validate({ type: 'date', blockBefore: false, blockAfter: false });
        $('input[name="txtRucEmpresa"]').Validate({ type: 'numeric', max: 11 })
        $('input[name="txtNumeroSerie"]').Validate({ type: 'numeric', max: 11 })
        $('input[name="txtDocumento"]').Validate({ type: 'numeric', max: 11 })
    },
    Vars: {
        detalle: []
    }

}




$(function () {

    ListaGuia.CargarUbigeo();
    ListaGuia.CargarDoc();
    ListaGuia.CargarSerieDoc();
    ListaGuia.Inicializar();
    CargarListaCotizacion();

    $("#btnLimpiar").click(function () {
        // window.location.href = General.Utils.ContextPath('Reporte/ImprimirGuia?Codigo=' + 33 + "&EnvioCorreo=" + 0);
        Limpiar()
    });

    $("#ListaDepartamento").change(function () {
        $.ajax({
            async: true,
            type: 'POST',
            url: General.Utils.ContextPath("Shared/ListaUbigeo"),
            dataType: 'json',
            data: { Tipo: 'P', Dep: $(this).val(), Prov: 0 },
            success: function (response) {
                console.log(response);
                $("#ListaProvincia").empty();
                $("#ListaProvincia").append($('<option>', { value: 0, text: 'Seleccione' }));
                $.grep(response, function (oProvincia) {
                    $("#ListaProvincia").append($('<option>', { value: oProvincia["CodigoProvincia"], text: oProvincia["Nombre"] }));

                });

            }
        });
    });
    $("#ListaProvincia").change(function () {
        var depar = $("#ListaDepartamento").val();
        var pro = $("#ListaProvincia").val();
        console.log(depar);
        $.ajax({
            async: true,
            type: 'POST',
            url: General.Utils.ContextPath("Shared/ListaUbigeo"),
            dataType: 'json',
            data: { Tipo: 'I', Dep: depar, Prov: pro },
            success: function (response) {
                console.log(response);
                $("#ListaDistrito").empty();
                $("#ListaDistrito").append($('<option>', { value: 0, text: 'Seleccione' }));
                $.grep(response, function (oProvincia) {
                    $("#ListaDistrito").append($('<option>', { value: oProvincia["Id"], text: oProvincia["Nombre"] }));
                });
                //$("#ListaDistrito").val(0);
            }
        });
    });

    $('input[name="txtFechaEmision"]').datepicker({
        dateFormat: 'dd/mm/yy',
        prevText: '<i class="fa fa-angle-left"></i>',
        nextText: '<i class="fa fa-angle-right"></i>',
        minDate: new Date()
    });
    $("#txtFechaDoc").datepicker(
   {
       dateFormat: 'dd/mm/yy',
       firstDay: 1
   }).datepicker("setDate", new Date());

    $("#btnBuscarCot").click(function () {
        $("#ModalCotizacion").modal("show");
        $('input[name="txtNumeroSerie"]').val('');
        $('input[name="txtDocumento"]').val('');
        $('input[name="txtCliente"]').val('');
        $('#txtDocumento').val('');
        $("#txtFechaInicio").val('');
        $("#txtFechaFin").val('');
        CargarListaCotizacion();
    });

    //filtros para los doc
    $("#btnFiltrar").click(function () {
        CargarListaCotizacion();
    });
    $("#ltDocumento").change(function () {

        CargarListaCotizacion();
    });
    $('input[name="txtCliente"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarListaCotizacion();
        }
    });

    $('input[name="txtNumeroSerie"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarListaCotizacion();
        }
    });

    $('input[name="txtDocumento"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarListaCotizacion();
        }
    });

    $('input[name="txtFechaInicio"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarListaCotizacion();
        }
    });

    $('input[name="txtFechaFin"]').keyup(function (event) {
        if (event.keyCode == 13) {
            CargarListaCotizacion();
        }
    });
    $("#btnCerrar").click(function () {
        $('input[name="txtNumeroSerie"]').val('');
        $('input[name="txtDocumento"]').val('');
        $('input[name="txtCliente"]').val('');
        $('#txtDocumento').val('');
        $("#txtFechaInicio").val('');
        $("#txtFechaFin").val('');
        CargarListaCotizacion();
    });

    $("#dvCotizacion").scroll(function () {
        var numero = $('input[name="txtNumeroSerie"]').val(),
            cliente = $('input[name="txtCliente"]').val(),
            TipoDoc = $('select[name ="ltDocumento"]').val(),
            documento = $('#txtDocumento').val(),
            Fechainicio = $("#txtFechaInicio").val(),
            Fechafin = $("#txtFechaFin").val(),
            iComienzo = $('#tbCotizacion').find('tbody tr').length;
        console.log(TipoDoc);
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath('Reporte/ListaCotizacion'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
               data: { iComienzo: iComienzo, iMedia: 10, Numero: numero, Numdoc: documento, Cliente: cliente, TipoDocumento: TipoDoc, FechaInicio: Fechainicio, FechaFin: Fechafin },
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
                                                '<td>' + item["FechaEmision"] + '</td>' +
                                                 '<td>' + '<button class="btn btn-warning btn-xs"><i class="fa fa-edi" aria-hidden="true"></i> Seleccionar</button>' + '</td>' +
                                                '</tr>'
                                    );
                            });
                            $('#pHelperCotizacion').html('Existe(n) ' + response["Total"] + ' resultado(s) para mostrar.' +
                                (response["Total"] > 0 ? ' Del&iacute;cese hacia abajo para visualizar m&aacute;s...' : ''));
                        }
                        var element = document.getElementById('btnGuardar');
                        element.scrollIntoView(false);
                    } else {
                        General.Utils.ShowMessageRequest(response);
                    }

                }
            });
        }
    });


    $("#tbCotizacion").find('tbody').on('click', '.btn-warning', function () {
        var IdCotizacion = $(this).closest('tr').attr('data-id');
        ObtenerCotizacion(IdCotizacion);
        $("#ModalCotizacion").modal("toggle");

    });

    $("#btnGuardar").click(function (e) {
        e.preventDefault();
        if ($("#txtPuntoLlegada").val() == "") {
            $("#txtPuntoLlegada").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Por favor ingrese direccion de llegada');
        } else if ($("#ListaDepartamento").val() == 0) {
            $("#ListaDepartamento").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Deptartamento');
        } else if ($("#ListaProvincia").val() == 0) {
            $("#ListaProvincia").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Provincia');
        } else if ($("#ListaDistrito").val() == 0) {
            $("#ListaDistrito").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Distrito');
        } else if ($("#txtFechaEmision").val() == "") {
            $("#txtFechaEmision").focus();
            console.log($("#ListaDepartamento").val());
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese fecha de emisión');
        } else if ($("#IdDoc").val() == 0) {
            $("#btnBuscarCot").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione comprobante');
        } else if (!$("input[name=rbLineaNegocio]").is(":checked")) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Motivo de traslado');
        } else if ($("#txtAsunto").val() == "") {
            $("#txtAsunto").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el motivo porfavor');
        } else if ($("#txtMensaje").val() == "") {
            $("#txtMensaje").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese el mesnaje para enviar al correo');
        }
        else {
            var oGuiaRemision = {
                IDGuia: 1,
                Serie: $("#txtSerie").val(),
                Numero: $("#txtNumero").val(),
                PuntoLlegada: $("#txtPuntoLlegada").val(),
                Ubigeo: {
                    IdUbigeo: $("#ListaDistrito").val()
                },
                Marca: $("#txtmarca").val(),
                Placa: $("#txtplaca").val(),
                Liencia: $("#txtlicencia").val(),
                NombreEmpresa: $("#txtEmpresa").val(),
                RucEmpresa: $("#txtRucEmpresa").val(),
                FechaEmision: $("#txtFechaEmision").val(),
                FechaOperacion: $("#txtFechaDoc").val(),
                Comprobante: {
                    IdCotizacion: $("#IdDoc").val(),
                },
                Cliente: {
                    IdCliente: $("#IdCliente").val()
                },
                Motivo: $("input[name=rbLineaNegocio]:checked").val(),
                Mensaje: $("#txtMensaje").val(),
                Asunto: $("#txtAsunto").val()
            }
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath('Venta/InsGuiaRemision'),
                dataType: 'json',
                data: { oGuiaRemision: oGuiaRemision, oDetalle: ListaGuia.Vars.detalle, UserReg: '' },
                success: function (response) {
                    if (response.Id = 'success') {
                        General.Utils.ShowModalMessage(response.Id, "", response.Message, function () {
                                var Envio=0;
                            window.location.href = General.Utils.ContextPath('Reporte/ImprimirGuia?Codigo=' + response.Additionals[0] + '&Envio=' + Envio);
                            Limpiar()
                            $("#txtPuntoLlegada").focus();
                        });
                    }
                }
            });
        }
    });

});

function CargarListaCotizacion() {
    var numero = $('input[name="txtNumeroSerie"]').val(),
       cliente = $('input[name="txtCliente"]').val(),
       TipoDoc = $('select[name="ltDocumento"]').val(),
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
                    $td.find('tbody').html('<tr><td colspan="5">No hay resultados para el filtro ingresado</td></tr>');
                    $('#pHelperCotizacion').html('');
                } else {
                    $.grep(response["Datos"], function (item) {
                        $td.find('tbody').append(

                                        '<tr data-id="' + item["IdCotizacion"] + '">' +
                                        '<td>' + item["Item"] + '</td>' +
                                        '<td>' + item["Serie"] + '</td>' +
                                        '<td>' + item["Documento"]["Descripcion"] + '</td>' +
                                        '<td>' + item["Cliente"]["Nombre"] + '</td>' +
                                        '<td>' + item["FechaEmision"] + '</td>' +
                                        '<td>' + '<button class="btn btn-warning btn-xs"><i class="fa fa-edi" aria-hidden="true"></i> Seleccionar</button>' + '</td>' +
                                        '</tr>'
                            );
                    });
                    $('#pHelperCotizacion').html('Existe(n) ' + response["Total"] + ' resultado(s) para mostrar.' +
                        (response["Total"] > 0 ? ' Del&iacute;cese hacia abajo para visualizar m&aacute;s...' : ''));
                }

            }
        }
    });
}
function ObtenerCotizacion(IdCotizacion) {
    ListaGuia.Vars.detalle = [];
    $.ajax({
        asyn: false,
        type: 'post',
        url: General.Utils.ContextPath('Venta/ObtenerCotizacion'),
        dataType: 'json',
        data: { Codigo: IdCotizacion },
        success: function (response) {
            console.log(response);
            $("#IdCliente").val(response[0].Cliente.IdCliente);
            $("#IdDoc").val(response[0].Cotizacion.IdCotizacion);
            $("#txtSeriNumero").val(response[0].Cotizacion.Serie);
            $("#txtfechaEmindoc").val(response[0].Cotizacion.FechaEmision);
            $("#txtDocumentoCli").val(response[0].Cliente.NroDocumento);
            $("#txtRazonCli").val(response[0].Cliente.Nombre);
            $("#txtdireccionCli").val(response[0].Cliente.Direccion);

            var $td = $("#tbDetalle");
            $td.find('tbody').empty();

            $.grep(response, function (oDetalle) {
                ListaGuia.Vars.detalle.push({

                    Producto: { IdMaterial: oDetalle['Producto'].IdMaterial },
                    Codigo: { Codigo: oDetalle["Producto"].Codigo },
                    Nombre: oDetalle["Producto"].NombreMat,
                    iMedia: oDetalle["Producto"]["UM"].MedNom,
                    Cantidad: oDetalle["Detalle"].Cantidad,
                    Precio: oDetalle["Detalle"].Precio
                });

            });

            if (ListaGuia.Vars.detalle.length == 0) {
                $td.find('tbody').append('<tr><td colspan="4">No existen registros</td></tr>')
            } else {
                $.grep(ListaGuia.Vars.detalle, function (detalle) {
                    $td.find('tbody').append(
                        '<tr data-index="' + detalle["Producto"]["IdMaterial"] + '" >' +
                        '<td>' + detalle["Codigo"]["Codigo"] + '</td>' +
                        '<td>' + detalle["Nombre"] + '</td>' +
                        '<td>' + detalle["iMedia"] + '</td>' +
                        '<td>' + detalle["Cantidad"] + '</td>' +
                       '</tr>'
                        );
                });

            }
            console.log(ListaGuia.Vars.detalle);
        }
    });
}
function Limpiar() {
    $("#txtPuntoLlegada").val('');
    ListaGuia.CargarUbigeo();
    $("#ListaProvincia").val(0);
    $("#ListaDistrito").val(0);
    $("#txtmarca").val('');
    $("#txtplaca").val('');
    $("#txtlicencia").val('');
    $("#txtEmpresa").val('');
    $("#txtRucEmpresa").val('');
    $("#txtFechaEmision").val('');
    ListaGuia.CargarSerieDoc();
    $("#ltDocumento").val(1);
    $("#IdDoc").val(0);
    $("#IdCliente").val(0);
    $("#txtSeriNumero").val('');
    $("#txtfechaEmindoc").val('');
    $("#txtDocumentoCli").val('');
    $("#txtRazonCli").val('');
    $("#txtdireccionCli").val('');
    eliminartable();
    $("input[name=rbLineaNegocio]").prop('checked', false);
    $("#txtAsunto").val('');
    $("#txtMensaje").val('');


    console.log(ListaGuia.Vars.detalle);

}

function eliminartable() {

    var $btn = $(this);
    var $tb = $('#tbDetalle');

    var iIdProducto = 0;

    $.each(ListaGuia.Vars, function (index, value) {

        iIdProducto = value[0].Producto.IdMaterial;
       // BuscarIndexDetalleEnTabla(iIdProducto);

        arrDetalle = ListaGuia.Vars.detalle.filter(function (x) {
            return x.Producto.IdMaterial != iIdProducto;


        });
        ListaGuia.Vars = [];
        ListaGuia.Vars = arrDetalle;

        $tb.find('tbody').empty();
        if (ListaGuia.Vars.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="9">No existen registros</td></tr>')
        }
    });
}

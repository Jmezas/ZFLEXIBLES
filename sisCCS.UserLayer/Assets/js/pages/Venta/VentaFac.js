var ListaFactura = {
    CargarDocumento: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("venta/ListaDocumento"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    $.grep(response, function (oDocumento) {
                        $('select[name="lstDocumento"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));

                    });
                }
            }

        });
    },
    CargarDocumentoIdentitida: function () {
        var doc = 1
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("venta/ListaDoc"),
            dataType: 'json',
            data: { documento: doc },
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error

                    $.grep(response, function (oDocumento) {
                        $('select[name="lstTipoDocCli"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));

                    });
                }
            }

        });
    },
    CargarSerieDoc: function () {
        var doc = 1;
        var cmp = 1;
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
    Vars: {
        Detalle: []
    },
}

$(function () {
    ListaFactura.CargarDocumento();
    ListaFactura.CargarDocumentoIdentitida();
    ListaFactura.CargarSerieDoc();

    $("#txtDocumento").on("keydown", function (e) {
        var code = e.which;
        if (code == 13 || code == 9) {
            e.preventDefault();
            BuscarDocumento();
        }
    });
    $("#btnLimpiar").click(function () {
        LimpiarContenido();
      
    });

    $("#lstDocumento").change(function () {
        var doc = $("#lstDocumento").val();
        console.log(doc);
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("venta/ListaDoc"),
            dataType: 'json',
            data: { documento: doc },
            success: function (response) {
                console.log(response);
                var $Documento = $('select[name = "lstTipoDocCli"]');
                $Documento.empty();
                $.grep(response, function (oDocumento) {
                    $Documento.append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));
                });
            }
        });

        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("venta/ListaSerieDoc"),
            dataType: 'json',
            data: { tipo: 1, comprobante: doc },
            success: function (response) {
                console.log(response);
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    $("#txtSerie").val(response.Serie);
                    $("#txtNumero").val(response.Numero);
                }
            }
        });
        $("#txtDocCli").val('');
        $("#txtNomCli").val('');
        $("#txtDireccionInicio").val('');
    });
    $("#txtDocCli").autocomplete({

        source: function (request, response) {
            var vRuc = $("#txtDocCli").val();
            var iIDTipoDocumento = $("#lstTipoDocCli").val();

            if (iIDTipoDocumento == 2) {
                jQuery.ajax({
                    url: General.Utils.ContextPath('Venta/BuscarClientePorRUC'),
                    type: "POST",
                    dataType: "json",
                    data: { pRuc: vRuc ,Filtro2:vRuc},
                    success: function (data) {
                        response(jQuery.map(data, function (item) {
                            return {
                                id: item.IdCliente,
                                cod: item.NroDocumento,
                                label: item.NroDocumento + ' - ' + item.Nombre,
                                des: item.Nombre + ' | ' + item.Direccion

                            };
                        }));
                    }
                });
            } else if (iIDTipoDocumento == 1) {
                jQuery.ajax({
                    url: General.Utils.ContextPath('Venta/BuscarClientePorDNI'),
                    type: "POST",
                    dataType: "json",
                    data: { pRuc: vRuc,Filtro2:vRuc },
                    success: function (data) {
                        response(jQuery.map(data, function (item) {
                            return {
                                id: item.IdCliente,
                                cod: item.NroDocumento,
                                label: item.NroDocumento + ' - ' + item.Nombre,
                                des: item.Nombre + ' | ' + item.Direccion

                            };
                        }));
                    }
                });
            }

        },
        minLength: 2,
        select: function (event, ui) {
            $('#idCliente').val(ui.item.id);
            if ($('#lstTipoDocCli').val() == 2) {
                jQuery("#txtDocCli").val(ui.item ? ui.item.cod : '');
            } else {
                jQuery("#txtDocCli").val(ui.item ? ui.item.cod : jQuery("#txtDocCli").val());
                $('#idCliente').val(ui.item.id);
                console.log($('#idCliente').val());
            }
        },

        change: function (event, ui) {
            console.log(ui.item.des);
            if ($('#lstTipoDocCli').val() == 2 || $('#lstTipoDocCli').val() == 1) {
                if ($('#lstTipoDocCli').val() == 2) {
                    jQuery("#txtDocCli").val(ui.item ? ui.item.cod : '');
                } else {
                    jQuery("#txtDocCli").val(ui.item ? ui.item.cod : jQuery("#txtDocCli").val());
                }


                jQuery("#txtNomCli").val(ui.item ? ui.item.des.toString().split('|')[0] : '');
                jQuery("#txtDireccionInicio").val(ui.item ? ui.item.des.toString().split('|')[1] : '');
            } else {
                $('#idCliente').val('');
            }



        }
    });

    $("#aAgregarCliente").click(function () {
        $("#ModalNuevo").modal("show");
    });

    $("#Buscar").click(function () {
        BuscarDocumento();
    });
    $("#cerrarpx").click(function () {
        limpiar();
    });
    $("#btnCerrar").click(function () {
        limpiar();
    });

    $("#btnGrabarFinal").click(function () {
        var $form = $("#ModalNuevo");
        var oDatos = General.Utils.SerializeForm($form);
        if (General.Utils.ValidateForm($form)) {
            var oCliente = {
                IdCliente: 0,
                Documento: {
                    Codigo: $("#lstTipoDocCli").val(),
                },
                NroDocumento: $("#txtDocumento").val(),
                Nombre: $("#txtRazon").val(),
                Telefono: $("#txtTelefono").val(),
                Celular: $("#txtCelular").val(),
                Email: $("#txtEmail").val(),
                Direccion: $("#txtDireccionNew").val(),
                Usuario: {
                    Id: 1,
                },
                Estado: ($("#Estado").is(':checked') === true ? 'A' : 'I')
            }
            var dni = $("#txtDocumento").val();
            if ($("#lstTipoDocCli").val() == 1) {
                if (isNaN(dni) || dni < 10000000 || dni > 99999999) {
                    General.Utils.ShowMessage(TypeMessage.Error, 'El DNI debe de ser de 8 digitos');
                } else {
                    $.ajax({
                        async: true,
                        type: 'post',
                        url: General.Utils.ContextPath('Mantenimiento/RegistrarCliente'),
                        dataType: 'json',
                        data: oCliente,
                        success: function (response) {
                            console.log(response);
                            if (response["Id"] == TypeMessage.Success) {
                                $('#ModalNuevo').modal("toggle");
                                General.Utils.ShowMessageRequest(response);

                            } else {
                                $('#ModalNuevo').modal("toggle");
                                General.Utils.ShowMessageRequest(response);
                            }
                            limpiar();
                        }
                    });
                }

            }
            else if ($("#lstTipoDocCli").val() == 2) {
                var ruc = $("#txtDocumento").val();
                if (isNaN(ruc) || ruc < 10000000000 || ruc > 99999999999) {
                    General.Utils.ShowMessage(TypeMessage.Error, 'El RUC debe contener 11 dígitos');
                } else {
                    $.ajax({
                        async: true,
                        type: 'post',
                        url: General.Utils.ContextPath('Mantenimiento/RegistrarCliente'),
                        dataType: 'json',
                        data: oCliente,
                        success: function (response) {
                            console.log(response);
                            if (response["Id"] == TypeMessage.Success) {
                                $('#ModalNuevo').modal("toggle");
                                General.Utils.ShowMessageRequest(response);
                            }
                            else {
                                $('#ModalNuevo').modal("toggle");
                                General.Utils.ShowMessageRequest(response);
                            }
                            limpiar();
                        }
                    });
                }
            } else {
                $.ajax({
                    async: true,
                    type: 'post',
                    url: General.Utils.ContextPath('Mantenimiento/RegistrarCliente'),
                    dataType: 'json',
                    data: oCliente,
                    success: function (response) {
                        console.log(response);
                        if (response["Id"] == TypeMessage.Success) {
                            $('#ModalNuevo').modal("toggle");

                            General.Utils.ShowMessageRequest(response);
                        } else {
                            $('#ModalNuevo').modal("toggle");
                            General.Utils.ShowMessageRequest(response);
                        }
                        limpiar();
                    }
                });
            }
        }
    });

    $("#aBuscarProducto").click(function () {

        $("#ModalProducto").modal("show");

    });
    //lista de productos

    var table = $("#tbProductos").DataTable({
        "language": {
            "info": "Del _START_ al _END_ de _TOTAL_ ",
            "lengthMenu": "Mostrar _MENU_ registros",
            "zeroRecords": "No se encontraron datos.",
            "info": "Mostrando la p&aacute;gina _PAGE_ de _PAGES_",
            "infoEmpty": "No hay registros disponibles",

            "search": "Buscar:",
            "searchPlaceholder": "Buscar codigo o nombre",
            "zeroRecords": "No se han encontrado coincidencias.",
            "infoEmpty": "No hay registros disponibles",
            "infoFiltered": " ",


            "paginate": {
                "first": "Primero",
                "previous": "Anterior",
                "next": "Siguiente",
                "last": "&Uacute;timo"
            },
        },
        "processing": true,

        "order": [],
        "ajax": {
            "url": General.Utils.ContextPath('Mantenimiento/ListarProductosPaginacion'),
            "contentType": "application/json",
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                console.log(d);
                return JSON.stringify(d);
            },
        },
        "columns": [
            { "data": "IdMaterial" },
            { "data": "Codigo" },
            { "data": "NombreMat" },
            { "data": "UM.MedNom" },


            {
                "data": "IdMaterial", "render": function (data) {
                    return '<div id=btnEditar_' + data + ' class="btn btn-warning btn-xs evento"><i class="fa fa-edit"></i> Seleccionar</div>';
                },
                "orderable": false, "searchable": false, "width": "5%"
            }
        ],
    });


    $("#tbProductos").on('click', 'tbody .evento', function () {
        var data = table.row($(this).parents("tr")).data();
        $.when(ObtenerID(data.IdMaterial)).then(function () {
            $('#ModalProducto').modal('toggle');
        });
    });
    $("#txtCantidad").on('change', function () {
        let Precio = $('#txtPrecio').val();
        let Cantidad = $('#txtCantidad').val();
        let Total = $('#txtTotal').val();
        Total = Total * 1;
        Total = Precio * Cantidad;
        $("#txtTotal").val(Total.toFixed(3));

        //$('#txtTotalFinal').val(Total.toFixed(3));
    });

    $("#txtPrecio").on('change', function () {
        let Precio = $('#txtPrecio').val();
        let Cantidad = $('#txtCantidad').val();
        let Total = $('#txtTotal').val();

        Total = Precio * Cantidad;

        $('#txtTotal').val(Total.toFixed(3));
       // $('#txtTotalFinal').val(Total.toFixed(3));
    });

    $("#txtTotal").on('change', function () {
        let Precio = $('#txtPrecio').val();
        let Cantidad = $('#txtCantidad').val();
        let Total = $('#txtTotal').val();
        //let TotalFinal = $('#txtTotalFinal').val();

        let precio;
        precio = (Total * Cantidad) / Precio


        $('#txtPrecio').val(precio.toFixed(2));
    });

    $("#btnAgregar").click(function () {


        var iIdProducto = $("#hdfProducto").val();
        var material = $("#hdfProducto").val();
        var NombreMat = $("#txtProducto").val();
        var cantidad = $("#txtCantidad").val();
        var Precio = $("#txtPrecio").val();
        var Total = $("#txtTotal").val();
        var codigo = "";
        var IdMed = "";
        var MedNom = "";
        var nValorVta = 0;
        var Impuesto = 0;
        var senData = {
            IdMaterial: material
        }
        if (NombreMat == '') {
            General.Utils.ShowMessage(TypeMessage.Error, 'No se a cargado el producto');
        }
        else if (cantidad == '') {
            General.Utils.ShowMessage(TypeMessage.Error, 'Debe ingresar la cantidad');
        }
        else if (BuscarDetalleEnTabla(iIdProducto)) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El producto ya existe en la tabla');
        } else {
            $.ajax({
                async: true,
                type: 'post',
                url: General.Utils.ContextPath('Mantenimiento/ModificarProducto'),
                dataType: 'json',
                data: senData,
                success: function (response) {

                    if (!equalsIgnoreCase(response["Id"], 'error')) { // Si la petición no emitió error
                        iIdProducto = response.IdMaterial;
                        codigo = response.Codigo;
                        IdMed= response.UM.IdMed;
                        MedNom= response.UM.MedNom;
                        Impuesto= response.Impuesto;
                        $.ajax({
                            async: false,
                            type: 'post',
                            url: General.Utils.ContextPath('Venta/VerigicarStock'),
                            dataType: 'json',
                            data: { idProducto: iIdProducto, iCantidad: cantidad },
                            success: function (response) {
                                if (!equalsIgnoreCase(response["Id"], 'error')) {
                                    ListaFactura.Vars.Detalle.push({
                                        Producto: {
                                            IdMaterial: iIdProducto,
                                            Codigo: codigo,
                                            NombreMat: NombreMat
                                        },
                                        Unidad: {
                                            IdMed: IdMed,
                                            MedNom: MedNom
                                        },
                                        cantidad: cantidad,
                                        Precio: Precio,
                                        importe: cantidad * Precio,
                                        fImportSnIGV: Total / Impuesto.replace(",","."),
                                    });
                                    console.log(nValorVta);
                                    $("#txtProducto").val('');
                                    $("#txtPrecio").val('');
                                    $("#txtCantidad").val('');
                                    $("#hdfProducto").val('');
                                    $("#txtTotal").val('');
                                   // $("#txtTotalFinal").val('');
                                    var $tb = $('#tbDetalle');
                                    $tb.find('tbody').empty();
                                    CalcularTotales();
                                    if (ListaFactura.Vars.Detalle.length == 0) {
                                        $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
                                    } else {
                                        $.grep(ListaFactura.Vars.Detalle, function (oDetalle) {
                                            $tb.find('tbody').append(
                                                 '<tr data-index=' + oDetalle["Producto"].IdMaterial + '>' +
                                                 '<td>' + oDetalle["Producto"].Codigo + '</td>' +
                                                 '<td>' + oDetalle["Producto"].NombreMat + '</td>' +
                                                 '<td>' + oDetalle["Unidad"].MedNom + '</td>' +
                                                 '<td>' + oDetalle["cantidad"] + '</td>' +
                                                 '<td>' + oDetalle["Precio"] + '</td>' +
                                                 '<td>' + oDetalle["fImportSnIGV"].toFixed(2) + '</td>' +
                                                 '<td>' + oDetalle["importe"].toFixed(2) + '</td>' +
                                                 '<td class="text-center">' +
                                                 '<button type="submit" class="btn btn-danger btn-xs"><i class="fa fa-trash-o"></i></button>' +
                                                 '</td>' +
                                                 '</tr>'
                                                );
                                        });
                                        var element = document.getElementById('btnGuardar');
                                        element.scrollIntoView(false);
                                    }

                                } else {
                                    General.Utils.ShowMessageRequest(response);
                                }
                            }
                        });

                        console.log(ListaFactura.Vars.Detalle);
                    }
                }

            });
        }
    });
    $('#tbDetalle').find('tbody').on('click', '.btn-danger', function () {
        var $btn = $(this);
        var $tb = $('#tbDetalle');

        var iIdProducto = $btn.closest('tr').attr('data-index');

        BuscarIndexDetalleEnTabla(iIdProducto);




        arrDetalle = ListaFactura.Vars.Detalle.filter(function (x) {
            return x.Producto.IdMaterial != iIdProducto;
        });

        ListaFactura.Vars.Detalle = [];
        ListaFactura.Vars.Detalle = arrDetalle;

        $tb.find('tbody').empty();
        if (ListaFactura.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="9">No existen registros</td></tr>')

        } else {
            $.grep(ListaFactura.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                     '<tr data-index=' + oDetalle["Producto"].IdMaterial + '>' +
                                     '<td>' + oDetalle["Producto"].Codigo + '</td>' +
                                     '<td>' + oDetalle["Producto"].NombreMat + '</td>' +
                                     '<td>' + oDetalle["Unidad"].MedNom + '</td>' +
                                     '<td>' + oDetalle["cantidad"] + '</td>' +
                                     '<td>' + oDetalle["Precio"] + '</td>' +
                                     '<td>' + oDetalle["fImportSnIGV"].toFixed(2) + '</td>' +
                                     '<td>' + oDetalle["importe"].toFixed(2) + '</td>' +
                                     '<td class="text-center">' +
                                     '<button type="submit" class="btn btn-danger btn-xs"><i class="fa fa-trash-o"></i></button>' +
                                     '</td>' +
                                     '</tr>'
                );
            })
        }


        CalcularTotales();

    });
    $("#btnGuardar").click(function (e) {
        e.preventDefault();
        if ($("#txtDocCli").val() == "") {
            $("#txtDocCli").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Ingrese un número de documento correcto');
        } else if ($("#txtDocCli").val == "") {
            $("#txtDocCli").focus();
            $(this).prop('disabled', false);
            General.Utils.ShowMessage(TypeMessage.Error, 'Apellidos y Nombres o Razón Social no debe de estar bacio');
        } else if (ListaFactura.Vars.Detalle.length <= 0) {
            $(this).prop('disabled', false);
            General.Utils.ShowMessage(TypeMessage.Error, 'No se ha ingresado el detalle');
        } else if ($("#txtMotivo").val().length == 0) {
            $("#txtMotivo").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'Porfavor ingrese el sunto');
        } else if ($("#txtMensaje").val().length == 0) {
            $("#txtMensaje").focus();
            General.Utils.ShowMessage(TypeMessage.Error, 'El mensaje no debe de estar bacio');
        } else {
            var oVenta = {
                IdFactura: 0,
                Cliente: {
                    IdCliente: $("#idCliente").val()
                },
                Comprobante:{
                    Id: $("#lstDocumento").val(),
                    },

                Serie: $("#txtSerie").val(),
                Numero: $("#txtNumero").val(),
                EstadoDocumento: 1,
                Moneda: { idMoneda: 1 },
                Cantidad: $("#hCantidad").html(),
                SubTotal: $("#hTotalValorVta").html(),
                IGV: $("#hSumIgv").html(),
                Total: $("#hImpTotalVta").html(),
                TipoSerieComprobante: 'S',
                Mensaje: $("#txtMensaje").val(),
                Observacion: $("#txtObservaciones").val(),
                Motivo: $("#txtMotivo").val(),
                
            };
            console.log(oVenta);
            $.ajax({
                async: true,
                type: "POST",
                url: General.Utils.ContextPath('Venta/InsFacturaBol'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { oFactura: oVenta, FacturaDetalle: ListaFactura.Vars.Detalle },
                success: function (response) {
                    if (response.Id != 'error') {
                        General.Utils.ShowModalMessage(response.Id, "", response.Message, function () {
                            var EnvioCorreo = 0;
                            window.location.href = General.Utils.ContextPath('Reporte/ImprimirFactura?Codigo=' + response.Additionals[0] + "&Envio=" + EnvioCorreo);
                            LimpiarContenido();
                            $("#txtDocCli").focus();

                        })
                    }
                }
            });
        }
    });

});


function limpiar() {
    $("#Documento").val(0);
    $("#txtDocumento").val('');
    $("#txtRazon").val('');
    $('#txtCelular').val('');
    $('#txtTelefono').val('');
    $('#txtEmail').val('');
    $("#txtDireccionNew").val('');
}

//busqueda de ruc/dni
function BuscarDocumento() {
    var documento = $("#lstTipoDocCli").val();
    if (documento == 1) {
        var DNI = $("#txtDocumento").val();
        if (isNaN(DNI) || DNI < 10000000 || DNI > 99999999) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El DNI debe de ser de 8 digitos');
        } else {
            $.ajax({
                type: 'post',
                url: General.Utils.ContextPath('Mantenimiento/BuscarDNI'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                data: {
                    dni: DNI

                },
                success: function (response) {
                    console.log(response)
                    var valor = JSON.parse(response);
                    if (valor.nombre == "  ") {
                        General.Utils.ShowMessage(TypeMessage.Error, 'El DNI es invalido, intetalo denuevo o ingrese manualmente.');
                    }
                    else if (response.Id == 'error') {
                        General.Utils.ShowModalMessage(response.Id, "", response.Message + "Nro. de ruc invalido Ingrese los datos manualmente", function () {
                            $("#txtRazon").attr("readonly", false);
                            $("#txtDireccion").attr("readonly", false);
                        });
                    }
                    else {

                        $("#txtRazon").val(valor.nombre);
                    }
                }
            });
        }
    } else if (documento == 2) {
        var RUC = $("#txtDocumento").val();
        if (isNaN(RUC) || RUC < 10000000000 || RUC > 99999999999) {
            General.Utils.ShowMessage(TypeMessage.Error, 'El RUC debe contener 11 dígitos');
        } else {
            $.ajax({
                type: 'post',
                url: General.Utils.ContextPath('Mantenimiento/BuscarRuc'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                data: {
                    ruc: RUC

                },
                success: function (response) {
                    console.log(response)
                    if (response.Id == 'error') {
                        General.Utils.ShowModalMessage(response.Id, "", response.Message + "Nro. de ruc invalido Ingrese los datos manualmente", function () {
                            $("#txtRazon").attr("readonly", false);
                            $("#txtDireccionNew").attr("readonly", false);
                        });
                    } else {
                        var valor = JSON.parse(response);

                        $("#txtRazon").val(valor.razon_social);
                        $("#txtDireccionNew").val(valor.direccion);
                    }
                }
            });
        }
    } else if (documento == 0) {
        General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Documento');
    }

    else {
        alert("Ingese manualmente.");
    }
}

var ObtenerID = function (IdMaterial) {
    var senData = {
        IdMaterial: IdMaterial
    }
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('Mantenimiento/ModificarProducto'),
        dataType: 'json',
        data: senData,
        success: function (response) {
            $("#hdfProducto").val(response.IdMaterial);
            $("#txtProducto").val(response.NombreMat);
            $("#txtPrecio").val(response.PrecioVenta.toString().replace(",", "."));
            $('#txtCantidad').val('');
            $('#txtTotal').val('');
         //   $('#txtTotalFinal').val('');
            $("#txtCantidad").focus();
        }


    });
}

function BuscarDetalleEnTabla(iIdProducto) {
    var bFound = false;
    $.each(ListaFactura.Vars.Detalle, function (index, item) {
        if (item["Producto"].IdMaterial == iIdProducto) {
            bFound = true;
            return false;
        }
    });
    return bFound;
}

function BuscarIndexDetalleEnTabla(id) {
    for (var i = 0; i < ListaFactura.Vars.Detalle.length; i += 1) {
        if (ListaFactura.Vars.Detalle[i]["Producto"].IdMaterial == id) {
            return i;
        }
    }
    return -1;
}
function CalcularTotales() {
    var TotalValorVta = ImpTotalVta = SumIgv = SumDscto = cantidadlt = 0;

    $.grep(ListaFactura.Vars.Detalle, function (oDetalle) {
        var subTotal = oDetalle["fImportSnIGV"].toFixed(2);//sub total
        subTotal = subTotal * 1;
        TotalValorVta += subTotal;

        var nImpTotalVta = oDetalle["importe"];

        var cantidad = oDetalle["cantidad"];
        cantidad = cantidad * 1
        cantidadlt += cantidad

        nImpTotalVta = toFixed(nImpTotalVta, 2);
        nImpTotalVta = nImpTotalVta * 1;
        ImpTotalVta += nImpTotalVta;


    });

    SumIgv = ImpTotalVta - TotalValorVta;

    $('#hTotalValorVta').html(TotalValorVta.toFixed(2));
    $("#hCantidad").html(cantidadlt);
    $('#hImpTotalVta').html(ImpTotalVta.toFixed(2));//esto!
    $('#hSumIgv').html(SumIgv.toFixed(2));
}
function LimpiarContenido() {
    $('#idCliente').val('');
    $('#lstTipoDocCli').val(1);
    $('#txtDocCli').val('');
    $('#txtNomCli').val('');
    $('#txtDireccionInicio').val('');
    $('#txtMensaje').val('');
    $("#lstDocumento").val(1);
  

    ListaFactura.CargarSerieDoc();
    $("#lstTipoDocCli").val(1);
    eliminartable();
    $('#txtObservaciones').val('');
    $('#hCantidad').html('0.00');
    $('#hTotalValorVta').html('0.00');
    $('#hSumIgv').html('0.00');
    $('#hImpTotalVta').html('0.00');
}
function eliminartable() {

    var $btn = $(this);
    var $tb = $('#tbDetalle');

    var iIdProducto = 0;

    $.each(ListaFactura.Vars.Detalle, function (index, value) {

        iIdProducto = value.Producto.IdMaterial;
        BuscarIndexDetalleEnTabla(iIdProducto);

        arrDetalle = ListaFactura.Vars.Detalle.filter(function (x) {
            return x.Producto.IdMaterial != iIdProducto;
        });
        ListaFactura.Vars.Detalle = [];
        ListaFactura.Vars.Detalle = arrDetalle;

        $tb.find('tbody').empty();
        if (ListaFactura.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="9">No existen registros</td></tr>')
        }
    });
}
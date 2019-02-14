var ListadoCompra = {
    CargarConteniedo: function () {

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

    },
    Vars: {
        Detalle: []
    },
    CargarSerieNum: function () {

        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath('GestionActivos/SerieNumDoc'),
            dataType: 'json',
            success: function (response) {
                console.log(response);
                var ar = response.Text.split("-");
                $("#txtSerie").val(ar[0]);
                $("#txtNumero").val(ar[1]);
            }, error: function (xhr, textStatus, error) {
                console.log(xhr.statusText);
                console.log(textStatus);
                console.log(error);
            }
        });
    },
    CargarConteniedoNew: function () {
        $.ajax({
            async: true,
            type: 'post',

            url: General.Utils.ContextPath("GestionActivos/ListaProveedor"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    //$('select[name="ListaProveedor"]').append($('<option>', { value: -1, text: 'Seleccione' }))
                    $.grep(response, function (oProveedor) {
                        $('select[name="ListaProveedor"]').append($('<option>', { value: oProveedor["IdProveedor"], text: oProveedor["Nombre"] }));

                    });
                }
            }
        });
    },
    CargarMoneda: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaMonda"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error

                    $.grep(response, function (oMoneda) {
                        $('select[name="Moneda"]').append($('<option>', { value: oMoneda["idMoneda"], text: oMoneda["Nombre"] }));
                    });
                }
            }

        });
    }
}



$(function () {
    ListadoCompra.CargarConteniedo();
    ListadoCompra.CargarSerieNum();
    ListadoCompra.CargarConteniedoNew();
    ListadoCompra.CargarMoneda();
    $("#btnLimpiar").click(function () {

        Limpiar();

    });


    $('input[name="txtCantidad"]').Validate({ type: TypeValidation.Numeric, special: '.' });
  
    $("#aBuscarProducto").click(function () {
        $("#tbProductos").DataTable().ajax.reload();
        $('#ModalProducto').modal('show');

    });


    $("#tbProductos").on('click', 'tbody .evento', function () {

        var dataRow = $(this).parent().parent().children().first().text();
        console.log(dataRow);
        $.when(ObtenerID(dataRow)).then(function () {
            $('#ModalProducto').modal('toggle');
        });
    });


    $("#btnAgregar").click(function () {

        console.log($("#hdfProducto").val());
        var iIdProducto = $("#hdfProducto").val();
        var material = $("#hdfProducto").val();
        var NombreMat = $("#txtProducto").val();
        var cantidad = $("#txtCantidad").val();
        var Precio = $("#txtPrecio").val();
        var nValorVta = 0;
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
                        //  codigoMat=response.Codigo;
                        console.log(response.Impuesto);
                        ListadoCompra.Vars.Detalle.push({
                            Producto: {
                                IdMaterial: response.IdMaterial,
                                Codigo: response.Codigo,

                                NombreMat: NombreMat
                            },
                            Unidad: {
                                IdMed: response.UM.IdMed,
                                MedNom: response.UM.MedNom
                            },
                            cantidad: cantidad,
                            Precio: Precio,
                            importe: cantidad * Precio,
                            nValorVta: cantidad * Precio / (response.Impuesto.replace(",", "."))
                        });
                        $("#txtProducto").val('');
                        $("#txtPrecio").val('');
                        $("#txtCantidad").val('');
                        $("#hdfProducto").val('');
                        var $tb = $('#tbDetalle');
                        $tb.find('tbody').empty();
                        CalcularTotales();
                        if (ListadoCompra.Vars.Detalle.length == 0) {
                            $tb.find('tbody').append('<tr><td colspan="15">No existen registros</td></tr>')
                        } else {
                            $.grep(ListadoCompra.Vars.Detalle, function (oDetalle) {
                                $tb.find('tbody').append(
                                     '<tr data-index=' + oDetalle["Producto"].IdMaterial + '>' +
                                     '<td>' + oDetalle["Producto"].Codigo + '</td>' +
                                     '<td>' + oDetalle["Producto"].NombreMat + '</td>' +
                                     '<td>' + oDetalle["Unidad"].MedNom + '</td>' +
                                     '<td>' + oDetalle["cantidad"] + '</td>' +
                                     '<td>' + oDetalle["Precio"] + '</td>' +
                                     '<td>' + oDetalle["nValorVta"].toFixed(2) + '</td>' +
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




        arrDetalle = ListadoCompra.Vars.Detalle.filter(function (x) {
            return x.Producto.IdMaterial != iIdProducto;
        });

        ListadoCompra.Vars.Detalle = [];
        ListadoCompra.Vars.Detalle = arrDetalle;

        $tb.find('tbody').empty();
        if (ListadoCompra.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="9">No existen registros</td></tr>')

        } else {
            $.grep(ListadoCompra.Vars.Detalle, function (oDetalle) {
                $tb.find('tbody').append(
                     '<tr data-index=' + oDetalle["Producto"].IdMaterial + '>' +
                       '<td>' + oDetalle["Producto"].Codigo + '</td>' +
                       '<td>' + oDetalle["Producto"].NombreMat + '</td>' +
                       '<td>' + oDetalle["Unidad"].MedNom + '</td>' +
                       '<td>' + oDetalle["cantidad"] + '</td>' +
                       '<td>' + oDetalle["Precio"] + '</td>' +
                       '<td>' + oDetalle["nValorVta"].toFixed(2) + '</td>' +
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
        //   $(this).prop('disabled', 'disabled'); //disable further clicks
        if ($("#ListaProveedor").val() == -1) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Proveedor');
        }
        else if (ListadoCompra.Vars.Detalle.length <= 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'No se a llenado producto');
        }
        else if ($("#Moneda").val() == -1) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Moneda');
        }
        else {

            var oOrdenCompra = {
                IdOrden: 0,
                proveed: {
                    IdProveedor: $("#ListaProveedor").val()
                },
                Serie: $("#txtSerie").val(),
                NumeroDoc: $("#txtNumero").val(),
                Cantidad: $("#hCantidad").text(),
                SutTotal: $("#hTotalValorVta").text(),
                IGv: $("#hSumIgv").text(),
                Total: $("#hImpTotalVta").text(),
                usuario: {
                    Id: 1
                },
                Moneda: {
                    idMoneda: $("#Moneda").val()
                }
            };
            console.log(ListadoCompra.Vars.Detalle);


            $.ajax({
                async: true,
                type: 'post',
                url: General.Utils.ContextPath('GestionActivos/GeneraraOrdenCompra'),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { OrdenCompra: oOrdenCompra, OrdenCompraDetalle: ListadoCompra.Vars.Detalle },
                success: function (response) {
                    console.log(response);
                    if (response.Id != 'error') {
                        General.Utils.ShowModalMessage(response.Id, "", response.Message, function () {
                           // window.location.href = General.Utils.ContextPath('GestionActivos/ImpresionOrdenCompra?IdDoc=' + response.Additionals[0]);
                            Limpiar();
                            $("#btnLimpiar").focus();

                        });
                    }
                }
            });

        }
        // document.location.reload(true);
    });

});

function Limpiar() {
    ListadoCompra.CargarSerieNum();
    $("#ListaProveedor").val(1);

    ElimiarDatos();
    $('#hCantidad').html('0.00');
    $('#hTotalValorVta').html('0.00');
    $('#hSumIgv').html('0.00');
    $('#hImpTotalVta').html('0.00');

}
function ElimiarDatos() {

    var $btn = $(this);
    var $tb = $('#tbDetalle');
    var iIdProducto = 0;
    $.each(ListadoCompra.Vars.Detalle, function (index, value) {
        iIdProducto = value.Producto.IdMaterial;
        BuscarIndexDetalleEnTabla(iIdProducto);

        arrDetalle = ListadoCompra.Vars.Detalle.filter(function (x) {
            return x.Producto.IdMaterial != iIdProducto;
        });

        ListadoCompra.Vars.Detalle = [];
        ListadoCompra.Vars.Detalle = arrDetalle;
        $tb.find('tbody').empty();
        if (ListadoCompra.Vars.Detalle.length == 0) {
            $tb.find('tbody').append('<tr><td colspan="9">No existen registros</td></tr>')
        }
    });

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
            $("#txtPrecio").val(response.PrecioCompra.toString().replace(",", "."));
            $("#txtCantidad").focus();
        }


    });
}
function BuscarDetalleEnTabla(iIdProducto) {
    var bFound = false;
    $.each(ListadoCompra.Vars.Detalle, function (index, item) {
        if (item["Producto"].IdMaterial == iIdProducto) {
            bFound = true;
            return false;
        }
    });
    return bFound;
}

function BuscarIndexDetalleEnTabla(id) {
    for (var i = 0; i < ListadoCompra.Vars.Detalle.length; i += 1) {
        if (ListadoCompra.Vars.Detalle[i]["Producto"].IdMaterial == id) {
            return i;
        }
    }
    return -1;
}

function CalcularTotales() {
    var TotalValorVta = ImpTotalVta = SumIgv = SumDscto = cantidadlt = 0;

    $.grep(ListadoCompra.Vars.Detalle, function (oDetalle) {
        var subTotal = oDetalle["nValorVta"].toFixed(2);//sub total
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
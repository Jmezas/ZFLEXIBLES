var ListadoProductos = {
    CargarLista: function () {

        var table = $("#tbProductos").DataTable({
            "language": {
                "lengthMenu": "Mostrar _MENU_ registros por p&aacute;gina",
                "zeroRecords": "No se encontraron datos.",
                "info": "Mostrando la p&aacute;gina _PAGE_ de _PAGES_",
                "infoEmpty": "No hay registros disponibles",
                "infoFiltered": "(filtrando _MAX_ total de registros)",
                "search": "Buscar:",
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
                { "data": "Fechereg" },
                {
                    "data": "IdMaterial", "render": function (data) {
                        return '<div id=btnEditar_' + data + ' class="btn btn-warning btn-xs evento"><i class="fa fa-edit"></i> Editar</div>';
                    },
                    "orderable": false, "searchable": false, "width": "8%"
                }
            ],
            dom: 'Bfrtip',
            buttons: [


                 {
                     extend: 'pdf',
                     text: "<i class='fa fa-file-pdf-o'> PDF</i>",
                     titleAttr: "Exportar PDF",
                     className: "btn btn-danger btn-xs",
                 },
                 {

                     extend: 'excelHtml5',
                     text: "<i class='fa fa-file-excel-o'> Excel</i>",
                     titleAttr: "Exportar Excel",
                     className: "btn btn-success btn-xs",
                     customize: function (xlsx) {
                         var sheet = xlsx.xl.worksheets['sheet1.xml'];

                         $('row c[r^="C"]', sheet).attr('s', '2');

                     }
                 }
            ]

        });

    },
    CargarContenido: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Mantenimiento/ListaUnidad"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    $('select[name="Unidad"]').append($('<option>', { value: -1, text: 'Seleccione' }))
                    $.grep(response, function (oUnidadMedida) {
                        $('select[name="Unidad"]').append($('<option>', { value: oUnidadMedida["IdMed"], text: oUnidadMedida["MedNom"] }));
                        console.log(oUnidadMedida);
                    });
                }
            }

        });
    },
    CargarUnidad: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Mantenimiento/ListaUnidad"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    $('select[name="UnidadNew"]').append($('<option>', { value: -1, text: 'Seleccione' }))
                    $.grep(response, function (oUnidadMedida) {
                        $('select[name="UnidadNew"]').append($('<option>', { value: oUnidadMedida["IdMed"], text: oUnidadMedida["MedNom"] }));
                        console.log(oUnidadMedida);
                    });
                }
            }

        });
    },

  CargarCodigo: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Mantenimiento/ProductoCodigo"),
            dataType: 'json',
            success: function (response) {
               $('#txtCodigoNew').val(response.Codigo);
            }

        });
    },

    Inicializar: function () { },

    Vars: {

    },



};


$(function () {
    ListadoProductos.CargarUnidad();
    ListadoProductos.CargarLista();
    ListadoProductos.CargarContenido();
    ListadoProductos.CargarCodigo();


    $('#btnNuevo').click(function () {
        ListadoProductos.CargarCodigo();
        $("#ModalNuevo").modal("show");

        // limpiar();
        //   cargarProductos();

    });
    $("#btnCerrar").click(function () {
        limpiar();
    });
    $("#cerrarpx").click(function () {
        limpiar();
    });

    //nuevo
    $("#btnGrabarFinal").click(function () {

        var $form = $("#dvRegistroNew");
        var oDatos = General.Utils.SerializeForm($form);
        if (General.Utils.ValidateForm($form)) {
            var oProducto = {
                IdMaterial: 0,
                Codigo: oDatos["txtCodigoNew"].value,
                NombreMat: oDatos["txtNombreNew"].value,
                Descripcion: oDatos["txtDescripcionNew"].value,
                PrecioCompra: oDatos["txtPrecioCompraNew"].value,
                PrecioVenta: oDatos["txtPrecioVentaNew"].value,
                UM: {
                    IdMed: oDatos["UnidadNew"].value,
                },
                EstadoMat: "A",
                UsuarioCreador: {
                    Id: 1
                },
                Stock: 0
            };
            var precioCompra = $("#txtPrecioCompraNew").val();
            var precioVenta = $("#txtPrecioVentaNew").val();
            if (precioCompra > precioVenta) {
                General.Utils.ShowMessage('error', 'Precio de venta deve de ser mayor');
            } else {


                $.ajax({
                    async: true,
                    type: 'post',
                    url: General.Utils.ContextPath('Mantenimiento/RegistrarProducto'),
                    dataType: 'json',
                    data: oProducto,
                    success: function (response) {
                        if (response["Id"] == TypeMessage.Success) {
                            limpiar();
                            $('#ModalNuevo').modal("toggle");
                            $("#tbProductos").DataTable().ajax.reload();
                            General.Utils.ShowMessageRequest(response);

                        } else {
                            limpiar();
                            $('#ModalNuevo').modal("toggle");
                            $("#tbProductos").DataTable().ajax.reload();
                            General.Utils.ShowMessageRequest(response);
                        }
                            

                       
                    }

                });
            }
        }
    });


    //editar
    $("#btnGrabar").click(function () {

        var $form = $("#dvRegistro");
        var oDatos = General.Utils.SerializeForm($form);
        if (General.Utils.ValidateForm($form)) {
            var oProducto = {
                IdMaterial: $("#hdfProducto").val(),
                Codigo: oDatos["txtCodigo"].value,
                NombreMat: oDatos["txtNombre"].value,
                Descripcion: oDatos["txtDescripcion"].value,
                PrecioCompra: oDatos["txtPrecioCompra"].value,
                PrecioVenta: oDatos["txtPrecioVenta"].value,
                UM: {
                    IdMed: oDatos["Unidad"].value,
                },
                EstadoMat: ($("#Estado").is(':checked') === true ? 'A' : 'I'),

                UsuarioCreador: {
                    Id: 1
                },
                Stock: 0
            };
            var precioCompra = $("#txtPrecioCompra").val();
            var precioVenta = $("#txtPrecioVenta").val();
            if (precioCompra > precioVenta) {
                General.Utils.ShowMessage('error', 'Precio de venta deve de ser mayor');
            } else {

                $.ajax({
                    async: true,
                    type: 'post',
                    url: General.Utils.ContextPath('Mantenimiento/RegistrarProducto'),
                    dataType: 'json',
                    data: oProducto,
                    success: function (response) {
                        if (response["Id"] == TypeMessage.Success) {
                            $('#exampleModalCenter').modal("toggle");
                            $("#tbProductos").DataTable().ajax.reload();
                            General.Utils.ShowMessageRequest(response);
                        } else {
                            $('#exampleModalCenter').modal("toggle");
                            General.Utils.ShowMessageRequest(response);
                        }
                    }
                });
            }
        }
    });


    $("#tbProductos").on('click', 'tbody .evento', function () {

        var dataRow = $(this).parent().parent().children().first().text();
        console.log(dataRow);
        $.when(ObtenerID(dataRow)).then(function () {
            $("#exampleModalCenter").modal("show");
        });

    });



});
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
        //    General.Utils.ContextPath('GestionActivos/ImpresionOrdenCompra?id=' + 13);
            console.log(response)
            $("#hdfProducto").val(response.IdMaterial);
            $("#txtCodigo").val(response.Codigo);
            $("#txtNombre").val(response.NombreMat);
            $("#Unidad").val(response.UM.IdMed);
            $("#txtPrecioCompra").val(response.PrecioCompra.replace(",", "."));
            $("#txtPrecioVenta").val(response.PrecioVenta.replace(",", "."))
            $("#txtDescripcion").val(response.Descripcion);

            $("#Estado").prop('checked', response.EstadoMat === "A" ? true : false);
        }

    });
}






function limpiar() {
    $("#txtCodigoNew").val('');
    $("#txtNombreNew").val('');
    $("#txtDescripcionNew").val('');
    $("#txtPrecioCompraNew").val('');
    $("#txtPrecioVentaNew").val('');
    $('select[name="UnidadNew"]').val(-1)
}
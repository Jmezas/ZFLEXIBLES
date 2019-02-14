var ListaCliente = {
     cargarDocumento: function () {
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath("Shared/ListaDocumento"),
            dataType: 'json',
            success: function (response) {
                if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                    $('select[name="Documento"]').append($('<option>', { value: 0, text: 'Seleccione' }))
                    $.grep(response, function (oDocumento) {
                        $('select[name="Documento"]').append($('<option>', { value: oDocumento["Codigo"], text: oDocumento["Descripcion"] }));
                        
                    });
                }
            }

        });
     },
     cargarDocumentoNumero2: function () {
         $.ajax({
             async: true,
             type: 'post',
             url: General.Utils.ContextPath("Shared/ListaDocumento"),
             dataType: 'json',
             success: function (response) {
                 if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                     $('select[name="DocumentoEdit"]').append($('<option>', { value: 0, text: 'Seleccione' }))
                     $.grep(response, function (oDocumento) {
                         $('select[name="DocumentoEdit"]').append($('<option>', { value: oDocumento["Codigo"], text: oDocumento["Descripcion"] }));
                         
                     });
                 }
             }

         });
     }

}
$(function () {


    ListaCliente.cargarDocumento();
    ListaCliente.cargarDocumentoNumero2();

    //Lista Contenido
    var table = $("#tbCliente").DataTable({
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
            "url": General.Utils.ContextPath('Mantenimiento/ListadoCliente'),
            "contentType": "application/json",
            "type": "POST",
            "dataType": "JSON",
            "data": function (d) {
                console.log(d);
                return JSON.stringify(d);
            },

        },
        "columns": [
            { "data": "Documento.Descripcion" },
            { "data": "NroDocumento" },
            { "data": "Nombre" },
            { "data": "Telefono" },
            { "data": "Celular" },
            { "data": "Email" },
            { "data": "Direccion" },
            {
                "data": "IdCliente", "render": function (data) {
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

    $("#idrazon").hide();

    //=================
    //Evento lick
    //=================

    $('#btnNuevo').click(function () {
        $("#ModalNuevo").modal("show");
    });
    $("#cerrarpx").click(function () {
        limpiar();
    });
    $("#btnCerrar").click(function () {
        limpiar();
    });

    $("#Documento").change(function () {

        var documento = $("#Documento").val();
        if (documento == 1) {
            $("#idrazon").hide();
            $("#idNombre").show();
        }
        else if (documento == 2) {
            $("#idrazon").show();
            $("#idNombre").hide();
        }
        else {
            $("#idrazon").hide();
            $("#idNombre").show();
        }
    });
    $("#Buscar").click(function () {
        var documento = $("#Documento").val();


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
                       // console.log(response)
                        if (response.Id == 'error') {
                            General.Utils.ShowModalMessage(response.Id, "", response.Message + "Nro. de ruc invalido Ingrese los datos manualmente", function () {
                                $("#txtRazon").attr("readonly", false);
                                $("#txtDireccion").attr("readonly", false);
                            });
                        } else {
                            var valor = JSON.parse(response);
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
                    data: {
                        ruc: RUC

                    },
                    success: function (response) {
                        //console.log(response)
                        if (response.Id == 'error') {
                            General.Utils.ShowModalMessage(response.Id, "", response.Message + "Nro. de ruc invalido Ingrese los datos manualmente", function () {
                                $("#txtRazon").attr("readonly", false);
                                $("#txtDireccion").attr("readonly", false);
                            });
                        } else {
                            var valor = JSON.parse(response);
                            //$("#txtRazon").val(valor.nombre_o_razon_social);
                            //$("#txtDireccion").val(valor.direccion);
                            //$("#txtCorreo").val(valor.direccion);
                       
                            $("#txtRazon").val(valor.razon_social);
                            $("#txtDireccion").val(valor.direccion)
                        }
                    }
                });
            }
        } else if (documento == 0) {
            General.Utils.ShowMessage(TypeMessage.Error, 'Seleccione Documento');
        }

        else {
            alert("No es necesario realizar busqueda.");
        }
    });

    $("#btnGrabarFinal").click(function () {
        var $form = $("#ModalNuevo");
        var oDatos = General.Utils.SerializeForm($form);
        if (General.Utils.ValidateForm($form)) {
            var oCliente = {
                IdCliente: 0,
                Documento: {
                    Codigo: $("#Documento").val(),
                },
                NroDocumento: $("#txtDocumento").val(),
                Nombre: $("#txtRazon").val(),
                Telefono: $("#txtTelefono").val(),
                Celular: $("#txtCelular").val(),
                Email: $("#txtEmail").val(),
                Direccion: $("#txtDireccion").val(),
                Usuario: {
                    Id: 1,
                },
                Estado: ($("#Estado").is(':checked') === true ? 'A' : 'I')
            }
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
                        $("#tbCliente").DataTable().ajax.reload();
                        General.Utils.ShowMessageRequest(response);
                    } else {
                        $('#ModalNuevo').modal("toggle");
                        General.Utils.ShowMessageRequest(response);
                    }
                    limpiar();
                }
            });

        }
    });
    $("#btnGrabar").click(function () {

        var oCliente = {
            IdCliente: $("#hdfCliente").val(),
            Documento: {
                Codigo: $("#DocumentoEdit").val(),
            },
            NroDocumento: $("#txtDocumentoEdit").val(),
            Nombre: $("#txtRazonEdit").val(),
            Telefono: $("#txtTelefonoEdit").val(),
            Celular: $("#txtCelularEdit").val(),
            Email: $("#txtEmailEdit").val(),
            Direccion: $("#txtDireccionEdit").val(),
            Usuario: {
                Id: 1,
            },
            Estado: ($("#Estado").is(':checked') === true ? 'A' : 'I')
        }
        $.ajax({
            async: true,
            type: 'post',
            url: General.Utils.ContextPath('Mantenimiento/RegistrarCliente'),
            dataType: 'json',
            data: oCliente,
            success: function (response) {
                console.log(response);
                if (response["Id"] == TypeMessage.Success) {
                    $('#exampleModalCenter').modal("toggle");
                    $("#tbCliente").DataTable().ajax.reload();
                    General.Utils.ShowMessageRequest(response);
                } else {
                    $('#exampleModalCenter').modal("toggle");
                    General.Utils.ShowMessageRequest(response);
                }
                limpiar();
            }
        });
    });
   

    $("#tbCliente").on('click', 'tbody .evento', function () {
        var data = table.row($(this).parents("tr")).data();
        console.log(data);
        $.when(Obtener(data.IdCliente)).then(function () {
            $("#exampleModalCenter").modal("show");
        });
    });

});

var Obtener = function (idCliente) {
    var senData={
        IdCliente: idCliente
    }
    $.ajax({
        async: true,
        type: 'post',
        url: General.Utils.ContextPath('Mantenimiento/BuscarClienteId'),
        dataType: 'json',
        data: senData,
        success: function (response) {
            console.log(response);
            $("#hdfCliente").val(response.IdCliente);
            $("#DocumentoEdit").val(response.Documento.Codigo);
            $("#txtDocumentoEdit").val(response.NroDocumento);
            $("#txtRazonEdit").val(response.Nombre);
            $("#txtCelularEdit").val(response.Celular);
            $("#txtTelefonoEdit").val(response.Telefono);
            $("#txtEmailEdit").val(response.Email);
            $("#txtDireccionEdit").val(response.Direccion);
            $("#Estado").prop('checked', response.Estado === "A" ? true : false);

        }

    });
}

function limpiar() {
    $("#Documento").val(0);
    $("#txtDocumento").val('');
    $("#txtRazon").val('');
    $('#txtCelular').val('');
    $('#txtTelefono').val('');
    $('#txtEmail').val('');
    $("#txtDireccion").val('');
}
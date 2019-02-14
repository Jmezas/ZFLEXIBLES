var obj = {
    inicializar: function () {
        $('#txtMontoPagar').Validate({ type: 'decimal' })
    }
}

$(function () {

    obj.inicializar();
    var Id = $("#idPago").val();
    LlenarLista(Id);

 

    $("#btnGuardar").click(function () {
        var data = {
            IdPago: $("#lstDocumento").val(),
            Monto: $("#txtMontoPagar").val(),
          Factura:{
                IdVenta: $("#idPago").val()
            } 
        }
        var Mrestante = $("#txtRestante").val();
        var mPago = $("#txtMontoPagar").val();
        if (mPago == "") {
            General.Utils.ShowMessage('error', 'Ingrese Monto a pagar');
        }
          else if (Mrestante < mPago) {
            General.Utils.ShowMessage('error', 'La cantidad supera el total');
        } else {
            $.ajax({
                async: false,
                type: 'post',
                url: General.Utils.ContextPath("BandejaPago/InsrtarPago"),
                beforeSend: General.Utils.StartLoading,
                complete: General.Utils.EndLoading,
                dataType: 'json',
                data: { Comprobante: data, user: '' },
                success: function (response) {
                    if (response.Id = 'success') {
                        General.Utils.ShowModalMessage(response.Id, "", response.Message, function () {
                            var Id = $("#idPago").val();
                            window.location.href = General.Utils.ContextPath('BandejaPago/ReportePago?Codigo=' + response.Additionals[0]);
                            LlenarLista(Id);
                        });
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert(xhr.status);
                    alert(thrownError);
                }
            });
        }
    });

});

function LlenarLista(IdComprobante) {


    $.ajax({
        async: false,
        type: 'post',
        url: General.Utils.ContextPath("BandejaPago/ListComprobanteId"),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { idComprobante: IdComprobante },
        success: function (response) {
            console.log(response);
            $("#txtNumero").val(response.Factura.Serie);
            $("#txtFechaEmision").val(response.Pago.FechaPago);
            $("#txtnombre").val(response.Cliente.Nombre);
            $("#txtDireccion").val(response.Cliente.Direccion);
            $("#txtMonto").val(response.Factura.Total.replace(",", "."));
            $("#txtPagado").val(response.Pagado.replace(",", "."));
            $("#txtRestante").val(response.Pendiente.replace(",", "."));
            var TipoDoc = response.Comprobante.Id;

            ListaDoc(TipoDoc);
        }

    });
}
function ListaDoc(tipo) {

    $.ajax({
        async: false,
        type: 'post',
        url: General.Utils.ContextPath("Shared/ListaDocId"),
        beforeSend: General.Utils.StartLoading,
        complete: General.Utils.EndLoading,
        dataType: 'json',
        data: { Id: tipo },
        success: function (response) {
            console.log(response);
            if (!response.hasOwnProperty('ErrorMessage')) { // Si la petición no emitió error
                $.grep(response, function (oDocumento) {
                    $('select[name="lstDocumento"]').append($('<option>', { value: oDocumento["Id"], text: oDocumento["Nombre"] }));

                });
            }
        }
    });
}
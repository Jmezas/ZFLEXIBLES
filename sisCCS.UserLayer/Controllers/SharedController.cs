using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Mvc;

using sisCCS.BusinessLayer;
using sisCCS.UserLayer.Models;

namespace sisCCS.UserLayer.Controllers
{
    public class SharedController : Controller
    {

        Authentication Authentication = new Authentication();

        JavaScriptSerializer Serializer = new JavaScriptSerializer();
        BGeneral general = new BGeneral();


        [HttpPost]
        public void ListaDocumento()
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListaDoc()
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }
        [HttpPost]
        public void ListaMonda()
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListaMoneda()
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }

        [HttpPost]
        public void ListaUbigeo(string Tipo, string Dep, string Prov)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListarUbigeo(Tipo, Dep, Prov)
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }


        [HttpPost]
        public void ListaDocId(int Id)
        {
            try
            {
                Utils.Write(
                    ResponseType.JSON,
                    general.ListaFacBol(Id)
                );
            }
            catch (Exception Exception)
            {
                Utils.Write(
                    ResponseType.JSON,
                    "{ Code: 1, ErrorMessage: \"" + Exception.Message + "\" }"
                );
            }
        }
    }

}
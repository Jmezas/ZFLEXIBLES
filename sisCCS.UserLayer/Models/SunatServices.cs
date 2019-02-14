using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;

namespace sisCCS.UserLayer.Models
{
    public class SunatServices
    {

        //   string ValidSunatServices = "SunatProduccion";


        string ValidSunatServices = ConfigurationManager.AppSettings["ValidSunatServices"];
        //ConfigurationManager.AppSettings["ValidSunatServices"];
        // DocumentosElectronicoSunatBeta.billServiceClient service;
        //DocumentoElectronicoSunat.billServiceClient service;

        string inputsPath = "";
        string outputPath = "";

        public SunatServices(string inputsPath, string outputPath)
        {

            ServicePointManager.UseNagleAlgorithm = true;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.CheckCertificateRevocationList = true;


            this.inputsPath = inputsPath; // XML a Enviar
            this.outputPath = outputPath; // Reciben los XML de respuesta (Aceptados/Rechazados)
        }

        public string sendDocument(string NameOfFileZip)
        {
            string response = "";

            string folder = inputsPath + NameOfFileZip;
            NameOfFileZip = NameOfFileZip.Split('\\').Last();
            byte[] allbytes = File.ReadAllBytes(folder);

            string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh;mm;ss");

            try
            {
                byte[] resultBytes;
                if (ValidSunatServices == "SunatProduccion")
                {
                    DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                    service.Open();
                    resultBytes = service.sendBill(NameOfFileZip, allbytes, "");
                    service.Close();
                }
                else
                {
                    DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                    service.Open();
                    resultBytes = service.sendBill(NameOfFileZip, allbytes);
                    service.Close();
                }
                if (resultBytes != null)
                {
                    if (resultBytes.Length > 0)
                    {
                        string outputFile = outputPath + " " + FechaHora + " " + NameOfFileZip;

                        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                        {
                            fs.Write(resultBytes, 0, resultBytes.Length);
                            fs.Close();
                        }

                        using (ZipFile zip = ZipFile.Read(outputFile))
                        {
                            foreach (ZipEntry e in zip)
                            {
                                if (e.FileName == "R-" + NameOfFileZip.Replace(".zip", ".xml"))
                                {
                                    e.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                                }
                            }
                        }

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.PreserveWhitespace = true;
                        xmlDoc.Load(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));

                        string ResponseCode = "", Description = "";

                        XmlNodeList elemList = xmlDoc.GetElementsByTagName("cbc:ResponseCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            ResponseCode = elemList[i].InnerXml;
                        }

                        XmlNodeList elemList2 = xmlDoc.GetElementsByTagName("cbc:Description");
                        for (int i = 0; i < elemList2.Count; i++)
                        {
                            Description = elemList2[i].InnerXml;
                        }

                        if (ResponseCode == "0")
                        {
                            response = "success|" + ResponseCode + "&" + Description;
                        }
                        else
                        {
                            response = "error|" + ResponseCode + "&" + Description;
                        }

                        File.Delete(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));
                    }
                    else
                    {
                        response = "error|" + "0098&PENDIENTE DE APROBACION";
                    }
                }
                else
                {
                    response = "error|" + "0098&PENDIENTE DE APROBACION";
                }
                
            }
            // catch (Exception ex) 
            catch (System.ServiceModel.FaultException ex)
            {
                Console.WriteLine("EXCEPTION :/");

                Console.WriteLine("#### [DEVELOPER] ####");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("#### [/DEVELOPER] ####");

                Console.WriteLine("#### [CLIENT] ####");
                //Console.WriteLine(ex.Code.Name);
                Console.WriteLine("#### [/CLIENT] ####");


                if (ValidSunatServices == "SunatProduccion")
                {
                    DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                    service.Close();
                }
                else
                {
                    DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                    service.Close();
                }
               
                response = "error|" + ex.Code.Name.Replace("Client.", "") + "&" + ex.Message;
            }

            return response;
        }

        public string sendSummary(string NameOfFileZip)
        {
            string response = "";

            string folder = inputsPath + NameOfFileZip;
            byte[] allbytes = File.ReadAllBytes(folder);

            string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh;mm;ss");

            if (ValidSunatServices == "SunatProduccion")
            {
                try
                {

                    DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                    service.Open();
                    string resultBytes0 = service.sendSummary(NameOfFileZip, allbytes, "");
                    //DocumentosElectronicoSunatBeta.statusResponse resultBytes1 = service.getStatus(resultBytes0);
                    //DocumentoElectronicoSunat.statusResponse resultBytes1 = service.getStatus(resultBytes0);
                    //byte[] resultBytes = resultBytes1.content;

                    service.Close();

                    //string outputFile = outputPath + " " + FechaHora + " " + NameOfFileZip;
                    //using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                    //{
                    //    fs.Write(resultBytes, 0, resultBytes.Length);
                    //    fs.Close();
                    //}
                    //using (ZipFile zip = ZipFile.Read(outputFile))
                    //{
                    //    foreach (ZipEntry e in zip)
                    //    {
                    //        if (e.FileName == "R-" + NameOfFileZip.Replace(".zip", ".xml"))
                    //        {
                    //            e.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                    //        }
                    //    }
                    //}
                    //XmlDocument xmlDoc = new XmlDocument();
                    //xmlDoc.PreserveWhitespace = true;
                    //xmlDoc.Load(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));
                    //string ResponseCode = "", Description = "";
                    //XmlNodeList elemList = xmlDoc.GetElementsByTagName("cbc:ResponseCode");
                    //for (int i = 0; i < elemList.Count; i++)
                    //{
                    //    ResponseCode = elemList[i].InnerXml;
                    //}

                    //XmlNodeList elemList2 = xmlDoc.GetElementsByTagName("cbc:Description");
                    //for (int i = 0; i < elemList2.Count; i++)
                    //{
                    //    Description = elemList2[i].InnerXml;
                    //}

                    //if (ResponseCode == "0")
                    //{
                    //    response = "success|" + ResponseCode + "&" + Description + "&" + resultBytes0;
                    //}
                    //else
                    //{
                    //    response = "error|" + ResponseCode + "&" + Description + "&" + resultBytes0;
                    //}

                    //File.Delete(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));

                    response = "success|" + resultBytes0;

                }
                catch (System.ServiceModel.FaultException ex)
                {
                    Console.WriteLine("EXCEPTION :/");

                    Console.WriteLine("#### [DEVELOPER] ####");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("#### [/DEVELOPER] ####");

                    Console.WriteLine("#### [CLIENT] ####");
                    //Console.WriteLine(ex.Code.Name);
                    Console.WriteLine("#### [/CLIENT] ####");

                    //service.Close();
                    if (ValidSunatServices == "SunatProduccion")
                    {
                        DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                        service.Close();
                    }
                    else
                    {
                        DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                        service.Close();
                    }
                    response = "error|" + ex.Code.Name.Replace("Client.", "") + "&" + ex.Message + "&0";
                }

                return response;
            }
            else
            {
                try
                {
                    DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                    service.Open();
                    string resultBytes0 = service.sendSummary(NameOfFileZip, allbytes);
                    //DocumentosElectronicoSunatBeta.statusResponse resultBytes1 = service.getStatus(resultBytes0);
                    //DocumentoElectronicoSunat.statusResponse resultBytes1 = service.getStatus(resultBytes0);
                    //byte[] resultBytes = resultBytes1.content;

                    service.Close();

                    //string outputFile = outputPath + " " + FechaHora + " " + NameOfFileZip;

                    //using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                    //{
                    //    fs.Write(resultBytes, 0, resultBytes.Length);
                    //    fs.Close();
                    //}

                    //using (ZipFile zip = ZipFile.Read(outputFile))
                    //{
                    //    foreach (ZipEntry e in zip)
                    //    {
                    //        if (e.FileName == "R-" + NameOfFileZip.Replace(".zip", ".xml"))
                    //        {
                    //            e.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                    //        }
                    //    }
                    //}
                    //XmlDocument xmlDoc = new XmlDocument();
                    //xmlDoc.PreserveWhitespace = true;
                    //xmlDoc.Load(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));

                    //string ResponseCode = "", Description = "";

                    //XmlNodeList elemList = xmlDoc.GetElementsByTagName("cbc:ResponseCode");
                    //for (int i = 0; i < elemList.Count; i++)
                    //{
                    //    ResponseCode = elemList[i].InnerXml;
                    //}

                    //XmlNodeList elemList2 = xmlDoc.GetElementsByTagName("cbc:Description");
                    //for (int i = 0; i < elemList2.Count; i++)
                    //{
                    //    Description = elemList2[i].InnerXml;
                    //}

                    //if (ResponseCode == "0")
                    //{
                    //    response = "success|" + ResponseCode + "&" + Description + "&" + resultBytes0;
                    //}
                    //else
                    //{
                    //    response = "error|" + ResponseCode + "&" + Description + "&" + resultBytes0;
                    //}

                    //File.Delete(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));

                    response = "success|" + resultBytes0;

                }
                catch (System.ServiceModel.FaultException ex)
                {
                    Console.WriteLine("EXCEPTION :/");

                    Console.WriteLine("#### [DEVELOPER] ####");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("#### [/DEVELOPER] ####");

                    Console.WriteLine("#### [CLIENT] ####");
                    //Console.WriteLine(ex.Code.Name);
                    Console.WriteLine("#### [/CLIENT] ####");

                    //service.Close();
                    if (ValidSunatServices == "SunatProduccion")
                    {
                        DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                        service.Close();
                    }
                    else
                    {
                        DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                        service.Close();
                    }
                    response = "error|" + ex.Code.Name.Replace("Client.", "") + "&" + ex.Message + "&0";
                }

                return response;
            }
        }

        public string getStatus(string ticket, string NameOfFileZip)
        {
            string response = "";
            string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh;mm;ss");

            if (ValidSunatServices == "SunatProduccion")
            {
                try
                {
                    DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                    service.Open();
                    string resultBytes0 = ticket;
                    DocumentoElectronicoSunat.statusResponse resultBytes1 = service.getStatus(resultBytes0);
                    byte[] resultBytes = resultBytes1.content;

                    service.Close();

                    if (resultBytes.Length > 0)
                    {
                        string outputFile = outputPath + " " + FechaHora + " " + NameOfFileZip;

                        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                        {
                            fs.Write(resultBytes, 0, resultBytes.Length);
                            fs.Close();
                        }

                        using (ZipFile zip = ZipFile.Read(outputFile))
                        {
                            foreach (ZipEntry e in zip)
                            {
                                if (e.FileName == "R-" + NameOfFileZip.Replace(".zip", ".xml"))
                                {
                                    e.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                                }
                            }
                        }
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.PreserveWhitespace = true;
                        xmlDoc.Load(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));

                        string ResponseCode = "", Description = "";

                        XmlNodeList elemList = xmlDoc.GetElementsByTagName("cbc:ResponseCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            ResponseCode = elemList[i].InnerXml;
                        }

                        XmlNodeList elemList2 = xmlDoc.GetElementsByTagName("cbc:Description");
                        for (int i = 0; i < elemList2.Count; i++)
                        {
                            Description = elemList2[i].InnerXml;
                        }

                        if (ResponseCode == "0")
                        {
                            response = "success|" + ResponseCode + "&" + Description + "&" + resultBytes0;
                        }
                        else
                        {
                            response = "error|" + ResponseCode + "&" + Description + "&" + resultBytes0;
                        }

                        File.Delete(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));
                    }
                    else
                    {
                        response = "error|" + "0098&PENDIENTE DE APROBACION&" + ticket;
                    }
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    Console.WriteLine("EXCEPTION :/");

                    Console.WriteLine("#### [DEVELOPER] ####");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("#### [/DEVELOPER] ####");

                    Console.WriteLine("#### [CLIENT] ####");
                    //Console.WriteLine(ex.Code.Name);
                    Console.WriteLine("#### [/CLIENT] ####");

                    //service.Close();
                    if (ValidSunatServices == "SunatProduccion")
                    {
                        DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                        service.Close();
                    }
                    else
                    {
                        DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                        service.Close();
                    }
                    response = "error|" + ex.Code.Name.Replace("Client.", "") + "&" + ex.Message + "&0";
                }

                return response;
            }
            else
            {
                try
                {
                    DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                    
                    service.Open();
                    string resultBytes0 = ticket;
                    DocumentosElectronicoSunatBeta.statusResponse resultBytes1 = service.getStatus(resultBytes0);
                    byte[] resultBytes = resultBytes1.content;

                    service.Close();

                    if (resultBytes.Length > 0)
                    {
                        string outputFile = outputPath + " " + FechaHora + " " + NameOfFileZip;

                        using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                        {
                            fs.Write(resultBytes, 0, resultBytes.Length);
                            fs.Close();
                        }
                        using (ZipFile zip = ZipFile.Read(outputFile))
                        {
                            foreach (ZipEntry e in zip)
                            {
                                if (e.FileName == "R-" + NameOfFileZip.Replace(".zip", ".xml"))
                                {
                                    e.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                                }
                            }
                        }

                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.PreserveWhitespace = true;
                        xmlDoc.Load(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));

                        string ResponseCode = "", Description = "";

                        XmlNodeList elemList = xmlDoc.GetElementsByTagName("cbc:ResponseCode");
                        for (int i = 0; i < elemList.Count; i++)
                        {
                            ResponseCode = elemList[i].InnerXml;
                        }

                        XmlNodeList elemList2 = xmlDoc.GetElementsByTagName("cbc:Description");
                        for (int i = 0; i < elemList2.Count; i++)
                        {
                            Description = elemList2[i].InnerXml;
                        }

                        if (ResponseCode == "0")
                        {
                            response = "success|" + ResponseCode + "&" + Description + "&" + resultBytes0;
                        }
                        else
                        {
                            response = "error|" + ResponseCode + "&" + Description + "&" + resultBytes0;
                        }

                        File.Delete(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));
                    }
                    else
                    {
                        response = "error|" + "0098&PENDIENTE DE APROBACION&" + ticket;
                    }
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    Console.WriteLine("EXCEPTION :/");

                    Console.WriteLine("#### [DEVELOPER] ####");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("#### [/DEVELOPER] ####");

                    Console.WriteLine("#### [CLIENT] ####");
                    //Console.WriteLine(ex.Code.Name);
                    Console.WriteLine("#### [/CLIENT] ####");

                    //service.Close();
                    if (ValidSunatServices == "SunatProduccion")
                    {
                        DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                        service.Close();
                    }
                    else
                    {
                        DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                        service.Close();
                    }
                    response = "error|" + ex.Code.Name.Replace("Client.", "") + "&" + ex.Message + "&0";
                }

                return response;
            }
             
        }

        public string getStatusCdr(string NameOfFileZip)
        {
            string response = "";
            string FechaHora = DateTime.Now.ToString("yyyy-MM-dd hh;mm;ss");

            string identificador = NameOfFileZip.Split('.')[0];
            string ruc = identificador.Split('-')[0];
            string tipodoc = identificador.Split('-')[1];
            string serie = identificador.Split('-')[2];
            int numdoc = int.Parse(identificador.Split('-')[3]);

            if (ValidSunatServices == "SunatProduccion")
            {
                try
                {
                    ConsultaSunat.billServiceClient service = new ConsultaSunat.billServiceClient();
                    service.Open();
                    ConsultaSunat.statusResponse resultBytes1 = service.getStatusCdr(ruc, tipodoc, serie, numdoc);
                    byte[] resultBytes = resultBytes1.content;

                    service.Close();

                    if (resultBytes != null)
                    {
                        if (resultBytes.Length > 0)
                        {
                            string outputFile = outputPath + " " + FechaHora + " " + NameOfFileZip;

                            using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                            {
                                fs.Write(resultBytes, 0, resultBytes.Length);
                                fs.Close();
                            }

                            using (ZipFile zip = ZipFile.Read(outputFile))
                            {
                                foreach (ZipEntry e in zip)
                                {
                                    if (e.FileName == "R-" + NameOfFileZip.Replace(".zip", ".xml"))
                                    {
                                        e.Extract(outputPath, ExtractExistingFileAction.OverwriteSilently);
                                    }
                                }
                            }
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.PreserveWhitespace = true;
                            xmlDoc.Load(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));

                            string ResponseCode = "", Description = "";

                            XmlNodeList elemList = xmlDoc.GetElementsByTagName("cbc:ResponseCode");
                            for (int i = 0; i < elemList.Count; i++)
                            {
                                ResponseCode = elemList[i].InnerXml;
                            }

                            XmlNodeList elemList2 = xmlDoc.GetElementsByTagName("cbc:Description");
                            for (int i = 0; i < elemList2.Count; i++)
                            {
                                Description = elemList2[i].InnerXml;
                            }

                            if (ResponseCode == "0")
                            {
                                response = "success|" + ResponseCode + "&" + Description;
                            }
                            else
                            {
                                response = "error|" + ResponseCode + "&" + Description;
                            }

                            File.Delete(outputPath + "R-" + NameOfFileZip.Replace(".zip", ".xml"));
                        }
                        else
                        {
                            response = "error|" + "0098&PENDIENTE DE APROBACION";
                        }
                    }
                    else
                    {
                        response =  "errorR|" + resultBytes1.statusCode + "&" + resultBytes1.statusMessage;
                    }
                    
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    Console.WriteLine("EXCEPTION :/");

                    Console.WriteLine("#### [DEVELOPER] ####");
                    Console.WriteLine(ex.ToString());
                    Console.WriteLine("#### [/DEVELOPER] ####");

                    Console.WriteLine("#### [CLIENT] ####");
                    //Console.WriteLine(ex.Code.Name);
                    Console.WriteLine("#### [/CLIENT] ####");

                    //service.Close();
                    if (ValidSunatServices == "SunatProduccion")
                    {
                        DocumentoElectronicoSunat.billServiceClient service = new DocumentoElectronicoSunat.billServiceClient();
                        service.Close();
                    }
                    else
                    {
                        DocumentosElectronicoSunatBeta.billServiceClient service = new DocumentosElectronicoSunatBeta.billServiceClient();
                        service.Close();
                    }
                    response = "error|" + ex.Code.Name.Replace("Client.", "") + "&" + ex.Message + "&0";
                }
            }
            return response;
        }
    }
}
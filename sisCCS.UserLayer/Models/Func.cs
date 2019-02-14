using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace sisCCS.UserLayer.Models
{
    public static class Func
    {
        #region File Types
        /// <summary>
        /// Xml
        /// </summary>
        public const string eXtensibleMarkupLanguage = "xml";
        /// <summary>
        /// Pdf
        /// </summary>
        public const string PortableDocumentFormat = "pdf";
        /// <summary>
        /// Zip
        /// </summary>
        public const string ZIP = "zip";
        #endregion

        public static string Part(this DateTime pdDate, DatePart pdDatePart)
        {
            if (pdDate != null)
            {
                switch (pdDatePart)
                {
                    case DatePart.Day:
                        return pdDate.Day < 10 ? $"0{pdDate.Day}" : pdDate.Day.ToString();
                    case DatePart.Month:
                        return pdDate.Month < 10 ? $"0{pdDate.Month}" : pdDate.Month.ToString();
                    case DatePart.Year:
                        return pdDate.Year.ToString();
                }
            }
            return string.Empty;
        }

        public static bool ExistsParam(this HttpRequestBase poRequest, string psParam)
        {
            foreach (string lsParam in poRequest.Params)
                if (lsParam.Equals(psParam))
                    return true;
            return false;
        }

        public static string GetSimpleText(this XmlDocument poDocument, string psTag)
        {
            if (poDocument.GetElementsByTagName(psTag)[0].ChildNodes.Count == 0)
                return string.Empty;
            return poDocument.GetElementsByTagName(psTag)[0].ChildNodes.Item(0).InnerText;
        }

        public static XmlNodeList GetNode(this XmlDocument poDocument, string psTag)
        {
            return poDocument.GetElementsByTagName(psTag);
        }
    }

    public enum DatePart
    {
        Day,
        Month,
        Year
    }
}
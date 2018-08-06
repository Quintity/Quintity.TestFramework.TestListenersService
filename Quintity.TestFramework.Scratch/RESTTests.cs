using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using QTF = Quintity.TestFramework.Core;

namespace Quintity.TestFramework.Scratch
{
    [QTF.TestClass]
    public class RESTTests : QTF.TestClassBase
    {
        [QTF.TestMethod]
        public QTF.TestVerdict RESTQuery()
        {
            try
            {
                //string uri = "http://localhost/HyperVServices/VMs.svc/";
                //string uri = "http://ajax.googleapis.com/ajax/services/search/web?v=1.0&q=Earth%20Day";
                //string uri = @"http://en.wikipedia.org/wiki/Paris_Hilton";
                string uri = @"https://rally1.rallydev.com/slm/webservice/v2.0/security/authorize";

                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                SetBasicAuthHeader(webRequest, "jmothershead@quintity.com", "Invalid1!");

                //this is the default method/verb, but it's here for clarity
                webRequest.Method = "GET";

                var webResponse = (HttpWebResponse)webRequest.GetResponse();

                if (webResponse.StatusCode == HttpStatusCode.OK)
                {
                    var stream = webResponse.GetResponseStream();
                    var xr = XmlReader.Create(stream);
                    var xdoc = XDocument.Load(xr);
                    var vms = from v in xdoc.Root.Elements("VM")
                              select new { Name = v.Element("Name").Value };
                    foreach (var item in vms)
                    {
                        //Console.WriteLine(item.Name);
                    }
                }
                else
                {

                }

/*
                var xdoc = XDocument.Load(xr);

                var vms = from v in xdoc.Root.Elements("VM")
                          select new { Name = v.Element("Name").Value };
                foreach (var item in vms)
                {
                    QTF.TestTrace.Trace(item.Name);
                }
*/
                //Console.WriteLine("Response returned with status code of 0}",  webResponse.StatusCode);

                //if (webResponse.StatusCode == HttpStatusCode.OK)
                //    ProcessOKResponse(webResponse);
                //else
                //  ProcessNotOKResponse(webResponse);
                TestVerdict = QTF.TestVerdict.Pass;
            }
            catch (Exception e)
            {
                TestMessage = e.ToString();
                TestVerdict = QTF.TestVerdict.Fail;
            }
            finally
            { }

            return TestVerdict;
        }

        public void SetBasicAuthHeader(WebRequest request, String userName, String userPassword)
        {
            string authInfo = userName + ":" + userPassword;
            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
            request.Headers["Authorization"] = "Basic " + authInfo;
        }
    }
}

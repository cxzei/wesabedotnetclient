using System;   
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net.Security;
using System.Net;
using System.Xml.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace websabelib
{
    public class wesabe_rest
    {
        static wesabe_rest()
        {
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
        }

        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None &&
                certificate.Subject == @"CN=www.wesabe.com, OU=Thawte SSL123 certificate, OU=Go to https://www.thawte.com/repository/index.html, OU=Domain Validated, O=www.wesabe.com")
                return true;

            Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }


        public static Accounts getAccounts(string user, string password)
        {
            HttpWebRequest webrequest =
                (HttpWebRequest)WebRequest.Create("https://www.wesabe.com/accounts.xml");
            webrequest.KeepAlive = false;
            webrequest.Method = "GET";

            //I should have been able to use the CredentialCache, 
            // but the API isn't challenging and requesting auth. 
            // Failure always turns into a 302 redirected, \
            // so we add the headers preemptively.
            webrequest.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(user+":"+password)));
            
            using(HttpWebResponse webresponse = (HttpWebResponse)webrequest.GetResponse())
            {
                using(Stream s = webresponse.GetResponseStream())
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Accounts));
                    Accounts a = serializer.Deserialize(s) as Accounts;
                    return a;
                }
            }
        }
    }
}

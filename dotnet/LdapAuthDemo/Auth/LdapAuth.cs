using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Novell.Directory.Ldap;
using System.Net.Security;

namespace LdapAuthDemo.Auth
{
    public interface IAuth
    {
        bool Validate(string Username, string Passwd);
    }

    public class LdapAuth : IAuth
    {
        private readonly ILogger logger;
        public LdapAuth(ILogger logger)
        {
            this.logger = logger;
        }

        public bool Validate(string Username, string Passwd)
        {
            using (var cn = new LdapConnection())
            {
                try {
                    // cn.SecureSocketLayer = true;
                    // cn.UserDefinedServerCertValidationDelegate += new Novell.Directory.Ldap.RemoteCertificateValidationCallback(LdapSSLHandler);
                    cn.Connect("192.168.1.250", 389);
                    // cn.Bind(Username, Passwd);
                    cn.Bind("cn=admin,dc=example,dc=org", Passwd);

                    return cn.Connected;
                }
                catch(Exception ex) {
                    logger.LogWarning(ex.Message);
                    logger.LogWarning(ex.StackTrace);
                    return false;
                }
                
            }                
        }

        private bool LdapSSLHandler(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain,
                  System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            //if (sslPolicyErrors == SslPolicyErrors.None)
            //{
            //    return true;   //Is valid
            //}

            //if (certificate.GetCertHashString() == "YOUR CERTIFICATE HASH KEY") // Thumbprint value of the certificate
            //{
            //    return true;
            //}

            //return false;
            return true;
        }

        
    }
}

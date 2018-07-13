using APIRESTAD.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace APIRESTAD.Controllers
{
    public class GuachinangoController : ApiController
    {

        // GET api/values
        public string Get()
        {
            string resultado = "";
            try
            {
                // create LDAP connection object  


                DirectoryEntry myLdapConnection = new DirectoryEntry("LDAP://192.108.24.153/OU=FGE,DC=fiscalia,DC=test", "Administrador", "Diego01");

                DirectoryEntry newUser = myLdapConnection.Children.Add("CN=" + "mayonesa", "user");

                newUser.Properties["userPrincipalName"].Add("mayonesa" + "@" + "fiscalia.test");
                newUser.Properties["sAMAccountName"].Add("mayonesa");
                newUser.Properties["sn"].Add("McCormick");
                newUser.Properties["givenName"].Add("Mayonesa");
                newUser.Properties["title"].Add("Mejor mayonesa que Hellman's");
                newUser.Properties["department"].Add("Mayonesas chidoris");
                newUser.Properties["company"].Add("Mayonesas SA de CV");
                newUser.Properties["displayName"].Add("Mayonesa McCormick");
                newUser.Properties["mail"].Add("mayonesa@fiscaliaveracruz.gob.mx");
                newUser.CommitChanges();

                newUser.Properties["objectClass"].Add("top");
                newUser.Properties["objectClass"].Add("person");
                newUser.Properties["objectClass"].Add("organizationalPerson");
                newUser.Properties["objectClass"].Add("user");

                newUser.Invoke("SetPassword", new object[] { "Temporal01" });
                newUser.CommitChanges();

                newUser.Invoke("Put", new object[] { "userAccountControl", "512" });
                newUser.CommitChanges();

                myLdapConnection.Close();
                newUser.Close();

                // create search object which operates on LDAP connection object  
                // and set search object to only find the user specified  

                DirectorySearcher search = new DirectorySearcher(myLdapConnection);
                search.Filter = "(sAMAccountName=ola)";

                // create results objects from search object  

                SearchResult result = search.FindOne();

                if (result != null)
                {
                    // user exists, cycle through LDAP fields (cn, telephonenumber etc.)  

                    ResultPropertyCollection fields = result.Properties;

                    foreach (String ldapField in fields.PropertyNames)
                    {
                        // cycle through objects in each field e.g. group membership  
                        // (for many fields there will only be one object such as name)  

                        foreach (Object myCollection in fields[ldapField])
                            resultado += (String.Format("{0,-20} : {1} \n",
                                          ldapField, myCollection.ToString()));
                    }
                }
                else
                {
                    // user does not exist  
                    resultado = "User not found!";
                }
            }

            catch (Exception e)
            {
                resultado = "Exception caught:\n\n" + e.ToString();
            }
            return resultado;
        }
    }
}

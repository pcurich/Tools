using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace XpathAndXlinq.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var helper = new XmlHelper(@"Example/AccountsNS");
            //var arrayAccounts = helper.ReadXml("urn:accounting", "account");
            //foreach (var account in arrayAccounts )
            //{
            //    Console.Out.WriteLine(account);    
            //}

            //var sum = helper.SumValueInteger("urn:accounting", "account");
            //Console.Out.WriteLine(sum);
            //Console.ReadKey();


            Console.WriteLine(">>>>>>>> using xpath");
            var doc = XElement.Load(@"Example/AccountsNS.xml");
            var query = doc
                .GetElementsUsingXPath(
                    "/self::a:accounts/a:account", "a:urn:accounting");

            var xElements = query as IList<XElement> ?? query.ToList();
            foreach (var q in xElements)
            {
                Console.WriteLine(q);
            }
            
            var sum = (int) (double) doc.GetEvaluationUsingXPath(
                "sum(/self::a:accounts/a:account)", "a:urn:accounting");
            Console.WriteLine(sum);
            Console.ReadKey();
        }
    }
}
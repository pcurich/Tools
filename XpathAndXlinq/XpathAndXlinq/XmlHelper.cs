using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XpathAndXlinq
{
    public static class XmlHelper
    {
        //public string FileName { get; set; }
        //private string PathFileName { set; get; }
        private static readonly Regex DeclarationPattern =
            new Regex(
                @"(?x:^xmlns:(?<prefix>[^:\s]+)=(?<namespace>[^\s]+)$|
                 ^(?<prefix>[^:\s]+):(?<namespace>[^\s]+)$)$",
                RegexOptions.Compiled);

        //public XmlHelper(string fileName)
        //{
        //    FileName = fileName;
        //    if (!fileName.Contains(".xml"))
        //        FileName = fileName + ".xml";

        //    PathFileName = AppDomain.CurrentDomain.BaseDirectory + "/" + FileName;
        //}

        //public IList<String> ReadXml(string ns="",string node="")
        //{ 
        //    var doc = XElement.Load(PathFileName);
        //    if (!"".Equals(ns))
        //        ns = "{" + ns + "}";

        //    var query = from ele in doc.Elements(XName.Get(node,ns))
        //        select ele.ToString();
        //    return query.ToList();
        //}

        //public int SumValueInteger(string ns = "", string node = "")
        //{
        //    var doc = XElement.Load(PathFileName);
        //    if (!"".Equals(ns))
        //        ns = "{" + ns + "}";

        //    var query = from ele in doc.Elements(XName.Get(node, ns))
        //                select ele;
        //    return query.Sum(ele=> int.Parse(ele.Value));
        //}

        public static IEnumerable<XElement> GetElementsUsingXPath(this XNode node, string xpath,
            params string[] namespacesDeclarations)
        {
            var nav = node.CreateNavigator();
            var xnm = GetXmlNamespaceManager(nav, namespacesDeclarations);

            var q = from ele in GetNodesUsingXPath(nav, xpath, xnm)
                select (XElement) ele;
            return q ;
        }

        public static object GetEvaluationUsingXPath(this XNode node, string xpath,
            params string[] namespacesDeclarations)
        {
            var nav = node.CreateNavigator();
            var xnm = GetXmlNamespaceManager(nav, namespacesDeclarations);

            return nav.Evaluate(xpath, xnm);
        }

        #region Util

        private static IEnumerable<XNode> GetNodesUsingXPath(XPathNavigator nav, string xpath, IXmlNamespaceResolver nm)
        {
            var itr = nav.Select(xpath, nm);
            while (itr.MoveNext())
            {
                var uo = itr.Current.UnderlyingObject;
                yield return uo as XNode;
            }
        }

        private static XmlNamespaceManager GetXmlNamespaceManager(XPathNavigator nav,
            params string[] namespacesDeclararions)
        {
            if (nav.NameTable == null)
                return null;

            var xnm = new XmlNamespaceManager(nav.NameTable);
            if (namespacesDeclararions == null)
                return xnm;

            foreach (var namespacesDeclararion in namespacesDeclararions)
            {
                var match = DeclarationPattern.Match(namespacesDeclararion);
                if (!match.Success)
                {
                    throw new ArgumentException("Incorrecta declaracion de NameSpace", "namespacesDeclararions");
                }
                xnm.AddNamespace(match.Groups["prefix"].Value, match.Groups["namespace"].Value);
            }
            if (string.IsNullOrEmpty(xnm.LookupNamespace("xhtml")))
                xnm.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");
            return xnm;
        }

        #endregion
    }
}
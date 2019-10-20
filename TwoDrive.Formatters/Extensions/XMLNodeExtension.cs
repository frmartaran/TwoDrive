using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using TwoDrive.Formatters.Exceptions;

namespace TwoDrive.Formatters.Extensions
{
    public static class XMLNodeExtension
    {
        public static void ValidateDateNodes(this XmlElement parentNode, 
            XmlNodeList creationDateNodes, XmlNodeList dateModifiedNodes)
        {
            if (!parentNode.ExistsChildNode(creationDateNodes))
                throw new FormatterException("Missing Creation Date Tag");

            if (!parentNode.ExistsChildNode(dateModifiedNodes))
                throw new FormatterException("Missing Date Modified Tag");
        }

        public static bool ExistsChildNode(this XmlElement rootNode, XmlNodeList nodeList)
        {
            return nodeList.Count != 0 && nodeList.Item(0).ParentNode == rootNode;
        }
    }
}

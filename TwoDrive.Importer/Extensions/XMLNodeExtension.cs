using System.Xml;
using TwoDrive.Importer.Interface.Exceptions;

namespace TwoDrive.Importers.Extensions
{
    public static class XMLNodeExtension
    {
        public static void ValidateDateNodes(this XmlElement parentNode, 
            XmlNodeList creationDateNodes, XmlNodeList dateModifiedNodes)
        {
            if (!parentNode.ExistsChildNode(creationDateNodes))
                throw new ImporterException("Missing Creation Date Tag");

            if (!parentNode.ExistsChildNode(dateModifiedNodes))
                throw new ImporterException("Missing Date Modified Tag");
        }

        public static bool ExistsChildNode(this XmlElement rootNode, XmlNodeList nodeList)
        {
            return nodeList.Count != 0 && nodeList.Item(0).ParentNode == rootNode;
        }
    }
}

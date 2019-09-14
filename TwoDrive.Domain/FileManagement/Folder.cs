using System.Collections.Generic;

namespace TwoDrive.Domain.FileManagement
{
    public class Folder : Element
    {
        public IEnumerable<Element> FolderChilden { get; set; }
    }
    
}
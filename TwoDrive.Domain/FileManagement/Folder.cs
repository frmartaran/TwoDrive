using System.Collections.Generic;

namespace TwoDrive.Domain.FileManagement
{
    public class Folder : Element
    {
        public ICollection<Element> FolderChilden { get; set; }
    }
    
}
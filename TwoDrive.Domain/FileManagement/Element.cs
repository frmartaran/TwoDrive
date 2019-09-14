namespace TwoDrive.Domain.FileManagement
{
    public abstract class Element
    {
        public string Name { get; set; }

        public Folder ParentFolder { get; set; }
    }
}
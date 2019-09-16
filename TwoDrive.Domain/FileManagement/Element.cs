namespace TwoDrive.Domain.FileManagement
{
    public abstract class Element : IIdentifiable
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public Folder ParentFolder { get; set; }

        public Writer Owner { get; set; }        
    }
}
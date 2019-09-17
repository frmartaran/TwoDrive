namespace TwoDrive.Domain.FileManagement
{
    public abstract class Element : IIdentifiable
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public Folder ParentFolder { get; set; }

        public Writer Owner { get; set; }        

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var toElement = (Element) obj;
            return Id == toElement.Id;
        }
        
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
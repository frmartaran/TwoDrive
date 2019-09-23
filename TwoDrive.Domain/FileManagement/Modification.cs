
namespace TwoDrive.Domain.FileManagement
{
    public class Modification
    {

        public int Id { get; set; }

        public int ElementId { get; set; }
        public Element ElementModified { get; set; }

        public ModificationType type { get; set; }


    }
}
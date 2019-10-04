using System;
using TwoDrive.Domain.Interface;

namespace TwoDrive.Domain.FileManagement
{
    public class Modification
    {

        public int Id { get; set; }
        public int ElementId { get; set; }
        public Element ElementModified { get; set; }
        public ModificationType type { get; set; }
        public DateTime Date { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var modification = (Modification)obj;
            return Id == modification.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
using System;

namespace TwoDrive.DataAccess.Interface.LogicInput
{
    public class FileFilter
    {
        public string Name { get; set; } = "";

        public bool IsOrderDescending { get; set; }

        public bool IsOrderByName { get; set; }

        public bool IsOrderByCreationDate { get; set; }

        public bool IsOrderByModificationDate { get; set; }

        public int? Id { get; set; }
    }
}

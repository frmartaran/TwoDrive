using System;

namespace TwoDrive.Domain.Interface
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }

        DateTime DeletedDate { get; set; }
    }
}

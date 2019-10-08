using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoDrive.DataAccess.Interface.LogicInput;
using TwoDrive.Domain.FileManagement;

namespace TwoDrive.DataAccess.Extensions
{
    public static class FileRepositoryExtension
    {
        public static List<File> OrderByDescending(FileFilter filter, IQueryable<File> filesFilteredByName)
        {
            IOrderedQueryable<File> orderIntermediateResult = new EnumerableQuery<File>(filesFilteredByName);
            if (filter.IsOrderByName)
            {
                orderIntermediateResult = filesFilteredByName.OrderByDescending(f => f.Name);
            }
            if (filter.IsOrderByModificationDate)
            {
                orderIntermediateResult = !filter.IsOrderByName
                    ? orderIntermediateResult.OrderByDescending(f => f.DateModified)
                    : orderIntermediateResult.ThenByDescending(f => f.DateModified);
            }
            if (filter.IsOrderByCreationDate)
            {
                orderIntermediateResult = !(filter.IsOrderByName || filter.IsOrderByModificationDate)
                    ? orderIntermediateResult.OrderByDescending(f => f.CreationDate)
                    : orderIntermediateResult.ThenByDescending(f => f.CreationDate);
            }
            return orderIntermediateResult.ToList();
        }

        public static List<File> OrderBy(FileFilter filter, IQueryable<File> filesFilteredByName)
        {
            IOrderedQueryable<File> orderIntermediateResult = new EnumerableQuery<File>(filesFilteredByName);
            if (filter.IsOrderByName)
            {
                orderIntermediateResult = filesFilteredByName.OrderBy(f => f.Name);
            }
            if (filter.IsOrderByModificationDate)
            {
                orderIntermediateResult = !filter.IsOrderByName
                    ? orderIntermediateResult.OrderBy(f => f.DateModified)
                    : orderIntermediateResult.ThenBy(f => f.DateModified);
            }
            if (filter.IsOrderByCreationDate)
            {
                orderIntermediateResult = !(filter.IsOrderByName || filter.IsOrderByModificationDate)
                    ? orderIntermediateResult.OrderBy(f => f.CreationDate)
                    : orderIntermediateResult.ThenBy(f => f.CreationDate);
            }
            return orderIntermediateResult.ToList();
        }
    }
}

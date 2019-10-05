
using System.Collections.Generic;
using System.Linq;

namespace TwoDrive.WebApi.Models
{
    public abstract class Model<E, M>
    where E : class
    where M : Model<E, M>, new()
    {

        protected abstract E ToEntity(M model);

        protected abstract M ToModel(E entity);

        public static E ToDomain(M model)
        {
            if (model == null)
                return null;
            return model.ToEntity(model);
        }

        public static M FromDomain(E entity)
        {
            if (entity == null)
                return null;
            return new M().ToModel(entity);
        }

        public static ICollection<E> AllToEntity(ICollection<M> models)
        {
            return models.Select(m => ToDomain(m))
                    .ToList();
        }

        public static ICollection<M> AllToModel(ICollection<E> entities)
        {
            return entities.Select(e => FromDomain(e)).ToList();
        }
    }
}
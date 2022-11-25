using System;
using Domain.CoreEntities;

namespace Data.Extensions
{
    public static class EntityExtension
    {
        public static void PopulateMetaData(this BaseEntity entity)
        {
            if (entity.Id == 0)
                entity.CreationTs = DateTime.UtcNow;
        }
    }
}

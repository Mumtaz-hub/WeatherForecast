using Domain.CoreEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations.CoreConfigurations
{
    public abstract class BaseEntityTypeConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TBase> entityTypeBuilder)
        {
            //Base Configuration
        }
    }
}
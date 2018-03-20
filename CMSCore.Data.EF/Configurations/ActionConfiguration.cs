using CMSCore.Data.EF.Extensions;
using CMSCore.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMSCore.Data.EF.Configurations
{
    public class ActionConfiguration : DbEntityConfiguration<Action>
    {
        public override void Configure(EntityTypeBuilder<Action> entity)
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).IsRequired()
                .HasMaxLength(128)
            .HasColumnType("varchar(128)");
            // etc.
        }
    }
}
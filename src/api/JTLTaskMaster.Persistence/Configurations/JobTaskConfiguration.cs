using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JTLTaskMaster.Domain.Entities;

namespace JTLTaskMaster.Persistence.Configurations;

public class JobTaskConfiguration : IEntityTypeConfiguration<JobTask>
{
    public void Configure(EntityTypeBuilder<JobTask> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Parameters)
            .HasColumnType("jsonb");

        builder.Property(x => x.Order)
            .IsRequired();
    }
}
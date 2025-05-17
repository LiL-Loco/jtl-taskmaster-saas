using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using JTLTaskMaster.Domain.Entities;

namespace JTLTaskMaster.Persistence.Configurations;

public class JobConfiguration : IEntityTypeConfiguration<Job>
{
    public void Configure(EntityTypeBuilder<Job> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasMaxLength(1000);

        builder.HasMany(x => x.Tasks)
            .WithOne(x => x.Job)
            .HasForeignKey(x => x.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
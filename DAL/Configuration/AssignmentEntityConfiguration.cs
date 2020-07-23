using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configuration
{
    /// <summary>
    /// Performs configuration of the Assignment entity.
    /// </summary>
    internal class AssignmentEntityConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(a => a.Priority)
                .IsRequired();

            builder.Property(a => a.CompletionPercent)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired();

            builder.HasOne(a => a.Project)
                .WithMany(p => p.Assignments)
                .HasForeignKey(a => a.ProjectId);

            builder.HasOne(a => a.Assignee)
                .WithMany(u => u.IssuedAssignments)
                .HasForeignKey(a => a.AssigneeId);
        }
    }
}
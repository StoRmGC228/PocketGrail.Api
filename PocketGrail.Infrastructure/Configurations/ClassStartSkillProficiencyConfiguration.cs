namespace PocketGrail.Infrastructure.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.Domain.Entities.ClassEntities;

internal sealed class ClassStartSkillProficiencyConfiguration : IEntityTypeConfiguration<ClassStartSkillProficiency>
{
    public void Configure(EntityTypeBuilder<ClassStartSkillProficiency> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Skill).IsRequired();
    }
}

namespace PocketGrail.DataAccess.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PocketGrail.DataAccess.Entities.ClassEntities;

internal sealed class ClassStartSkillProficiencyConfiguration : IEntityTypeConfiguration<ClassStartSkillProficiency>
{
    public void Configure(EntityTypeBuilder<ClassStartSkillProficiency> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Skill).IsRequired();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGrail.Domain.Entities.ClassEntities
{
    using Characters;

    public class Class : BaseEntity
    {
        public int Level { get; set; }
        public string Name { get; set; }
        public string ShortDesсription { get; set; }
        public string SpellAbility { get; set; }
        public int TotalHitDice { get; set; }
        public int UsedHitDice { get; set; }
        public string HitDice { get; set; } = string.Empty;
        public int SkillChoiceCount { get; set; }
        public IEnumerable<Subclass> Subclasses { get; set; }
        public IEnumerable<CharacterClass> Characters { get; set; }
        public IEnumerable<ClassFeature> ClassFeatures { get; set; }
        public List<ClassSavingThrowProficiency> SavingThrows { get; set; } = [];
        public List<ClassSpellSlotTemplate> SpellSlotTemplates { get; set; } = [];
        public List<MulticlassPrerequisite> MulticlassPrerequisites { get; set; } = [];
    }
}
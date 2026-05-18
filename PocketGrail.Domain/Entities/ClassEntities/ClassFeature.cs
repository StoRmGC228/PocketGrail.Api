using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketGrail.Domain.Entities.ClassEntities
{
    public class ClassFeature : Feature
    {
        public int GainingLevel { get; set; }
        public string Name { get; set; }
        public string DescriptionText { get; set; }

        public int ClassId { get; set; }
        public Class SourceClass { get; set; }
    }
}
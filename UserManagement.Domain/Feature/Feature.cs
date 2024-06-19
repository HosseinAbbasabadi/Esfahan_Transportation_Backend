using System.Collections.Generic;
using PhoenixFramework.Domain;

namespace UserManagement.Domain.Feature
{
    public class Feature : AggregateRootBase<int>
    {
        public int? ParentId { get; private set; }
        public string Title { get; private set; }
        public string Name { get; private set; }
        public int? Sort { get; private set; }
        public int SystemId { get; private set; }
        public long? ClassificationLevelId { get; private set; }
        public bool? IsPage { get; private set; }
        public Feature Parent { get; private set; }
        public IList<Feature> Children { get; private set; }

        public void SetClassificationLevel(long classificationLevelId)
        {
            ClassificationLevelId = classificationLevelId;
        }
    }
}
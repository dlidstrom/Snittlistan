using System;
using EventStoreLite;

namespace Snittlistan.Web.Areas.V2.ReadModels
{
    public class AggregateIdReadModel : IReadModel
    {
        public AggregateIdReadModel(string aggregateId, Type type)
        {
            AggregateId = aggregateId;
            Type = type;
        }

        public string Id { get; private set; }

        public string AggregateId { get; private set; }

        public Type Type { get; private set; }
    }
}
#nullable enable

namespace Snittlistan.Web.Infrastructure.Database
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;
    using Snittlistan.Queue.Messages;

    public class PublishedTask
    {
        public PublishedTask(
            ITask task,
            int tenantId,
            Guid correlationId,
            Guid? causationId,
            Guid messageId)
        {
            Task = task;
            BusinessKey = task.BusinessKey;
            TenantId = tenantId;
            CorrelationId = correlationId;
            CausationId = causationId;
            MessageId = messageId;
            CreatedDate = DateTime.Now;
        }

        private PublishedTask()
        {
        }

        public int PublishedTaskId { get; private set; }

        public int TenantId { get; private set; }

        public Guid CorrelationId { get; private set; }

        public Guid? CausationId { get; private set; }

        public Guid MessageId { get; private set; }

        [NotMapped]
        public BusinessKey BusinessKey
        {
            get => JsonConvert.DeserializeObject<BusinessKey>(BusinessKeyColumn)!;
            private set => BusinessKeyColumn = JsonConvert.SerializeObject(value);
        }

        [NotMapped]
        public ITask Task
        {
            get => (ITask)JsonConvert.DeserializeObject(DataColumn)!;

            private set => DataColumn = JsonConvert.SerializeObject(value);
        }

        [Column("BusinessKey")]
        public string BusinessKeyColumn { get; private set; } = null!;

        [Column("Data")]
        public string DataColumn { get; private set; } = null!;

        public DateTime CreatedDate { get; private set; }
    }
}

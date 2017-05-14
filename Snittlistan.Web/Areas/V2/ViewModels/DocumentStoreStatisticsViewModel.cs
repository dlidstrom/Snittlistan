using System;
using System.Collections.Generic;
using Raven.Abstractions.Data;

namespace Snittlistan.Web.Areas.V2.ViewModels
{
    public class DocumentStoreStatisticsViewModel
    {
        public DocumentStoreStatisticsViewModel(DatabaseStatistics databaseStatistics)
        {
            ActualIndexingBatchSize = databaseStatistics.ActualIndexingBatchSize ?? new ActualIndexingBatchSize[0];
            ApproximateTaskCount = databaseStatistics.ApproximateTaskCount;
            CountOfDocuments = databaseStatistics.CountOfDocuments;
            CountOfIndexes = databaseStatistics.CountOfIndexes;
            CurrentNumberOfItemsToIndexInSingleBatch = databaseStatistics.CurrentNumberOfItemsToIndexInSingleBatch;
            CurrentNumberOfItemsToReduceInSingleBatch = databaseStatistics.CurrentNumberOfItemsToReduceInSingleBatch;
            DatabaseId = databaseStatistics.DatabaseId;
            DatabaseTransactionVersionSizeInMB = databaseStatistics.DatabaseTransactionVersionSizeInMB;
            Errors = databaseStatistics.Errors ?? new ServerError[0];
            Extensions = databaseStatistics.Extensions ?? new ExtensionsLog[0];
            InMemoryIndexingQueueSize = databaseStatistics.InMemoryIndexingQueueSize;
            Indexes = databaseStatistics.Indexes ?? new IndexStats[0];
            LastAttachmentEtag = databaseStatistics.LastAttachmentEtag;
            LastDocEtag = databaseStatistics.LastDocEtag;
            Prefetches = databaseStatistics.Prefetches ?? new FutureBatchStats[0];
            StaleIndexes = databaseStatistics.StaleIndexes ?? new string[0];
            Triggers = databaseStatistics.Triggers ?? new DatabaseStatistics.TriggerInfo[0];
        }

        public ActualIndexingBatchSize[] ActualIndexingBatchSize { get; private set; }
        public long ApproximateTaskCount { get; private set; }
        public long CountOfDocuments { get; private set; }
        public int CountOfIndexes { get; private set; }
        public int CurrentNumberOfItemsToIndexInSingleBatch { get; private set; }
        public int CurrentNumberOfItemsToReduceInSingleBatch { get; private set; }
        public Guid DatabaseId { get; private set; }
        // ReSharper disable once InconsistentNaming
        public decimal DatabaseTransactionVersionSizeInMB { get; private set; }
        public ServerError[] Errors { get; private set; }
        public IEnumerable<ExtensionsLog> Extensions { get; private set; }
        public IndexStats[] Indexes { get; private set; }
        public int InMemoryIndexingQueueSize { get; private set; }
        public Guid LastAttachmentEtag { get; private set; }
        public Guid LastDocEtag { get; private set; }
        public FutureBatchStats[] Prefetches { get; private set; }
        public string[] StaleIndexes { get; private set; }
        public DatabaseStatistics.TriggerInfo[] Triggers { get; private set; }
    }
}
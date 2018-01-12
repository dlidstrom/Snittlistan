namespace Snittlistan.Queue.Messages
{
    public class TaskRequest
    {
        public TaskRequest(string taskJson)
        {
            TaskJson = taskJson;
        }

        public string TaskJson { get; }
    }
}
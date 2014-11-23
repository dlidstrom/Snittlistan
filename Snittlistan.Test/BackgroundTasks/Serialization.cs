using NUnit.Framework;
using Snittlistan.Test.ApiControllers;
using Snittlistan.Web.Infrastructure.BackgroundTasks;

namespace Snittlistan.Test.BackgroundTasks
{
    [TestFixture]
    public class Serialization : WebApiIntegrationTest
    {
        [Test]
        public void SerializesCorrectly()
        {
            // Arrange
            var task = BackgroundTask.Create(new MyClass(5));
            Transact(session => session.Store(task));

            // Act
            BackgroundTask stored = null;
            Transact(session => stored = session.Load<BackgroundTask>(task.Id));

            // Assert
            Assert.That(stored, Is.Not.Null);
            var body = stored.Body as MyClass;
            Assert.That(body, Is.Not.Null);
            Assert.That(body.Data, Is.EqualTo(5));
        }

        public class MyClass
        {
            public MyClass(int data)
            {
                Data = data;
            }

            public int Data { get; private set; }
        }
    }
}
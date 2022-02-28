#nullable enable

using Snittlistan.Queue;

namespace Snittlistan.Web.Infrastructure;

public class MsmqFactory
{
    private readonly string path;

    public MsmqFactory(string path)
    {
        this.path = path;
    }

    public MsmqGateway Create()
    {
        return new(path);
    }
}

using Xunit.Abstractions;

namespace Xunit.KRunner
{
    public interface IMessageSinkFactory
    {
        IMessageSink CreateMessageSink(TestingContext context);
    }
}
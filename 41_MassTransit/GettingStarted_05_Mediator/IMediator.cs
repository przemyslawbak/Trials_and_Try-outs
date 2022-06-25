using MassTransit;

namespace GettingStarted
{
    public interface IMediator :
        ISendEndpoint,
        IPublishEndpoint,
        IClientFactory,
        IConsumePipeConnector,
        IRequestPipeConnector,
        IConsumeObserverConnector,
        IConsumeMessageObserverConnector
    {
    }
}

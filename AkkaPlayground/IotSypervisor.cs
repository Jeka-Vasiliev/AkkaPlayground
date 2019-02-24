using Akka.Actor;
using Akka.Event;

namespace AkkaPlayground
{
    public class IotSypervisor : UntypedActor
    {
        public ILoggingAdapter Log { get; } = Context.GetLogger();
        protected override void PreStart() => Log.Info("IoT application started");
        protected override void PostStop() => Log.Info("IoT application stopped");

        protected override void OnReceive(object message)
        {
        }

        public static Props Props() => Akka.Actor.Props.Create<IotSypervisor>();
    }
}

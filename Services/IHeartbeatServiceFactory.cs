using HR.Shared;

namespace HR.Services {
    public interface IHeartbeatServiceFactory {
        string GetIdentifier();
        string GetName();

        IHeartbeatService CreateService(KVArgs args);
    }
}

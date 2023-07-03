
namespace HR.Services {
    public interface IHeartbeatService {
        event Action<int> OnHeartbeat;
    }
}

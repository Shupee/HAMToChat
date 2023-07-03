using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Services {
    public class ServiceDiscovery {
        public static IHeartbeatServiceFactory GetFactory (string service) {
            try {
                var factoryInterface = typeof(IHeartbeatServiceFactory);
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var types = assemblies.SelectMany(assembly => assembly.GetTypes());
                var implementations = types.Where(type => type.GetInterfaces().Contains(factoryInterface));

                foreach (var type in implementations) {
                    var implementation = (IHeartbeatServiceFactory)Activator.CreateInstance(type);

                    string serviceName = implementation.GetName();
                    string identifier = implementation.GetIdentifier();

                    if (service == identifier) {
                        return implementation;
                    }
                }
            } catch (Exception ex) {
                Logger.Error($"Error occured during service discovery: [{ex.GetType().FullName}] {ex.Message}");
            }

            return null;
        }
    }
}

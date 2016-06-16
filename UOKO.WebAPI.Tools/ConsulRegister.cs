using System;
using Consul;

namespace UOKO.WebAPI.Tools
{
    public static class ConsulRegister
    {
        public class ServiceInfo
        {
            public string Name { get; set; }
            public int Port { get; set; }
        }

        /// <summary>
        /// 設置 consul httpaddr ，默認 http://127.0.0.1:8500
        /// </summary>
        /// <param name="httpAddr">address:port</param>
        public static void SetConsulHttpAddr(string httpAddr)
        {
            Environment.SetEnvironmentVariable("CONSUL_HTTP_ADDR", httpAddr);
        }

        public static void RegisterService(ServiceInfo service)
        {
            var serviceReg = new AgentServiceRegistration
                             {
                                 Name = service.Name,
                                 Port = service.Port,
                                 Checks = new AgentServiceCheck[]
                                          {
                                              new AgentCheckRegistration()
                                              {
                                                  Name = service.Name,
                                                  TCP = $"localhost:{service.Port}",
                                                  Notes = "tcp default check",
                                                  Interval = TimeSpan.FromSeconds(10),
                                                  Timeout = TimeSpan.FromSeconds(2),
                                              },
                                          },
                             };

            RegisterService(serviceReg);
        }

        public static void RegisterService(AgentServiceRegistration serviceReg)
        {
            using (var consulClient = new ConsulClient())
            {
                consulClient.Agent.ServiceRegister(serviceReg);
            }
        }

    }
}
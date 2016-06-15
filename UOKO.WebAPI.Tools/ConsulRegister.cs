using Consul;

namespace UOKO.WebAPI.Tools
{
    public class ConsulRegister
    {

        public void RegisteService(string port)
        {
            
            var consulClient = new ConsulClient();
            consulClient.Agent.ServiceRegister(new AgentServiceRegistration()
                                               {
                                                   Name =,
                                                   Address =,
                                                   Port =,
                                                   Checks = new[]
                                                            {
                                                                new AgentCheckRegistration()
                                                                {
                                                                    TCP = "",
                                                                    Name = "",
                                                                    Notes = "",
                                                                    Interval = ,
                                                                    Timeout = ,
                                                                },
                                                            },
                                               });


        }

        
    }
}
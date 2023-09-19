using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MonitoringTask.Startup))]
namespace MonitoringTask
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

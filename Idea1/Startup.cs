using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Idea1.Startup))]
namespace Idea1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

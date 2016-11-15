using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cosevi.SIBOAC.Startup))]
namespace Cosevi.SIBOAC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GamesControl.Web.Startup))]
namespace GamesControl.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

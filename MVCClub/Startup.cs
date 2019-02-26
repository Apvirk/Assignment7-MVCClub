using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVCClub.Startup))]
namespace MVCClub
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

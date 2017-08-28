using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CGrimShoppingApp.Startup))]
namespace CGrimShoppingApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

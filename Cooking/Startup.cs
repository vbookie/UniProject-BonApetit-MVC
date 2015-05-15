using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Cooking.Startup))]
namespace Cooking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

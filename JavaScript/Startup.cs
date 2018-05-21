using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JavaScript.Startup))]
namespace JavaScript
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

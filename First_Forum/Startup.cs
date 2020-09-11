using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(First_Forum.Startup))]
namespace First_Forum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

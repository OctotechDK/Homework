using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Homework.Startup))]
namespace Homework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

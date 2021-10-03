using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChangeUserProfileSample.Startup))]
namespace ChangeUserProfileSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

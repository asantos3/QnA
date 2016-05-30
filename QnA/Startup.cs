using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QnA.Startup))]
namespace QnA
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ApartmentCash.Startup))]
namespace ApartmentCash
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}

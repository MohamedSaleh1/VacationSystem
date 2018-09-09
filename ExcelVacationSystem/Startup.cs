using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(ExcelVacationSystem.Startup))]
namespace ExcelVacationSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

        }

    }
}

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.Owin;
using Owin;
using WebGrease.Css.Extensions;

[assembly: OwinStartup(typeof(Chat.DataService.Startup))]

namespace Chat.DataService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            InitializeAutoMapper();

            app.UseWebApi(config);
        }

        private void InitializeAutoMapper()
        {
            var profileType = typeof(Profile);
            // Get an instance of each Profile in the executing assembly.
            var profiles = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => profileType.IsAssignableFrom(t)
                    && t.GetConstructor(Type.EmptyTypes) != null)
                .Select(Activator.CreateInstance)
                .Cast<Profile>();

            // Initialize AutoMapper with each instance of the profiles found.
            Mapper.Initialize(a => profiles.ForEach(a.AddProfile));

            Mapper.AssertConfigurationIsValid();
        }
    }
}

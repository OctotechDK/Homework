using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Homework.Business;
using Homework.Business.Models;
using Homework.Business.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Homework.DependencyInjection
{
    public class DependencyInjectionConfig
    {
        public static void RegisterContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ApplicationDbContext>().As<IApplicationDbContext>().InstancePerLifetimeScope();

            var dbContextParameter = new ResolvedParameter((pi, ctx) => pi.ParameterType == typeof(DbContext),
                (pi, ctx) => ctx.Resolve<IApplicationDbContext>());

            builder.RegisterType<UserStore<ApplicationUser>>().As<IUserStore<ApplicationUser>>().WithParameter(dbContextParameter).InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.Register(c => HttpContext.Current.GetOwinContext().Authentication);
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterFilterProvider();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Custom types
            builder.RegisterType<DrawRepository>().As<IDrawRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubmissionsRepository>().As<ISubmissionsRepository>().InstancePerLifetimeScope();

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}

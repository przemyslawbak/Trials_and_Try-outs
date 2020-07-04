using Autofac;
using Financial.DAL;
using Financial.ViewModels;
using Financial.Views;
using Microsoft.EntityFrameworkCore;

namespace Financial
{
    public class Startup
    {
        /// <summary>
        /// IoC container
        /// </summary>
        /// <returns></returns>
        public static IContainer BootStrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<EFSampleRepository>()
                .As<ISampleRepository>();

            builder.RegisterType<MainView>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<SampleDbContext>()
                .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Portfolio_Strona;Trusted_Connection=True;MultipleActiveResultSets=true");

            builder.RegisterType<SampleDbContext>()
            .WithParameter("options", dbContextOptionsBuilder.Options)
            .InstancePerLifetimeScope();

            return builder.Build();
        }


    }
}

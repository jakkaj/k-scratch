using System.Reflection;
using Autofac;
using ks.model.Contract.Services;
using ks.model.Services;

namespace ks.model.Glue
{
    public class CoreGlue
    {
        protected ContainerBuilder Builder;

        public virtual void Init(params Autofac.Module[] module)
        {
            Builder = new ContainerBuilder();

            Builder.RegisterAssemblyTypes(typeof(CoreGlue).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Repo"))
                .AsImplementedInterfaces()
                .SingleInstance();

            Builder.RegisterType<FileWatcherService>().As<IFileWatcherService>().InstancePerDependency();

            if (module != null)
            {
                foreach (var mod in module)
                {
                    Builder.RegisterModule(mod);
                }
            }

            Container = Builder.Build();
        }

        public Autofac.IContainer Container { get; protected set; }
    }
}

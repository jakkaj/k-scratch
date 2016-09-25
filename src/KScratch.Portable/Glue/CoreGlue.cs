using System.Reflection;
using Autofac;

namespace KScratch.Portable.Glue
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
                .InstancePerDependency();

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

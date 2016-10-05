using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace ks.Glue
{
    public class KSModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandProcessor>().AsSelf();
            base.Load(builder);
        }
    }
}

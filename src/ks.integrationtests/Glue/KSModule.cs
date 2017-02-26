using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ks.Impl;
using ks.model.Contract.Services;

namespace ks.Glue
{
    public class KSModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConsoleService>().As<IConsoleService>();
            
            base.Load(builder);
        }
    }
}

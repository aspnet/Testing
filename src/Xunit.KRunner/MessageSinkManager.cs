using System;
using System.Linq;
using System.Reflection;
using Microsoft.Framework.DependencyInjection;

namespace Xunit.KRunner
{
    public class MessageSinkManager : IMessageSinkManager
    {
        private readonly IServiceProvider _services;

        public MessageSinkManager(IServiceProvider services)
        {
            _services = services;
        }

        public IMessageSinkFactory GetMessageSinkFactory(string messageSinkFactoryIdentifier)
        {
            if (string.IsNullOrEmpty(messageSinkFactoryIdentifier))
            {
                throw new ArgumentException("TODO: string null or empty message", "messageSinkFactoryIdentifier");
            }

            var nameParts = Utilities.SplitTypeName(messageSinkFactoryIdentifier);
            string typeName = nameParts.Item1;
            string assemblyName = nameParts.Item2;

            var assembly = Assembly.Load(new AssemblyName(assemblyName));
            if (assembly == null)
            {
                throw new Exception(String.Format("TODO: assembly {0} failed to load message", assemblyName));
            }

            Type type = null;
            Type interfaceInfo;
            if (string.IsNullOrEmpty(typeName))
            {
                foreach (var typeInfo in assembly.DefinedTypes)
                {
                    interfaceInfo = typeInfo.ImplementedInterfaces.FirstOrDefault(interf => interf.Equals(typeof(IMessageSinkFactory)));
                    if (interfaceInfo != null)
                    {
                        type = typeInfo.AsType();
                    }
                }

                if (type == null)
                {
                    throw new Exception(String.Format("TODO: type {0} failed to load message", typeName ?? "<null>"));
                }
            }
            else
            {
                type = assembly.GetType(typeName);

                if (type == null)
                {
                    throw new Exception(String.Format("TODO: type {0} failed to load message", typeName ?? "<null>"));
                }

                interfaceInfo = type.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(interf => interf.Equals(typeof(IMessageSinkFactory)));

                if (interfaceInfo == null)
                {
                    throw new Exception("TODO: IServerFactory interface not found");
                }
            }

            return (IMessageSinkFactory)ActivatorUtilities.GetServiceOrCreateInstance(_services, type);
        }
    }
}

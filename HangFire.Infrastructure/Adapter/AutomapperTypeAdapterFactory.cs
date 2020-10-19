namespace HangFire.Infrastructure.Adapter
{
    using AutoMapper;
    using System;
    using System.Linq;
    using System.Reflection;

    public class AutomapperTypeAdapterFactory
        : ITypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        public AutomapperTypeAdapterFactory()
        {
            //////scan all assemblies finding Automapper Profile
            ////var profiles = AppDomain.CurrentDomain
            ////    .GetAssemblies()
            ////    .SelectMany(a => a.GetTypes())
            ////    .Where(t => t.BaseType == typeof(Profile));

            ////Mapper.Initialize(cfg =>
            ////{
            ////    foreach (var item in profiles)
            ////    {
            ////        if (item.FullName != "AutoMapper.SelfProfiler`2")
            ////            cfg.AddProfile(Activator.CreateInstance(item) as Profile);
            ////    }
            ////});

            var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Auth0API.Application.SeedWork") && a.FullName.Contains("DTO")).ToArray();
            var allTypes = assembliesToScan
                .Where(a => a.GetName().Name != nameof(AutoMapper))
                .SelectMany(a => a.DefinedTypes)
                .ToArray();

            var profiles =
                allTypes
                    .Where(t => typeof(Profile).GetTypeInfo().IsAssignableFrom(t))
                    .Where(t => !t.IsAbstract);

            Mapper.Initialize(cfg =>
            {
                foreach (var profile in profiles.Select(t => t.AsType()))
                {
                    cfg.AddProfile(profile);
                }
            });
        }

        #endregion

        #region ITypeAdapterFactory Members

        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}

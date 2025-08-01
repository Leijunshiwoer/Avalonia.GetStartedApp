using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp.AutoMapper
{
    public class AppMapper : IAppMapper
    {
        public AppMapper()
        {
            var configuration = new MapperConfiguration(configure =>
            {
                var assemblys = AppDomain.CurrentDomain.GetAssemblies();
                configure.AddMaps(assemblys);
            },new LoggerFactory());
            Current = configuration.CreateMapper();
        }

        public IMapper Current { get; }

        public TDestination Map<TDestination>(object source) => Current.Map<TDestination>(source);
    }
}

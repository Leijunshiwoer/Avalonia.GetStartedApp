using AutoMapper;

namespace GetStartedApp.AutoMapper
{
    public interface IAppMapper
    {
        IMapper Current { get; }

        TDestination Map<TDestination>(object source);
    }
}
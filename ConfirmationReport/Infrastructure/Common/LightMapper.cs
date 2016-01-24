using System;
using AutoMapper;

namespace ConfirmRep.Infrastructure.Common
{
    public class LightMapper : IMapper
    {
        private readonly IMappingEngine engine;

        public LightMapper(IMappingEngine engine)
        {
            Contract.Requires<ArgumentNullException>(engine != null, "IMappingEngine");
            this.engine = engine;
        }

        public TDestination Map<TDestination>(object source)
        {
            return engine.Map<TDestination>(source);
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return engine.Map<TDestination>(source);
        }
    }
}
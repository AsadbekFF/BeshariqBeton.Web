﻿namespace BeshariqBeton.Web.Infrastructure
{
    public interface IMapper
    {
        TDestination Map<TSource, TDestination>(TSource source);
    }
}

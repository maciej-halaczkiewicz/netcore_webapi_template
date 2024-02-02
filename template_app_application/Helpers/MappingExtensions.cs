using AutoMapper;

namespace template_app_application.Helpers;

public static class MappingExtensions
{
    public static Task<PageableList<TDestination>> PaginatedListAsync<TSource,TDestination>(this IQueryable<TSource> queryable, int pageNumber, int pageSize, IConfigurationProvider configuration)
    {
        return PageableList<TDestination>.CreateAsync<TDestination, TSource>(queryable, pageNumber, pageSize, configuration);
    }

}
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace template_app_application.Helpers;

public class PageableList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    // Required for json deserialization
    public PageableList()
    {
    }

    public PageableList(List<T> items, int count, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = count;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PageableList<TDestination>> CreateAsync<TDestination, TSource>(IQueryable<TSource> source, int pageNumber, int pageSize, IConfigurationProvider configuration)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ProjectTo<TDestination>(configuration).ToListAsync();

        return new PageableList<TDestination>(items, count, pageNumber, pageSize);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using MusicCatalog.Entities;

namespace MusicCatalog.SearchStrategies
{
    public class NameSearchStrategy : ISearchStrategy
    {
        public List<object> Search(Catalog catalog, object criteria, SearchType searchType)
        {
            switch (searchType) 
            {
                case SearchType.Artist:
                    return catalog.GetArtists()
                        .Where(a => a.Name.IndexOf(criteria.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                        .Cast<object>()
                        .ToList();
                case SearchType.Album:
                    return catalog.GetAlbums()
                        .Where(al => al.Title.IndexOf(criteria.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                        .Cast<object>()
                        .ToList();
                case SearchType.Track:
                    return catalog.GetTracks()
                        .Where(tr => tr.Title.IndexOf(criteria.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                        .Cast<object>()
                        .ToList();
                case SearchType.All:
                    return catalog.GetArtists()
                        .Where(a => a.Name.IndexOf(criteria.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                        .Cast<object>()
                        .Concat(catalog.GetAlbums()
                        .Where(al => al.Title.IndexOf(criteria.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                        .Cast<object>())
                        .Concat(catalog.GetTracks()
                        .Where(tr => tr.Title.IndexOf(criteria.ToString(), StringComparison.OrdinalIgnoreCase) >= 0)
                        .Cast<object>())
                        .ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchType), searchType, null);
            }

        }
    }
}

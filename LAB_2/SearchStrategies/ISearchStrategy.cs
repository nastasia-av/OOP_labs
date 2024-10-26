using System;
using System.Collections.Generic;
using MusicCatalog.Entities;

namespace MusicCatalog.SearchStrategies
{
    public interface ISearchStrategy
    {
        List<object> Search(Catalog catalog, object criteria, SearchType searchType);
    }

    public enum SearchType { 
        Artist,
        Album,
        Track,
        All
    }

}


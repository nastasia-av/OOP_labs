using System;
using System.Collections.Generic;
using System.Linq;
using MusicCatalog.Entities;

namespace MusicCatalog.SearchStrategies
{
    public class DurationSearchStrategy : ISearchStrategy
    {
        public List<object> Search(Catalog catalog, object criteria, SearchType searchType)
        {
            if (searchType == SearchType.Track)
            {
                if (criteria is Tuple<TimeSpan, TimeSpan> durations) {
                    TimeSpan minDuration = durations.Item1;
                    TimeSpan maxDuration = durations.Item2;
                    return catalog.GetTracks()
                        .Where(tr => tr.Duration >= minDuration && tr.Duration <= maxDuration)
                        .Cast<object>()
                        .ToList();
                }

                throw new ArgumentOutOfRangeException("Criteria must be tuple for duration search");
            }

            throw new ArgumentOutOfRangeException("Duration search is only for tracks");
        }
    }
}

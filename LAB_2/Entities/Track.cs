using System;

namespace MusicCatalog.Entities
{
    public class Track
    {
        public string Title { get; private set; }
        public TimeSpan Duration { get; private set; }
        public string Genre { get; private set; }
        public Album Album { get; private set; }

        private Track() { }
        public static TrackBuilder Builder() => new TrackBuilder();
        public class TrackBuilder
        {
            private readonly Track _track = new Track();

            public TrackBuilder SetTitle(string title)
            {
                _track.Title = title;
                return this;
            }

            public TrackBuilder SetDuration(TimeSpan duration)
            {
                _track.Duration = duration;
                return this;
            }

            public TrackBuilder SetGenre(string genre)
            {
                _track.Genre = genre;
                return this;
            }

            public TrackBuilder SetAlbum(Album album)
            {
                _track.Album = album;
                return this;
            }

            public Track Build() => _track;
        }
    }
}


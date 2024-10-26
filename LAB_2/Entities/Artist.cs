using System.Windows.Documents;
using System.Collections.Generic;

namespace MusicCatalog.Entities
{
    public class Artist
    {
        public string Name { get; private set; }
        public int YearOfDebut { get; private set; }
        private Artist() { }
        public static ArtistBuilder Builder() => new ArtistBuilder();

        public class ArtistBuilder
        {
            private readonly Artist _artist = new Artist();

            public ArtistBuilder SetName(string name)
            {
                _artist.Name = name;
                return this;
            }
            public ArtistBuilder SetYear(int year)
            {
                _artist.YearOfDebut = year;
                return this;
            }

            public Artist Build() => _artist;
        }
    }
}


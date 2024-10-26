using System;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace MusicCatalog.Entities
{
    public class Album
    {
        public string Title { get; private set; }
        public DateTime ReleaseDate { get; private set; }
        public Artist Artist { get; private set; }

        private Album() { }

        public static AlbumBuilder Builder() => new AlbumBuilder();

        public class AlbumBuilder
        {
            private readonly Album _album = new Album();

            public AlbumBuilder SetTitle(string title)
            {
                _album.Title = title;
                return this;
            }
            public AlbumBuilder SetReleaseDate(DateTime date)
            {
                _album.ReleaseDate = date;
                return this;
            }

            public AlbumBuilder SetArtist(Artist artist)
            {
                _album.Artist = artist;
                return this;
            }

            public Album Build() => _album;
        }
    }
}



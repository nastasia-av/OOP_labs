using MusicCatalog.SearchStrategies;
using System.Windows.Documents;
using System.Collections.Generic;
using System.Windows.Navigation;

namespace MusicCatalog.Entities
{
    public class Catalog
    {
        private List<Artist> Artists { get; set; }
        private List<Album> Albums { get; set; }
        private List<Track> Tracks { get; set; }

        public Catalog()
        {
            Artists = new List<Artist>();
            Albums = new List<Album>();
            Tracks = new List<Track>();
        }

        public void AddArtist(Artist artist)
        {
            Artists.Add(artist);
        }

        public void AddAlbum(Album album)
        {
            Albums.Add(album);
        }

        public void AddTrack(Track track)
        {
            Tracks.Add(track);
        }

        public List<Artist> GetArtists() => Artists;
        public List<Album> GetAlbums() => Albums;
        public List<Track> GetTracks() => Tracks;
    }
}

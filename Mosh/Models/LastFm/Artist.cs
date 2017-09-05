using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mosh.Models.LastFm
{
    public class TopArtistsRoot
    {
        public TopArtists TopArtists { get; set; }
    }

    public class TopArtists
    {
        public List<Artist> Artist { get; set; }

        public TopArtists()
        {
            Artist = new List<Artist>();
        }
    }

    public class Artist
    {
        public string Name { get; set; }
        public int PlayCount { get; set; }
        public string Mbid { get; set; }
        public string Url { get; set; }
    }
}

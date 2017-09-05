using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mosh.Models.Data
{
    public class MoshContext : DbContext
    {
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Release> Releases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mosh.db");
        }
    }

    public class Artist
    {
        public int ArtistId { get; set; }
        public int Mbid { get; set; }
        public string Name { get; set; }

        public List<Release> Releases { get; set; }
    }

    public class Release
    {
        public int ReleaseId { get; set; }
        public int Mbid { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string ReleaseDate { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }
    }
}

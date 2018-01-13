using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComicBookShared.Models;

namespace ComicBookShared.Data
{
    public class ArtistsRepository : BaseRepository<Artist>
    {
        public ArtistsRepository(Context context) : base(context)
        {
        }

        public override Artist Get(int id, bool includeRelatedEntities = true)
        {
            var artists = Context.Artists.AsQueryable();

            if (includeRelatedEntities)
            {
                artists = artists
                    .Include(a => a.ComicBooks.Select(cb => cb.ComicBook.Series))
                    .Include(a => a.ComicBooks.Select(cb => cb.Role));
            }

            return artists
                .Where(a => a.Id == id)
                .SingleOrDefault();
        }

        public override IList<Artist> GetList()
        {
            return Context.Artists
                .OrderBy(s => s.Name)
                .ToList();
        }

        public bool ArtistExsist(int artistId, string name)
        {
            return Context.Artists.Any(s => s.Id != artistId && s.Name == name);
        }
    }
}

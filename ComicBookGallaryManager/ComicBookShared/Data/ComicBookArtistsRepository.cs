using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComicBookShared.Models;

namespace ComicBookShared.Data
{
    public class ComicBookArtistsRepository:BaseRepository<ComicBookArtist>
    {

        public ComicBookArtistsRepository(Context context)
            :base(context)
        {
        }

        public override ComicBookArtist Get(int id, bool includeRelatedEntities = true)
        {
            var comicbookArtists = Context.ComicBookArtists.AsQueryable();

            if (includeRelatedEntities)
            {
                comicbookArtists = comicbookArtists
                    .Include(cba => cba.ComicBook.Series)
                    .Include(cba => cba.Artist)
                    .Include(cba => cba.Role);
            }

            return comicbookArtists
                .Where(cba => cba.Id == (int)id)
                .FirstOrDefault();
        }

        public override IList<ComicBookArtist> GetList()
        {
            throw new NotImplementedException();
        }
    }
}

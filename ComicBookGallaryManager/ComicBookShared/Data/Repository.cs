using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ComicBookShared.Models;

namespace ComicBookShared.Data
{
    public class Repository
    {
        private Context _context = null;

        public Repository(Context context)
        {
            _context = context;
        }

        public IList<ComicBook> GetComicBooks()
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .OrderBy(cb => cb.Series.Title)
                .ThenBy(cb => cb.IssueNumber)
                .ToList();
        }

        public ComicBook GetComicBook(int id)
        {
            return _context.ComicBooks
                 .Include(cb => cb.Series)
                 .Where(cb => cb.Id == id)
                 .SingleOrDefault();
        }

        public ComicBook GetComicBookDetails(int id)
        {
            return _context.ComicBooks
                .Include(cb => cb.Series)
                .Include(cb => cb.Artists.Select(a => a.Artist))
                .Include(cb => cb.Artists.Select(a => a.Role))
                .Where(cb => cb.Id == id)
                .SingleOrDefault();
        }

        public ComicBookArtist GetComicBookArtist(int? id)
        {
            return _context.ComicBookArtists.Include(cba => cba.ComicBook.Series)
                .Include(cba => cba.Artist)
                .Include(cba => cba.Role)
                .Where(cba => cba.Id == (int)id)
                .FirstOrDefault();
        }

        public bool IsComicBookArtistExist(int artistId, int roleId, int comicBookId)
        {
            return _context.ComicBookArtists.Any(cba => cba.ComicBookId == comicBookId &&
                                                        cba.ArtistId == artistId &&
                                                        cba.RoleId == roleId);
        }

        public bool IsIssueNumberExsist(int comicBookId, int seriesId, int issueNumber)
        {
            return _context.ComicBooks.Any(cb => cb.Id != comicBookId &&
                                                cb.SeriesId == seriesId &&
                                                cb.IssueNumber == issueNumber);
        }

        public void DeleteComicBook(int id)
        {
            var comicBook = new ComicBook() { Id = id };
            _context.Entry(comicBook).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void DeleteComicBookArtist(int id)
        {
            var comicBookArtist = new ComicBookArtist() { Id = id };
            _context.Entry(comicBookArtist).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void AddComicBookArtist(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);

            _context.SaveChanges();
        }

        public void AddComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);

            if (comicBook.Series != null && comicBook.Series.Id > 0)
            {
                _context.Entry(comicBook.Series).State = EntityState.Unchanged;
            }

            foreach (ComicBookArtist artist in comicBook.Artists)
            {
                if (artist.Artist != null && artist.Artist.Id > 0)
                {
                    _context.Entry(artist.Artist).State = EntityState.Unchanged;
                }

                if (artist.Role != null && artist.Role.Id > 0)
                {
                    _context.Entry(artist.Role).State = EntityState.Unchanged;
                }
            }

            _context.SaveChanges();
        }

        public void EditComicBook(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public SelectList OrderSeries()
        {
            return new SelectList(
                _context.Series.OrderBy(s => s.Title).ToList(),
                "Id", "Title");
        }

        public SelectList OrderArtists()
        {
            return new SelectList(
                _context.Artists.OrderBy(a => a.Name).ToList(),
                "Id", "Name");
        }

        public SelectList OrderRoles()
        {
            return new SelectList(
                _context.Roles.OrderBy(r => r.Name).ToList(),
                "Id", "Name");
        }
    }
}

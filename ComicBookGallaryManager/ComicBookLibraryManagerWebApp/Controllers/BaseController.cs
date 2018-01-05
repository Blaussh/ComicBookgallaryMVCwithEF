using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ComicBookShared.Data;

namespace ComicBookLibraryManagerWebApp.Controllers
{
    public abstract class BaseController : Controller
    {
        private bool _disposed = false;

        private Context _context = null;

        protected Repository Repository { get; private set; }

        public BaseController()
        {
            _context = new Context();
            Repository = new Repository(_context);
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}

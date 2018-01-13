using System;
using System.Collections;
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

        public IEnumerable GetRoles()
        {
            return _context.Roles
                .OrderBy(r => r.Name)
                .ToList();
        }

    }
}

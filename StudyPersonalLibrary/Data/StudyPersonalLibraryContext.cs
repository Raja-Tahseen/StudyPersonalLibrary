using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudyPersonalLibrary.Models;

namespace StudyPersonalLibrary.Data
{
    public class StudyPersonalLibraryContext : DbContext
    {
        public StudyPersonalLibraryContext (DbContextOptions<StudyPersonalLibraryContext> options)
            : base(options)
        {
        }

        public DbSet<StudyPersonalLibrary.Models.Book> Book { get; set; } = default!;
    }
}

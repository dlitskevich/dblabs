using System;

using System.Collections;

using dbLabs.Classes;
using Microsoft.EntityFrameworkCore;

namespace dbLabs {
    class ElectionContext : DbContext {

        public ElectionContext() : base(SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(),"DBConnection").Options) {
        }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<CandidateProfile> CandidateProfiles { get; set; }
        public DbSet<Promise> Promises { get; set; }
        public DbSet<Confident> Confidents { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
    public class Candidate {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public float Rating { get; set; }

        //public CandidateProfile CandidateProfile { get; set; }
        public ICollection<Confident> Confidents { get; set; }
        public ICollection<CandidatePromise> CandidatePromise { get; set; }

        public Candidate() {
            Confidents = new List<Confident>();
            CandidatePromise = new List<CandidatePromise>();
        }
    }
}

using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
    public class Promise {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<Candidate> Candidates { get; set; }
        public Promise() {
            Candidates = new List<Candidate>();
        }
    }
}

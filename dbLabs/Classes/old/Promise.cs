using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
    public class Promise {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<CandidatePromise> CandidatePromise { get; set; }
        public Promise() {
            CandidatePromise = new List<CandidatePromise>();
        }
    }
}

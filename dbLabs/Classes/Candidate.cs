using System;
using System.Collections.Generic;

namespace dbLabs.Classes {
    public class Candidate {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public float Rating { get; set; }
        public ICollection<Confident> Confidents { get; set; }
        //public ICollection<Promise> Promises { get; set; }
        public Candidate() {
            Confidents = new List<Confident>();
            //Promises = new List<Promise>();
        }
    }
}

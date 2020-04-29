using System;
namespace dbLabs.Classes {
	public class CandidatePromise {
		public int CandidateId { get; set; }
		public Candidate Candidate { get; set; }
		public int PromiseId { get; set; }
		public Promise Promise { get; set; }
	}
}

using System;


namespace dbLabs.Classes {
	public class Confident {
		public int Id { get; set; }
		public string FullName { get; set; }
		public string PoliticalPreferences { get; set; }
		public int Age { get; set; }
		public int? CandidateId { get; set; }
		public virtual Candidate Candidate { get; set; }
		
	}
}

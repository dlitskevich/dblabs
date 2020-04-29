using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dbLabs.Classes {
	public class CandidateProfile {
		//[Key]
		//[ForeignKey("Candidate")]
		public int Id { get; set; }
		public int Age { get; set; }
		public string Description { get; set; }
		public int CandidateId { get; set; }
		public Candidate Candidate { get; set; }
	}
}

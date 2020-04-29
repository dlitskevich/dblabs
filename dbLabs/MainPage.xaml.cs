using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Configuration;
using System.Diagnostics;
//using Syncfusion.Data.Extensions;
using AppKit;
using dbLabs.Classes;

namespace dbLabs {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage {

		//private string connString = "server=127.0.0.1; user=root; password=Password; database=autoschool";



		public MainPage() {

			using(ElectionContext db = new ElectionContext()) {
				// Creates the database if not exists
				db.Database.EnsureCreated();


				//
				

				//
				Candidate c1 = new Candidate { Name = "Alex", Surname = "Lukashenko", Rating = 35 };
				Candidate c2 = new Candidate { Name = "Zianon", Surname = "Pozniak", Rating = 65 };
				Candidate c3 = new Candidate { Name = "Maria", Surname = "Vasilevish", Rating = 0 };
				//db.Candidates.AddRange(new List<Candidate>() { c1, c2, c3 });
				db.Candidates.Add(c1);
				db.Candidates.Add(c2);
				db.Candidates.Add(c3);
				db.SaveChanges();
				
				Confident conf1 = new Confident { FullName = "Alex Matskevich", Age = 40, PoliticalPreferences = "Neutral", Candidate = c1 };
				Confident conf2 = new Confident { FullName = "Karyna Kluchnik", Age = 18, PoliticalPreferences = "Monarch", Candidate = c2 };
				Confident conf3 = new Confident { FullName = "Tanya Verbovich", Age = 25, PoliticalPreferences = "Liberal", Candidate = c2 };
				//db.Confidents.AddRange(new List<Confident>() { conf1, conf2, conf3 });
				db.Confidents.Add(conf1);
				db.Confidents.Add(conf2);
				db.Confidents.Add(conf3);

				Promise prom1 = new Promise { Text = "Stop War" };
				Promise prom2 = new Promise { Text = "Increase revenue" };

				// many-to-many
				prom1.CandidatePromise = new List<CandidatePromise> {
					new CandidatePromise {Candidate = c1, Promise = prom1},
					new CandidatePromise {Candidate = c2, Promise = prom1},
				};
				prom2.CandidatePromise = new List<CandidatePromise> {
					new CandidatePromise {Candidate = c1, Promise = prom2}
				};

				db.Promises.AddRange(new List<Promise> { prom1, prom2 });
				//db.Promises.Add(prom1);
				//db.Promises.Add(prom2);

				db.SaveChanges();
				CandidateProfile prof1 = new CandidateProfile { Id = c1.Id, Age = 50, Description = "Current president" };
				CandidateProfile prof2 = new CandidateProfile { Id = c2.Id, Age = 40, Description = "Legend" };
				//db.CandidateProfiles.AddRange(new List<CandidateProfile> { prof1, prof2 });
				db.CandidateProfiles.Add(prof1);
				db.CandidateProfiles.Add(prof2);

				db.SaveChanges();

				
				/*
				prom1.Candidates.Add(c1);
				prom2.Candidates.Add(c1);
				prom2.Candidates.Add(c2);
				*/
			}

			InitializeComponent();


		}

		public void PageLoaded( ) {

		}
	}
}

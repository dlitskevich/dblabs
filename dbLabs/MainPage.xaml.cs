using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Xamarin.Forms;

namespace dbLabs {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible (false)]
	public partial class MainPage : ContentPage {
		private DataSet shopDS = new DataSet("Shop");
		private string connString = string.Empty;
		private MySqlDataAdapter manufAdapter;
		private MySqlDataAdapter productAdapter;

		public MainPage ()
		{
			InitializeComponent ();
		}
	}
}

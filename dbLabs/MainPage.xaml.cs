using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Xamarin.Forms;
using System.Configuration;

namespace dbLabs {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible (false)]
	public partial class MainPage : ContentPage {
		private DataSet shopDS = new DataSet("Shop");
		private string connString = "server=127.0.0.1; user=root; password=Password; database=shop_example";
		private MySqlDataAdapter manufAdapter;
		private MySqlDataAdapter productAdapter;

		private MySqlCommandBuilder manufCommands;
		private MySqlCommandBuilder productCommands;

		public MainPage (){
			InitializeComponent ();
			manufAdapter = new MySqlDataAdapter("select * from manufact", connString);
			productAdapter = new MySqlDataAdapter("select * from product", connString);

			manufCommands = new MySqlCommandBuilder(manufAdapter);
			productCommands = new MySqlCommandBuilder(productAdapter);

			manufAdapter.Fill(shopDS,"manufact");
			productAdapter.Fill(shopDS, "product");

			DataRelation manufToProduct = new DataRelation("ManufProduct",
				shopDS.Tables["manufact"].Columns["manuf_id"],
				shopDS.Tables["product"].Columns["prod_manuf_id"]);

			shopDS.Relations.Add(manufToProduct);

		}

		private void PageLoaded(object sender, EventArgs e) {
			manufGrid.ItemsSource = shopDS.Tables["manufact"].DefaultView;
		}

		private void testbutton_Click(object sender, EventArgs e) {
			var result = shopDS.Tables["manufact"].Select();
			resultGrid.IsVisible = true;
			DataTable dt = new DataTable();
			resultGrid.ItemsSource = result.CopyToDataTable<DataRow>();
		}
	}
}

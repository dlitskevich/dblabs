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
		private MySqlDataAdapter customerAdapter;

		private MySqlCommandBuilder manufCommands;
		private MySqlCommandBuilder productCommands;
		private MySqlCommandBuilder customerCommands;

		public MainPage (){
			InitializeComponent ();
			manufAdapter = new MySqlDataAdapter("select * from manufact", connString);
			productAdapter = new MySqlDataAdapter("select * from product", connString);
			customerAdapter = new MySqlDataAdapter("select * from customer", connString);

			manufCommands = new MySqlCommandBuilder(manufAdapter);
			productCommands = new MySqlCommandBuilder(productAdapter);
			customerCommands = new MySqlCommandBuilder(customerAdapter);

			manufAdapter.Fill(shopDS,"manufact");
			productAdapter.Fill(shopDS, "product");
			customerAdapter.Fill(shopDS, "customer");

			DataRelation manufToProduct = new DataRelation("ManufProduct",
				shopDS.Tables["manufact"].Columns["manuf_id"],
				shopDS.Tables["product"].Columns["prod_manuf_id"]);

			shopDS.Relations.Add(manufToProduct);

		}

		private void PageLoaded(object sender, EventArgs e) {
			manufGrid.ItemsSource = shopDS.Tables["manufact"].DefaultView;
			resultGrid.ItemsSource = shopDS.Tables["product"];
		}

		//private void testbutton_Click(object sender, EventArgs e) {
		//	//var result = shopDS.Tables["manufact"].Select("manuf_name = 'dbDealer'");

		//	/*
		//	var propertyCollection = manufGrid.View.GetPropertyAccessProvider();
		//	int compID = (int)propertyCollection.GetValue(manufGrid.SelectedItem, "id");
		//	*/
		//	/*
		//	var compID = (int)(manufGrid.SelectedItem as DataRowView).Row.ItemArray[0];
		//	var result = shopDS.Tables["manufact"].Select($"manuf_id = '{compID}'")[0].GetChildRows("ManufProduct");
		//	*/
		//	/* !
		//	DataView quantityView = new DataView(shopDS.Tables["product"]) {
		//		RowFilter = "prod_quantity > 410"
		//	};*/
		//	var result = from m in shopDS.Tables["product"].AsEnumerable()
		//				 where ((string)m["prod_type"] == "entertaining" && (int)m["prod_quantity"] > 1000)
		//				 select m;
		//	resultGrid.IsVisible = true;
		//	resultGrid.ItemsSource = result.CopyToDataTable().DefaultView;
		//}
		
		private void Filter(object sender, EventArgs e) {
			
			var result = from m in shopDS.Tables["product"].AsEnumerable()
						where
						(float.TryParse(minPrice.Text, out float min) ? min : 0) < (float)m["prod_price"]
						&& (float)m["prod_price"] < (float.TryParse(maxPrice.Text, out float max) ? max : 200)						
					 select m;

			IEnumerable<DataRow> resultQuery;
			if(byType.IsChecked && byName.IsChecked) {
				resultQuery = result.OrderBy(x => x.ItemArray[2]).ThenBy(x => x.ItemArray[1]);
			}else if(byName.IsChecked) {
				resultQuery = result.OrderBy(x => x.ItemArray[1]);
			} else if(byType.IsChecked) {
				resultQuery = result.OrderBy(x => x.ItemArray[2]);
			} else {
				resultQuery = result;
			};
 

			resultGrid.IsVisible = true;
			resultGrid.ItemsSource = resultQuery.CopyToDataTable().DefaultView;
		}

		private void FindProduct(object sender, EventArgs e) {

			var result = from m in shopDS.Tables["product"].AsEnumerable()
						 where ((string)m["prod_name"]).Contains(productPattern.Text.ToString())
						 select m;

			IEnumerable<DataRow> resultQuery;
			if(byType.IsChecked && byName.IsChecked) {
				resultQuery = result.OrderBy(x => x.ItemArray[2]).ThenBy(x => x.ItemArray[1]);
			} else if(byName.IsChecked) {
				resultQuery = result.OrderBy(x => x.ItemArray[1]);
			} else if(byType.IsChecked) {
				resultQuery = result.OrderBy(x => x.ItemArray[2]);
			} else {
				resultQuery = result;
			};


			resultGrid.IsVisible = true;
			resultGrid.ItemsSource = resultQuery;
		}

		private void Refresh(object sender, EventArgs e) {


			//manufAdapter.Update(shopDS, "manufact");
			resultGrid.EndEdit();
			productAdapter.Update(shopDS,"product");

			shopDS.Tables["product"].AcceptChanges();
			productAdapter.Fill(shopDS.Tables["product"]);
			//shopDS.Clear();
			//manufAdapter.Fill(shopDS, "manufact");
			//productAdapter.Fill(shopDS, "product");

			resultGrid.IsVisible = true;
		}

		private void AddProduct(object sender, EventArgs e) {

			var result = from m in shopDS.Tables["product"].AsEnumerable()						
						 select m;

			
			

			resultGrid.IsVisible = true;
			resultGrid.ItemsSource = result.CopyToDataTable().DefaultView;
		}
	}
}

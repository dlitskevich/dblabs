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
using System.Diagnostics;
using Syncfusion.Data.Extensions;

namespace dbLabs {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage {
		private DataSet worldDS = new DataSet("World");
		private string connString = "server=127.0.0.1; user=root; password=Password; database=world";
		private MySqlDataAdapter cityAdapter;
		private MySqlDataAdapter countryAdapter;
		private MySqlDataAdapter countrylanguageAdapter;

		private MySqlCommandBuilder cityCommands;
		private MySqlCommandBuilder countryCommands;
		private MySqlCommandBuilder countrylanguageCommands;

		public MainPage() {
			InitializeComponent();
			cityAdapter = new MySqlDataAdapter("select * from city", connString);
			countryAdapter = new MySqlDataAdapter("select * from country", connString);
			countrylanguageAdapter = new MySqlDataAdapter("select * from countrylanguage", connString);


			cityCommands = new MySqlCommandBuilder(cityAdapter);
			countryCommands = new MySqlCommandBuilder(countryAdapter);
			countrylanguageCommands = new MySqlCommandBuilder(countrylanguageAdapter);

			cityAdapter.Fill(worldDS, "city");
			countryAdapter.Fill(worldDS, "country");
			countrylanguageAdapter.Fill(worldDS, "countrylanguage");

			DataRelation cityToCountry = new DataRelation("CityCountry",
				worldDS.Tables["country"].Columns["Code"],
				worldDS.Tables["city"].Columns["CountryCode"]);

			DataRelation countrylanguageToCountry = new DataRelation("countrylanguageCountry",
				worldDS.Tables["country"].Columns["Code"],
				worldDS.Tables["countrylanguage"].Columns["CountryCode"]);

			worldDS.Relations.Add(cityToCountry);
			worldDS.Relations.Add(countrylanguageToCountry);


		}

		private void PageLoaded(object sender, EventArgs e) {
			resultGrid.ItemsSource = worldDS.Tables["city"].DefaultView;
		}

		private void GNP(object sender, EventArgs e) {
			var result = from x in worldDS.Tables["country"].AsEnumerable()
						 where x.Field<float>("GNP")>20000
						 select new {
							 Name = x.Field<string>("Name"),
							 GNP = x.Field<float>("GNP")
						 };

			resultGrid.ItemsSource = result;
		}

		/*
		private void Filter(object sender, EventArgs e) {

			var result = from m in shopDS.Tables["product"].AsEnumerable()
						 where
						 (float.TryParse(minPrice.Text, out float min) ? min : 0) < (float)m["prod_price"]
						 && (float)m["prod_price"] < (float.TryParse(maxPrice.Text, out float max) ? max : 200)
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
			ProductManage.IsVisible = true;
			resultGrid.ItemsSource = resultQuery.CopyToDataTable().DefaultView;
		}


		private void FindProduct(object sender, EventArgs e) {

			var result = from m in shopDS.Tables["product"].AsEnumerable()
						 where ((string)m["prod_name"]).Contains(productPattern.Text.ToString())
						 select m;

			resultGrid.ItemsSource = result.CopyToDataTable().DefaultView;
			ProductManage.IsVisible = true;
		}



		private void Refresh(object sender, EventArgs e) {

			resultGrid.EndEdit();
			//shopDS.Tables["product"].AcceptChanges();
			//productAdapter.Up;

			try {
				productAdapter.Update(shopDS.Tables["product"]);
			} catch(MySqlException ex) {
				Debug.WriteLine(ex.Message);
			}
			shopDS.Clear();
			manufAdapter.Fill(shopDS, "manufact");
			productAdapter.Fill(shopDS, "product");
			customerAdapter.Fill(shopDS, "customer");

			PageLoaded(null, null);
		}

		private void AddProduct(object sender, EventArgs e) {
			DataRow row;
			row = shopDS.Tables["product"].NewRow();
			row[0] = (int)(shopDS.Tables["product"].AsEnumerable()).Last()[0] + 1;
			shopDS.Tables["product"].Rows.Add(row);

			resultGrid.IsVisible = true;
			resultGrid.ItemsSource = shopDS.Tables["product"].DefaultView;
		}

		private void RemoveProduct(object sender, EventArgs e) {
			if(resultGrid.SelectedItem != null) {
				//shopDS.Tables["product"].Rows.Remove((DataRow)((DataRowView)resultGrid.SelectedItem).Row);
				//shopDS.Tables["product"].Rows.RemoveAt((int)resultGrid.SelectedIndex-1);
				((DataRow)((DataRowView)resultGrid.SelectedItem).Row).Delete();
				Refresh(null, null);
			}
		}


		private void Group(object sender, EventArgs e) {
			//var result = ((shopDS.Tables["product"].Select().Join(
			//	shopDS.Tables["manufact"].Select(),
			//	prod => prod[3],
			//	manuf => manuf[0],
			//	(prod, manuf) => new {
			//		id = manuf[0],
			//		Name = manuf[1],
			//		Country = manuf[2],
			//		Address = manuf[4],
			//		Price = prod[4],
			//		TotalQuantity = (int)prod[5]
			//	})).GroupBy(x => x.id, (key, rest) => new {
			//		id = key,
			//		AVGprice = rest.Average(y => (float)y.Price),
			//		TotalQuantity = rest.Sum(y => y.TotalQuantity)
			//	})).Select(prop => new { prop.id, prop.AVGprice, prop.TotalQuantity });

			var result2 = (shopDS.Tables["manufact"].Select()).GroupJoin(
				(shopDS.Tables["product"].Select().Join(
				shopDS.Tables["manufact"].Select(),
				prod => prod[3],
				manuf => manuf[0],
				(prod, manuf) => new {
					id = manuf[0],
					Name = manuf[1],
					Country = manuf[2],
					Address = manuf[4],
					Price = prod[4],
					TotalQuantity = (int)prod[5]
				})),
					manuf => manuf[0],
					key => key.id,
					(manuf, stats) => new {
						id = manuf[0],
						Name = manuf[1],
						Country = manuf[2],
						Address = manuf[4],
						AVGprice = stats.Average(prop => (float)prop.Price),
						TotalQuantity = stats.Sum(prop => prop.TotalQuantity)
					});

			//var result1 = (shopDS.Tables["manufact"].Select()).Join(
			//	result,
			//	manuf => manuf[3],
			//	stats => stats.id,
			//	(manuf, stats) => new {
			//		stats.id,
			//		Name = manuf[1],
			//		Country = manuf[2],
			//		Address = manuf[4],
			//		stats.AVGprice,
			//		stats.TotalQuantity
			//	})
			//;

			ProductManage.IsVisible = false;
			resultGrid.ItemsSource = result2;

		}

		private void GroupLINQ(object sender, EventArgs e) {
			var result = from prod in shopDS.Tables["product"].AsEnumerable()
						 join manuf in shopDS.Tables["manufact"].AsEnumerable()
						 on prod["prod_manuf_id"] equals manuf["manuf_id"]
						 group new { prod, manuf }
						 by new {
							 id = manuf.Field<int>("manuf_id"),
							 mn = manuf.Field<string>("manuf_name"),
							 address = manuf.Field<string>("manuf_address")
						 } into mt
						 orderby mt.Key.id ascending
						 select new {
							 id = mt.Key.id,
							 Name = mt.Key.mn,
							 Address = mt.Key.address,
							 AVGprice = mt.Average(x => x.prod.Field<float>("prod_price")),
							 ManufQuantity = mt.Sum(x => x.prod.Field<int>("prod_quantity"))
						 };

			ProductManage.IsVisible = false;
			resultGrid.ItemsSource = result;
		}
		*/
	}
}

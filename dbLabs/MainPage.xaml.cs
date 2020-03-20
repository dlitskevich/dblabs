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
		private DataSet shopDS = new DataSet("Shop");
		private string connString = "server=127.0.0.1; user=root; password=Password; database=autoschool";
		private MySqlDataAdapter customerAdapter;
		private MySqlDataAdapter teacherAdapter;
		private MySqlDataAdapter autoAdapter;
		private MySqlDataAdapter contractAdapter;
		private MySqlDataAdapter practiceAdapter;

		private MySqlCommandBuilder customerCommands;
		private MySqlCommandBuilder teacherCommands;
		private MySqlCommandBuilder autoCommands;
		private MySqlCommandBuilder contractCommands;
		private MySqlCommandBuilder practiceCommands;

		public MainPage() {
			InitializeComponent();

			customerAdapter = new MySqlDataAdapter("select * from customer", connString);
			teacherAdapter = new MySqlDataAdapter("select * from teacher", connString);
			autoAdapter = new MySqlDataAdapter("select * from auto", connString);
			contractAdapter = new MySqlDataAdapter("select * from contract", connString);
			practiceAdapter = new MySqlDataAdapter("select * from practice", connString);


			customerCommands = new MySqlCommandBuilder(customerAdapter);
			teacherCommands = new MySqlCommandBuilder(teacherAdapter);
			autoCommands = new MySqlCommandBuilder(autoAdapter);
			contractCommands = new MySqlCommandBuilder(contractAdapter);
			practiceCommands = new MySqlCommandBuilder(practiceAdapter);


			customerAdapter.Fill(shopDS, "customer");
			teacherAdapter.Fill(shopDS, "teacher");
			autoAdapter.Fill(shopDS, "auto");
			contractAdapter.Fill(shopDS, "contract");
			practiceAdapter.Fill(shopDS, "practice");

			var UpdateCmd = customerCommands.GetUpdateCommand();
			UpdateCmd.CommandText = $"UPDATE `product` SET `customer_name` = '@p1', `customer_surname` = '@p2', `customer_phone` = @p3, `customer_birth` = @p4 WHERE(`customer_id` = @p5)";
			customerAdapter.UpdateCommand = UpdateCmd;

			var DeleteCmd = customerCommands.GetDeleteCommand();
			DeleteCmd.CommandText = $"DELETE FROM `product` WHERE(`customer_id` = @p1)";
			customerAdapter.DeleteCommand = UpdateCmd;

			/*
			DataRelation manufToProduct = new DataRelation("ManufProduct",
				shopDS.Tables["manufact"].Columns["manuf_id"],
				shopDS.Tables["product"].Columns["prod_manuf_id"]);

			shopDS.Relations.Add(manufToProduct);
			*/
		}

		private void PageLoaded(object sender, EventArgs e) {
			ShowGrid.ItemsSource = shopDS.Tables["customer"].DefaultView;
		}

		private void ShowCustomer(object sender, EventArgs e) {
			ShowGrid.ItemsSource = shopDS.Tables["customer"].DefaultView;
			addcustomer.IsVisible = true;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
		
		}

		private void ShowTeacher(object sender, EventArgs e) {
			ShowGrid.ItemsSource = shopDS.Tables["teacher"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = true;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
		}

		private void ShowAuto(object sender, EventArgs e) {
			ShowGrid.ItemsSource = shopDS.Tables["auto"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = true;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
		}

		private void ShowContract(object sender, EventArgs e) {
			ShowGrid.ItemsSource = shopDS.Tables["contract"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = true;
			addpractice.IsVisible = false;
		}

		private void ShowPractice(object sender, EventArgs e) {
			ShowGrid.ItemsSource = shopDS.Tables["practice"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = true;
		}


		private void Refresh(object sender, EventArgs e) {

			ShowGrid.EndEdit();
			//shopDS.Tables["product"].AcceptChanges();
			//productAdapter.Up;

			try {
				customerAdapter.Update(shopDS.Tables["customer"]);
				teacherAdapter.Update(shopDS.Tables["teacher"]);
				autoAdapter.Update(shopDS.Tables["auto"]);
				contractAdapter.Update(shopDS.Tables["contract"]);
				practiceAdapter.Update(shopDS.Tables["practice"]);
			} catch(MySqlException ex) {
				Debug.WriteLine(ex.Message);
			}
			shopDS.Clear();
			customerAdapter.Fill(shopDS, "customer");
			teacherAdapter.Fill(shopDS, "teacher");
			autoAdapter.Fill(shopDS, "auto");
			contractAdapter.Fill(shopDS, "contract");
			practiceAdapter.Fill(shopDS, "practice");

			// PageLoaded(null, null);
		}

		private void AddCustomer(object sender, EventArgs e) {
			DataRow row;
			row = shopDS.Tables["customer"].NewRow();
			row[0] = (int)(shopDS.Tables["customer"].AsEnumerable()).Last()[0] + 1;
			shopDS.Tables["customer"].Rows.Add(row);

			ShowGrid.ItemsSource = shopDS.Tables["customer"].DefaultView;
		}

		private void AddTeacher(object sender, EventArgs e) {
			DataRow row;
			row = shopDS.Tables["teacher"].NewRow();
			row[0] = (int)(shopDS.Tables["teacher"].AsEnumerable()).Last()[0] + 1;
			shopDS.Tables["teacher"].Rows.Add(row);

			ShowGrid.ItemsSource = shopDS.Tables["teacher"].DefaultView;
		}

		private void AddAuto(object sender, EventArgs e) {
			DataRow row;
			row = shopDS.Tables["auto"].NewRow();
			row[0] = (int)(shopDS.Tables["auto"].AsEnumerable()).Last()[0] + 1;
			shopDS.Tables["auto"].Rows.Add(row);

			ShowGrid.ItemsSource = shopDS.Tables["auto"].DefaultView;
		}

		private void AddContract(object sender, EventArgs e) {
			DataRow row;
			row = shopDS.Tables["contract"].NewRow();
			row[0] = (int)(shopDS.Tables["contract"].AsEnumerable()).Last()[0] + 1;
			row[4] = (DateTime)DateTime.Now;
			row[5] = (DateTime)DateTime.Now.AddMonths(3);
			shopDS.Tables["contract"].Rows.Add(row);

			ShowGrid.ItemsSource = shopDS.Tables["contract"].DefaultView;
		}

		private void AddPractice(object sender, EventArgs e) {
			DataRow row;
			row = shopDS.Tables["practice"].NewRow();
			row[0] = (int)(shopDS.Tables["practice"].AsEnumerable()).Last()[0] + 1;
			row[4] = (DateTime)DateTime.Now;
			shopDS.Tables["practice"].Rows.Add(row);

			ShowGrid.ItemsSource = shopDS.Tables["practice"].DefaultView;
		}

		private void RemoveItem(object sender, EventArgs e) {
			if(ShowGrid.SelectedItem != null) {
				//shopDS.Tables["product"].Rows.Remove((DataRow)((DataRowView)ShowGrid.SelectedItem).Row);
				//shopDS.Tables["product"].Rows.RemoveAt((int)ShowGrid.SelectedIndex-1);
				((DataRow)((DataRowView)ShowGrid.SelectedItem).Row).Delete();
				Refresh(null, null);
			}
		}



		/// ////////////////////////////////////////////////
		/// Filters part
		/// ////////////////////////////////////////////////

		private void FilterContract(object sender, EventArgs e) {

			var result = from contract in shopDS.Tables["contract"].AsEnumerable()
						 where
						 (float.TryParse(minPrice.Text, out float min) ? min : 0) < (float)contract["payment"]
						 && (float)contract["payment"] < (float.TryParse(maxPrice.Text, out float max) ? max : 5000)
						 select contract;

			IEnumerable<DataRow> resultQuery;
			if(byType.IsChecked && byDate.IsChecked) {
				resultQuery = result.OrderBy(x => x["contract_type"]).ThenBy(x => x["contract_start_date"]);
			} else if(byDate.IsChecked) {
				resultQuery = result.OrderBy(x => x.ItemArray[4]);
			} else if(byType.IsChecked) {
				resultQuery = result.OrderBy(x => x.ItemArray[2]);
			} else {
				resultQuery = result;
			};

			resultGrid.ItemsSource = resultQuery.CopyToDataTable().DefaultView;
		}

		private void FindAuto(object sender, EventArgs e) {

			var result = from auto in shopDS.Tables["auto"].AsEnumerable()
						 where ((string)auto["auto_name"]).Contains(autoPattern.Text.ToString())
						 select auto;

			resultGrid.ItemsSource = result.CopyToDataTable().DefaultView;
		}


		private void JoinContract(object sender, EventArgs e) {
			var result = shopDS.Tables["contract"].Select().Join(
				shopDS.Tables["customer"].Select(),
				contract => contract["customer_id"],
				customer => customer["customer_id"],
				(contract, customer) => new {
					id = contract[0],
					customer_id = customer[0],
					Name = customer[1],
					Surname = customer[2],
					Phone = customer[3],
					Type = contract[2],
					Payment = contract[3],
				});

			resultGrid.ItemsSource = result;
		}

		private void AverageContract(object sender, EventArgs e) {
			var result = shopDS.Tables["contract"].Select().GroupBy(
					contract => contract["contract_type"],
					(type, rest) => new {
						Type = type,
						Payment = rest.Average(x => x["payment"]),
					}
				);

			resultGrid.ItemsSource = result;
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

			resultGrid.ItemsSource = result;
		}


		

	}
}

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
using AppKit;

namespace dbLabs {
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage {
		private DataSet autoschoolDS = new DataSet("Autoschool");
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


			customerAdapter.Fill(autoschoolDS, "customer");
			teacherAdapter.Fill(autoschoolDS, "teacher");
			autoAdapter.Fill(autoschoolDS, "auto");
			contractAdapter.Fill(autoschoolDS, "contract");
			practiceAdapter.Fill(autoschoolDS, "practice");

			var UpdateCmd = customerCommands.GetUpdateCommand();
			UpdateCmd.CommandText = $"UPDATE `product` SET `customer_name` = '@p1', `customer_surname` = '@p2', `customer_phone` = @p3, `customer_birth` = @p4 WHERE(`customer_id` = @p5)";
			customerAdapter.UpdateCommand = UpdateCmd;

			var DeleteCmd = customerCommands.GetDeleteCommand();
			DeleteCmd.CommandText = $"DELETE FROM `product` WHERE(`customer_id` = @p1)";
			customerAdapter.DeleteCommand = UpdateCmd;

			/*
			DataRelation manufToProduct = new DataRelation("ManufProduct",
				autoschoolDS.Tables["manufact"].Columns["manuf_id"],
				autoschoolDS.Tables["product"].Columns["prod_manuf_id"]);

			autoschoolDS.Relations.Add(manufToProduct);
			*/
		}

		/// ////////////////////////////////////////////////
		/// Initialize page
		/// ////////////////////////////////////////////////
		
		private void PageLoaded(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["customer"].DefaultView;
			UpdateColumnsNames(null, null);
		}

		/// ////////////////////////////////////////////////
		/// Show properties
		/// ////////////////////////////////////////////////		

		private void UpdateColumnsNames(object sender, EventArgs e) {
			string tableName = sender==null? "customer" : (string)((Picker)sender).SelectedItem;
			DataColumnCollection columns = autoschoolDS.Tables[tableName].Columns;

			List<string> names = new List<string>();
			foreach(DataColumn column in columns) {
				names.Add(column.ColumnName);
			}
			columnNames.ItemsSource = names.ToArray();
		}

		private void ShowProperty(object sender, EventArgs e) {
			string tableName = tableSelected.SelectedItem == null ? "customer" : (string)tableSelected.SelectedItem;
			string colname = columnNames.SelectedItem == null? (string)columnNames.Items[0] : (string)columnNames.SelectedItem;

			var columnSelected = autoschoolDS.Tables[tableName].Columns[colname];
			
			string colproperty = "";
			colproperty += "Name - > " + columnSelected.ColumnName + "\n";
			colproperty += "Type - > " + columnSelected.DataType + "\n";
			colproperty += "Allow NULL - > " + columnSelected.AllowDBNull + "\n";
			colproperty += "Autoincrement - > " + columnSelected.AutoIncrement + "\n";
			colproperty += "Unique - > " + columnSelected.Unique + "\n";
			colproperty += "Number of primary keys - >" + autoschoolDS.Tables[tableName].PrimaryKey.Length + "\n";

			var alert = new NSAlert() {
				AlertStyle = NSAlertStyle.Informational,
				InformativeText = colproperty,
				MessageText = "Properties of"+ colname,
			};
			alert.AddButton("Ok");
			alert.RunModal();
		}


		/// ////////////////////////////////////////////////
		/// CRUD
		/// ////////////////////////////////////////////////

		private void ShowCustomer(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["customer"].DefaultView;
			addcustomer.IsVisible = true;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
		}

		private void ShowTeacher(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["teacher"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = true;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
		}

		private void ShowAuto(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["auto"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = true;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
		}

		private void ShowContract(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["contract"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = true;
			addpractice.IsVisible = false;
		}

		private void ShowPractice(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["practice"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = true;
		}


		private void Refresh(object sender, EventArgs e) {

			ShowGrid.EndEdit();
			//autoschoolDS.Tables["product"].AcceptChanges();
			//productAdapter.Up;

			try {
				customerAdapter.Update(autoschoolDS.Tables["customer"]);
				teacherAdapter.Update(autoschoolDS.Tables["teacher"]);
				autoAdapter.Update(autoschoolDS.Tables["auto"]);
				contractAdapter.Update(autoschoolDS.Tables["contract"]);
				practiceAdapter.Update(autoschoolDS.Tables["practice"]);
			} catch(MySqlException ex) {
				Debug.WriteLine(ex.Message);
			}
			autoschoolDS.Clear();
			customerAdapter.Fill(autoschoolDS, "customer");
			teacherAdapter.Fill(autoschoolDS, "teacher");
			autoAdapter.Fill(autoschoolDS, "auto");
			contractAdapter.Fill(autoschoolDS, "contract");
			practiceAdapter.Fill(autoschoolDS, "practice");

			// PageLoaded(null, null);
		}

		private void AddCustomer(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["customer"].NewRow();
			row[0] = (int)(autoschoolDS.Tables["customer"].AsEnumerable()).Last()[0] + 1;
			autoschoolDS.Tables["customer"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["customer"].DefaultView;
		}

		private void AddTeacher(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["teacher"].NewRow();
			row[0] = (int)(autoschoolDS.Tables["teacher"].AsEnumerable()).Last()[0] + 1;
			autoschoolDS.Tables["teacher"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["teacher"].DefaultView;
		}

		private void AddAuto(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["auto"].NewRow();
			row[0] = (int)(autoschoolDS.Tables["auto"].AsEnumerable()).Last()[0] + 1;
			autoschoolDS.Tables["auto"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["auto"].DefaultView;
		}

		private void AddContract(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["contract"].NewRow();
			row[0] = (int)(autoschoolDS.Tables["contract"].AsEnumerable()).Last()[0] + 1;
			row[4] = (DateTime)DateTime.Now;
			row[5] = (DateTime)DateTime.Now.AddMonths(3);
			autoschoolDS.Tables["contract"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["contract"].DefaultView;
		}

		private void AddPractice(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["practice"].NewRow();
			row[0] = (int)(autoschoolDS.Tables["practice"].AsEnumerable()).Last()[0] + 1;
			row[4] = (DateTime)DateTime.Now;
			autoschoolDS.Tables["practice"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["practice"].DefaultView;
		}

		private void RemoveItem(object sender, EventArgs e) {
			if(ShowGrid.SelectedItem != null) {
				//autoschoolDS.Tables["product"].Rows.Remove((DataRow)((DataRowView)ShowGrid.SelectedItem).Row);
				//autoschoolDS.Tables["product"].Rows.RemoveAt((int)ShowGrid.SelectedIndex-1);
				((DataRow)((DataRowView)ShowGrid.SelectedItem).Row).Delete();
				Refresh(null, null);
			}
		}



		/// ////////////////////////////////////////////////
		/// Filters part
		/// ////////////////////////////////////////////////

		private void FilterContract(object sender, EventArgs e) {

			var result = from contract in autoschoolDS.Tables["contract"].AsEnumerable()
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

			var result = from auto in autoschoolDS.Tables["auto"].AsEnumerable()
						 where
						 ((string)auto["auto_name"]).Contains(autoPattern.Text.ToString())
						 &&
						 (float.TryParse(minPriceAuto.Text, out float min) ? min : 0) < (float)auto["price"]
						 && (float)auto["price"] < (float.TryParse(maxPriceAuto.Text, out float max) ? max : 70000)
						 select auto;

			resultGrid.ItemsSource = result.CopyToDataTable().DefaultView;
		}


		private void JoinContract(object sender, EventArgs e) {
			var result = autoschoolDS.Tables["contract"].Select().Join(
				autoschoolDS.Tables["customer"].Select(),
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
			var result = autoschoolDS.Tables["contract"].Select().GroupBy(
					contract => contract["contract_type"],
					(type, rest) => new {
						Type = type,
						Payment = rest.Average(x => (float)x["payment"]),
					}
				).OrderBy(x => (string)x.Type);

			resultGrid.ItemsSource = result;
		}

		private void AutoUsage(object sender, EventArgs e) {
			var result = autoschoolDS.Tables["auto"].Select().GroupJoin(
					autoschoolDS.Tables["practice"].Select(),
					auto => auto["auto_id"],
					practice => practice["auto_id"],

					(auto, practice) => new {						
						Name = auto[1],
						Count = practice.Count()
					}
				).OrderBy(x => (string)x.Name);

			resultGrid.ItemsSource = result;
		}

		private void TeacherPractice(object sender, EventArgs e) {
			var result = from teacher in autoschoolDS.Tables["teacher"].AsEnumerable()
						 join practise in autoschoolDS.Tables["practice"].AsEnumerable()
						 on teacher["teacher_id"] equals practise["teacher_id"]
						 group new { teacher, practise }
						 by new {
							 id = teacher.Field<int>("teacher_id"),
							 Name = teacher.Field<string>("teacher_name"),
							 Surname = teacher.Field<string>("teacher_surname"),
							 Phone = teacher.Field<string>("teacher_phone")
						 } into teacherPractise
						 orderby teacherPractise.Key.id ascending
						 select new {
							 teacherPractise.Key.id,
							 teacherPractise.Key.Name,
							 teacherPractise.Key.Surname,
							 teacherPractise.Key.Phone,
							 Lessons = (int)teacherPractise.Count()
						 };

			resultGrid.ItemsSource = result;
		}

		private void CustomerMark(object sender, EventArgs e) {
			var result = from customer in autoschoolDS.Tables["customer"].AsEnumerable()
						 join practise in autoschoolDS.Tables["practice"].AsEnumerable()
						 on customer["customer_id"] equals practise["customer_id"]
						 group new { customer, practise }
						 by new {
							 id = customer.Field<int>("customer_id"),
							 Name = customer.Field<string>("customer_name"),
							 Surname = customer.Field<string>("customer_surname"),
							 Phone = customer.Field<string>("customer_phone")
						 } into customerPractise
						 orderby customerPractise.Key.id ascending
						 select new {
							 customerPractise.Key.id,
							 customerPractise.Key.Name,
							 customerPractise.Key.Surname,
							 customerPractise.Key.Phone,
							 minMark = customerPractise.Min(x => x.practise.Field<int>("mark")),
							 maxMark = customerPractise.Max(x => x.practise.Field<int>("mark")),
							 averageMark = (int)customerPractise.Average(x => x.practise.Field<int>("mark"))
						 };

			resultGrid.ItemsSource = result;
		}



		

	}
}

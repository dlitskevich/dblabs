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
		private DataSet bankDS = new DataSet("Bank");
		//private Dictionary<DataRow, DateTime> modified = new Dictionary<DataRow, DateTime>();
		//private Dictionary<DataRow, DateTime> changed = new Dictionary<DataRow, DateTime>();

		private string connString = "server=127.0.0.1; user=root; password=Password; database=bankkr";
		private MySqlDataAdapter customerAdapter;
		private MySqlDataAdapter acctAdapter;
		private MySqlDataAdapter sellerAdapter;

		private MySqlCommandBuilder customerCommands;
		private MySqlCommandBuilder acctCommands;
		private MySqlCommandBuilder sellerCommands;


		public MainPage() {
			InitializeComponent();

			customerAdapter = new MySqlDataAdapter("select * from customer", connString);
			acctAdapter = new MySqlDataAdapter("select * from acct", connString);
			sellerAdapter = new MySqlDataAdapter("select * from seller", connString);


			customerCommands = new MySqlCommandBuilder(customerAdapter);
			acctCommands = new MySqlCommandBuilder(acctAdapter);
			sellerCommands = new MySqlCommandBuilder(sellerAdapter);

			

			customerAdapter.Fill(bankDS, "customer");
			acctAdapter.Fill(bankDS, "acct");
			sellerAdapter.Fill(bankDS, "seller");



			//ForeignKeyConstraint CustomerToAcct =
			//	new ForeignKeyConstraint(
			//		"CustomerContract",
			//		bankDS.Tables["customer"].Columns["customer_id"],
			//		bankDS.Tables["acct"].Columns["acct_owner"]
			//		);
			//CustomerToAcct.DeleteRule = Rule.Cascade;
			//bankDS.Tables["acct"].Constraints.Add(CustomerToAcct);
			


		}
		
		/// ////////////////////////////////////////////////
		/// Initialize page
		/// ////////////////////////////////////////////////
		
		private void PageLoaded(object sender, EventArgs e) {
			ShowGrid.ItemsSource = bankDS.Tables["customer"].DefaultView;
			
		}

		/// ////////////////////////////////////////////////
		/// Show properties
		/// ////////////////////////////////////////////////		
		private void addSellerTransaction(object s, EventArgs e) {
			using(MySqlConnection connection = new MySqlConnection(connString)) {
				connection.Open();

				// Start a local transaction.
				MySqlTransaction sqlTran = connection.BeginTransaction();

				// Enlist a command in the current transaction.
				MySqlCommand command = connection.CreateCommand();
				command.Transaction = sqlTran;
				int id = (int)(bankDS.Tables["seller"].AsEnumerable()).Last()[0] + 1; 
				try {
					command.CommandText ="INSERT INTO bankkr.seller VALUES("+ id.ToString()+ ", \"Name\",\"Surname\")";
					command.ExecuteNonQuery();

					// Commit the transaction.
					sqlTran.Commit();
					Console.WriteLine("Both records were written to database.");
				} catch(Exception ex) {
					// Handle the exception if the transaction fails to commit.
					Console.WriteLine(ex.Message);

					try {
						// Attempt to roll back the transaction.
						sqlTran.Rollback();
					} catch(Exception exRollback) {
						Console.WriteLine(exRollback.Message);
					}
				}
			}
		}
		/// ////////////////////////////////////////////////
		/// CRUD
		/// ////////////////////////////////////////////////

		private void ShowCustomer(object sender, EventArgs e) {
			ShowGrid.ItemsSource = bankDS.Tables["customer"].DefaultView;
			addcustomer.IsVisible = true;
			
			//ShowGrid.Columns[0].IsHidden = true;
		}

		private void ShowAcct(object sender, EventArgs e) {
			ShowGrid.ItemsSource = bankDS.Tables["acct"].DefaultView;
			

			//ShowGrid.Columns[0].IsHidden = true;
		}

		private void ShowSeller(object sender, EventArgs e) {
			ShowGrid.ItemsSource = bankDS.Tables["seller"].DefaultView;


			//ShowGrid.Columns[0].IsHidden = true;
		}


		private void Sync(object sender, EventArgs e) {

			ShowGrid.EndEdit();
			//bankDS.Tables["product"].AcceptChanges();
			//productAdapter.Up;

			try {
				acctAdapter.Update(bankDS.Tables["acct"]);
				sellerAdapter.Update(bankDS.Tables["seller"]);
				customerAdapter.Update(bankDS.Tables["customer"]);
				

			} catch(MySqlException ex) {
				Console.WriteLine(ex.Message);
			}
			bankDS.AcceptChanges();
			bankDS.Clear();
			customerAdapter.Fill(bankDS, "customer");
			acctAdapter.Fill(bankDS, "acct");

			sellerAdapter.Fill(bankDS, "seller");
			
			


			// PageLoaded(null, null);
		}

		private void Cancel(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			bankDS.RejectChanges();
		}

		private void CancelDeleted(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			foreach(DataTable table in bankDS.Tables) {
				foreach(DataRow row in table.Rows) {
					if(row.RowState == DataRowState.Deleted) {
						row.RejectChanges();
					}
				}
			}
		}

		private void CancelModified(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			foreach(DataTable table in bankDS.Tables) {
				foreach(DataRow row in table.Rows) {
					if(row.RowState == DataRowState.Modified) {
						row.RejectChanges();
					}
				}
			}
		}
		/*
		private void CancelLastModified(object sender, EventArgs e) {
			if(modified.Count > 0) {
				DataRow row = modified.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
				modified.Remove(row);
				try {
					changed.Remove(row);
				} catch {

				}
				row.RejectChanges();
			}
		}

		private void CancelLast(object sender, EventArgs e) {
			if(changed.Count > 0) {
				DataRow row = changed.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
				changed.Remove(row);
				try {
					modified.Remove(row);
				} catch {

				}
				row.RejectChanges();
			}
		}


		private void StackDeleted(object sender, DataRowChangeEventArgs e) {			
	
			if(e.Row.RowState == DataRowState.Unchanged) {
				try {
					modified.Remove(e.Row);
				} catch {

				}
				changed.Remove(e.Row);
			} else {
				changed.Add(e.Row, DateTime.Now);
				try {
					modified.Remove(e.Row);
				} catch {
					
				}
			}

		}
		*/
		/*
		private void AutoTypeSync(object sender, DataColumnChangeEventArgs e) {

			if(e.Column.Table.TableName == "auto" && e.Column.ColumnName == "auto_type_id") {
				try {
					e.Row["auto_type"] = ((AutoTypes)e.Row["auto_type_id"]).ToString();
				} catch  {
				}
			} 

		}
		*/
		//private void StackAdded(object sender, DataRowChangeEventArgs e) {

		//	if(e.Row.RowState == DataRowState.Unchanged) {				
		//		changed.Remove(e.Row);
		//	} else {				
		//		changed.Add(e.Row, DateTime.Now);
		//	}

		//}
		/*
		private void StackModified(object sender, DataRowChangeEventArgs e) {
			
			if(e.Row.RowState == DataRowState.Unchanged) {
				try {
					modified.Remove(e.Row);
					changed.Remove(e.Row);
				} catch {
				}
			} else {
				modified.Add(e.Row, DateTime.Now);
				try {
					changed.Add(e.Row, DateTime.Now);
				} catch {
					changed[e.Row] = DateTime.Now;
				}
			}
			

		}
		*/
		private void AddCustomer(object sender, EventArgs e) {
			DataRow row;
			row = bankDS.Tables["customer"].NewRow();
			//row[0] = (int)(bankDS.Tables["customer"].AsEnumerable()).Last()[0] + 1;
			row[1] = "Name";
			row[2] = "Surname";
			row[4] = (DateTime)DateTime.Now.AddYears(-18);
			bankDS.Tables["customer"].Rows.Add(row);

			ShowGrid.ItemsSource = bankDS.Tables["customer"].DefaultView;
		}

		private void AddSeller(object sender, EventArgs e) {
			DataRow row;
			row = bankDS.Tables["seller"].NewRow();
			row[0] = (int)(bankDS.Tables["seller"].AsEnumerable()).Last()[0] + 1;
			row[1] = "Name";
			row[2] = "Surname";
			bankDS.Tables["seller"].Rows.Add(row);

			ShowGrid.ItemsSource = bankDS.Tables["seller"].DefaultView;
		}

		

		private void RemoveItem(object sender, EventArgs e) {
			if(ShowGrid.SelectedItem != null) {
				//bankDS.Tables["product"].Rows.Remove((DataRow)((DataRowView)ShowGrid.SelectedItem).Row);
				//bankDS.Tables["product"].Rows.RemoveAt((int)ShowGrid.SelectedIndex-1);
							
				((DataRow)((DataRowView)ShowGrid.SelectedItem).Row).Delete();
				
				//Refresh(null, null);
			}
		}

		private void RemoveSelected(object sender, EventArgs e) {
			if(ShowGrid.SelectedItems.Count > 0) {
				var selected = ShowGrid.SelectedItems.ToArray();
				foreach(DataRowView row in selected) {
					((DataRow)row.Row).Delete();
				}
				
			}
		}


		private void MultSelection(object sender, EventArgs e) {
			if(((CheckBox)sender).IsChecked) {
				ShowGrid.SelectionMode = Syncfusion.SfDataGrid.XForms.SelectionMode.Multiple;
			} else {
				
				ShowGrid.SelectionMode = Syncfusion.SfDataGrid.XForms.SelectionMode.Single;
			}
		}
		



		/// ////////////////////////////////////////////////
		/// Filters part
		/// ////////////////////////////////////////////////

		

		private void UpdatePriceAuto(object sender, EventArgs e) {

			(from auto in bankDS.Tables["auto"].AsEnumerable()
			 join newPrice in (from auto in bankDS.Tables["auto"].AsEnumerable()
			 select new { id = auto.Field<int>("auto_id"), newPrice = auto.Field<string>("colour") == "blue" ? 16001 : (auto.Field<string>("colour") == "black" ? 16000 : auto.Field<int>("price")) })
			 on auto.Field<int>("auto_id") equals newPrice.id
			where
			 auto.Field<int>("price") != newPrice.newPrice
			 select new { auto, newPrice.newPrice }).ForEach(row => (row.auto)["price"] = row.newPrice);

		}


		private void JoinContract(object sender, EventArgs e) {
			var result = bankDS.Tables["contract"].Select().Join(
				bankDS.Tables["customer"].Select(),
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
			var result = bankDS.Tables["contract"].Select().GroupBy(
					contract => contract["contract_type"],
					(type, rest) => new {
						Type = type,
						Payment = rest.Average(x => (int)x["payment"]),
					}
				).OrderBy(x => (string)x.Type);

			resultGrid.ItemsSource = result;
		}

		private void AutoUsage(object sender, EventArgs e) {
			var result = bankDS.Tables["auto"].Select().GroupJoin(
					bankDS.Tables["practice"].Select(),
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
			var result = from teacher in bankDS.Tables["teacher"].AsEnumerable()
						 join practice in bankDS.Tables["practice"].AsEnumerable()
						 on teacher["teacher_id"] equals practice["teacher_id"]
						 group new { teacher, practice }
						 by new {
							 id = teacher.Field<int>("teacher_id"),
							 Name = teacher.Field<string>("teacher_name"),
							 Surname = teacher.Field<string>("teacher_surname"),
							 Phone = teacher.Field<string>("teacher_phone")
						 } into teacherpractice
						 orderby teacherpractice.Key.id ascending
						 select new {
							 teacherpractice.Key.id,
							 teacherpractice.Key.Name,
							 teacherpractice.Key.Surname,
							 teacherpractice.Key.Phone,
							 Lessons = (int)teacherpractice.Count()
						 };

			resultGrid.ItemsSource = result;
		}

		private void CustomerMark(object sender, EventArgs e) {
			var avg = from acc in bankDS.Tables["acct"].AsEnumerable()
					  group new { acc } by acc.Field<int>("acct_owner")
					  into accgrouped
					  select accgrouped.Sum(x =>x.acc.Field<double>("acct_balance"))
					  ;
			var avvg = avg.Average();
			var result = from customer in bankDS.Tables["customer"].AsEnumerable()
						 join acc in bankDS.Tables["acct"].AsEnumerable()
						 on customer["customer_id"] equals acc["acct_owner"]
						 group new { customer, acc }
						 by new {
							 id = customer.Field<int>("customer_id"),
							 Name = customer.Field<string>("customer_name"),
							 Surname = customer.Field<string>("customer_surname"),
							 Phone = customer.Field<string>("customer_phone"),
							 
						 } into customerpractice
						 orderby customerpractice.Key.id ascending
						 
						 select new {
							 customerpractice.Key.id,
							 customerpractice.Key.Name,
							 customerpractice.Key.Surname,
							 customerpractice.Key.Phone,
							 total = customerpractice.Sum(x => x.acc.Field<double>("acct_balance")),
							 // maxMark = customerpractice.Max(x => x.practice.Field<int>("mark")),
							 //averageMark = (int)customerpractice.Average(x => x.practice.Field<int>("mark"))
						 }
						 
						 ;

			resultGrid.ItemsSource = result.Where(x =>x.total> avvg);
		}



		

	}
}

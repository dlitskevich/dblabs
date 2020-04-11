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
		private Dictionary<DataRow, DateTime> modified = new Dictionary<DataRow, DateTime>();
		private Dictionary<DataRow, DateTime> changed = new Dictionary<DataRow, DateTime>();

		private enum AutoTypes{
			A=1,
			B=2,
			C=3,
			D=4
		};
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
			// auto_type+0 as auto_type_id
			autoAdapter = new MySqlDataAdapter("select *, auto_type+0 as auto_type_id from auto", connString);

			contractAdapter = new MySqlDataAdapter("select * from contract", connString);
			practiceAdapter = new MySqlDataAdapter("select * from practice", connString);


			customerCommands = new MySqlCommandBuilder(customerAdapter);
			teacherCommands = new MySqlCommandBuilder(teacherAdapter);
			autoCommands = new MySqlCommandBuilder(autoAdapter);
			contractCommands = new MySqlCommandBuilder(contractAdapter);
			practiceCommands = new MySqlCommandBuilder(practiceAdapter);

			//customerAdapter.Fill(autoschoolDS, "customer");
			//teacherAdapter.Fill(autoschoolDS, "teacher");
			//autoAdapter.Fill(autoschoolDS, "auto");
			//contractAdapter.Fill(autoschoolDS, "contract");
			//practiceAdapter.Fill(autoschoolDS, "practice");


			//autoschoolDS.Tables["customer"].PrimaryKey = new DataColumn[] { autoschoolDS.Tables["customer"].Columns["customer_id"] };
			//autoschoolDS.Tables["teacher"].PrimaryKey = new DataColumn[] { autoschoolDS.Tables["teacher"].Columns["teacher_id"] };
			//autoschoolDS.Tables["auto"].PrimaryKey = new DataColumn[] { autoschoolDS.Tables["auto"].Columns["auto_id"] };
			//autoschoolDS.Tables["contract"].PrimaryKey = new DataColumn[] { autoschoolDS.Tables["contract"].Columns["contract_id"] };
			//autoschoolDS.Tables["practice"].PrimaryKey = new DataColumn[] { autoschoolDS.Tables["practice"].Columns["practice_id"] };

			customerAdapter.FillSchema(autoschoolDS, SchemaType.Source,"customer");
			teacherAdapter.FillSchema(autoschoolDS, SchemaType.Source, "teacher");
			autoAdapter.FillSchema(autoschoolDS, SchemaType.Source, "auto");
			contractAdapter.FillSchema(autoschoolDS, SchemaType.Source, "contract");
			practiceAdapter.FillSchema(autoschoolDS, SchemaType.Source, "practice");

			//autoschoolDS.Tables["auto"].Rows.Clear();

			//autoschoolDS.Tables.Add("customer");
			//autoschoolDS.Tables.Add("teacher");
			//autoschoolDS.Tables.Add("auto");
			//autoschoolDS.Tables.Add("contract");
			//autoschoolDS.Tables.Add("practice");

			//autoschoolDS.Tables["customer"].Columns.Add("customer_name", Type.GetType("System.String"));
			//autoschoolDS.Tables["customer"].Columns["customer_name"].AllowDBNull = false;
			//autoschoolDS.Tables["customer"].Columns.Add("customer_surname", Type.GetType("System.String"));
			//autoschoolDS.Tables["customer"].Columns["customer_surname"].AllowDBNull = false;
			//autoschoolDS.Tables["customer"].Columns.Add("customer_phone", Type.GetType("System.String"));
			//autoschoolDS.Tables["customer"].Columns.Add("customer_birth", Type.GetType("System.DateTime"));

			//teacherAdapter.Fill(autoschoolDS, "customer");
			//teacherAdapter.Fill(autoschoolDS, "teacher");
			//autoAdapter.Fill(autoschoolDS, "auto");
			//contractAdapter.Fill(autoschoolDS, "contract");
			//practiceAdapter.Fill(autoschoolDS, "practice");


			
			autoschoolDS.Tables["customer"].Columns["customer_name"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["customer"].Columns["customer_name"].AllowDBNull = false;
			autoschoolDS.Tables["customer"].Columns["customer_surname"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["customer"].Columns["customer_surname"].AllowDBNull = false;
			autoschoolDS.Tables["customer"].Columns["customer_phone"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["customer"].Columns["customer_birth"].DataType = Type.GetType("System.DateTime");


			autoschoolDS.Tables["teacher"].Columns["teacher_name"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["teacher"].Columns["teacher_name"].AllowDBNull = false;
			autoschoolDS.Tables["teacher"].Columns["teacher_surname"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["teacher"].Columns["teacher_surname"].AllowDBNull = false;
			autoschoolDS.Tables["teacher"].Columns["teacher_phone"].DataType = Type.GetType("System.String");

			
			autoschoolDS.Tables["auto"].Columns["auto_name"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["auto"].Columns["auto_name"].DefaultValue = "Gillie";
			autoschoolDS.Tables["auto"].Columns["auto_type"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["auto"].Columns["auto_type"].DefaultValue = "A";
			autoschoolDS.Tables["auto"].Columns["auto_type_id"].DataType = typeof(AutoTypes);
			autoschoolDS.Tables["auto"].Columns["auto_type_id"].DefaultValue = AutoTypes.A;
			//Enum.Parse(typeof(types), "A");
			//autoschoolDS.Tables["auto"].Columns["auto_type"].Expression = "CONVERT(auto_type,typeof(types))";

			autoschoolDS.Tables["auto"].Columns["colour"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["auto"].Columns["colour"].DefaultValue = "red";
			autoschoolDS.Tables["auto"].Columns["available"].DataType = Type.GetType("System.Boolean");
			
			autoschoolDS.Tables["auto"].Columns["available"].DefaultValue = true;
			autoschoolDS.Tables["auto"].Columns["price"].DataType = Type.GetType("System.Int32");
			autoschoolDS.Tables["auto"].Columns["price"].DefaultValue = 5000;



			autoschoolDS.Tables["contract"].Columns["customer_id"].DataType = Type.GetType("System.Int32");
			autoschoolDS.Tables["contract"].Columns["customer_id"].AllowDBNull = false;
			autoschoolDS.Tables["contract"].Columns["contract_type"].DataType = Type.GetType("System.String");
			autoschoolDS.Tables["contract"].Columns["contract_type"].DefaultValue = "A";
			autoschoolDS.Tables["contract"].Columns["payment"].DataType = Type.GetType("System.Int32");
			autoschoolDS.Tables["contract"].Columns["payment"].DefaultValue = 500;
			autoschoolDS.Tables["contract"].Columns["contract_start_date"].DataType = Type.GetType("System.DateTime");
			//autoschoolDS.Tables["contract"].Columns["contract_start_date"].DefaultValue = DateTime.Now;
			autoschoolDS.Tables["contract"].Columns["contract_end_date"].DataType = Type.GetType("System.DateTime");
			//autoschoolDS.Tables["contract"].Columns["contract_end_date"].DefaultValue = DateTime.Now.AddMonths(3);

			autoschoolDS.Tables["practice"].Columns["customer_id"].DataType = Type.GetType("System.Int32");
			autoschoolDS.Tables["practice"].Columns["customer_id"].AllowDBNull = false;
			autoschoolDS.Tables["practice"].Columns["teacher_id"].DataType = Type.GetType("System.Int32");
			autoschoolDS.Tables["practice"].Columns["teacher_id"].AllowDBNull = false;
			autoschoolDS.Tables["practice"].Columns["auto_id"].DataType = Type.GetType("System.Int32");
			autoschoolDS.Tables["practice"].Columns["auto_id"].AllowDBNull = false;
			autoschoolDS.Tables["practice"].Columns["practice_date"].DataType = Type.GetType("System.DateTime");
			autoschoolDS.Tables["practice"].Columns["practice_date"].DefaultValue = DateTime.Now;
			autoschoolDS.Tables["practice"].Columns["mark"].DataType = Type.GetType("System.Int32");
			autoschoolDS.Tables["practice"].Columns["mark"].DefaultValue = 8;


			customerAdapter.Fill(autoschoolDS, "customer");
			teacherAdapter.Fill(autoschoolDS, "teacher");
			autoAdapter.Fill(autoschoolDS, "auto");
			contractAdapter.Fill(autoschoolDS, "contract");
			practiceAdapter.Fill(autoschoolDS, "practice");


			foreach(string table in new string[] { "customer", "teacher", "auto", "contract", "practice" }) {
				autoschoolDS.Tables[table].PrimaryKey = new DataColumn[] { autoschoolDS.Tables[table].Columns[table + "_id"] };

				autoschoolDS.Tables[table].Columns[table + "_id"].AutoIncrement = true;
				var lastId = (from m in autoschoolDS.Tables[table].AsEnumerable()
							  select m[table + "_id"]).Max();
				autoschoolDS.Tables[table].Columns[table + "_id"].AutoIncrementSeed = (int)lastId + 1;
				autoschoolDS.Tables[table].Columns[table + "_id"].AutoIncrementStep = 1;
			};


			ForeignKeyConstraint CustomerToContract =
				new ForeignKeyConstraint(
					"CustomerContract",
					autoschoolDS.Tables["customer"].Columns["customer_id"],
					autoschoolDS.Tables["contract"].Columns["customer_id"]
					);
			autoschoolDS.Tables["contract"].Constraints.Add(CustomerToContract);
			

			DataRelation CustomerToPractice =
				new DataRelation(
					"CustomerPractice",
					autoschoolDS.Tables["customer"].Columns["customer_id"],
					autoschoolDS.Tables["practice"].Columns["customer_id"]
					);
			autoschoolDS.Relations.Add(CustomerToPractice);

			DataRelation AutoToPractice =
				new DataRelation(
					"AutoPractice",
					autoschoolDS.Tables["auto"].Columns["auto_id"],
					autoschoolDS.Tables["practice"].Columns["auto_id"]
					);
			autoschoolDS.Relations.Add(AutoToPractice);
			AutoToPractice.ChildKeyConstraint.DeleteRule = Rule.Cascade;

			DataRelation TeacherToPractice =
				new DataRelation(
					"TeacherPractice",
					autoschoolDS.Tables["teacher"].Columns["teacher_id"],
					autoschoolDS.Tables["practice"].Columns["teacher_id"]
					);
			autoschoolDS.Relations.Add(TeacherToPractice);



			
			//var UpdateCmd = autoCommands.GetUpdateCommand();
			//var parameter = UpdateCmd.Parameters.Add("@auto_type", MySqlDbType.Int32,13, "auto_type");
			//auto_type = @auto_type,     `colour` = @p2, `available` = @p3, `price` = @p4
			//UpdateCmd.CommandText = $"UPDATE `auto` SET `auto_name` = @p1 where `auto_id` = @p6";
			//autoAdapter.UpdateCommand = UpdateCmd;
		
			/*
			var DeleteCmd = customerCommands.GetDeleteCommand();
			DeleteCmd.CommandText = $"DELETE FROM `customer` WHERE(`customer_id` = @p1)";
			customerAdapter.DeleteCommand = DeleteCmd;
			*/
			/*
			var DeleteCmd = autoCommands.GetDeleteCommand();
			DeleteCmd.CommandText = $"DELETE FROM `auto` WHERE(`auto_id` = @p1)";
			customerAdapter.DeleteCommand = DeleteCmd;
			*/
			/*
			DataRelation manufToProduct = new DataRelation("ManufProduct",
				autoschoolDS.Tables["manufact"].Columns["manuf_id"],
				autoschoolDS.Tables["product"].Columns["prod_manuf_id"]);

			autoschoolDS.Relations.Add(manufToProduct);
			*/

			autoschoolDS.Tables["customer"].RowChanged += StackModified;
			autoschoolDS.Tables["teacher"].RowChanged += StackModified;
			autoschoolDS.Tables["auto"].RowChanged += StackModified;
			autoschoolDS.Tables["contract"].RowChanged += StackModified;
			autoschoolDS.Tables["practice"].RowChanged += StackModified;

			autoschoolDS.Tables["customer"].RowDeleted += StackDeleted;
			autoschoolDS.Tables["teacher"].RowDeleted += StackDeleted;
			autoschoolDS.Tables["auto"].RowDeleted += StackDeleted;
			autoschoolDS.Tables["contract"].RowDeleted += StackDeleted;
			autoschoolDS.Tables["practice"].RowDeleted += StackDeleted;


			autoschoolDS.Tables["auto"].ColumnChanged += AutoTypeSync;
			//autoschoolDS.Tables["customer"].N += StackAdded;
			//autoschoolDS.Tables["teacher"].TableNewRow += StackAdded;
			//autoschoolDS.Tables["auto"].TableNewRow += StackAdded;
			//autoschoolDS.Tables["contract"].TableNewRow += StackAdded;
			//autoschoolDS.Tables["practice"].TableNewRow += StackAdded;

			practiceAdapter.InsertCommand = new MySqlCommand("add_practice");
			practiceAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
			MySqlParameter param = new MySqlParameter("id", MySqlDbType.Int32, 0, "practice_id");
			param.Direction = ParameterDirection.Output;
			practiceAdapter.InsertCommand.Parameters.Add(param);


		}
		// TODO:
		// 1) update configurate
		// 2) fk restrict deleting row with 3 and more child rows
		// 3) call procedure updating rows, without fill
		// 4) linq update non-linearly (update + join)
		// TODO:

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

		private void ShowRowState(object sender, EventArgs e) {
			string tableName = tableSelected.SelectedItem == null ? "customer" : (string)tableSelected.SelectedItem;
			int rowIndex = 1;
			try {
				if(Int32.Parse(rowNumber.Text) == 0) {
					rowIndex = ShowGrid.SelectedIndex;
				} else {
					rowIndex = Int32.Parse(rowNumber.Text);
				}
			} catch {
				rowIndex = 1;
			}
			try {
				rowState.Text = autoschoolDS.Tables[tableName].Rows[rowIndex-1].RowState.ToString();
			} catch {
				rowState.Text = "smth_wrong";
			}
			

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
				MessageText = "Properties of "+ colname,
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
			contractPK.IsVisible = true;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
			ShowGrid.Columns[0].IsHidden = true;
		}

		private void ShowTeacher(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["teacher"].DefaultView;
			addcustomer.IsVisible = false;	
			addteacher.IsVisible = true;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
			contractPK.IsVisible = false;
			ShowGrid.Columns[0].IsHidden = true;
		}

		private void ShowAuto(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["auto"].DefaultView;
			ShowGrid.Columns["auto_type"].AllowEditing = false;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = true;
			addcontract.IsVisible = false;
			addpractice.IsVisible = false;
			contractPK.IsVisible = false;
			//ShowGrid.Columns[0].IsHidden = true;
		}

		private void ShowContract(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["contract"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = true;
			addpractice.IsVisible = false;
			ShowGrid.Columns[0].IsHidden = true;
			contractPK.IsVisible = false;
		}
		
		private void ShowPractice(object sender, EventArgs e) {
			ShowGrid.ItemsSource = autoschoolDS.Tables["practice"].DefaultView;
			addcustomer.IsVisible = false;
			addteacher.IsVisible = false;
			addauto.IsVisible = false;
			addcontract.IsVisible = false;
			addpractice.IsVisible = true;
			contractPK.IsVisible = false;
		}


		private void Sync(object sender, EventArgs e) {

			ShowGrid.EndEdit();
			//autoschoolDS.Tables["product"].AcceptChanges();
			//productAdapter.Up;

			try {
				contractAdapter.Update(autoschoolDS.Tables["contract"]);
				practiceAdapter.Update(autoschoolDS.Tables["practice"]);
				customerAdapter.Update(autoschoolDS.Tables["customer"]);
				autoAdapter.Update(autoschoolDS.Tables["auto"]);
				teacherAdapter.Update(autoschoolDS.Tables["teacher"]);
				changed.Clear();
				modified.Clear();

			} catch(MySqlException ex) {
				Console.WriteLine(ex.Message);
			}
			//autoschoolDS.Clear();
			customerAdapter.Fill(autoschoolDS, "customer");
			teacherAdapter.Fill(autoschoolDS, "teacher");
			autoAdapter.Fill(autoschoolDS, "auto");
			contractAdapter.Fill(autoschoolDS, "contract");
			practiceAdapter.Fill(autoschoolDS, "practice");

			// PageLoaded(null, null);
		}

		private void Cancel(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			autoschoolDS.RejectChanges();
		}

		private void CancelDeleted(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			foreach(DataTable table in autoschoolDS.Tables) {
				foreach(DataRow row in table.Rows) {
					if(row.RowState == DataRowState.Deleted) {
						row.RejectChanges();
					}
				}
			}
		}

		private void CancelModified(object sender, EventArgs e) {
			ShowGrid.EndEdit();
			foreach(DataTable table in autoschoolDS.Tables) {
				foreach(DataRow row in table.Rows) {
					if(row.RowState == DataRowState.Modified) {
						row.RejectChanges();
					}
				}
			}
		}

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

		private void AutoTypeSync(object sender, DataColumnChangeEventArgs e) {

			if(e.Column.Table.TableName == "auto" && e.Column.ColumnName == "auto_type_id") {
				try {
					e.Row["auto_type"] = ((AutoTypes)e.Row["auto_type_id"]).ToString();
				} catch  {
				}
			} 

		}

		//private void StackAdded(object sender, DataRowChangeEventArgs e) {

		//	if(e.Row.RowState == DataRowState.Unchanged) {				
		//		changed.Remove(e.Row);
		//	} else {				
		//		changed.Add(e.Row, DateTime.Now);
		//	}

		//}

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

		private void AddCustomer(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["customer"].NewRow();
			//row[0] = (int)(autoschoolDS.Tables["customer"].AsEnumerable()).Last()[0] + 1;
			row[1] = "Name";
			row[2] = "Surname";
			row[4] = (DateTime)DateTime.Now.AddYears(-18);
			autoschoolDS.Tables["customer"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["customer"].DefaultView;
		}

		private void AddTeacher(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["teacher"].NewRow();
			//row[0] = (int)(autoschoolDS.Tables["teacher"].AsEnumerable()).Last()[0] + 1;
			row[1] = "Name";
			row[2] = "Surname";
			row[3] = "";
			autoschoolDS.Tables["teacher"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["teacher"].DefaultView;
		}

		private void AddAuto(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["auto"].NewRow();
			//row[0] = (int)(autoschoolDS.Tables["auto"].AsEnumerable()).Last()[0] + 1;
			autoschoolDS.Tables["auto"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["auto"].DefaultView;
		}

		private void AddContract(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["contract"].NewRow();
			//row[0] = (int)(autoschoolDS.Tables["contract"].AsEnumerable()).Last()[0] + 1;
			row[4] = (DateTime)DateTime.Now;
			row[5] = (DateTime)DateTime.Now.AddMonths(3);
			autoschoolDS.Tables["contract"].Rows.Add(row);

			ShowGrid.ItemsSource = autoschoolDS.Tables["contract"].DefaultView;
		}

		private void AddPractice(object sender, EventArgs e) {
			DataRow row;
			row = autoschoolDS.Tables["practice"].NewRow();
			//row[0] = (int)(autoschoolDS.Tables["practice"].AsEnumerable()).Last()[0] + 1;
			row[1] = 1;
			row[2] = 1;
			row[3] = 1;
			row[4] = (DateTime)DateTime.Now;
			autoschoolDS.Tables["practice"].Rows.Add(row);

			practiceAdapter.Update(autoschoolDS, "practice");
			autoschoolDS.AcceptChanges();
			changed.Clear();
			modified.Clear();

			ShowGrid.ItemsSource = autoschoolDS.Tables["practice"].DefaultView;
		}

		private void ContractPK(object sender, EventArgs e) {
			if(ShowGrid.SelectedItem != null) {
				DataRow selectedRow = ((DataRow)((DataRowView)ShowGrid.SelectedItem).Row);
				DataRow row;
				row = autoschoolDS.Tables["contract"].NewRow();
				row[1] = selectedRow[0];
				row[2] = contractType.Text;
				row[4] = (DateTime)DateTime.Now;
				row[5] = (DateTime)DateTime.Now.AddMonths(3);
				autoschoolDS.Tables["contract"].Rows.Add(row);


				ShowGrid.ItemsSource = autoschoolDS.Tables["contract"].DefaultView;
			}
		}

		private void RemoveItem(object sender, EventArgs e) {
			if(ShowGrid.SelectedItem != null) {
				//autoschoolDS.Tables["product"].Rows.Remove((DataRow)((DataRowView)ShowGrid.SelectedItem).Row);
				//autoschoolDS.Tables["product"].Rows.RemoveAt((int)ShowGrid.SelectedIndex-1);
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

		private void FilterContract(object sender, EventArgs e) {

			var result = from contract in autoschoolDS.Tables["contract"].AsEnumerable()
						 where
						 (int.TryParse(minPrice.Text, out int min) ? min : 0) < (int)contract["payment"]
						 && (int)contract["payment"] < (int.TryParse(maxPrice.Text, out int max) ? max : 5000)
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
						 (auto["auto_name"].ToString()).Contains(autoPattern.Text.ToString())
						 &&
						 (int.TryParse(minPriceAuto.Text, out int min) ? min : 0) < (int)auto["price"]
						 && (int)auto["price"] < (int.TryParse(maxPriceAuto.Text, out int max) ? max : 70000)
						 select auto;
			if(result.Count() != 0) {
				resultGrid.ItemsSource = result.CopyToDataTable().DefaultView;
			}
		}

		private void UpdatePriceAuto(object sender, EventArgs e) {

			(from auto in autoschoolDS.Tables["auto"].AsEnumerable()
			 where
			 ((string)auto["auto_name"]).Contains(autoPattern.Text.ToString())
			 select auto).ForEach(row => row["price"]= (int.TryParse(updatePriceAuto.Text, out int newPrice) ? newPrice : 7000));

		}


		private void DeleteAutoName(object sender, EventArgs e) {

			(from auto in autoschoolDS.Tables["auto"].AsEnumerable()
			 where
			 ((string)auto["auto_name"]).Contains(autoPattern.Text.ToString())
			 select auto).ForEach(row => row.Delete()) ;

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
						Payment = rest.Average(x => (int)x["payment"]),
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
						 join practice in autoschoolDS.Tables["practice"].AsEnumerable()
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
			var result = from customer in autoschoolDS.Tables["customer"].AsEnumerable()
						 join practice in autoschoolDS.Tables["practice"].AsEnumerable()
						 on customer["customer_id"] equals practice["customer_id"]
						 group new { customer, practice }
						 by new {
							 id = customer.Field<int>("customer_id"),
							 Name = customer.Field<string>("customer_name"),
							 Surname = customer.Field<string>("customer_surname"),
							 Phone = customer.Field<string>("customer_phone")
						 } into customerpractice
						 orderby customerpractice.Key.id ascending
						 select new {
							 customerpractice.Key.id,
							 customerpractice.Key.Name,
							 customerpractice.Key.Surname,
							 customerpractice.Key.Phone,
							 minMark = customerpractice.Min(x => x.practice.Field<int>("mark")),
							 maxMark = customerpractice.Max(x => x.practice.Field<int>("mark")),
							 averageMark = (int)customerpractice.Average(x => x.practice.Field<int>("mark"))
						 };

			resultGrid.ItemsSource = result;
		}



		

	}
}

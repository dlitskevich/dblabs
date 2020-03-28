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

		private void Russians(object sender, EventArgs e) {
			var result = from lang in worldDS.Tables["countrylanguage"].AsEnumerable()
						 group lang by new { id=lang.Field<string>("Language") } into langGrouped
						 where (langGrouped.Key.id).ToLower() == "russian"
						 select new {
							 Language = langGrouped.Key.id,
							 Quantity = langGrouped.Count()
						 };

			resultGrid.ItemsSource = result;
		}

		private void Areas(object sender, EventArgs e) {
			var result = from country in worldDS.Tables["country"].AsEnumerable()
						 from max in (
							from africa in worldDS.Tables["country"].AsEnumerable()
							group africa by new {
								id=africa.Field<string>("continent")
							} into africaGrouped
							where africaGrouped.Key.id.ToLower() == "africa"
							select new { max = africaGrouped.Max(x => x.Field<float>("SurfaceArea")) }

							)
						 where country.Field<float>("SurfaceArea") > max.max
						 select new {
							 Name = country.Field<string>("Name"),
							 Area = country.Field<float>("SurfaceArea"),
							 AfricaMaxArea = max.max
						 };

			resultGrid.ItemsSource = result;
		}

		private void Cities(object sender, EventArgs e) {
			var population = from country in worldDS.Tables["country"].AsEnumerable()
							 where country.Field<string>("Continent").ToLower() == "europe"
							 select (int)country[6];

			var result = from city in worldDS.Tables["city"].AsEnumerable()
						 where (
							from countryPop in population
							where countryPop <= city.Field<int>("Population")
							select countryPop).Count() == 3
						 select city;

			resultGrid.ItemsSource = result.CopyToDataTable().DefaultView;
		}


		private void Capital(object sender, EventArgs e) {
			var CountryAVGPopulation = from city in worldDS.Tables["city"].AsEnumerable()
									group city
									   by new { Code=city.Field<string>("Countrycode") }
									   into cityGrouped
									select new { cityGrouped.Key.Code, AVGPopulation=cityGrouped.Average(x => x.Field<int>("Population")) };

			var Capitals = from city in worldDS.Tables["city"].AsEnumerable()
						   join country in worldDS.Tables["country"].AsEnumerable()
						   on city["ID"] equals country["Capital"]
						   select new {
							   Code=country["Code"],
							   Name=country["Name"],
							   Capital=city["Name"],
							   CapitalPopulation=city["Population"]
						   };

			var result = from country in CountryAVGPopulation.AsEnumerable()
						 join city in Capitals.AsEnumerable()
						 on country.Code equals city.Code
						 where (int)country.AVGPopulation > (int)city.CapitalPopulation
						 select new { city.Name, AVGPopulation = (int)country.AVGPopulation, city.Capital, city.CapitalPopulation };

			resultGrid.ItemsSource = result;
		}


		private void LangPop(object sender, EventArgs e) {
			var language =
				from lang in worldDS.Tables["countrylanguage"].AsEnumerable()
				group lang by new { CountryCode = lang.Field<string>("CountryCode") } into langGrouped

				select new {
					CountryCode = langGrouped.Key.CountryCode,
					Quantity = langGrouped.Count()
				};

			var countries = from city in worldDS.Tables["city"].AsEnumerable()
									   group city
										  by new { CountryCode = city.Field<string>("Countrycode") }
									   into cityGrouped
									   select new {
										   cityGrouped.Key.CountryCode,
										   QuantCitiesPopGreater = cityGrouped.Count(x => x.Field<int>("Population")>1000000) };

			
			var result = from country in countries.AsEnumerable()
						 join lang in language.AsEnumerable()
						 on country.CountryCode equals lang.CountryCode
						 where (int)country.QuantCitiesPopGreater > (int)lang.Quantity
						 select new { country.CountryCode, country.QuantCitiesPopGreater,
							 lang.Quantity
						 };

			resultGrid.ItemsSource = result;
		}

		/*
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

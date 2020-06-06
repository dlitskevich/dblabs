using System;
using MongoDB.Bson;
using MongoDB.Driver;

namespace dblabs_mongo {
	class Program {
		public static string conn = "mongodb://127.0.0.1:27017";
        public static MongoClient clientMDB = new MongoClient(conn);
		public static IMongoDatabase database = clientMDB.GetDatabase("maskshop");
        public static IMongoCollection<Customer> customer = database.GetCollection<Customer>("customer");
        public static IMongoCollection<Provider> provider = database.GetCollection<Provider>("provider");
        public static IMongoCollection<Item> item = database.GetCollection<Item>("item");
        public static IMongoCollection<Purchase> purchase = database.GetCollection<Purchase>("purchase");
        public static IMongoCollection<Staff> staff = database.GetCollection<Staff>("staff");

        public static void Main (string [] args)
		{
			Console.WriteLine("Hello World!\n");

			BsonDocument empty_filter = new BsonDocument();

			//BsonDocument filter = new BsonDocument("Age", new BsonDocument("$gte", 20));
			//BsonDocument filter = new BsonDocument("$and", new BsonArray {
			//	new BsonDocument("Age", new BsonDocument("$gte", 20)),
			//	new BsonDocument("Name","Volodya")
			//});

			Program.Find<Customer>(customer, empty_filter);


			Console.WriteLine ("Aggregation total bought by mask\n");
			var aggregation = purchase.Aggregate ().Unwind("Items")
				.Group (
			new BsonDocument {
				{ "_id", "$Items.Name" },
				{ "Total", new BsonDocument("$sum", "$Items.Amount") }
			}).Sort(new BsonDocument("_id", -1)).ToList();

			foreach (BsonDocument p in aggregation) {
				Console.WriteLine("{0} - {1}", p.GetValue("_id"), p.GetValue("Total"));
			}

            Console.WriteLine("Aggregation total bought by mask\n");
            var aggregation2 = purchase.Aggregate()
				.Lookup("customer","Customer.id", "_id", "CustomerInfo")
				.Unwind("CustomerInfo").ToList();

            foreach(BsonDocument p in aggregation2) {
                Console.WriteLine("Customer: {1,-10}  {2, -16}  Seller: {3, -10}  Time: {4, -10}   {0,-20}",
					p.GetValue("_id"),
					p.GetValue("Customer").AsBsonDocument.GetValue("Name"),
                    p.GetValue("CustomerInfo").AsBsonDocument.GetValue("Surname"),
                    p.GetValue("Staff").AsBsonDocument.GetValue("Name"),
					p.GetValue("Time"));

                Console.WriteLine("Items:");
                foreach(BsonDocument i in p.GetValue("Items").AsBsonArray) {
                    
                    Console.WriteLine("  {0,-20}   {1,-10}   Amount: {2, -5}   Price: {3, -4}", i.GetValue("id"), i.GetValue("Name"), i.GetValue("Amount"), i.GetValue("Price"));
                }
                Console.WriteLine();
            }


            //Provider prov = new Provider().Find();
            //Item itm = new Item().Create();
            //Item itmf = new Item().Find();
            //Purchase pu = new Purchase().Create();

            CRUDitems();

            new Purchase().MakePurchase();
            //Console.ReadLine ();
        }


		public static void CRUDitems() {

			Console.WriteLine
				("Press select your option from the following\n1 - insert\n2 - update One Document\n3 - delete\n4 - read All\n");
			string userSelection = Console.ReadLine();

			switch(userSelection) {
				case "insert":
			//Insert  
			item.InsertOne(new Item().Create());
			break;

				case "update":
				//Update
				Console.WriteLine("To Update!!");
				var new_item = new Item().Create();
				
				item.FindOneAndUpdate<Item>
					(Builders<Item>.Filter.And(
				        new BsonDocument("$and", new BsonArray { 
				            new BsonDocument( "Name", new BsonDocument("$regex", new_item.Name) ),
							new BsonDocument("Provider.Name", new BsonDocument("$regex", new_item.Provider.GetValue("Name")))
						})),
						Builders<Item>.Update.Set("Amount", new_item.Amount).Set("Price", new_item.Price));
				break;
				
				case "delete":
			//Find and Delete  
			Console.WriteLine("Please Enter the item name to delete: ");
			var deletefirstName = Console.ReadLine();
            Console.WriteLine(item.DeleteOne(new BsonDocument("Name", new BsonDocument("$regex", deletefirstName))  ));

			break;

			case "read":
			//Read all existing document  
			var all = item.Find(new BsonDocument());
			Console.WriteLine();

			foreach(var i in all.ToEnumerable()) {
                
                //Console.WriteLine("{0} - {1}", p.GetValue("_id"), p.GetValue("Total"));
                
                Console.WriteLine("{0,-20}   {1,-10}   {2, -8}   {3, -5}   {4, -4}",i.Id, i.Name, i.Provider.GetValue("Name"), i.Amount, i.Price);
			} 

			break;

			default:
			Console.WriteLine("Please choose a correct option");
			break;
			}

			//To continue with Program  
			Console.WriteLine("\n--------------------------------------------------------------\nEnter anything to exit...\n");
			string userChoice = Console.ReadLine();

			if(userChoice != "") {
				CRUDitems();
			}
		}
		
		public static void Find<T>(IMongoCollection<T> collection, BsonDocument filter) {
			using(var cursor = collection.FindSync(filter)) {
				while(cursor.MoveNext()) {

					var entry = cursor.Current;
					foreach(var doc in entry) {
						foreach(var p in doc.ToBsonDocument().Elements) {
							Console.WriteLine(p);
						}
						Console.WriteLine();
					}
				}
			}
		}


        public class Purchase {
            public ObjectId Id { get; set; }
            public BsonDocument Customer { get; set; }
            public BsonDocument Staff { get; set; }
            public BsonArray Items { get; set; }
            public string Time { get; set; }

            public Purchase Create() {
                bool enough = false;
                Items = new BsonArray();

                Customer customer_current = new Customer().Find();
                Staff staff_current = new Staff().Find();

				while(!enough) {
                    Console.WriteLine("  Please enter item info  ");
                    Item new_item = new Item().Find();
                    Console.WriteLine("  Please enter amount ");
                    var amount = Console.ReadLine();

                    Items.Add(new BsonDocument { { "Name", new_item.Name}, {"Amount", amount }, { "Name", new_item.Price }, { "Name", new_item.Id } });

                    Console.WriteLine("  To stop enter anything  ");
                    var next = Console.ReadLine();
					if(next != null) {
                        enough = true;
                    }
                }


                this.Customer = new BsonDocument { { "Name", customer_current.Name } , { "_id", customer_current.Id }};
                this.Staff = new BsonDocument { { "Name", staff_current.Name }, { "_id", staff_current.Id } };

                Time = DateTime.Now.ToShortDateString();

                return this;
            }

            public void MakePurchase() {
                bool enough = false;
                Items = new BsonArray();

                Customer customer_current = new Customer().Find();
                Staff staff_current = new Staff().Find();

                while(!enough) {
                    double a;
					Console.WriteLine("  Please enter item info  ");
					Item new_item = new Item().Find();
                    Console.WriteLine("  Please enter amount ");
                    var amount = double.TryParse(Console.ReadLine(), out a) ? a : default(double);

                    if(new_item.Amount > amount && amount>0) {
                        Items.Add(new BsonDocument { { "Name", new_item.Name }, { "Amount", amount }, { "Price", new_item.Price }, { "id", new_item.Id } });
                        item.FindOneAndUpdate<Item>(Builders<Item>.Filter.Eq("_id", new_item.Id),
								Builders<Item>.Update.Set("Amount", new_item.Amount-amount)
							);
                    } else {
                        Console.WriteLine("  Not added ");
                    }

                    if(Items.Count > 0) {
                        Console.WriteLine("  To stop enter anything  ");
                        var next = Console.ReadLine();
                        if(next != null) {
                            enough = true;
                        }
                    }
                    
                }


                this.Customer = new BsonDocument { { "Name", customer_current.Name }, { "_id", customer_current.Id } };
                this.Staff = new BsonDocument { { "Name", staff_current.Name }, { "_id", staff_current.Id } };

                Time = DateTime.Now.ToShortDateString();

                purchase.InsertOne(this);

                return;
            }

        }


        public class Item {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
            public BsonDocument Provider { get; set; }
            public double Amount { get; set; }
            public double Price { get; set; }

            public Item Create() {
                double a;
                Console.WriteLine("Please enter item Name : ");
                Name = Console.ReadLine();

                Provider prov = new Provider().Find();

                this.Provider = new BsonDocument { { "_id", prov.Id}, { "Name", prov.Name} };

                Console.WriteLine("Please enter item Amount : ");
                Amount = double.TryParse(Console.ReadLine(), out a) ? a : default(double);
                Console.WriteLine("Please enter item Price : ");
                Price = double.TryParse(Console.ReadLine(), out a) ? a : default(double); 

                return this;
            }

            public Item Find() {
                Console.WriteLine("Please enter item Name : ");
                Name = Console.ReadLine();

                Provider prov = new Provider().Find();

                var found = item.FindSync<Item>(new BsonDocument("$and",
					new BsonArray { 
                        new BsonDocument( "Name", new BsonDocument("$regex", Name) ),
                        new BsonDocument("Provider.Name", new BsonDocument("$regex", prov.Name) )
                    })).FirstOrDefault();

                if(found == null) {
                    Console.WriteLine("Nothing found. Try again : ");
                    return this.Find();
                }

                return found;
            }
        }

        public class Provider {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
            public string Info { get; set; }

            public Provider Find() {
                Console.WriteLine("Please enter provider Name : ");
                Name = Console.ReadLine();

                var found = provider.FindSync<Provider>(new BsonDocument( "Name", new BsonDocument("$regex", Name))).FirstOrDefault();

				if(found == null) {
                    Console.WriteLine("Nothing found. Try again : ");
                    return this.Find();
                }
                //Id = (ObjectId)found.GetValue("_id");
                //Name = (string)found.GetValue("Name");
                //Info = (string)found.GetValue("Info");

             return found;
            }
        }


        public class Customer {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }

			public Customer Find() {
                Console.WriteLine("Please enter customer Name : ");
                Name = Console.ReadLine();

                Console.WriteLine("Please enter customer Surname : ");
                Surname = Console.ReadLine();

                var found = customer.FindSync<Customer>(
					new BsonDocument("$and",
                    new BsonArray {
                        new BsonDocument( "Name", new BsonDocument("$regex", Name) ),
                        new BsonDocument( "Surname", new BsonDocument("$regex", Surname) )
                    })).FirstOrDefault();

                if(found == null) {
                    Console.WriteLine("Nothing found. Try again : ");
                    return this.Find();
                }
                //Id = (ObjectId)found.GetValue("_id");
                //Name = (string)found.GetValue("Name");
                //Surname = (string)found.GetValue("Surname");

                return found;
            }
        }


        public class Staff {
            public ObjectId Id { get; set; }
            public string Name { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Info { get; set; }

            public Staff Find() {
                Console.WriteLine("Please enter staff Name : ");
                Name = Console.ReadLine();

                var found = staff.FindSync<Staff>(new BsonDocument("Name", new BsonDocument("$regex", Name))).FirstOrDefault();

                if(found == null) {
                    Console.WriteLine("Nothing found. Try again : ");
                    return this.Find();
                }
                //Id = (ObjectId)found.GetValue("_id");
                //Name = (string)found.GetValue("Name");
                //From = (string)found.GetValue("From");
                //To = (string)found.GetValue("To");
                //Info = (string)found.GetValue("Info");

                return found;
            }
        }
    }

    
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class Builder : MonoBehaviour 
{
	#region/***************** RANDOM INFORMATION ***********************/
	int CategoryTotal;
	int ProductTotal;
	#endregion
	//Selectable number of aisles for store
	int NumberOfAisles;
	//Selectable number of nodes per aisle
	int NumberOfNodes= 5;
	//Aisle Prefab
	public GameObject AisleObject;
	//Store floor
	public GameObject Floor;
	//which side from the center to spawn the next aisle in
	int SideOfCenter;
	//location of the file that represents the inventory
	string InventoryFile;
	//array of aisles
	GameObject[] AisleList;
	//list of every product read from inventory
	List<Product> Inventory = new List<Product>();

	//The product class which represents an item in a store
	public class Product
	{
		public string Category;
		public string Name;
		public string Chilled;

		public Product(string Cat, string Nam, string Chil)
		{
			Category = Cat;
			Name = Nam;
			Chilled = Chil;
		}
		public Product()
		{
			Category = "";
			Name = "";
			Chilled = "";
		}
	}
	void Start () 
	{
		//constant location to store files in plus name of the file
		InventoryFile = Application.streamingAssetsPath.ToString() + "\\Inventory.txt";
		//Set the side of center to 1 to start on right
		//SideOfCenter = 1;
		NumberOfAisles = 5;
		//counter
		//int AislesSpawned = 0;
		//the x posistion in the world to spawn the aisle
		float Xpos;
		//fill the inventory list first
		FillList();
		//arrays are faster so make one the size of the selected number of aisles to fill
		AisleList = new GameObject[NumberOfAisles];
//		print (NumberOfAisles.ToString());
		//loop to fill
		for(int Aisles = 0; Aisles < NumberOfAisles; Aisles++) 
		{
			//gets the size of the floor
			float FloorSize = Floor.renderer.bounds.size.x;
			//calculates the x posistion based on the location of the floor then divided up into equal parts based on the number selected
			//for the total number of aisles
			Xpos = (Floor.transform.position.x - (FloorSize / 2)) + ((FloorSize / NumberOfAisles) * Aisles) + ((FloorSize / NumberOfAisles) / 2);
			//spawns in an aisle and adds it to the array
			AisleList[Aisles] = (Instantiate(AisleObject, new Vector3(Xpos,
			                               Floor.transform.position.y + (AisleObject.renderer.bounds.size.y / 2),
			                               Floor.transform.position.z),
			                          Quaternion.identity) as GameObject);
			//feeds in the number of nodes the aisle should spawn
			AisleList[Aisles].GetComponent<Aisle>().NumberOfNodes = NumberOfNodes;
			AisleList[Aisles].GetComponent<Aisle>().SpawnNodes();
			AisleList[Aisles].GetComponent<Aisle>().SetupNodes(AisleList[Aisles].GetComponent<Aisle>().LeftSide);
			AisleList[Aisles].GetComponent<Aisle>().SetupNodes(AisleList[Aisles].GetComponent<Aisle>().RightSide);
//			print ("Spawned, Current Aisle count is: " + AisleList.Length.ToString());
			//changes Side of Center;
			//SideOfCenter *= -1;
			//only adds to the counter thus moving the location onces both left and right of the center has been spawned
			//if(SideOfCenter == 1)
			//{
			//	AislesSpawned++;
			//}
		}
		//now pass the shelves the items they should hold and give to their nodes
		StockShelves();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	void FillList()
	{
		//used for knowing when we found a new cetegory
		string CurrentCategory = "";
		//string representing the read in line
		string line;
		//gets a reader for the file
		System.IO.StreamReader file = new System.IO.StreamReader(InventoryFile);
		//read until a null line is found
		while((line = file.ReadLine()) != null)
		{
			//create a new "product" object to fill with data
			Product newProduct = new Product();
			//splits the properties based on a comma
			string[] words = line.Split(',');
			//first word found will be the product name
			newProduct.Name = words[0];
			//second will be the category of product
			newProduct.Category = words[1];
			//now the object has been created so add it to the masterlist
			Inventory.Add(newProduct);
			//if we don't have a current category we know this is the first
			if(CurrentCategory == "")
			{
				//make the currently being read category the "currentcategory"
				CurrentCategory = newProduct.Category;
				//up the category total
				CategoryTotal++;
			}
			//its not the first category but it doesn't match which means we're in a new category
			if(newProduct.Category != CurrentCategory)
			{
				//up the category total
				CategoryTotal++;
				//switch the current category with the new one we've found
				CurrentCategory = newProduct.Category;
			}

			
		}
		//close the file up
		file.Close();
		//sort the list by category
		Inventory.Sort((a, b) =>  a.Category.CompareTo(b.Category));
		//counts the number of categories for multiple reasons

		//sets the product total to the number of products read from file
		ProductTotal = Inventory.Count;
		//each aisle has two sides and each side needs to have a MIN of 1 category
		if (CategoryTotal < NumberOfAisles / 2)
		{
			//if we asked for more aisles than that a reduction is made
			NumberOfAisles = CategoryTotal;
		}
		//Overstocked shelves suck so if all the aisles have to handle more than 8 types of things
		else if(CategoryTotal / NumberOfAisles > 8)
		{
			//make an increse in aisles
			NumberOfAisles = CategoryTotal / 8;
			//if theres a remainder fix it
			if(CategoryTotal % 8 > 0)
			{
				NumberOfAisles++;
			}
		}
	}
	void StockShelves()
	{

		string CurrentCategory = "";
		int CurrentAisle = 0;
		List<Product> Products = new List<Product>();
		foreach(Product Item in Inventory)
		{
			if(CurrentCategory == "")
			{
				CurrentCategory = Item.Category;
			}
			if(Item.Category == CurrentCategory)
			{
				Products.Add(Item);
			}
			else
			{
//				print("Aisle Size is: " + AisleList.Length.ToString());
				AisleList[CurrentAisle].GetComponent<Aisle>().StockShelves(Products);
				if(CurrentAisle + 1 >= NumberOfAisles)
				{
					CurrentAisle = 0;
				}
				else
				{
					CurrentAisle++;
				}
				Products.Clear();
				CurrentCategory = Item.Category;
			}
//			print(CurrentAisle.ToString());
		}
	}
}

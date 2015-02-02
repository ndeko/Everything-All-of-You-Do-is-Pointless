using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Aisle : MonoBehaviour 
{

	public int NumberOfNodes;
	public GameObject PreviousAisle;
	public GameObject NextAisle;
	public GameObject Node;
	public GameObject[] LeftSide = new GameObject[NumberOfNodes];
	public GameObject[] RightSide = new GameObject[NumberOfNodes];
	int SideOfCenter;
	bool StockLeft= true;
	void Start () 
	{
//		SideOfCenter = 1;

		//LeftSide = new List<GameObject>();


		//SpawnNodes();
		//SetupNodes(LeftSide);
		//SetupNodes(RightSide);
	}

	public void SpawnNodes()
	{
		for(int SpawnedNodes = 0; SpawnedNodes < NumberOfNodes; SpawnedNodes++) 
		{
			float AisleLength = this.gameObject.renderer.bounds.size.z;
			float Zpos = (this.transform.position.z - (AisleLength / 2)) + ((AisleLength / NumberOfNodes) * SpawnedNodes) + ((AisleLength / NumberOfNodes) / 2) ;
			
			GameObject NewNode = Instantiate(Node, new Vector3(this.transform.position.x + 1f, this.transform.position.y, Zpos),
			                                 Quaternion.identity) as GameObject;
			NewNode.gameObject.transform.Rotate(90,0,0);
			RightSide[SpawnedNodes] = NewNode;
		}
		for(int SpawnedNodes = 0; SpawnedNodes < NumberOfNodes; SpawnedNodes++) 
		{
			float AisleLength = this.gameObject.renderer.bounds.size.z;
			float Zpos = (this.transform.position.z - (AisleLength / 2)) + ((AisleLength / NumberOfNodes) * SpawnedNodes) + ((AisleLength / NumberOfNodes) / 2) ;
			
			GameObject NewNode = Instantiate(Node, new Vector3(this.transform.position.x - 1f, this.transform.position.y, Zpos),
			                                 Quaternion.identity) as GameObject;
			NewNode.gameObject.transform.Rotate(90,0,0);
			LeftSide[SpawnedNodes] = NewNode;
		}
	}
	public void SetupNodes(GameObject[] Side)
	{

		for (int CurrentIndex = 0; CurrentIndex < NumberOfNodes; CurrentIndex++)
		{
			if (CurrentIndex > 0)
			{
				Side[CurrentIndex].GetComponent<ProductNode>().PreviousNode = Side[CurrentIndex - 1];
			}
			if(CurrentIndex + 1 < NumberOfNodes)
			{
				Side[CurrentIndex].GetComponent<ProductNode>().NextNode = Side[CurrentIndex + 1];
			}
		}
	}
	public void StockShelves(List<Builder.Product> Products)
	{
		int NodeCounter = 0;
		if(StockLeft)
		{
			foreach(Builder.Product Item in Products)
			{
				if(NodeCounter + 1 < NumberOfNodes)
				{
					LeftSide[NodeCounter].GetComponent<ProductNode>().Products.Add(Item);
				}
				else
				{
					NodeCounter = 0;
				}
			}
		}
		else
		{
			//NodeCounter = 0;
			foreach(Builder.Product Item in Products)
			{
				if(NodeCounter + 1 < NumberOfNodes)
				{
					print(RightSide.Length.ToString());
					RightSide[NodeCounter].GetComponent<ProductNode>().Products.Add(Item);
				}
				else
				{
					NodeCounter = 0;
				}
			}
		}
		StockLeft = !StockLeft;
	}
	// Update is called once per frame
	void Update () 
	{
	
	}
}

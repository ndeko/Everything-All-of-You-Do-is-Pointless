using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ProductNode : MonoBehaviour 
{

	// Use this for initialization
	public GameObject PreviousNode;
	public GameObject NextNode;
	public GameObject OppositeNode;
	public List<Builder.Product> Products = new List<Builder.Product>();
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown()
	{
		foreach (Builder.Product currentItem in Products)
		{
			print(currentItem.Name);
		}
	}
}

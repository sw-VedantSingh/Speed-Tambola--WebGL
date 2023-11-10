using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTambolaPool : MonoBehaviour
{
	public GameObject Token;
	public GameObject PoolContainer;
	public List<GameObject> pooledObjectsList;
	
	public void TokenPool()
	{
		pooledObjectsList = new List<GameObject>();
		if(pooledObjectsList.Count > 0) pooledObjectsList.Clear();
		GameObject tmp;
		for (int i = 0; i <10; i++)
		{
			tmp = Instantiate(Token, PoolContainer.transform);
			//tmp.transform.localScale = Vector3.one;
			pooledObjectsList.Add(tmp);
		}
	}
	
	public GameObject GetToken()
	{
		GameObject TokenObj;
		TokenObj = PoolContainer.transform.GetChild(0).gameObject;
		return TokenObj;
	}
}
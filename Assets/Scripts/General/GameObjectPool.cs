using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool
{
	protected List<GameObject> m_UsingObjects = new List<GameObject>();
	protected Stack<GameObject> m_FreeObjects = new Stack<GameObject>();
	protected string Name;
	
	public GameObjectPool ()
	{
	}
	
	public void Create(GameObject Prefab, int count)
	{
		Destroy();
		//m_Pool = new GameObject[count];
		for(int i = 0; i < count; ++i)
		{
			GameObject Obj = GameObject.Instantiate(Prefab) as GameObject;
			Obj.active = false;
			m_FreeObjects.Push(Obj);
		}
	}
	
	public void Destroy()
	{
		foreach(GameObject Obj in m_UsingObjects)
		{
			GameObject.Destroy(Obj);
		}
		
		m_UsingObjects.Clear();
	}
	
	public GameObject GetNext()
	{
		if(m_FreeObjects.Count == 0)
		{
			Debug.LogError(string.Format("No object left in object pool: {0}", Name));
			return null;
		}
		
		GameObject Obj = m_FreeObjects.Pop();
		Obj.active = true;
		m_UsingObjects.Add(Obj);
		return Obj;
	}
	
	public void Update()
	{
		foreach(GameObject Obj in m_UsingObjects)
		{
			if(!Obj.active)
			{
				m_FreeObjects.Push(Obj);
				m_UsingObjects.Remove(Obj);
			}
		}
	}
}


using UnityEngine;
using System.Collections;

public class Utils
{
	public static void EnableGameObjectRender(GameObject Obj, bool enable)
    {
        Obj.renderer.enabled = enable;
        foreach (Transform T in Obj.transform)
        {
            EnableGameObjectRender(T.gameObject, enable);
        }
    }

    public  static Unit CreateRandomUnit(Object Parent, bool playerUnit)
    {
        //Unit Unit = new Unit();
        //Unit Unit = 
        //Unit.Name = Random.Range(0, 1000).ToString();

        //return Unit;
        return null;
    }

    public static GameObject FindChild(GameObject Obj, string childName)
    {
        foreach (Transform T in Obj.transform)
        {
            Debug.Log(T.name);
            if (T.name == childName)
                return T.gameObject;
            return FindChild(T.gameObject, childName);
        }
        return null;
    }
}


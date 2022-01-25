using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilManager<T> : MonoBehaviour where T : Component
{
    public static T GetTargetObject(GameObject _parent)
    {
        for(int i=0; i< _parent.transform.childCount; i++)
        {
            if(_parent.transform.GetChild(i).GetComponent<T>() is T)
            {
                return _parent.transform.GetChild(i) as T;
            }
        }
        return null;
    }

    public static T GetRayCastTarget(Vector3 _touchposition , string _layername)
    {
        Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(_touchposition), Vector2.up);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 100f, 1<<LayerMask.NameToLayer(_layername));
        if(hit.collider != null)
        {
            Debug.Log($"Ray Hit Collider name : {hit.collider.name}");
            return hit.collider.GetComponent<T>();
        }
        return null;
    }
}

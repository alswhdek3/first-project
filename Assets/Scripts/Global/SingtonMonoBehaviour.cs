using System.Collections;
using System.Collections.Generic;

using Photon.Realtime;
using Photon.Pun;

using UnityEngine;

public class SingtonMonoBehaviour<T> : MonoBehaviourPun where T : SingtonMonoBehaviour<T>
{
    static public T Instance { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(Instance == (T)this)
        {
            OnStart();
        }
    }
    virtual protected void OnAwake()
    {

    }
    virtual protected void OnStart()
    {

    }
}

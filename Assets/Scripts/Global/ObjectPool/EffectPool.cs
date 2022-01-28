using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : Singleton<EffectPool>
{
    protected override void OnAwake()
    {
        GameObject[] effects = ObjectPool<Effect>.GetAllObjects("Prefabs/Units/Effects");
        string[] keys = new string[effects.Length];
        for (int i = 0; i < effects.Length; i++)
            keys[i] = effects[i].transform.name;

        ObjectPool<Effect>.ObjectPoolEventHandler += (e) =>
        {
            string message = $"** EffectPool({e.ListCount}) **\n";
            foreach (KeyValuePair<string, List<Effect>> pool in e.Pool)
            {
                message += $"Key : {pool.Key} / Value : {pool.Value.Count}";
            }
            Debug.Log(message);
        };
        Dictionary<string, List<Effect>> effectpool = ObjectPool<Effect>.GetGameObjectPool(keys, effects, gameObject);
    }
}

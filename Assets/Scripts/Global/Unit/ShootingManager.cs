using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingManager : Singleton<ShootingManager>
{
    public Bullet GetBullet(string _key , Vector2 _position)
    {
        Bullet newbullet = ObjectPool<Bullet>.GetObject(_key, gameObject);
        if(newbullet != null)
        {
            newbullet.gameObject.SetActive(true);
            newbullet.transform.position = _position;

            return newbullet;
        }
        return null;
    }
    protected override void OnAwake()
    {
        // object pool Setting
        GameObject[] bullets = ObjectPool<Bullet>.GetAllObjects("Prefabs/Units/Bullets");
        string[] keys = new string[bullets.Length];
        for (int i = 0; i < keys.Length; i++)
            keys[i] = bullets[i].transform.name;

        Dictionary<string, List<Bullet>> bulletobjectpool = ObjectPool<Bullet>.GetGameObjectPool(keys, bullets, gameObject);
        Debug.Log(bulletobjectpool);
    }
}

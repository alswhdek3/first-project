using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage { get; private set; }

    public string Key { get; private set; }

    public GameObject Target { get; private set; }

    public void SetBullet(int _damage , string _key , GameObject _target = null)
    {
        Damage = _damage;
        Key = _key;

        if(_target != null)
            Target = _target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Monster"))
        {
            collision.gameObject.GetComponent<Monster>().OnDamge(Damage);
            ObjectPool<Bullet>.RemoveObject(Key, gameObject);
        }
    }

    private void Update()
    {
        if(Target != null)
        {
            Vector2 distance = Target.transform.position - transform.position;
            float angle = Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}

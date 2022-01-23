using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject dmgTextPrefab;
    private GameObjectPool<DamageText> damagetextpool;

    public DamageText GetCreateDamageText()
    {
        var damagetext = damagetextpool.Get();
        return damagetext;
    }

    protected override void OnAwake()
    {
        damagetextpool = new GameObjectPool<DamageText>(5, () =>
        {
            var obj = Instantiate(dmgTextPrefab);
            obj.transform.SetParent(transform);
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;

            var dmgtext = obj.GetComponent<DamageText>();
            dmgtext.gameObject.SetActive(false);

            return dmgtext;
        });
    }
}

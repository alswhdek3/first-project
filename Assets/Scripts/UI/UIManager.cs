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

    public void RemoveDamageText(DamageText _text)
    {
        _text.transform.SetParent(transform);
        _text.transform.localPosition = Vector3.zero;

        _text.gameObject.SetActive(false);
        damagetextpool.Set(_text);
    }

    protected override void OnAwake()
    {
        damagetextpool = new GameObjectPool<DamageText>(5, () =>
        {
            var obj = Instantiate(dmgTextPrefab);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;

            var dmgtext = obj.GetComponent<DamageText>();
            dmgtext.gameObject.SetActive(false);

            return dmgtext;
        });
    }
}

using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private IEnumerator Coroutine_AnimationDamageText(Transform _damageTextDistance)
    {
        float currenttime = 0f;
        float percent = 0f;

        float velocity = (_damageTextDistance.transform.position - transform.position).y - 9.81f;
        
        while(percent < 1f)
        {
            currenttime += Time.deltaTime;
            percent = Vector2.Distance(transform.position,_damageTextDistance.transform.position) * 2f;

            Vector2 position = Vector2.Lerp(transform.position, _damageTextDistance.transform.position, percent);
            position.y = transform.position.y + (velocity * percent) + (-9.81f * percent * percent);

            yield return null;
        }
        Destroy(gameObject);
    }

    public void SetText(int damage , Transform _damageTextDistance)
    {
        gameObject.GetComponent<Text>().text = damage.ToString();
        StartCoroutine(Coroutine_AnimationDamageText(_damageTextDistance));
    }
}

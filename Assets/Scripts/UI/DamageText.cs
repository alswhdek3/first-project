using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private IEnumerator Coroutine_AnimationDamageText(Transform _damageTextDistance , Vector2 _distance , float _colliderminy)
    {
        float time = 3f;
        float currenttime = 0f;
        float percent = 0f;
        float velocity = (_damageTextDistance.transform.position - transform.position).y - 9.81f;
        
        while(transform.localPosition.y > _colliderminy || percent > 1f)
        {
            currenttime += Time.deltaTime;
            percent = currenttime / (time);

            Vector2 position = Vector2.Lerp(transform.localPosition , _distance , percent);
            position.y = transform.localPosition.y + (velocity * percent) + (-9.81f * percent * percent);

            transform.localPosition = position;
            yield return null;
        }
        UIManager.Instance.RemoveDamageText(this);
    }

    public void SetText(int damage , Transform _damageTextDistance , Vector2 _distance , float _colliderminy)
    {
        gameObject.GetComponent<Text>().text = damage.ToString();
        StartCoroutine(Coroutine_AnimationDamageText(_damageTextDistance , _distance , _colliderminy));
    }
}

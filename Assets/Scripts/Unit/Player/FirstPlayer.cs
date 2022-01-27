using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayer : BasePlayer
{
    protected override IEnumerator Coroutine_Attack()
    {
        float length = GetTargetAnimationClipLength("Attack");

        int bulletcount = 20;
        float defaultangle = 360f / bulletcount;
        float plusangle = 0f;

        while(validtime > 0f)
        {
            yield return new WaitForSeconds(length);
            for(int i=0; i<bulletcount; i++)
            {
                float angle = plusangle + (i * defaultangle);
                Vector2 direction = new Vector2(Mathf.Cos(angle * (Mathf.PI / 180f)), Mathf.Sin(angle * (Mathf.PI / 180f)));
                
                // �Ѿ� ����
            }
            plusangle += 1;

            yield return null;
        }
    }

    public override void AnimEvent_Attack()
    {
        // ���� �ֱ⸶�� Scale X�� -1,1 ���÷� ����
        float value = transform.localScale.x.Equals(1) ? -1 : 1;
        transform.localScale = new Vector3(value, 1f, 1f);
    }
    void Start()
    {
        CommonComponet();
    }
}

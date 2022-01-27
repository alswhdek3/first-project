using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayer : BasePlayer
{
    [SerializeField]
    private int bulletcount = 20;
    protected override IEnumerator Coroutine_Attack()
    {
        float length = GetTargetAnimationClipLength("Attack");

        float defaultangle = 360f / bulletcount;
        float plusangle = 0f;

        while(true)
        {
            yield return new WaitForSeconds(length);
            for(int i=0; i<bulletcount; i++)
            {
                float angle = plusangle + (i * defaultangle);
                Vector2 direction = new Vector2(Mathf.Cos(angle * (Mathf.PI / 180f)), Mathf.Sin(angle * (Mathf.PI / 180f)));

                // �Ѿ� ����
                string key = "Bullet_1";
                Bullet newbullet = ShootingManager.Instance.GetBullet("Bullet_1", transform.position);
                newbullet.SetBullet(1, key);
                newbullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * 10f;
            }
            plusangle += 1;

            yield return new WaitForSeconds(validtime);
        }
    }

    public override void AnimEvent_Attack()
    {
        // ���� �ֱ⸶�� Scale X�� -1,1 ���÷� ����
        float value = transform.localScale.x.Equals(1) ? -1 : 1;
        transform.localScale = new Vector3(value, 1f, 1f);
    }
    protected override void Start()
    {
        CommonComponet();
        validtime = animtor.runtimeAnimatorController.animationClips[0].length;
        StartCoroutine(Coroutine_Attack());
    }
}

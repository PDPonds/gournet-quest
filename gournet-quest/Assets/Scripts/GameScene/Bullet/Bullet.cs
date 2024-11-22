using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    Vector3 dir;

    public void Setup(Vector3 dir, float speed, float time)
    {
        this.dir = dir;
        this.speed = speed;
        Destroy(gameObject, time);
    }

    private void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Monster>(out Monster monster))
        {
            monster.TakeDamage(1);
            monster.SwitchBehavior(MonsterBehavior.Chase);
        }
    }

}

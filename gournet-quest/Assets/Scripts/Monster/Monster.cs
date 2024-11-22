using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterBehavior
{
    Idle, Escape, Chase, ChageAttack, Death
}

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] MonsterType monsterType;
    Collider collider;
    Rigidbody rb;
    NavMeshAgent agent;

    MonsterBehavior curBehavior;

    [Header("- Mesh")]
    [SerializeField] GameObject mesh;

    float curAttackDelay;
    float curChageTime;

    public int curHP { get; set; }
    public int maxHP { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        SetupMonster();
    }

    private void Update()
    {
        DecreaseAttackTime();
        UpdateBehavior();
    }

    void SetupMonster()
    {
        maxHP = monsterType.monster_HP;
        ResetHP();
        SwitchBehavior(MonsterBehavior.Idle);
    }

    public void ResetHP()
    {
        curHP = maxHP;
    }

    public void Heal(int amount)
    {
        curHP += amount;
        if (curHP >= maxHP)
        {
            curHP = maxHP;
        }
    }

    public void TakeDamage(int damage)
    {
        curHP -= damage;

        Debug.Log(curHP);

        if (curHP <= 0)
        {
            Death();
        }
    }

    public void Death()
    {
        List<InventorySlot> dropSlot = monsterType.GetDropItem();
        if (dropSlot.Count > 0)
        {
            for (int i = 0; i < dropSlot.Count; i++)
            {
                InventorySlot slot = dropSlot[i];
                ItemSO item = slot.Item;
                int count = slot.count;
                PlayerManager.Instance.player_Inventory.AddItem(item, count);
            }
        }
        Destroy(gameObject);
    }

    public void SwitchBehavior(MonsterBehavior behavior)
    {
        curBehavior = behavior;
        switch (curBehavior)
        {
            case MonsterBehavior.Idle:
                break;
            case MonsterBehavior.Escape:
                break;
            case MonsterBehavior.Chase:
                break;
            case MonsterBehavior.ChageAttack:
                break;
            case MonsterBehavior.Death:
                break;
        }
    }

    void UpdateBehavior()
    {
        switch (curBehavior)
        {
            case MonsterBehavior.Idle:
                agent.velocity = Vector3.zero;
                break;
            case MonsterBehavior.Escape:
                break;
            case MonsterBehavior.Chase:

                float dist = Vector3.Distance(transform.position, PlayerManager.Instance.transform.position);
                if (dist > monsterType.monster_AttackRange)
                {
                    agent.SetDestination(PlayerManager.Instance.transform.position);
                }
                else
                {
                    agent.velocity = Vector3.zero;
                    if (curAttackDelay <= 0)
                    {
                        SwitchBehavior(MonsterBehavior.ChageAttack);
                    }
                }

                break;
            case MonsterBehavior.ChageAttack:

                curChageTime += Time.deltaTime;
                if (curChageTime >= monsterType.monster_changeTime)
                {
                    Debug.Log("Attack");
                    curChageTime = 0;
                    curAttackDelay = monsterType.monster_AttackDelay;
                    SwitchBehavior(MonsterBehavior.Chase);
                }

                break;
            case MonsterBehavior.Death:
                break;
        }
    }

    void DecreaseAttackTime()
    {
        if (curAttackDelay > 0)
        {
            curAttackDelay -= Time.deltaTime;
            if (curAttackDelay <= 0)
            {
                curAttackDelay = 0;
            }
        }
    }

    public bool IsBehavior(MonsterBehavior behavior)
    {
        return curBehavior == behavior;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, monsterType.monster_AttackRange);
    }

}

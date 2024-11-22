using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int curHP { get; set; }
    public int maxHP { get; set; }
    public void ResetHP();
    public void TakeDamage(int damage);
    public void Heal(int amount);
    public void Death();
}

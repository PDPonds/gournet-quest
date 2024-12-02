using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MixedType
{
    Salt, Pepper
}

[CreateAssetMenu(menuName = "Menu/Process/Mixed")]
public class Mixed_Process : ScriptableObject
{
    public MixedType[] mixedTypes;
    public GameObject mainIngredient;
}

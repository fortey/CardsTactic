using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Variables", menuName = "Cards/Variables", order = 0)]

public class Variables : ScriptableObject
{
    public CardSprite[] cardSprites;
}

[System.Serializable]
public struct CardSprite
{
    public string name;
    public Sprite sprite;
}

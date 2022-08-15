using UnityEngine;

[CreateAssetMenu(fileName = "Variables", menuName = "Cards/Variables", order = 0)]

public class Variables : ScriptableObject
{
    public CardSprite[] cardSprites;

    public Material allyCardMaterial;
    public Material enemyCardMaterial;
}

[System.Serializable]
public struct CardSprite
{
    public string name;
    public Sprite sprite;
    public AnimatorOverrideController animator;
}

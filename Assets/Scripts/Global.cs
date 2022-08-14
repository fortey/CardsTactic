using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Global : Singleton<Global>
{
    [SerializeField] private Variables Variables;

    public Dictionary<string, CardSprite> CardSprites;

    public Material AllyCardMaterial { get => Variables.allyCardMaterial; }
    public Material EnemyCardMaterial { get => Variables.enemyCardMaterial; }

    public Pool PopupTextPool;
    public Pool ArrowPool;
    public Squads Squads;
    public UnityEngine.UI.GraphicRaycaster GraphicRaycaster;

    public override void Awake()
    {
        base.Awake();

        CardSprites = Variables.cardSprites.ToDictionary(cs => cs.name, cs => cs);
    }
}

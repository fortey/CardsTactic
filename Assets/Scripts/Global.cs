using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Global : Singleton<Global>
{
    [SerializeField] private Variables Variables;

    public Dictionary<string, Sprite> CardSprites;

    public override void Awake()
    {
        base.Awake();

        CardSprites = Variables.cardSprites.ToDictionary(cs => cs.name, cs => cs.sprite);
    }
}

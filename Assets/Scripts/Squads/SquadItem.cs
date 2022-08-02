using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SquadItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public Squad Squard { get; private set; }

    public void Initialize(Squad squad)
    {
        this.Squard = squad;
        _text.text = squad.name;
    }
}

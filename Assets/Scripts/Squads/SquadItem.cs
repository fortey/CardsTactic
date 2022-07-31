using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SquadItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private Squad _squard;

    public void Initialize(Squad squad)
    {
        this._squard = squad;
        _text.text = squad.name;
    }
}

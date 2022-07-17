using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    public Action OnJoin;
    [SerializeField] private TextMeshProUGUI _nameLabel;
    private string _roomID;

    public void Setup(string name, Action onJoin)
    {
        _nameLabel.text = name;
        OnJoin = onJoin;
    }

    public void Join()
    {
        OnJoin?.Invoke();
    }
}

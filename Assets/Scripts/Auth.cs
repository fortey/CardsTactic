using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Auth : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInput;

    public void Login()
    {
        print(_nameInput.text);
    }
}

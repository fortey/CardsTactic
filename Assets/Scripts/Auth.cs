using TMPro;
using UnityEngine;

public class Auth : MonoBehaviour
{
    [SerializeField] private TMP_InputField _nameInput;

    public void Login()
    {
        if (_nameInput.text == string.Empty) return;
        MyColyseusManager.Instance.ConnectToMain(_nameInput.text);

        gameObject.SetActive(false);
    }
}

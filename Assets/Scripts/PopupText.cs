using System.Collections;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _time;
    [SerializeField] private float _speed;

    public void Play(Vector2 position, Color color, string text)
    {
        transform.position = position;
        _text.color = color;
        _text.text = text;

        StartCoroutine(Popup());
    }

    private IEnumerator Popup()
    {
        var position = transform.position;
        var time = _time;

        while (time > 0)
        {
            time -= Time.deltaTime;
            position.y += _speed * Time.deltaTime;
            transform.position = position;
            yield return null;
        }
        Global.Instance.PopupTextPool.Push(gameObject);
    }
}

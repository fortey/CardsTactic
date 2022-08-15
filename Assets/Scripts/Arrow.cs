using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private RectTransform _transform;
    [SerializeField] private float _startWidth;

    public void Show(Vector2 start, Vector2 end)
    {
        _transform.position = start;
        _transform.sizeDelta = new Vector2(0f, _transform.sizeDelta.y);
        var direction = (end - start).normalized;
        var distance = Vector2.Distance(start, end);

        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        _transform.eulerAngles = new Vector3(0, 0, n);

        StartCoroutine(Moving(distance));

        Invoke(nameof(ReturnToPool), 0.5f);
    }

    private IEnumerator Moving(float width)
    {
        var size = _transform.sizeDelta;
        var time = 0.3f;
        var timer = 0f;

        while (timer < time)
        {
            yield return null;
            timer += Time.deltaTime;
            size.x = Mathf.Lerp(_startWidth, width, timer / time);
            _transform.sizeDelta = size;
        }
    }

    private void ReturnToPool()
    {
        Global.Instance.ArrowPool.Push(gameObject);
    }
}

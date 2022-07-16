using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private RectTransform _transform;

    public void Show(Vector2 start, Vector2 end)
    {
        _transform.position = start;
        var direction = (end - start).normalized;
        var distance = Vector2.Distance(start, end);

        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        _transform.eulerAngles = new Vector3(0, 0, n);
        _transform.sizeDelta = new Vector2(distance, _transform.sizeDelta.y);

        Invoke(nameof(ReturnToPool), 0.5f);
    }

    private void ReturnToPool()
    {
        Global.Instance.ArrowPool.Push(gameObject);
    }
}

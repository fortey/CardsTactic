using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private GameRoomController _gameRoomController;
    [SerializeField] private Image _valueImage;
    const float MAX_TIME = 10f;

    private void Start()
    {
        _gameRoomController.OnTurnChanged += RestartTimer;
    }
    public void RestartTimer(bool isEnemyTurn)
    {
        if (isEnemyTurn)
            _valueImage.color = Color.red;
        else
            _valueImage.color = Color.white;

        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }

    private IEnumerator StartTimer()
    {
        var time = MAX_TIME;
        while (time >= 0)
        {
            time -= Time.deltaTime;
            _valueImage.fillAmount = time / MAX_TIME;
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squads : MonoBehaviour
{
    [SerializeField] private Pool _squadItems;

    private void OnEnable()
    {

        MyColyseusManager.Instance.GetSquads();
    }
}

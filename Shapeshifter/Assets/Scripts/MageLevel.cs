using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MageLevel : MonoBehaviour
{
    public Player player;

    private void Start()
    {
        player.classIndex = 2;
    }

    private void FixedUpdate()
    {
        player.isAllowedToChange = false;
    }
}

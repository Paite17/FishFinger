using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;

    private Gun gun;


    private void Start()
    {
        gun = FindObjectOfType<Gun>();
        Debug.Log(gun.name);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            shootInput?.Invoke();
        }
        else
        {
            gun.shooting = false;
        }
    }
}

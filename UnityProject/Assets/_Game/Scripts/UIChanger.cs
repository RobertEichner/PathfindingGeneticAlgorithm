using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChanger : MonoBehaviour
{
    [SerializeField] private GameObject canvTile = null;

    [SerializeField] private GameObject canvSim = null;

    private void Awake()
    {
        ActivateTile();
    }


    public void ActivateSim()
    {
        canvTile.SetActive(false);
        canvSim.SetActive(true); 
    }

    public void ActivateTile()
    {
        canvTile.SetActive(true);
        canvSim.SetActive(false);
    }

    public void ChangeTime(float time)
    {
        Time.timeScale = time;
    }
}

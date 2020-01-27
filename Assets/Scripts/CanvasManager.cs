using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    public GameObject gamoOver;
    public GameObject timeOut;

    void Start()
    {
        gamoOver.SetActive(false);
        timeOut.SetActive(false);
    }
}

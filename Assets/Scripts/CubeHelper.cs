using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHelper : MonoBehaviour
{
    // Тут нарушены все пародигмы ООП
    public GameObject mainObj;
    public PowerUP pw;
    
    private void Start()
    {
        pw = mainObj.GetComponent<PowerUP>();
    }
}

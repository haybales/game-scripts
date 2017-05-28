using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class TimeController : MonoBehaviour {

    Tenkoku.Core.TenkokuModule tenkokuModule;
    SerialPort sp = new SerialPort("COM3", 9600);

    void Start()
    {
        tenkokuModule = GameObject.Find("Tenkoku DynamicSky").gameObject.GetComponent<Tenkoku.Core.TenkokuModule>();
        sp.Open();
    }

    void Update ()
    {
        float val = (float.Parse(sp.ReadLine())/100f)*90f;
        tenkokuModule.setLatitude = val;


	}
}

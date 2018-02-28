using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitTool : MonoBehaviour {

    public static Action<IEnumerator> ToolStartCoroutine;
    public static void AddToolStartCoroutine(Action<IEnumerator> method)
    {
        ToolStartCoroutine = method;
    }

    public static Action ToolStopAllCoroutines;
    public static void AddToolStopAllCoroutines(Action method)
    {
        ToolStopAllCoroutines = method;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

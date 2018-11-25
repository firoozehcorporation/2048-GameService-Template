using System.Collections;
using System.Collections.Generic;
using Sourav.Utilities.Scripts.Attributes;
using UnityEngine;

public class ReadonlyTest : MonoBehaviour 
{
    [ReadOnly]
    public string field = "This should be readonly";

	// Use this for initialization
	void Start () 
    {
        field = "This should be readonly";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

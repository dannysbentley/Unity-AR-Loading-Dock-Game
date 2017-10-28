using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BrakeButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler  {

	public bool braking;

	// Use this for initialization
	void Start () {
		GetComponent<Button> ();

	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		braking = false;
		Debug.Log ("button up " + braking);
	}

	public virtual void OnPointerDown(PointerEventData ped)
	{
		braking = true;
		Debug.Log ("button down " + braking);
	}
}

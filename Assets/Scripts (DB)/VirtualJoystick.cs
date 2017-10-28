using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {
	public delegate void JoystickAction(Vector2 joystickAxes);
	public static event JoystickAction JoystickMoved;

	private Image bgImg;
	private Image joystickImg;
	//private Vector3 inputVector;

	private bool usingJoystick;
	public Vector2 joystickAxes;
	private RectTransform bgTransform;
	private RectTransform stickTransform;


	private void Start()
	{
		bgImg = GetComponent<Image> ();
		joystickImg = transform.GetChild (0).GetComponent<Image> ();

		bgTransform = gameObject.GetComponent<RectTransform> ();
		stickTransform = transform.GetChild (0).GetComponent<RectTransform> ();
	}

	void Update() {
		if (usingJoystick && JoystickMoved != null) {
			JoystickMoved (joystickAxes);
		}
	}

	public virtual void OnDrag(PointerEventData ped)
	{
		Vector2 pos;

		if (RectTransformUtility.ScreenPointToLocalPointInRectangle (bgImg.rectTransform, 
			    ped.position, ped.pressEventCamera, out pos)) {
			Vector2 clampedPos = GetClampPosition (pos);

			//Normalize 
			joystickAxes = new Vector2 (
				clampedPos.x / (bgTransform.rect.width / 2f),
				clampedPos.y / (bgTransform.rect.height / 2f));

			//Set the position of the inner joystick
			if (joystickAxes.magnitude > 1f) {
				joystickImg.GetComponent<RectTransform> ().anchoredPosition = clampedPos.normalized * stickTransform.rect.width;
			} else {
				joystickImg.GetComponent<RectTransform> ().anchoredPosition = clampedPos;
			}
		}
	}

	private Vector2 GetClampPosition(Vector2 pos)
	{
		Vector2 bgMin = bgTransform.rect.min;
		Vector2 bgMax = bgTransform.rect.max;

		return new Vector2 (
			Mathf.Clamp (pos.x, bgMin.x, bgMax.x),
			Mathf.Clamp (pos.y, bgMin.x, bgMax.y));
	}

	public virtual void OnPointerDown(PointerEventData ped)
	{
		usingJoystick = true;
		OnDrag (ped);
	}

	public virtual void OnPointerUp(PointerEventData ped)
	{
		usingJoystick = false;
		joystickAxes = joystickImg.GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;

		if (JoystickMoved != null) JoystickMoved (Vector2.zero);
	}

	public float Horizontal()
	{
		if (joystickAxes.x != 0)
			return joystickAxes.x;
		else
			return Input.GetAxis ("Horizontal");
	}

	public float Vertical()
	{
		if (joystickAxes.y != 0)
			return joystickAxes.y;
		else
			return Input.GetAxis ("Vertical");
	}
}

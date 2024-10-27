using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TouchMapInput : MonoBehaviour
{
	bool isMouseOn = false;

	public List<UnityEvent> mouseMoveEvents;
	public List<UnityEvent> mouseExitEvents;
	public List<UnityEvent> mouseDownEvents;
	public List<UnityEvent> mouseUpEvents;

	public void OnMouseEnter(){
		isMouseOn = true;
	}

	public void OnMouseExit(){
		isMouseOn = false;
		foreach(var e in mouseExitEvents){
			e.Invoke();
		}
	}

	void Update(){
		if(isMouseOn){
			foreach(var e in mouseMoveEvents){
				e.Invoke();
			}
		}
	}

	public void OnMouseDown(){
		if(Input.GetMouseButtonDown(0)){
			foreach(var e in mouseDownEvents){
				e.Invoke();
			}
		}
	}

	public void OnMouseUp(){
		if(Input.GetMouseButtonUp(0)){
			foreach(var e in mouseUpEvents){
				e.Invoke();
			}
		}
	}
}

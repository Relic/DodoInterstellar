using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CreditsLink : MonoBehaviour, IPointerClickHandler
{
	public string Link;

	public void OnPointerClick(PointerEventData eventData)
	{
		Application.OpenURL ( Link );
	}
}

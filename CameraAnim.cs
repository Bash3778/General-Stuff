using UnityEngine;
using System.Collections;
public class CameraAnim : MonoBehaviour
{
	[SerializeField] Animator animator;
	[SerializeField] float wait;
	[SerializeField] GameObject cam;
	//Reset() defines the default values for properties in the inspector
	void Reset ()
	{
		//Grab the local animator component
		animator = GetComponent<Animator> ();
	}

	void Start()
	{
		StartCoroutine(Example());
	}
	IEnumerator Example()
	{
			yield return new WaitForSecondsRealtime (wait);
			animator.enabled = false;
			cam.transform.eulerAngles = new Vector3 (60, 0, 0);
	}
}

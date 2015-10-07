using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {
	public void LoadScene(int index){
		Application.LoadLevel (index);
	}

	public void Quit(){
		Application.Quit ();
	}
}

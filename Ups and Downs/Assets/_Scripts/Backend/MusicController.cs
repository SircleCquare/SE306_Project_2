using UnityEngine;
using System.Collections;

//This code is added to the MusicController in the Start Scene so that the music player persists across all level changes.
//The code was provided by Superrodan on answers.unity3d.com.
public class MusicController : MonoBehaviour {

	public GameObject musicPlayer;

	void Awake() {
		//When the scene loads it checks if there is an object called "Music".
		musicPlayer = GameObject.Find("Music");
		if(musicPlayer==null)
		{
			//If this object does not exist then it does the following:
			//1. Sets the object this script is attached to as the music player
			musicPlayer = this.gameObject;
			//2. Renames THIS object to "Music" for next time
			musicPlayer.name = "Music";
			//3. Tells THIS object not to die when changing scenes.

			DontDestroyOnLoad(musicPlayer);
		}else{
			if(this.gameObject.name!="Music"){
				//If there WAS an object in the scene called "Music" (because we have come back to
				//the scene where the music was started) then it just tells this object to 
				//destroy itself if this is not the original
				Destroy(this.gameObject);

			}
		}
	}
}

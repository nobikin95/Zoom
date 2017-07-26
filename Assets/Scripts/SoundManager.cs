using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {
	public bool isMusicOn{
		get{ 
			if (PlayerPrefs.GetInt ("IsMusicOn") == 0) {
				return true;
			} else
				return false;
		}
		set { 
			if (value) {
				PlayerPrefs.SetInt ("IsMusicOn",0);
			}
			else PlayerPrefs.SetInt ("IsMusicOn",1);
		}
	}
	public GameObject soundOnIcon;
	public GameObject soundOffIcon;

	[Header("-----Sound Files-----")]
	[SerializeField] AudioClip buttonSound;

	[SerializeField] AudioSource soundEffect;
	[SerializeField] AudioSource titleMusic;
	[SerializeField] AudioSource gameMusic;

	static SoundManager _instance;
	public static SoundManager instance {
		get {
			return _instance;
		}
	}

	void Awake ()
	{
		if (_instance != null) {
			Destroy (this.gameObject);
		} else {
			_instance = this;
		}
		DontDestroyOnLoad(this.gameObject) ;
	}

	public void Play (AudioClip clip, float volume = 1f)
	{
		if (isMusicOn) {
			var go = new GameObject ("Sound", typeof(AudioSource));
			go.GetComponent<AudioSource> ().PlayOneShot (clip, volume);
			UnityEngine.Object.Destroy (go, clip.length);
		}
	}
	public void PlayButtonSound (float volume = 1f)
	{
		if (isMusicOn) {
			var go = new GameObject ("Button Sound", typeof(AudioSource));
			go.GetComponent<AudioSource> ().PlayOneShot (buttonSound, volume);
			UnityEngine.Object.Destroy (go, buttonSound.length);
		}
	}

	public void PlaySoundEffect(AudioClip sound){
		soundEffect.clip = sound;
		if (!soundEffect.isPlaying) {			
			soundEffect.Play ();
		}
	}

	public void PlayTitleMusic(){
		if (!titleMusic.isPlaying) {
			if (gameMusic.isPlaying)
				gameMusic.Stop ();
			titleMusic.Play ();
		}
	}
	public void PlayGameMusic(){
		if (!gameMusic.isPlaying){
			if (titleMusic.isPlaying)
				titleMusic.Stop ();
			}
		gameMusic.Play ();
	}

	public void Mute(){
		if (isMusicOn) {
			isMusicOn = false;
			if(titleMusic.isPlaying)
			titleMusic.Stop ();
			soundOnIcon.SetActive (false);
			soundOffIcon.SetActive (true);
		} else {
			isMusicOn = true;
			if(!titleMusic.isPlaying)
			titleMusic.Play();
			soundOnIcon.SetActive (true);
			soundOffIcon.SetActive (false);
		}
	}

}

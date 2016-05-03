using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour 
{
	static AudioSource audioSource;
	static string audioPath = "Audio/";
	bool menuClip = false;

	static AudioClip menuMusic, gameMusic;

	void Awake () 
	{
		//gameObject.AddComponent<AudioListener>();
		audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.loop = true;
	}

	void Start()
	{
		menuMusic = Resources.Load<AudioClip>(audioPath + "2");
		gameMusic = Resources.Load<AudioClip>(audioPath + "1");
	}
	

	public IEnumerator PlayForTest(bool mMusic)
	{
		if(mMusic)
		{
//			ResourceRequest res = Resources.LoadAsync<AudioClip>(audioPath + "2");
//
//			while(!res.isDone)
//				yield return null;
//
//			audioSource.clip = res.asset as AudioClip;//Resources.LoadAsync<AudioClip>(audioPath + "2");
			audioSource.clip = menuMusic;
			audioSource.Play();
			menuClip = true;
		}
		else
		{
			AudioClip clip = new AudioClip();

			if(menuClip)
			{
//				ResourceRequest res = Resources.LoadAsync<AudioClip>(audioPath + "1");
//				
//				while(!res.isDone)
//					yield return null;
//
//
//				clip = res.asset as AudioClip;//Resources.LoadAsync<AudioClip>(audioPath + "1");
				clip = gameMusic;
			}

			float i = 0.0f;
			float step = 1.0f/1.5f; 
			float start = 1f;
			float end = menuClip ? 0f : 0.2f;

			while (i <= 1.0) {                          // до тех пор, ПОКА "0" (громкость) равна или меньше "1" исполнять ↓ , 
				//вплоть до получения значения "0"
				i += step * Time.deltaTime;
				//Debug.LogWarning(i);
				audioSource.volume = Mathf.Lerp(start, end, i); 
				// Mathf.Lerp - находим промежуточные значения громкости соответственно имеющимся start, end, и значение "i"
				//растягиваем во времени изменение звука
				yield return null;
				//yield return new WaitForSeconds(0.001f);
			}
			//yield return new WaitForSeconds(0.3f);
			if(menuClip)
			{
				audioSource.clip = clip;
				audioSource.Play();
			}

			i = 0f;
			start = menuClip ? 0f : 0.2f;
			end = 2f;

			while (i <= 1.0) {                          // до тех пор, ПОКА "0" (громкость) равна или меньше "1" исполнять ↓ , 
				//вплоть до получения значения "0"
				i += (2f*step) * Time.deltaTime;
				audioSource.volume = Mathf.Lerp(start, end, i); 
				// Mathf.Lerp - находим промежуточные значения громкости соответственно имеющимся start, end, и значение "i"
				//растягиваем во времени изменение звука
				yield return null;
			}

			menuClip = false;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

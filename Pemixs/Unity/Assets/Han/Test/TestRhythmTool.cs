using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRhythmTool : MonoBehaviour {

	public List<AudioClip> clips;
	public List<RhythmTool> tools;
	public RhythmEventProvider eventProvider;


	[Range(0, 50)]
	public float timer;
	[SerializeField]
	bool isFever = false;

	// Use this for initialization
	void Start () {
		//subscribe to events.
		eventProvider.onSongLoaded.AddListener(OnSongLoaded);
		eventProvider.onBeat.AddListener(OnBeat);
		eventProvider.onSubBeat.AddListener(OnSubBeat);
		eventProvider.timingUpdate.AddListener (OnTimeingUpdate);

		tools[0].NewSong(clips[0]);
		tools[1].NewSong(clips[1]);

	}

	void Update(){
		timer = tools [0].TimeSeconds ();
	}

	void OnGUI(){
		GUILayout.BeginArea (new Rect (0, 0, 500, 500));
		if (GUILayout.Button ("fever")) {
			OnFever ();
		}
		if (GUILayout.Button ("pause")) {
			OnPause ();
		}
		if (GUILayout.Button ("resume")) {
			OnResume ();
		}
		GUILayout.EndArea ();
	}

	void OnFever(){
		tools [0].volume = 0;
		tools [1].Play ();
		isFever = true;
	}

	void OnPause(){
		if (tools [0].isPlaying) {
			tools [0].Pause ();
		}
		if (tools [1].isPlaying) {
			tools [1].Pause ();
		}
	}

	void OnResume(){
		if (tools [0].isPlaying==false) {
			tools [0].Play ();
		}
		if (isFever) {
			if (tools [1].isPlaying == false) {
				tools [1].Play ();
			}
		}
	}

	private void OnSongLoaded(string name, int totalFrames){
		if (name == "MUL01A0A") {
			tools [0].Play ();
		} else {
			//tools [1].Play ();
		}
	}

	private void OnBeat(Beat beat){
		//Debug.Log ("OnBeat:"+beat.index);
	}

	private void OnSubBeat(Beat beat, int count){
		//Debug.Log ("OnSubBeat:"+beat.index+"/"+count);
	}

	void OnTimeingUpdate(int index, float interpolation, float beatLength, float beatTime){
		//Debug.Log ("OnTimeingUpdate:"+index+"/"+interpolation+"/"+beatLength+"/"+beatTime);

		//var time = clips [0].length * (index / (double)tools [0].totalFrames);

	}
}

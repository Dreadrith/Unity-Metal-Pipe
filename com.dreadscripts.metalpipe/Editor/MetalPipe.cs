using UnityEditor;
using UnityEngine;

namespace DreadScripts.MetalPipe
{
	public class MetalPipe : EditorWindow, IHasCustomMenu
	{
		private const string PREF_KEY = "MetalPipeSettings";
		private const string TEXTURE_PATH = "MP_Protagonist";
		private const string AUDIO_PATH = "MP_Sooth (LOUD)";

		private Texture2D _pipeTexture;

		private Texture2D pipeTexture
		{
			get
			{
				if (_pipeTexture == null) _pipeTexture = Resources.Load<Texture2D>(TEXTURE_PATH);
				return _pipeTexture;
			}
		}
		private AudioClip pipeAudio;
		private bool displaySettings;
		private bool hasModifiedSettings;
		private double nextPlayTime;
		private Vector2 scroll;

		public float volume = 0.25f;
		public bool autoPlay;
		public float minAutoTime = 20;
		public float maxAutoTime = 420;

		#region Properties
		private static GameObject _pipePlayerContainer;

		private static GameObject pipePlayerContainer
		{
			get
			{
				_pipePlayerContainer = new GameObject("Pipe");
				_pipePlayerContainer.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
				var source = _pipePlayerContainer.AddComponent<AudioSource>();
				source.hideFlags = HideFlags.DontSave;
				source.spatialBlend = 0;
				source.playOnAwake = false;
				source.priority = 0;
				return _pipePlayerContainer;
			}
		}
		private static AudioSource pipeAudioSource => pipePlayerContainer.GetComponent<AudioSource>();

		#endregion


		[MenuItem("DreadTools/Utility/Metal Pipe")]
		public static void ShowWindow()
		{
			var inst = GetWindow<MetalPipe>("Metal Pipe");
			inst.titleContent.image = inst.pipeTexture;
		}

		public void OnGUI()
		{
			if (!displaySettings)
			{
				var ratio = pipeTexture.width / (float)pipeTexture.height;
				var rect = GUILayoutUtility.GetAspectRect(ratio);

				GUI.DrawTexture(rect, pipeTexture);

				var e = Event.current;
				switch (e.type)
				{
					case EventType.Repaint:
						EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
						break;
					case EventType.MouseDown when e.button == 0 && rect.Contains(e.mousePosition):
						PlayClip();
						break;
				}

			}
			else
			{
				scroll = EditorGUILayout.BeginScrollView(scroll);
				EditorGUI.BeginChangeCheck();
				volume = EditorGUILayout.Slider("Volume", volume, 0, 1);

				bool wasAutoPlaying = autoPlay;
				autoPlay = EditorGUILayout.Toggle(new GUIContent("Auto Play", ""), autoPlay);
				if (wasAutoPlaying != autoPlay)
				{
					EditorApplication.update -= CheckForAutoPlay;
					if (autoPlay)
					{
						RandomizeTimer();
						EditorApplication.update += CheckForAutoPlay;
					}
				}

				using (new EditorGUI.DisabledScope(!autoPlay))
				{
					EditorGUI.indentLevel++;
					minAutoTime = Mathf.Max(0, EditorGUILayout.FloatField("Min Auto Time", minAutoTime));
					maxAutoTime = Mathf.Max(0, EditorGUILayout.FloatField("Max Auto Time", maxAutoTime));
					EditorGUI.indentLevel--;
				}

				hasModifiedSettings |= EditorGUI.EndChangeCheck();

				using (new GUILayout.HorizontalScope())
				{
					if (GUILayout.Button("Back", GUILayout.ExpandWidth(false)))
						displaySettings = false;

					using (new EditorGUI.DisabledScope(!hasModifiedSettings))
					{
						if (GUILayout.Button("Revert Changes"))
							LoadSettings();

						if (GUILayout.Button("Save Settings"))
							SaveSettings();
					}
				}

				EditorGUILayout.EndScrollView();
			}
		}

		public void PlayClip(bool isTest = false)
		{
			pipeAudioSource.PlayOneShot(pipeAudio, volume);
			if (!isTest) RandomizeTimer();
		}


		public void OnEnable()
		{
			pipeAudio = Resources.Load<AudioClip>(AUDIO_PATH);
			LoadSettings();

			if (autoPlay)
			{
				RandomizeTimer();
				EditorApplication.update -= CheckForAutoPlay;
				EditorApplication.update += CheckForAutoPlay;
			}
		}

		public void OnDisable() => EditorApplication.update -= CheckForAutoPlay;


		public void CheckForAutoPlay()
		{
			if (Time.realtimeSinceStartup > nextPlayTime)
				PlayClip();
		}
		public void RandomizeTimer() => nextPlayTime = Time.realtimeSinceStartup + Random.Range(minAutoTime, maxAutoTime);
		public void AddItemsToMenu(GenericMenu menu) => menu.AddItem(new GUIContent("Settings"), displaySettings, () => displaySettings = !displaySettings);
		
		#region Saving
		public void SaveSettings()
		{
			EditorPrefs.SetString(PREF_KEY, JsonUtility.ToJson(this));
			hasModifiedSettings = false;
		}
		public void LoadSettings()
		{
			if (EditorPrefs.HasKey(PREF_KEY)) JsonUtility.FromJsonOverwrite(EditorPrefs.GetString(PREF_KEY), this);
			hasModifiedSettings = false;
		}
		#endregion
	}

}
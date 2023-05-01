using System;
using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	private static T instance;

	private static readonly object @lock = new();

	public static T Instance {
		get {
			if (applicationIsQuitting) {
				Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed on application quit. Won't create again - returning null.");
				return null;
				}

			lock (@lock) {
				if (instance == null) {
					var all = FindObjectsOfType<T>();
					instance = all != null && all.Length > 0 ? all[0] : null;

					if (all != null && all.Length > 1) {
						Debug.LogWarning("[Singleton] There are " + all.Length + " instances of " + typeof(T) + "... This may happen if your singleton is also a prefab, in which case there is nothing to worry about.");
						}

					if (instance == null) {
						GameObject singleton = new();
						instance = singleton.AddComponent<T>();
						singleton.name = "(singleton) " + typeof(T).ToString();

						Debug.Log("[Singleton] An instance of " + typeof(T) + " is needed in the scene, so '" + singleton + "' was created with DontDestroyOnLoad.");
						}

					if (Application.isPlaying)
						DontDestroyOnLoad(instance.gameObject);
					}

				return instance;
				}
			}
		}

	private static bool applicationIsQuitting = false;

	/// <summary>
	/// When Unity quits, it destroys objects in a random order.
	/// In principle, a Singleton is only destroyed when application quits.
	/// If any script calls Instance after it have been destroyed, 
	///   it will create a buggy ghost object that will stay on the Editor scene
	///   even after stopping playing the Application. Really bad!
	/// So, this was made to be sure we're not creating that buggy ghost object.
	/// </summary>
	public void OnDestroy() {
		applicationIsQuitting = true;
		}
	}

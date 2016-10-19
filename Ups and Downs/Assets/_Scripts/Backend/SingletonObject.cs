using UnityEngine;
using System.Collections;

/// <summary>
/// SingletonObject is a generic class that enables easy singleton use
/// for other classes, such as GameController.
/// </summary>
public class SingletonObject<T> : MonoBehaviour where T : SingletonObject<T> {
	private static T singleton;

	public static T Singleton {
		get {
			return singleton;
		}
	}

	protected virtual void Awake() {
		singleton = (T)this;
	}
}

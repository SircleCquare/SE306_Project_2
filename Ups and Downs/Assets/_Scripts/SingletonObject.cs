using UnityEngine;
using System.Collections;

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

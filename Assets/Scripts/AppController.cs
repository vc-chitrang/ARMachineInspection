using System;
using UnityEngine;

public class AppController:MonoBehaviour {
    public static AppController Instance { get; private set; } = null;

    private Machine _machine;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void SetMachie(Machine machine) { 
        this._machine = machine;
    }

    public Machine GetMachine() {
        return _machine;
    }
}//AppController class end.

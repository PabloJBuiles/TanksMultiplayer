using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using System;
using UnityEngine.SceneManagement;

public class LoadSceneAuthentication : MonoBehaviour {

    
    void Start() {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChange;
    }

    private void HandleAuthStateChange(object sender, EventArgs e) {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null) {
            SceneManager.LoadScene("_Complete-Game");
          
        }
    }

}


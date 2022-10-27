using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour {
    [SerializeField]
    private Button _loginButton;

    [SerializeField] InputField email;
    [SerializeField] InputField password;

    private Coroutine coroutine;

    public event Action<FirebaseUser> OnLoginSucceded;
    public event Action<string> OnLoginFailed;


    private void Reset() {
        _loginButton = GetComponent<Button>();
    }

    void Start() {
        _loginButton.onClick.AddListener(ClickLogin);
    }

    private void ClickLogin() {
        if (coroutine == null) {
            coroutine = StartCoroutine(LoginCoroutine(email.text, password.text));
        }
    }

    private IEnumerator LoginCoroutine(string email, string password) {
        var auth = FirebaseAuth.DefaultInstance;
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null) {
            Debug.LogWarning($"Login Failed with {loginTask.Exception}");
            OnLoginFailed?.Invoke($"Login Failed with {loginTask.Exception}");
        } else {
            Debug.Log($"Login succeeded with {loginTask.Result}");
            OnLoginSucceded?.Invoke(loginTask.Result);
            SceneManager.LoadScene("Game");
        }
    }


}

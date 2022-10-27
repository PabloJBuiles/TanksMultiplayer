using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class ChangePassword : MonoBehaviour {
    [SerializeField]
    private Button ChangeButton;

    [SerializeField]
    private InputField email;

    private Coroutine changePasswordCoroutine;

    public GameObject resetPanel;

    private void Reset() {
        ChangeButton = GetComponent<Button>();

    }

    void Start() {
        ChangeButton.onClick.AddListener(ClickChange);
    }

    private void ClickChange() {
        if (changePasswordCoroutine == null) {
            changePasswordCoroutine = StartCoroutine(ChangePasswordcoroutine(email.text));
        }
    }

    private IEnumerator ChangePasswordcoroutine(string email) {
        var auth = FirebaseAuth.DefaultInstance;
        var resetTask = auth.SendPasswordResetEmailAsync(email);

        yield return new WaitUntil(() => resetTask.IsCompleted);

        if (resetTask.Exception != null) {
            Debug.LogWarning($"Reset Failed with {resetTask.Exception}");
        } else {
            resetPanel.SetActive(true);
            Debug.Log("Password reset email sent successfully");
        }
    }
}
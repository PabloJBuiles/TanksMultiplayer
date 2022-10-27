using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using System;
using Firebase.Database;

public class SignUp : MonoBehaviour {
    [SerializeField]
    private Button registrationButton;
    private Coroutine _registrationCoroutine;

    [SerializeField] InputField email_;
    [SerializeField] InputField password_;

    public event Action<FirebaseUser> OnUserRegistered;
    public event Action<string> OnUserRegistrationFailed;
    private void Reset() {
        registrationButton = GetComponent<Button>();
    }

    void Start() {

        registrationButton.onClick.AddListener(ClickSignUp);
    }


    private void ClickSignUp() {
        string email = email_.text;
        string password = password_.text;
        _registrationCoroutine = StartCoroutine(RegisterUser(email, password));
    }



    private IEnumerator RegisterUser(string email, string password) {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null) {
            Debug.LogWarning($"Failed to register task {registerTask.Exception}");
            OnUserRegistrationFailed.Invoke($"Failed to register task {registerTask.Exception}");
        } else {
            Debug.Log($"Succesfully registered user {registerTask.Result.Email}");

            UserData data = new UserData();

            data.username = GameObject.Find("Username").GetComponent<InputField>().text;
            string json = JsonUtility.ToJson(data);

            FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(registerTask.Result.UserId).SetRawJsonValueAsync(json);

            print("Nombre usuario: "+ data.username);
            OnUserRegistered?.Invoke(registerTask.Result);
        }

        _registrationCoroutine = null;
    }


}

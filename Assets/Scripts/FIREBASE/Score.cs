using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;


public class Score : MonoBehaviour {

    DatabaseReference mDatabase;
    string UserID;
    public int playerScore;
    public int highestScore;
    public int scoreInt;
    public Text scoreDisplay;
    public Dictionary<string, object> userObject;
    [SerializeField] Text[] scores = new Text[5];

    

    private void Start() {
        UserID = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        mDatabase = FirebaseDatabase.DefaultInstance.RootReference;
       


    }
    public void AddScore() {

        playerScore += 1;
        
    }

    private void Update() {
        scoreDisplay.text = playerScore.ToString();
    }



    public void SetScore() {

        FirebaseDatabase.DefaultInstance.GetReference("users/" + UserID).GetValueAsync().ContinueWithOnMainThread(task => {
               if (task.IsFaulted) {
                   Debug.Log(task.Exception);
               } else if (task.IsCompleted) {
                   try {
                       DataSnapshot snapshot = task.Result;
                       var data = (Dictionary<string, object>)snapshot.Value;
                       highestScore = Convert.ToInt32(data["score"]);
                       var username = Convert.ToString(data["username"]);
                       UserData update = new UserData();
                       if (playerScore >= highestScore) {
                          
                           update.username = username;
                           update.score = playerScore;
                           string json = JsonUtility.ToJson(update);

                           mDatabase.Child("users").Child(UserID).SetRawJsonValueAsync(json);
                           highestScore = playerScore;
                           print("New score: " + highestScore);
                       }
                   } catch (Exception e) {
                       Debug.Log(e);
                   }

               }
           });
    }
    public void ScoretoInt() {
        scoreInt = Convert.ToInt32(userObject["score"]);

    }

    public void GetUserScore() { FirebaseDatabase.DefaultInstance.GetReference("users/" + UserID).GetValueAsync().ContinueWith(task => {
        if (task.IsFaulted) {
            Debug.Log(task.Exception);
        } else if (task.IsCompleted) {
            DataSnapshot snapshot = task.Result;
            Debug.Log(snapshot.Value);

            var data = (Dictionary<string, object>)snapshot.Value;
            highestScore = (int)data["score"];

            }
        });
    }


    public void GetScores() {
        FirebaseDatabase.DefaultInstance.GetReference("users").OrderByChild("score").LimitToLast(5).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted) {
            } else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                int i = 0;
                Dictionary<string, string> dictionary = new Dictionary<string, string>();

                foreach (var userDoc in (Dictionary<string, object>)snapshot.Value) {
                    userObject = (Dictionary<string, object>)userDoc.Value;
                    dictionary.Add(userObject["username"].ToString(), userObject["score"].ToString());
                    scores[i].text = userObject["username"] + " - " + userObject["score"];
                    i += 1;


                }
                //var ordered = dictionary.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                //foreach (KeyValuePair<string, string> author in ordered) {
                //    print("Key: "+ author.Key+", Value: "+ author.Value);
                //    scores[i].text = author.Key + " - " + author.Value;
                   
                //}


            }
        });
    }

   

}

public class UserData {
    public int score;
    public string username;
}
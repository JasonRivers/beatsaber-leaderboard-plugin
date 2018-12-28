using IllusionPlugin;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PostScoreBehavior : MonoBehaviour
{
    private static PostScoreBehavior _instance;

    private string scores;

    private string PostUrl
    {
        get { return ModPrefs.GetString(this.ModPrefsKey, "SaberPartyUrl", "", true); }
    }

    private string LeaderboardFilePath
    {
        get
        {
            return String.Format(
                "{0}\\AppData\\LocalLow\\Hyperbolic Magnetism\\Beat Saber\\LocalLeaderboards.dat",
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
            );
        }
    }

    private string ModPrefsKey
    {
        get { return "PointSaber"; }
    }

    private string GetScoresString()
    {
        return File.ReadAllText(this.LeaderboardFilePath);
    }

    public static void PostScores()
    {
        Log("PostScoresBehavior.PostScores()");
        if (_instance == null)
        {
            _instance = new GameObject("PointSaber").AddComponent<PostScoreBehavior>();
        }
        Log("Call PostScoresRoutine");
        _instance.StartCoroutine(_instance.PostScoresRoutine());
        Log("Coroutine started");
    }

    public IEnumerator PostScoresRoutine()
    {
        var newScores = this.GetScoresString();
        if (newScores == this.scores)
        {
            Log("Scores haven't changed");
            yield return null;
        }
        else
        {
            Log(String.Format("Logging scores to {0}", this.PostUrl));
            this.scores = newScores;
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");
            using (var www = new WWW(this.PostUrl, Encoding.UTF8.GetBytes(this.scores), headers))
            {
                yield return www;
                Log(String.Format("Upload Response: {0}", www.text));
            }
        }
    }

    private static void Log(string data)
    {
        var now = DateTime.Now.ToLocalTime();
        File.AppendAllText(@"PointSaberPluginLog.txt", String.Format("{0} {1} - {2}{3}", now.ToShortDateString(), now.ToLongTimeString(), data, Environment.NewLine));
    }
}
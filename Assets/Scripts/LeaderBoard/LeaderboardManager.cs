using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public int maxLeaderboardEntries = 10;
    private List<LeaderboardEntry> leaderboardEntries;

    public GameObject leaderboardEntryPrefab;
    public Transform entriesParent;

    private void Awake()
    {
        LoadLeaderboard();
        UpdateUI();
    }

    public void AddEntry(LeaderboardEntry entry)
    {
        leaderboardEntries.Add(entry);
        leaderboardEntries.Sort((x, y) => y.score.CompareTo(x.score));
        if (leaderboardEntries.Count > maxLeaderboardEntries)
        {
            leaderboardEntries.RemoveAt(leaderboardEntries.Count - 1);
        }
        SaveLeaderboard();
    }

    public void AddNewEntry(string playerName, int score)
    {
        LeaderboardEntry newEntry = new LeaderboardEntry(playerName, score);
        AddEntry(newEntry);
        UpdateUI();
    }

    public void AddTestEntry()
    {
        AddNewEntry("Player", UnityEngine.Random.Range(1, 1000));
    }

    public void UpdateUI()
    {
        foreach (Transform child in entriesParent)
        {
            Destroy(child.gameObject);
        }

        foreach (LeaderboardEntry entry in leaderboardEntries)
        {
            GameObject newEntry = Instantiate(leaderboardEntryPrefab, entriesParent);
            TextMeshProUGUI entryText = newEntry.GetComponent<TextMeshProUGUI>();
            entryText.text = string.Format("{0}. {1} - {2}", leaderboardEntries.IndexOf(entry) + 1, entry.playerName, entry.score);
        }
    }

    private void LoadLeaderboard()
    {
        string jsonString = PlayerPrefs.GetString("Leaderboard", "");
        if (string.IsNullOrEmpty(jsonString))
        {
            leaderboardEntries = new List<LeaderboardEntry>();
        }
        else
        {
            LeaderboardEntry[] entriesArray = JsonHelper.FromJson<LeaderboardEntry>(jsonString);
            leaderboardEntries = new List<LeaderboardEntry>(entriesArray);
        }
    }

    private void SaveLeaderboard()
    {
        string jsonString = JsonHelper.ToJson(leaderboardEntries);
        PlayerPrefs.SetString("Leaderboard", jsonString);
        PlayerPrefs.Save();
    }

    public List<LeaderboardEntry> GetLeaderboardEntries()
    {
        return leaderboardEntries;
    }
}

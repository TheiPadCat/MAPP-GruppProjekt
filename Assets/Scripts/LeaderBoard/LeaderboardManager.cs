using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public int maxLeaderboardEntries = 10;
    private List<LeaderboardEntry> leaderboardEntries;

    public GameObject leaderboardEntryPrefab;
    public Transform entriesParent;
    public TextMeshProUGUI nameTextField;
    public GameObject nameField;
    public string plaName;
    public Island island;
    public int scr;


    public void NameInputButton(){
        island.playerName = nameTextField.text;
        plaName = nameTextField.text;
        ToggleNameBox(false);
        LeaderboardEntry newEntry = new LeaderboardEntry(plaName, scr);
        AddEntry(newEntry);
        UpdateUI();
    }
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
    public bool IsScoreInTopTen(int score)
    {
        // If the leaderboard has less than 10 entries, the score is automatically in the top 10
        if (leaderboardEntries.Count < maxLeaderboardEntries)
        {
            return true;
        }

        // Otherwise, compare the score to the lowest score currently on the leaderboard
        int lowestScore = leaderboardEntries[leaderboardEntries.Count - 1].score;
        if (score > lowestScore)
        {
            return true;
        }

        return false;
    }

    private void ToggleNameBox(bool type){
        nameField.SetActive(type);
    }
    public void AddNewEntry(string playerName, int score)
    {
        plaName = playerName;
        scr = score;
        if(IsScoreInTopTen(score)){
            ToggleNameBox(true);
        }
    }
    public void ClearLeaderboard()
    {
        // Clear the list of leaderboard entries
        leaderboardEntries.Clear();

        // Save the (now empty) leaderboard
        SaveLeaderboard();
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

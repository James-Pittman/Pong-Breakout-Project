using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Below is an example of how to use the leaderboard.
//
// leaderboard = Leaderboard.LoadRecords();
// board = leaderboard.GetTopRecords();
// if (leaderboard.IsTopRecord(1))
// {
//      Record newRecord = new Record(1, "testName");
//      leaderboard.AddRecord(newRecord);
//      leaderboard.SaveRecords();
// }
public class Leaderboard
{
    private List<Record> recordList;
    private int maxSize = 10;

    public Leaderboard(List<Record> recordList)
    {
        this.recordList = recordList;
    }

    // Loads the current leaderboard from local storage.
    public static Leaderboard LoadRecords()
    {
        if (File.Exists(Application.persistentDataPath +
            "/leaderboard.dat"))
        {
            // A leaderboard already exists; load it.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath +
                "/leaderboard.dat", FileMode.Open);

            List<Record> records = (List<Record>)(bf.Deserialize(file));
            file.Close();
            Leaderboard leaderboard = new Leaderboard(records);
            return leaderboard;
        }
        else
        {
            // A leaderboard does not exist; create a new one.
            List<Record> records = new List<Record>();
            Leaderboard leaderboard = new Leaderboard(records);
            return leaderboard;
        }

    }

    // Saves the leaderboard to local storage.
    public void SaveRecords()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath +
            "/leaderboard.dat", FileMode.Create);

        bf.Serialize(file, recordList);
        file.Close();
    }

    // Adds a new Record object to the end of the leaderboard. This method
    // assumes that a new record *should* be added. Use isTopRecord to test
    // if a record should be added first.
    public void AddRecord(Record record)
    {
        if (recordList.Count < maxSize)
        {
            recordList.Add(record);
        }
        else
        {
            recordList[recordList.Count - 1] = record;
        }

        recordList.Sort();
        recordList.Reverse();
    }

    // Returns the List of records in the leaderboard.
    public List<Record> GetTopRecords()
    {
        return recordList;
    }

    // Test if a new score (from a game that was just played) should be added to the leaderboard.
    public Boolean IsTopRecord(int score)
    {
        if (recordList.Count < maxSize || score > recordList[recordList.Count - 1].score)
        {
            return true;
        }

        return false;
    }
}

[Serializable]
public class Record : IComparable
{
    public int score;
    public String name;

    public Record(int score, String name)
    {
        this.score = score;
        this.name = name;
    }

    // Adapted from https://docs.microsoft.com/en-us/dotnet/api/system.icomparable?view=net-5.0
    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        Record otherRecord = obj as Record;
        if (otherRecord != null)
            return this.score.CompareTo(otherRecord.score);
        else
            throw new ArgumentException("Object is not a Record");
    }
}
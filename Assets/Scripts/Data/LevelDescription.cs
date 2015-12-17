using System;
using System.Collections.Generic;

public class LevelDescription : Description
{
    public int id;

    private int currentSpeed = 0;
    private List<KeyValuePair<float, float>> angularSpeeds;

    public float areaRadius;
    public float bulletRadius;
    public float bulletSpeed;
    public int goal;
    public int paletteIndex;

    private static int levelId = 0;

    public LevelDescription()
    {
        angularSpeeds = new List<KeyValuePair<float, float>>();
    }

    public override bool ParseCsvLine(string[] row)
    {
        SetAutoId();
        ParseSpeeds(row[0]);
        areaRadius = float.Parse(row[1]);
        bulletRadius = float.Parse(row[2]);
        bulletSpeed = float.Parse(row[3]);
        goal = int.Parse(row[4]);
        paletteIndex = int.Parse(row[5]);
        return true;
    }

    private void ParseSpeeds(string data)
    {
        string[] elements = data.Split(':');
        Assert.PositiveLength(elements, "You must insert at least one speed value for ID: " + id.ToString());

        float[] values = new float[elements.Length];
        for (int i = 0; i < elements.Length; i ++)
        {
            values[i] = float.Parse(elements[i]);
            Assert.Conditional(i % 2 == 1, values[i] > 0f, "You must insert positive value for waiting time: " + values[i].ToString());
        }
        SetAngularSpeeds(values);
    }

    public float GetCurrentSpeed()
    {
        return angularSpeeds[currentSpeed].Key;
    }

    public float GetCurrentSpeedDuration()
    {
        return angularSpeeds[currentSpeed].Value;
    }

    public void NextSpeed()
    {
        currentSpeed = ++currentSpeed % angularSpeeds.Count;
    }

    public void SetAutoId()
    {
        id = levelId++;
    }

    public void SetAngularSpeeds(float[] values)
    {
        Assert.That(values.Length == 1 || (values.Length % 2) == 0, "You can specify 1 length with no time value or a pair of number separated by ':'");
        angularSpeeds.Clear();

        if (values.Length == 1)
        {
            KeyValuePair<float, float> pair = new KeyValuePair<float, float>(values[0], -1f);
            angularSpeeds.Add(pair);
        }
        else
        {
            for (int i = 0; i < values.Length; i += 2)
            {
                KeyValuePair<float, float> pair = new KeyValuePair<float, float>(values[i], values[i + 1]);
                angularSpeeds.Add(pair);
            }
        }
    }

    public override string ToString()
    {
        string output = string.Format("{0},{1},{2},{3},{4},{5}, {6}", id, angularSpeeds.ToString(), areaRadius, bulletRadius, bulletSpeed, goal, paletteIndex);
        return output;
    }
}
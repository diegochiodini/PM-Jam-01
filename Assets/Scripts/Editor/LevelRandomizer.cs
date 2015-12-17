using UnityEngine;
using UnityEditor;
using System.Collections;
using Data;
using System.IO;

public class LevelRandomizer 
{
    protected class Range
    {
        public float min;
        public float max;
        public float weight;

        public Range(float minValue, float maxValue, float weightValue = 1f)
        {
            min = minValue;
            max = maxValue;
            weight = weightValue;
        }

        public float length
        {
            get
            {
                return max - min;
            }
        }

        public float GetWeightedValue(float value)
        {
            return (value / (max - min)) * weight;
        }
    }

    private const int LEVEL_NUMBER = 100;

    private Range angularSpeedRange = new Range(90f, 270f);
    private Range areaRadiusRange = new Range(0.5f, 1.0f);
    private Range bulletRadiusRange = new Range(0.08f, 0.2f);
    private Range bulletSpeedRange = new Range(3f, 6f);
    private Range goalRange = new Range(2f, 1000f);

    [MenuItem("Chiodini/Tools/Create random levels")]
    public static void CreateRandomLevels()
    {
        LevelRandomizer randomizer = new LevelRandomizer();

        LevelDescription[] descriptions = new LevelDescription[LEVEL_NUMBER];
        string[] lines = new string[LEVEL_NUMBER];
        for (int i = 0; i < LEVEL_NUMBER; i++)
        {
            descriptions[i] = randomizer.GetDescription();
            lines[i] = descriptions[i].ToString() + "," + randomizer.GetDifficulty(descriptions[i]);
        }

        string fullPath = CommonConstants.LOCAL_RANDOM_LEVEL_DATA;
        GameUtils.Log.Print("Writing " + fullPath + "...");
        System.IO.File.WriteAllLines(fullPath, lines);
    }

    public float GetDifficulty(LevelDescription description)
    {
        //return angularSpeedRange.GetWeightedValue(description.angularSpeed) + areaRadiusRange.GetWeightedValue(description.areaRadius)
        //    + bulletRadiusRange.GetWeightedValue(description.bulletRadius) + bulletSpeedRange.GetWeightedValue(description.bulletSpeed) + goalRange.GetWeightedValue(description.goal);

        return (bulletRadiusRange.GetWeightedValue(description.bulletRadius) / areaRadiusRange.GetWeightedValue(description.areaRadius)) * angularSpeedRange.GetWeightedValue(description.GetCurrentSpeed())
            * goalRange.GetWeightedValue(description.goal);
    }

    private LevelDescription GetDescription()
    {
        LevelDescription description = new LevelDescription();
        description.SetAutoId();
        description.SetAngularSpeeds(new float[] { GetAngularSpeed() });
        description.areaRadius = GetAreaRadius();
        description.bulletRadius = GetBulletRadius();
        description.bulletSpeed = GetBulletSpeed();
        description.goal = GetGoal(description.areaRadius, description.bulletRadius, 0.5f);

        return description;
    }

    private float GetAngularSpeed() 
    {
        int length = (int)angularSpeedRange.length / 10;
        float[] values = new float[length];
        for (int i = 0; i < length; i++)
        {
            values[i] = angularSpeedRange.min + (i * 10);
        }
        return GameUtils.Random.RangeCollection<float>(values);

        //return (float)Random.Range((int)angularSpeedRange.min, (int)angularSpeedRange.max);
    }

    private float GetAreaRadius()
    {
        float[] values = {0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1f };
        return GameUtils.Random.RangeCollection<float>(values);

        //return Random.Range(areaRadiusRange.min, areaRadiusRange.max);
    }

    private float GetBulletRadius()
    {
        float[] values = { 0.08f, 0.12f, 0.16f, 0.2f, 0.24f, 0.28f};
        return GameUtils.Random.RangeCollection<float>(values);

        //return Random.Range(bulletRadiusRange.min, bulletRadiusRange.max);
    }

    private float GetBulletSpeed()
    {
        //return (float)Random.Range((int)bulletSpeedRange.min, (int)bulletSpeedRange.max);
        return 6f;
    }

    private int GetGoal(float containerRadius, float bulletRadius, float perc)
    {
        float containerArea = containerRadius * containerRadius * Mathf.PI;
        float bulletrArea = bulletRadius * bulletRadius * Mathf.PI;
        float max = Mathf.Floor((containerArea / bulletrArea) * perc);
        //int min = Mathf.Max((int)goalRange.min, (int)(max * 0.5f));
        //return Random.Range(min , (int)max);

        return (int)max;
    }
}

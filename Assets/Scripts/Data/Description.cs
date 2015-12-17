using System;

public abstract class Description
{
    public abstract bool ParseCsvLine(string[] row);
}
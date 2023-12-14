using System;

public class LevelState
{
    public bool Passed{get => _passed; set => _passed = value;}
    public int Points{
        get => _points;
        set {
            if (value < 0 || value > 3)
                throw new ArgumentException("Incorrect points count");
            _points = value;
        }
    }

    private int _points = 0;
    private bool _passed = false;
}

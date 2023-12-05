using System;

public class LevelState
{
    public bool Unlocked{get => _unlocked; set => _unlocked = value;}
    public bool Passed{get => _passed; set => _passed = value;}
    public int Points{
        get => _points;
        set {
            if (value < 0 || value > 3)
                throw new ArgumentException("Incorrect points count");
            _points = value;
        }
    }

    private bool _unlocked = false;
    private bool _passed = false;
    private int _points = 0;
}

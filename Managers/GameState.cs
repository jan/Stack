using System;

[Serializable]
public class GameState
{
    public enum State
    {
        Loading,
        Loaded,
        Playing,
        Won,
        Lost,
        Over
    }

    public enum Reason
    {
        None,
        Ground,
        Obstacle,
        NotALandingSite,
        NotForward,
        Bounds,
        TooHighAboveLanding
    }

    public GameState(State state)
    {
        this.state = state;
    }

    public State state = State.Loading;
    public Reason reason = Reason.None;
    public int stackables = 0;
    public int score = 0;
    public float time = 0;

    public override string ToString()
    {
        return "GameState[" + state + ", " + reason + "]";
    }
}

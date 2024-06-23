using gishadev.golf.Gameplay;

namespace gishadev.golf.Core
{
    public interface IGameManager
    {
        GolfPlayer[] Players { get; }
        GolfPlayer CurrentTurnPlayer { get; }
    }
}
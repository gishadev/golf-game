namespace gishadev.golf.Gameplay
{
    public class GolfPlayer
    {
        public GolfPlayerContainer GolfPlayerContainer { get; private set; }
        
        public GolfPlayer(GolfPlayerContainer golfPlayerContainer)
        {
            GolfPlayerContainer = golfPlayerContainer;
        }
    }
}
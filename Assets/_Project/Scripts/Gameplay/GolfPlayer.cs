namespace gishadev.golf.Gameplay
{
    public class GolfPlayer
    {
        public int Index { get; private set; }
        public GolfPlayerContainer GolfPlayerContainer { get; private set; }
        
        public GolfPlayer(int index,GolfPlayerContainer golfPlayerContainer)
        {
            Index = index;
            GolfPlayerContainer = golfPlayerContainer;
        }
    }
}
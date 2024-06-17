namespace YellowSquad.CashierSimulator.Gameplay
{
    public class Boost : IReadOnlyBoost
    {
        public bool Active { get; private set; }
        
        public void Activate()
        {
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }
    }
}

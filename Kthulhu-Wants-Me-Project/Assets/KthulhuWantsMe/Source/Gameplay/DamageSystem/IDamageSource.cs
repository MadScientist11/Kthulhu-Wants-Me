using System.Collections.Generic;

namespace KthulhuWantsMe.Source.Gameplay.Interactables.Items
{
    public interface IDamageSource
    {
        List<IDamageProvider> DamageProviders
        {
            get;
            set;
        }
    }
}
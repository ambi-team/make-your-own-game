using System;

public class FirefliesTrigger: Component, IUsable
{
    [Property] public FirefliesStand FirefliesStand { get; set; }
    
    [Property] public Action OnTrigger { get; set; }

    public void Use(Player ply)
    {
        OnTrigger?.Invoke();
    }
}
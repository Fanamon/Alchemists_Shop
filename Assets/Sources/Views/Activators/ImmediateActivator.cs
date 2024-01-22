using UnityEngine.Events;

public class ImmediateActivator : Activator
{
    public event UnityAction<Player> PlayerEntered;

    protected override void Activate(Player player)
    {
        PlayerEntered?.Invoke(player);
    }
}

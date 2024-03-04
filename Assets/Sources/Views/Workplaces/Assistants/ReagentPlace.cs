
public class ReagentPlace : KeeperPlace
{
    public bool IsProcessing { get; private set; }

    public void StartProcessing()
    {
        IsProcessing = true;
    }

    public override void DropObject()
    {
        if (ObjectToKeep != null)
        {
            ObjectToKeep.gameObject.SetActive(false);
            ObjectToKeep = null;
            IsEmpty = true;
            IsProcessing = false;
        }
    }
}
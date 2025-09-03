using UnityEngine;

public class IntPortal : BaseInteracter
{
    public override void Interact()
    {
        base.Interact();
        Loader.Load(Loader.Scene.GameScene);
    }
}

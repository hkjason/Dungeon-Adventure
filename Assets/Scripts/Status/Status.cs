using UnityEngine;

public abstract class Status : ScriptableObject
{
    public int id;
    public string statusName;
    public int remainedClick;
    public Sprite artWork;
    public abstract Status Deepcopy();

    public abstract void StatusFunctioning(Character target);

    public virtual void StatusRemove(Character target)
    {
        Debug.Log("virtual");
    }
}

public enum StatusType
{
    DamageStatus,
    ValueStatus
}

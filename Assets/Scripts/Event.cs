using UnityEngine;

[System.Serializable]
public class Event
{
    public EventType eventType;
    public string eventName;
    public string eventDescription;

    public Sprite eventPicture;
}

public enum EventType
{
    ItemDrop,
    SkillDrop,
    BuffEvent,
    DebuffEvent,
    DeadBody,
    Artifact,
    Trap
}
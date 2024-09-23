using UnityEngine;

[CreateAssetMenu(fileName = "New Artifact", menuName = "Artifact")]
public class Artifacts : ScriptableObject
{
    public ArtifactType artifactType;
    public string artifactName;
    public string artifactDescription;
    public bool used;
    public Sprite artifactArtwork;

    public Artifacts Deepcopy()
    {
        Artifacts copy = CreateInstance<Artifacts>();
        copy.artifactType = artifactType;
        copy.artifactName = artifactName;
        copy.artifactDescription = artifactDescription;
        copy.used = used;
        copy.artifactArtwork = artifactArtwork;
        return copy;
    }

}
public enum ArtifactType
{
    Click,
    Debuff,
    Event,
    Death,
    Skill,
    Multicast,
    Pure,
    Reflect,
    Vampiric,
    EnemyCD,
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactManager : MonoBehaviour
{
    public static ArtifactManager instance;
    public List<Artifacts> artifacts;
    public Player player;

    public Text description;
    public Text buttonText;
    public Image image;

    private int _artifactIdx;

    public Image prefabImg;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("two singleton failure");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = Player.instance;
        foreach (Artifacts a in player.artifacts)
        {
            artifacts.Remove(a);
        }
    }

    /////////////////////////
    //Generate a new artifact
    /////////////////////////

    public int iiiii = 0;
    public void GenerateArtifact()
    {
        if (artifacts.Count <= 0)
        {
            GiveMoney();
            return;
        }
        _artifactIdx = Random.Range(0, artifacts.Count);
        _artifactIdx = 4;
        ArtifactDisplay(artifacts[_artifactIdx]);
    }

    private void GiveMoney()
    {
        UIManager.instance.artifactMoneyPanel.SetActive(true);
        UIManager.instance.moneyText.text = "Gold Found: 100!";
    }

    public void GiveMoneyEnd()
    {
        UIManager.instance.artifactMoneyPanel.SetActive(false);
        LevelManager.instance.tempMoneyCollected += 100;
    }

    //////////////////////////////////////
    //Equip called by artifactPanel button
    //////////////////////////////////////
    public void EquipArtifact()
    {
        player.artifacts.Add(artifacts[_artifactIdx].Deepcopy());
        GameObject go1 = UIManager.instance.battleArtifactPanel.transform.GetChild(player.artifacts.Count).gameObject;
        GameObject go2 = UIManager.instance.normalArtifactPanel.transform.GetChild(player.artifacts.Count).gameObject;
        go1.GetComponent<Image>().sprite = go2.GetComponent<Image>().sprite = artifacts[_artifactIdx].artifactArtwork;
        go1.SetActive(true);
        go2.SetActive(true);

        artifacts.RemoveAt(_artifactIdx);

        UIManager.instance.artifactDropPanel.SetActive(false);
        RoomManager.instance.CheckRoomEnd();
    }

    ////////////////////
    /// Discard artifact
    ////////////////////
    public void DiscardArtifact()
    {
        UIManager.instance.artifactDropPanel.SetActive(false);
        RoomManager.instance.CheckRoomEnd();
    }

    ////////////////////////////////
    //Display artifact at drop panel
    ////////////////////////////////
    private void ArtifactDisplay(Artifacts artifact)
    {
        UIManager.instance.artifactDropPanel.SetActive(true);
        buttonText.text = "Equip " + artifact.artifactName;
        description.text = artifact.artifactDescription;
        image.sprite = artifact.artifactArtwork;
    }

    ///////////////////////////////
    //Check if artifact exist//////
    //Called by other script///////
    //Pass in the artifact to check
    ///////////////////////////////
    public bool ArtifactCheck(ArtifactType type)
    {
        bool truthVal = false;
        foreach(Artifacts artifact in player.artifacts)
        {
            if(artifact.artifactType == type)
            {
                truthVal = true;
                if (type == ArtifactType.Death)
                {
                    if (artifact.used)
                        return false;
                    else
                    {
                        artifact.used = true;
                        return true;
                    }
                }
                return truthVal;
            }
        }
        return truthVal;
    }
}

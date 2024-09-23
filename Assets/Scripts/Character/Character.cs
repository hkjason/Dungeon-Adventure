using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
public abstract class Character : MonoBehaviour
{
    public string characterName;

    [SerializeField] public BaseStat baseStat;

    [Header("Stats")]
    public int maxHp;
    public int hp;
    public int temphp;
    public int armor;
    public int magicResistent;
    public float dodgeRate;
    public int baseDamage;
    public float critChance;
    public int luck;
    public int faith;

    [Header("Attributes")]
    public int strength;
    public int intelligence;
    public int dexterity;

    [Header("Equipment")]
    public Equipment helmat;
    public Equipment chestplate;
    public Equipment gauntlet;
    public Equipment greaves;
    public Equipment weapon;

    [Space]
    public bool isDead;
    public List<Skill> skills = new List<Skill>();
    public List<Status> status = new List<Status>();
    public int[] skillsCurrentClick = new int[4];
    public Animator animator;

    public int lastDamageTaken;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void BaseStatIni()
    {
        baseStat.maxHp = maxHp;
        baseStat.hp = this.hp;
        baseStat.temphp = this.temphp;
        baseStat.armor = this.armor;
        baseStat.magicResistent = this.magicResistent;
        baseStat.dodgeRate = this.dodgeRate;
        baseStat.baseDamage = this.baseDamage;
        baseStat.critChance = this.critChance;
        baseStat.luck = this.luck;
        baseStat.faith = this.faith;
        baseStat.strength = this.strength;
        baseStat.intelligence = this.intelligence;
        baseStat.dexterity = this.dexterity;
}

    public void TakeDamage(int delta, int attributeIdx, float critChance)
    {
        if(!isDead)
        {
            //Camera shake
            CameraShaker.Instance.ShakeOnce(2f, 4, 0.1f, 1f);
            bool isCri = false;
            float tempCri = Random.Range(0, 1f);
            if (tempCri <= critChance)
            {
                //critical
                delta = (int)(delta *1.5f);
                isCri = true;
            }
            int temp = CheckShield(delta);
            int tempDamage = 0;
            if (this == Enemy.instance)
            {
                if (ArtifactManager.instance.ArtifactCheck(ArtifactType.Pure))
                {
                    attributeIdx = 3;
                }
            }
            switch(attributeIdx)
            {
                case 0:
                    if (temp - armor > 0)
                    {
                        tempDamage = temp - armor;
                    }
                    break;
                case 1:
                    if (temp - magicResistent > 0)
                    {
                        tempDamage = temp - magicResistent;
                    }
                    break;
                case 2:
                    if((temp - (int)(armor/2) - (int)(magicResistent / 2)) > 0)
                    {
                        tempDamage = temp - (int)(armor / 2) - (int)(magicResistent / 2);
                    }
                    break;
                //pure
                case 3:
                    if (temp > 0)
                    {
                        tempDamage = temp;
                    }
                    break;
                default:
                    Debug.Log("FAILURE");
                    break;
            }
            hp -= tempDamage;
            lastDamageTaken = tempDamage;
            switch (isCri)
            {
                case true:
                    AfterTakeDamage(tempDamage, PopUpType.CriticalDamage);
                    break;
                case false:
                    AfterTakeDamage(tempDamage, PopUpType.Damage);
                    break;
            }
        }
    }

    protected abstract void AfterTakeDamage(int damageVal, PopUpType popUpType);

    private int CheckShield(int delta)
    {
        if(temphp>0)
        {
            if(temphp<delta)
            {
                temphp=0;
                return (delta-temphp);
            }
            else if(temphp>=delta)
            {
                temphp -= delta;
                return 0;
            }
        }
        return delta;
    }

    public void NormalAttack(Character target)
    {
        if (!isDead)
        {
            animator.SetTrigger("Atk");

            List<int> tempList = new List<int>{intelligence, strength, dexterity};
            tempList.Sort((x, y) => y.CompareTo(x));

            if(tempList[0]==tempList[1] && tempList[0]==tempList[2])
            {
                int temp = Random.Range(0,3);
                target.TakeDamage(tempList[temp], temp,critChance);
            }
            else if(tempList[0]==tempList[1])
            {
                int temp1 = Random.Range(0,2);
                target.TakeDamage(tempList[temp1], temp1, critChance);
            }
            else
            {
                if(tempList[0]==strength)
                {
                    target.TakeDamage(tempList[0], 0, critChance);
                }
                if(tempList[0]==intelligence)
                {
                    target.TakeDamage(tempList[0], 1, critChance);
                }
                if(tempList[0]==dexterity)
                {
                    target.TakeDamage(tempList[0], 2, critChance);
                }
            }
            AfterAttack();
        }
    }

    public virtual void AfterAttack()
    {
        Debug.Log("AfterAttack Virtual Void");
    }

    public void TakeHeal(int delta, int healTypeIdx)
    {
        //healTypeIdx
        //0 for normal(Luck)
        //1 for pure
        float tempHealMultiplier = 0;

        switch (healTypeIdx)
        {
            case 0:
                tempHealMultiplier = faith / 100;
                break;
            case 1:
                tempHealMultiplier = 1;
                break;
            default:
                Debug.Log("failure");
                break;
        }

        int tempHealAmount = Mathf.RoundToInt(delta * tempHealMultiplier);

        hp += tempHealAmount;
        if (hp >= maxHp)
        {
            //OverHeal, keep for artifact//
            //temphp += hp - maxhp; 
            //if (temphp >= maxhp)
            //    temphp = maxhp;
            hp = maxHp;
        }
        AfterHeal(tempHealAmount);
    }

    protected abstract void AfterHeal(int delta);

    protected void PopUp(GameObject popUpObj, Vector3 position, PopUpType popUpType)
    {
        TextMesh tm = popUpObj.transform.GetChild(0).GetComponent<TextMesh>();
        switch (popUpType)
        {
            case (PopUpType.Damage):
                tm.color = Color.white;
                break;
            case (PopUpType.CriticalDamage):
                tm.color = Color.yellow;
                break;
            case (PopUpType.Healing):
                tm.color = Color.green;
                break;
        }
        Instantiate(popUpObj, position, Quaternion.identity);
    }

    public abstract void EquipItem(Equipment item);

    protected enum PopUpType
    {
        Healing,
        Damage,
        CriticalDamage
    }

    [System.Serializable]
    public class BaseStat
    {
        public int maxHp;
        public int hp;
        public int temphp;
        public int armor;
        public int magicResistent;
        public float dodgeRate;
        public int baseDamage;
        public float critChance;
        public int luck;
        public int faith;
        public int strength;
        public int intelligence;
        public int dexterity;
    }
}
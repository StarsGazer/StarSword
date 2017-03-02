using UnityEngine;

/// <summary>
/// 玩家管理类
/// </summary>
public class PlayerManager : MonoBehaviour
{
    public int userId { get; set; }
    public int characterId { get; set; }

    public string nickName { get; set; }
    public int profession { get; set; }
    public bool sex { get; set; }

    public int level { get; set; }
    public int coin { get; set; }
    public int gold { get; set; }
    public int life { get; set; }
    public int attack { get; set; }
    public int defend { get; set; }
    public int magic { get; set; }
    public int agility { get; set; }
    public int speed { get; set; }
    public int fortune { get; set; }
    public int technique { get; set; }
    public int skillPoint { get; set; }
    public int status { get; set; }
    public int positionX { get; set; }
    public int positionY { get; set; }

    public void SetLogin(int userId, int characterId)
    {
        this.userId = userId;
        this.characterId = characterId;
    }

    public void SetCreateRole(int characterId)
    {
        this.characterId = characterId;
    }

    public void SetLoginRole(string nickName, int profession, bool sex, int level, int coin, int gold, int life, int attack, int defend, int magic, int agility,
        int speed, int fortune, int technique, int skillPoint, int status, int positionX, int positionY)
    {
        this.nickName = nickName;
        this.profession = profession;
        this.sex = sex;

        this.level = level;
        this.coin = coin;
        this.gold = gold;
        this.life = life;
        this.attack = attack;
        this.defend = defend;
        this.magic = magic;
        this.agility = agility;
        this.speed = speed;
        this.fortune = fortune;
        this.technique = technique;
        this.skillPoint = skillPoint;
        this.status = status;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}

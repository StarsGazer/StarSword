using System;
using UnityEngine;
using UnityEngine.UI;

public class TestInfo : MonoBehaviour
{
    private Text content;

    void Awake()
    {
        content = GetComponent<Text>();
        string format = " nickName : {0} \n profession : {1} \n sex : {2} \n" +
        "level : {3} \n coin : {4} \n gold : {5} \n life : {6} \n attack : {7} \n defend : {8} \n magic : {9} \n agility : {10} \n speed : {11} \n" +
        "fortune : {12} \n technique : {13} \n skillPoint : {14} \n status : {15} \n positionX : {16} \n positionY : {17} \n";
        PlayerManager pm = Globals.Instance.PlayerManager;
        content.text = String.Format(format, pm.nickName, pm.profession, pm.sex,
            pm.level, pm.coin, pm.gold, pm.life, pm.attack, pm.defend, pm.magic, pm.agility, pm.speed,
            pm.fortune, pm.technique, pm.skillPoint, pm.status, pm.positionX, pm.positionY);
    }
}

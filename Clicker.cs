using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Game : MonoBehaviour
{
    Vector3 position = new Vector3(360, 640, 0);
    private double score;
    private UInt64 bonus = 1;
    private UInt64 workersCount, workersBonus = 1;
    private UInt64 cost = 2;
    public Text ScoreText;
    public Text EffectText;
    public Text MarketText;
    public Text WorkText;
    [Header("Магазин")]
    public GameObject ShopPanel;
    public GameObject ErrorPanel;
    public GameObject BoostErrorPanel;
    public GameObject RedPanel;
    public UInt64[] shopCosts;
    public UInt64[] shopBonuses;
    public float[] bonusTime;   
    public Text[] shopBttnText;
    public Button[] ShopBttns;


    void Start()
    {
        StartCoroutine(BonusPerSec());
    }
    public void shopPanel_showAndHide()
    {
        ShopPanel.SetActive(!ShopPanel.activeSelf);
        ErrorPanel.SetActive(false);
        BoostErrorPanel.SetActive(false);
        if (ShopPanel.activeSelf)
        {
            MarketText.text = "GAME";
        }
        else if(!ShopPanel.activeSelf)
        {
            MarketText.text = "MARKET";
        }
    }
    IEnumerator shopErrorPanel_showAndHide()
    {
        ErrorPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        ErrorPanel.SetActive(false);
    }
    IEnumerator BoostErrorPanel_showAndHide()
    {
        BoostErrorPanel.SetActive(true);
        yield return new WaitForSeconds(2);
        BoostErrorPanel.SetActive(false);
    }

    public void shopBttn_addBonus(int index)
    {
        if (score >= shopCosts[index])
        {
            bonus *= shopBonuses[index];
            score -= shopCosts[index];
            shopCosts[index] *= 3;       
        }
        else if (score < shopCosts[index])
        {
            BoostErrorPanel.SetActive(false);
            StartCoroutine(shopErrorPanel_showAndHide());
        }
        ScoreText.text = "<color=orange>" + score + "</color>$";
        shopBttnText[index].text = "Купить улучшение\n<color=orange>" + shopCosts[index] + "</color>$";
    }

    public void Worker(Int32 index)
    {
        if (score >= shopCosts[index])
        {
            workersCount++;
            score -= shopCosts[index];
            cost = shopCosts[index] / 10;
            shopCosts[index] *= 3;
            if (shopCosts[index] > 360)
            {
                workersBonus *= 2;
            }
        }
        else if (score < shopCosts[index])
        {
            BoostErrorPanel.SetActive(false);
            StartCoroutine(shopErrorPanel_showAndHide());
        }
        ScoreText.text = "<color=orange>" + score + "</color>$";
        shopBttnText[2].text = "Купить рабочим шавуху\n<color=orange>" + cost + "</color>$";
        WorkText.text = "Нанять рабочего<color=orange>\n" + shopCosts[index] + "</color>$";
    }

    public void startBonusTimer(int index)
    {
        if (workersCount > 0 && score >= cost)
        {
            score -= cost;
            bonus *= 2;
            cost *= 2;
            StartCoroutine(bonusTimer(bonusTime[index], index));
            GameObject canvas = GameObject.Find("Canvas/BG");
            GameObject panel = Instantiate(RedPanel, position, Quaternion.identity);
            panel.transform.SetParent(canvas.transform);
        }
        else if (score < cost)
        {
            BoostErrorPanel.SetActive(false);
            StartCoroutine(shopErrorPanel_showAndHide());
        }
        else
        {
            ErrorPanel.SetActive(false);
            StartCoroutine(BoostErrorPanel_showAndHide());
        }
        ScoreText.text = "<color=orange>" + score + "</color>$";
        shopBttnText[2].text = "Купить рабочим шавуху\n<color=orange>" + cost + "</color>$";
    }

    IEnumerator BonusPerSec()
    {
        while (true)
        {
            score += (workersCount * workersBonus);
            ScoreText.text = "<color=orange>" + score + "</color>$";
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator bonusTimer(float time, int index)
    {
        ShopBttns[index].interactable = false;
        if (index == 0 && workersCount> 0)
        {
            workersBonus *= 2;
            yield return new WaitForSeconds(time);
            workersBonus /= 2;
        }
        ShopBttns[index].interactable = true;
    }

   public void OnClick()
    {
        score += bonus;
        GetComponent<Animation>().Play();
        ScoreText.text = "<color=orange>" + score + "</color>$";
        EffectText.text = "+ " + (bonus) + "$";
    }
}

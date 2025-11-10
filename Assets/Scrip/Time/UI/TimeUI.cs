using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
public class TimeUI : MonoBehaviour
{
    public RectTransform dayNightImage;
    public RectTransform clockPart;
    public Image seasonImage;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;

    public Sprite[] seasonSprites;

    private List<GameObject> clockBlocks = new List<GameObject>();

    void OnEnable()
    {
        EventHandler.GameMinuteEvent += OnGameMinuteEvent;
        EventHandler.GameDateEvent += OnGameDateEvent;
    }

    void OnDisable()
    {
        EventHandler.GameMinuteEvent -= OnGameMinuteEvent;
        EventHandler.GameDateEvent -= OnGameDateEvent;
    }

    void Awake()
    {
        for (int i = 0; i < clockPart.childCount; i++)
        {
            clockBlocks.Add(clockPart.GetChild(i).gameObject);
            clockPart.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnGameMinuteEvent(int minute, int hour)
    {
        timeText.text = hour.ToString("00") + ":" + minute.ToString("00");
        SwitchHourImage(hour);
        DayNightImageSwitch(hour);
    }

    private void OnGameDateEvent(int day, int month, int year, Season season)
    {
        dateText.text = year.ToString("0000") + "年" + month.ToString("00") + "月" + day.ToString("00") + "日";
        seasonImage.sprite = seasonSprites[(int)season];
    }
    /// <summary>
    /// 根据小时切换时间块显示
    /// </summary>
    /// <param name="hour"></param>
    private void SwitchHourImage(int hour)
    {
        int index = hour / 4;
        if (index == 0)
        {
            foreach (var block in clockBlocks)
            {
                block.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < clockBlocks.Count; i++)
            {
                if (i < index + 1)
                    clockBlocks[i].SetActive(true);
                else
                    clockBlocks[i].SetActive(false);
            }
        }
    }
    
    private void DayNightImageSwitch(int hour)
    {
        Vector3 rotate = new Vector3(0, 0, hour * 15 - 90);
        dayNightImage.DORotate(rotate, 1f, RotateMode.Fast);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public TextMeshProUGUI mainText;

    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI handsMessage;

    [SerializeField] private Image activeCrosshair;
    [SerializeField] private Image inActiveCrossHair;

    [SerializeField] private TextMeshProUGUI[] quests = null;

    [SerializeField] private float sendMessageTime = 1.5f;

    [SerializeField] private Image endScreen;
    [SerializeField] private TextMeshProUGUI endText;

    [SerializeField] private TextMeshProUGUI loseText;

    [SerializeField] private TextMeshProUGUI winText;

    [SerializeField] private Image startScreen;
    [SerializeField] private TextMeshProUGUI startText;

    [SerializeField] private float endDelay = 2f;
    [SerializeField] private float startDelay = 5f;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI daysText;

    [SerializeField] private Image[] needBars;
    [SerializeField] private Sprite[] potencialNeedBars;

    [SerializeField] private TextMeshProUGUI moneyText;

    [SerializeField] private Image ritualStage1Bar;

    private int timerStartDelay;
    private int timerTime;

    private IEnumerator timerCor;

    public static UIManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            foreach(TextMeshProUGUI quest in quests)
            {
                quest.gameObject.SetActive(!quest.gameObject.activeInHierarchy);
            }
        }
    }

    public void ChangeQuests(TextMeshProUGUI[] newQuests)
    {
        foreach (TextMeshProUGUI quest in quests)
        {
            if (quest != quests[0])
            {
                quest.gameObject.SetActive(false);
            }
        }
        quests = newQuests;
        foreach (TextMeshProUGUI quest in quests)
        {
            quest.gameObject.SetActive(true);
        }
    }

    public void CompleteQuest(int index)
    {
        quests[index].color = Color.green;
    }

    public void PrintMessage(string message)
    {
        StartCoroutine(SendMessage(message));
    }

    public void ShowMessage(string message)
    {
        mainText.text = message;
    }
    public void EraseMessage()
    {
        mainText.text = "";
    }


    public void Lose()
    {
        HideCursor();
        mainText.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        StartCoroutine(LoseGame());
    }

    public void Win()
    {
        HideCursor();
        mainText.gameObject.SetActive(false);
        endScreen.gameObject.SetActive(true);
        StartCoroutine(WinGame());
    }

    public void SetTimer(int startDelay, int time)
    {
        timerStartDelay = startDelay;
        timerTime = time;
    }

    public void HideCursor()
    {
        activeCrosshair.gameObject.SetActive(false);
        inActiveCrossHair.gameObject.SetActive(false);
    }

    public void ShowCursor()
    {
        activeCrosshair.gameObject.SetActive(true);
        inActiveCrossHair.gameObject.SetActive(true);
    }

    public void UpdateTime(int hours, int minutes)
    {
        string temporaryText = "";
        if (hours / 10 == 0)
        {
            temporaryText += '0';
        }
        temporaryText += hours.ToString() + ':';
        if (minutes / 10 == 0)
        {
            temporaryText += '0';
        }
        temporaryText += minutes.ToString();
        timeText.text = temporaryText;
    }

    public void UpdateDay(int day)
    {
        daysText.text = day.ToString() + " days left";
    }

    public void UpdateNeed(int needIndex, int barValue)
    {
        needBars[needIndex].sprite = potencialNeedBars[barValue];
    }

    public void UpdateRitual(int barValue)
    {
        ritualStage1Bar.sprite = potencialNeedBars[barValue];
    }

    public void ShowRitualBar()
    {
        ritualStage1Bar.gameObject.SetActive(true);
    }

    public void HideRitualBar()
    {
        ritualStage1Bar.gameObject.SetActive(false);
    }

    public void UpdateMoney(float value)
    {
        moneyText.text = value.ToString("F2") + '$';
    }

    IEnumerator Timer(int startDelay, int time)
    {
        yield return new WaitForSeconds(startDelay);
        timer.gameObject.SetActive(true);
        while (time > 0)
        {
            timer.text = time.ToString();
            time--;
            yield return new WaitForSeconds(1f);
        }
        timer.gameObject.SetActive(false);
    }

    IEnumerator showObject(GameObject obj, float time)
    {
        obj.SetActive(true);
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    IEnumerator SendMessage(string message)
    {
        mainText.text = message;
        yield return new WaitForSeconds(sendMessageTime);
        mainText.text = "";
    }

    IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(endDelay);
        loseText.gameObject.SetActive(true);
        yield return new WaitForSeconds(endDelay);
        Application.Quit();
    }

    IEnumerator WinGame()
    {
        yield return new WaitForSeconds(endDelay);
        winText.gameObject.SetActive(true);
        yield return new WaitForSeconds(endDelay);
        Application.Quit();
    }

    IEnumerator StartGame()
    {
        endScreen.gameObject.SetActive(true);
        EraseMessage();
        startText.gameObject.SetActive(true);
        yield return new WaitForSeconds(startDelay);
        endScreen.gameObject.SetActive(false);
        EraseMessage();
        startText.gameObject.SetActive(false);
    }

    IEnumerator ShowScreen(Image screen, TextMeshProUGUI text)
    {
        screen.gameObject.SetActive(true);
        yield return new WaitForSeconds(endDelay);
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(endDelay);
        text.gameObject.SetActive(false);
        screen.gameObject.SetActive(false);
    }
}

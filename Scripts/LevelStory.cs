using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelStory : MonoBehaviour
{
    private GameObject gameData;
    private ResultDatas[] gmResult;

    [SerializeField]
    public int gameLevel;
    [SerializeField]
    private TextMeshProUGUI storyText;
    [SerializeField]
    private GameObject panelStory;
    [SerializeField]
    private Button storyClose;
    [SerializeField]
    private Button storyOpen;
    [SerializeField]
    private Image face01;
    [SerializeField]
    private Image face02;
    [SerializeField]
    private Image chattingBox01;
    [SerializeField]
    private Image chattingBox02;
    [SerializeField]
    private TextMeshProUGUI chat01;
    [SerializeField]
    private TextMeshProUGUI chat02;

    [SerializeField]
    private Button[] blockObjects01;
    [SerializeField]
    private Button[] blockObjects02;
    [SerializeField]
    private Button[] blockObjects03;
    [SerializeField]
    private Button[] blockObjects04;
    [SerializeField]
    private Button[] blockObjects05;

    private bool storyOn = false;

    private string[] story;
    private int storyNum = 0;
    private int sceneNum = 0;
    private bool skipStory = false;

    private void Awake()
    {
        story = storyText.text.Split(new[] { '\r', '\n' });

        storyNum = 0;
        sceneNum = 0;
        chat01.text = "";
        chat02.text = "";

        for (int i = 0; i < blockObjects01.Length; i++)
        {
            blockObjects01[i].enabled = false;
        }

        for (int i = 0; i < blockObjects02.Length; i++)
        {
            blockObjects02[i].enabled = false;
        }

        for (int i = 0; i < blockObjects03.Length; i++)
        {
            blockObjects03[i].enabled = false;
        }

        for (int i = 0; i < blockObjects04.Length; i++)
        {
            blockObjects04[i].enabled = false;
        }

        for (int i = 0; i < blockObjects05.Length; i++)
        {
            blockObjects05[i].enabled = false;
        }

        StoryPanelOnOff();
        ReadingStory();
    }




    public void PlusSceneNum(int num)
    {
        if (skipStory == false && sceneNum == num)
        {
            sceneNum += 1;
            if (gameLevel == 1 && sceneNum == 5)
            {
                SkipStory();
            }
            else
            {
                chat01.text = "...";
                chat02.text = "...";
                StoryPanelOnOff();
                ReadingStory();
            }

        }
    }

    public void SkipStory()
    {
        panelStory.SetActive(false);
        storyClose.gameObject.SetActive(false);
        storyOpen.gameObject.SetActive(false);
        for (int i = 0; i < blockObjects01.Length; i++)
        {
            blockObjects01[i].enabled = true;
        }

        for (int i = 0; i < blockObjects02.Length; i++)
        {
            blockObjects02[i].enabled = true;
        }

        for (int i = 0; i < blockObjects03.Length; i++)
        {
            blockObjects03[i].enabled = true;
        }

        for (int i = 0; i < blockObjects04.Length; i++)
        {
            blockObjects04[i].enabled = true;
        }

        for (int i = 0; i < blockObjects05.Length; i++)
        {
            blockObjects05[i].enabled = true;
        }
        skipStory = true;
    }

    public void ReadingStory()
    {
        string readingText = story[storyNum];

        if(readingText.Contains("#"))
        {
            if (readingText.Replace("\n","") == "#" + sceneNum)
            {
                storyNum += 1;
                readingText = story[storyNum];
            }
        }

        if (readingText.Replace("\n", "") == "1")
        {
            face02.gameObject.SetActive(false);
            chattingBox02.gameObject.SetActive(false);
            storyNum += 1;
            readingText = story[storyNum].Replace("\n", "");
            string chatting = "";
            while (true)
            {
                if (readingText == "##")
                {
                    SkipStory();
                    return;
                }
                if (readingText == "1" || readingText == "2" || readingText.Contains("#"))
                {
                    if (readingText == "#1")
                    {
                        for (int i = 0; i < blockObjects01.Length ; i++)
                        {
                            blockObjects01[i].enabled = true;
                        }
                    }
                    else if (readingText == "#2")
                    {
                        for (int i = 0; i < blockObjects02.Length; i++)
                        {
                            blockObjects02[i].enabled = true;
                        }
                    }
                    else if (readingText == "#3")
                    {
                        for (int i = 0; i < blockObjects03.Length; i++)
                        {
                            blockObjects03[i].enabled = true;
                        }
                    }
                    else if (readingText == "#4")
                    {
                        for (int i = 0; i < blockObjects04.Length; i++)
                        {
                            blockObjects04[i].enabled = true;
                        }
                    }
                    else if (readingText == "#5")
                    {
                        for (int i = 0; i < blockObjects05.Length; i++)
                        {
                            blockObjects05[i].enabled = true;
                        }
                    }

                    break;
                }


                if (chatting == "")
                    chatting = readingText + "\n";
                else
                    chatting = chatting + readingText + "\n";

                storyNum += 1;
                
                readingText = story[storyNum].Replace("\n", "");
            }
            chat01.text = chatting;
        }
        else if (readingText.Replace("\n","") == "2")
        {
            face02.gameObject.SetActive(true);
            chattingBox02.gameObject.SetActive(true);
            storyNum += 1;
            readingText = story[storyNum].Replace("\n", "");
            string chatting = "";
            while (true)
            {
                if (readingText == "##")
                {
                    SkipStory();
                    return;
                }
                if (readingText == "1" || readingText == "2" || readingText.Contains("#"))
                {
                    if (readingText == "#1")
                    {
                        for (int i = 0; i < blockObjects01.Length; i++)
                        {
                            blockObjects01[i].enabled = true;
                        }
                    }
                    else if (readingText == "#2")
                    {
                        for (int i = 0; i < blockObjects02.Length; i++)
                        {
                            blockObjects02[i].enabled = true;
                        }
                    }
                    else if (readingText == "#3")
                    {
                        for (int i = 0; i < blockObjects03.Length; i++)
                        {
                            blockObjects03[i].enabled = true;
                        }
                    }
                    else if (readingText == "#4")
                    {
                        for (int i = 0; i < blockObjects04.Length; i++)
                        {
                            blockObjects04[i].enabled = true;
                        }
                    }
                    else if (readingText == "#5")
                    {
                        for (int i = 0; i < blockObjects05.Length; i++)
                        {
                            blockObjects05[i].enabled = true;
                        }
                    }

                    break;
                }

                if (chatting == "")
                    chatting = readingText + "\n";
                else
                    chatting = chatting + readingText + "\n";

                storyNum += 1;
                readingText = story[storyNum].Replace("\n", "");
            }
            chat02.text = chatting;
        }
    }

    public void StoryPanelOnOff()
    {
        if (storyOn)
        {
            panelStory.SetActive(false);
            storyClose.gameObject.SetActive(false);
            storyOpen.gameObject.SetActive(true);
            storyOn = false;
        }
        else
        {
            panelStory.SetActive(true);
            storyClose.gameObject.SetActive(true);
            storyOpen.gameObject.SetActive(false);
            storyOn = true;
        }
    }
}
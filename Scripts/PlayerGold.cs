using UnityEngine;

public class PlayerGold : MonoBehaviour
{
    [SerializeField]
    private int currentGold = 100;
    [SerializeField]
    private int missionGold = 500;
    private int plusGold = 0;
    private int prePlusGold = 0;
    
    private int payGold = 0;
    public int TopGold = 0;

    public int CurrentGold
    {
        set => currentGold = Mathf.Max(0, value);
        get => currentGold;
    }

    public int PlusGold
    {
        set => plusGold = Mathf.Max(0, value);
        get => plusGold;
    }

    public int PrePlusGold
    {
        set => prePlusGold = Mathf.Max(0, value);
        get => prePlusGold;
    }

    public int PayGold
    {
        set => payGold = Mathf.Max(0, value);
        get => payGold;
    }

    public int MissionGold
    {
        set => missionGold = Mathf.Max(0, value);
        get => missionGold;
    }

    public void Cheate(int gold)
    {
        currentGold += gold;
    }

    public void Update()
    {
        if (currentGold > TopGold)
        {
            TopGold = currentGold;
        }
    }

}

using UnityEngine;
using UnityEngine.UI;

/*
 Script for displaying the players HP, based upon the values.
 */

public class hpManager : MonoBehaviour
{

    [SerializeField] private Image[] hp;
    [SerializeField] private Sprite full, half, empty;
    [SerializeField] private FloatValue maxHp, currHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startHP();
    }

    private void startHP()
    {
        for (int i = 0; i < maxHp.runtimeValue; i++)
        {
            hp[i].gameObject.SetActive(true);
            hp[i].sprite = full;
        }
    }
    // Update is called once per frame
    public void UpdateHP()
    {
        startHP();

        // HP has a half state, so we divide this by 2
        float temp = currHp.runtimeValue / 2;

        // displays the correct sprite based upon players HP
        for (int i = 0;i < maxHp.runtimeValue; i++)
        {
            if (i <= temp - 1)
            {
                hp[i].sprite = full;
            } else if (i >= temp)
            {
                hp[i].sprite = empty;
            } else
            {
                hp[i].sprite = half;
            }
            
        }
    }
}

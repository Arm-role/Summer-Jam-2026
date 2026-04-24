using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class HPBar : MonoBehaviour
{
  [Header("UI References")]
  public Image playerBar;
  public Image enemyBar;
  public TextMeshProUGUI playerText;
  public TextMeshProUGUI enemyText;


  [Header("Settings")]
  public float playerMaxHP = 100f;
  public float enemyMaxHP = 100f;
  public float healAmount = 5f;
  public float damageAmount = 10f;
  public float smoothSpeed = 5f;

  private float p_HP;
  private float e_HP;


  private float display_p_HP;
  private float display_e_HP;

  void Update()
  {
    display_p_HP = Mathf.Lerp(display_p_HP, p_HP, Time.deltaTime * smoothSpeed);
    display_e_HP = Mathf.Lerp(display_e_HP, e_HP, Time.deltaTime * smoothSpeed);

    UpdateUI(display_p_HP, display_e_HP);
  }

  public void SetupHP(
    int playerCurrent,
    int playerMax,
    int enemyCurrent,
    int enemyMax)
  {
    p_HP = playerCurrent;
    e_HP = enemyCurrent;

    playerMaxHP = playerMax;
    enemyMaxHP = enemyMax;

    display_p_HP = p_HP;
    display_e_HP = e_HP;

    UpdateUI(display_p_HP, display_e_HP);
  }

  public void UpdatePlayerHP(int current, int max)
  {
    p_HP = current;
    playerMaxHP = max;
  }

  public void UpdateEnemyHP(int current, int max)
  {
    e_HP = current;
    enemyMaxHP = max;
  }


  void UpdateUI(float p_val, float e_val)
  {

    float totalCurrentHP = p_val + e_val;


    if (totalCurrentHP <= 0)
    {
      playerBar.fillAmount = 0.5f;
      enemyBar.fillAmount = 0.5f;
      return;
    }


    float playerRatio = p_val / totalCurrentHP;


    if (playerBar != null) playerBar.fillAmount = playerRatio;
    if (enemyBar != null) enemyBar.fillAmount = 1f - playerRatio;


    if (playerText != null) playerText.text = "HP: " + Mathf.RoundToInt(p_val);
    if (enemyText != null) enemyText.text = "HP: " + Mathf.RoundToInt(e_val);


  }
}

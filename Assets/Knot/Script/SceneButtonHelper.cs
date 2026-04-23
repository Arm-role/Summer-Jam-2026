using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtonHelper : MonoBehaviour
{
    public void GoToNextLevel()
    {
        // เช็กก่อนว่ามี SceneController ข้ามฉากมาไหม
        if (SceneController.instance != null)
        {
            // สั่งผ่าน instance ได้เลย ไม่ต้องลากใส่ช่อง Inspector!
            SceneController.instance.Nextlevel();
            {
                Debug.Log("เจอ");
            }
            
        }
        else
        {
            Debug.LogWarning("หา SceneController ไม่เจอ! (อาจจะไม่ได้เริ่มเล่นจากฉากแรก)");
        }
    }
}

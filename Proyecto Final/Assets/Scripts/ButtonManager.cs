using Unity.VisualScripting;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public static void OnButtonClick()
    {
        SoundManager.Instance().PlayUISound();
    }
}

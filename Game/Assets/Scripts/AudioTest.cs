using UnityEngine;
using UnityEngine.UI;

public class AudioTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic("BGM_1");
        AudioManager.Instance.PlayLoopSFX("Rock_Roll");
        
        Invoke("PlayAfterDelay",3);
        Invoke("PlayAfterDelay2",4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAfterDelay()
    {
        AudioManager.Instance.PlaySFX("Car_AlarmSFX");
    }

     public void PlayAfterDelay2()
    {
        AudioManager.Instance.PlaySFX("BreakSFX");
    }
}

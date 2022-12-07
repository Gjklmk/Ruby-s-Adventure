using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
 
public class T : MonoBehaviour
{
    [DllImport("user32.dll", EntryPoint = "keybd_event")]
    public static extern void Keybd_event(
          byte bvk,//虚拟键值 Enter键对应的是13
          byte bScan,//0
          int dwFlags,//0为按下，1按住，2释放
          int dwExtraInfo//0
          );
    public InputField input;

    public void Enter()
    {
        //  input.text = "\n";
        Keybd_event(84, 0, 0, 0);
        input.readOnly = true;
    }
}
using UnityEngine;
using VegetaSystem.UI;

public class Sample_UIManger : UIManagerSystem
{
    void Start()
    {
        HideAllScreens();
        ShowScreen<Sample_Menuscreen>();
    }
}

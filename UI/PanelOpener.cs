using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public enum OpenOn
    {
        NONE = 0, // Panel will not be loaded
        AWAKE, // Panel loaded on awake
        START, // Panel loaded on start
    }

    [System.Serializable]
    public class PanelOpenData
    {
        public OpenOn openOn;
        public Panel panelToOpen;
        public PanelData panelData;
    }

    [SerializeField]
    PanelOpenData[] panelsToOpen;

    void Awake()
    {
        for (int i = 0; i < panelsToOpen.Length; ++i)
        {
            if (panelsToOpen[i].openOn == OpenOn.AWAKE)
                PanelManager.OpenPanel(panelsToOpen[i].panelToOpen, panelsToOpen[i].panelData);
        }
    }
    void Start()
    {
        for (int i = 0; i < panelsToOpen.Length; ++i)
        {
            if (panelsToOpen[i].openOn == OpenOn.START)
                PanelManager.OpenPanel(panelsToOpen[i].panelToOpen, panelsToOpen[i].panelData);
        }
    }
}

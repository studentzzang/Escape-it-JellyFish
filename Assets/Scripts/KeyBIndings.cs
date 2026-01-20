using UnityEngine;

/// <summary>
/// jellyfish 기준 left right 
/// </summary>
[System.Serializable]
public class KeyBindings
{
    public KeyCode left = KeyCode.LeftArrow;
    public KeyCode right = KeyCode.RightArrow;

    const string K_LEFT = "bind_left";
    const string K_RIGHT = "bind_right";


    public void Save()
    {
        PlayerPrefs.SetInt(K_LEFT, (int)left);
        PlayerPrefs.SetInt(K_RIGHT, (int)right);

        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(K_LEFT)) left = (KeyCode)PlayerPrefs.GetInt(K_LEFT);
        if (PlayerPrefs.HasKey(K_RIGHT)) right = (KeyCode)PlayerPrefs.GetInt(K_RIGHT);
    }

    public void SetPresetArrows()
    {
        left = KeyCode.LeftArrow; 
        right = KeyCode.RightArrow;
    }

    public void SetPresetWASD()
    {
        left = KeyCode.A; right = KeyCode.D;
    }
}

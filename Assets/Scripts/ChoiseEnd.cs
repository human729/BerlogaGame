using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class ChoiseEnd : MonoBehaviour
{
    [SerializeField] RoomPanelUI roompanelui;
    [SerializeField] GameObject Story;
    [SerializeField] List<Image> StoryList;
    [SerializeField] Image image;
    public void ChooseProgrammers()
    {
        roompanelui.startRoomId = 4;
        image.sprite = StoryList[3].sprite;
        Story.SetActive(true);
    } 
    public void ChoosePioneers()
    {
        roompanelui.startRoomId = 5;
        image.sprite = StoryList[4].sprite;
        Story.SetActive(true);
    }
    public void ChooseBioengineers()
    {
        roompanelui.startRoomId = 3;
        image.sprite = StoryList[2].sprite;
        Story.SetActive(true);
    }
    public void ChooseBeekeepers()
    {
        roompanelui.startRoomId = 1;
        image.sprite = StoryList[0].sprite;
        Story.SetActive(true);
    }
    public void ChooseConstructors()
    {
        roompanelui.startRoomId = 2;
        image.sprite = StoryList[1].sprite;
        Story.SetActive(true);
    }
    public void ChooseCreators()
    {
        roompanelui.startRoomId = 6;
        image.sprite = StoryList[5].sprite;
        Story.SetActive(true);
    }
}

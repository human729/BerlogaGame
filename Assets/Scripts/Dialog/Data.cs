using System;
using System.Collections.Generic;
namespace BearLab.Story
{


    [Serializable]
    public class Data
    {
        public string intro;
        public MainMenu main_menu;
        public List<Room> rooms;
    }

    [Serializable]
    public class MainMenu
    {
        public string arrive;
        public string arrive2;
        public List<Choice> choices;
        public string final_prompt;
    }

    [Serializable]
    public class Choice
    {
        public int door_number;
        public string tradition_name;
        public string description;
    }

    [Serializable]
    public class Room
    {
        public int room_id;
        public string name;
        public string tradition;
        public string start_message;
        public List<RoomTask> rooms_tasks;
    }

    [Serializable]
    public class RoomTask
    {
        public int task_number;
        public string message;
        public string after_passing;
    }
}
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class DronMovement : MonoBehaviour
{
    [SerializeField] Comands com;

    public void AddUp()
    {
        com.move.Add("�����");
        com.MarkPathDirty();
    }

    public void AddDown()
    {
        com.move.Add("����");
        com.MarkPathDirty();
    }

    public void AddLeft()
    {
        com.move.Add("�����");
        com.MarkPathDirty();
    }

    public void AddRight()
    {
        com.move.Add("������");
        com.MarkPathDirty();
    }

    public void AddPause()
    {
        com.move.Add("�����");
        com.MarkPathDirty();
    }

    public void DeleteOneInstruction()
    {
        if (com.move.Count > 0)
        {
            com.move.RemoveAt(com.move.Count - 1);
            com.MarkPathDirty();
        }
    }

    public void DeleteAllInstructions()
    {
        com.move.Clear();
        com.MarkPathDirty();
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Help : MonoBehaviour {


	public static string print(params object[] list)
	{
		string printString = "";
		for (int i = 0; i < list.Length; i++)
		{
			if (list[i] is IList)
			{
                //list[i]
                IList temp = (IList)list[i];
				//print(temp.Count);
                for (int j = 0; j < temp.Count; j++)
                {
                    printString += ToString(temp[j]);
                    if (j != temp.Count - 1)
                    {
                        
                        printString += "  \t";
                    }
                }
            }
			else
			{
				printString += ToString(list[i]);
				
			}
			if (i != list.Length-1)
			{
					printString += "  \t";
			}
		}
		MonoBehaviour.print(printString);
		return printString;
	}
	public static string ToString(object element)
	{
		string temp = "";
        if (element is RaycastHit)
        {
            //temp = String.Format( "RaycastHit: {0} at {1} away", ((RaycastHit)element).collider.name, ((RaycastHit)element).distance);
			temp = String.Format( "RaycastHit: {0}", ((RaycastHit)element).collider.name);
        }
		else if(element == null)
        {
            temp = "null";
        }
        else
        {
            temp = element.ToString();
        }
        return temp;
	}

	/// <summary>
	/// prints string into a ui text named "Debugger" great for debugging in realtime especially when you only want to see just the current value and not every value since the start
	/// </summary>
	/// <param name="debuggingText"></param>
    public static void LiveDebugText(string debuggingText)
    {
        // Text debugger = GameObject.Find("Debugger").GetComponent<Text>();
        if (GameObject.Find("Debugger") != null)
        {
            GameObject.Find("Debugger").GetComponent<Text>().text = debuggingText;
        }
        else
        {
            MonoBehaviour.print(String.Format("ERROR: Coulnd't find UI Text ''Debugger''.\nCould not show: {0}", debuggingText));
        }

    }
    public static void LiveDebugText(params object[] list)
	{
		string printString = "";
		for (int i = 0; i < list.Length; i++)
		{
			if (list[i] is IList)
			{
                //list[i]
                IList temp = (IList)list[i];
				//print(temp.Count);
                for (int j = 0; j < temp.Count; j++)
                {
                    printString += ToString(temp[j]);
                    if (j != temp.Count - 1)
                    {
                        
                        printString += "  \t";
                    }
                }
            }
			else
			{
				printString += ToString(list[i]);
				
			}
			if (i != list.Length-1)
			{
					printString += "  \t";
			}
		}
		LiveDebugText(printString);
	}
}
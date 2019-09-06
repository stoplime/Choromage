using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {
	private static Dictionary<string, KeyCode> defaultControls = new Dictionary<string, KeyCode>();
	private static Dictionary<string, KeyCode> controls = new Dictionary<string, KeyCode>();

	//  TODO: clean up the controls using enum rather than strings
	public enum ControlEnum
	{
		camera_up,
		camera_down,
		camera_right,
		camera_left,

		camera_zoom_in,
		camera_zoom_out,

		character_up,
		character_down,
		character_right,
		character_left,

		character_navigate,

		pause,

		off_hand,
		spell_1,
		spell_2,
		spell_3,
		spell_4,
		spell_5,

		interact,

		inventory,
		spell_book,
		stats,
		journal,
		settings,
		debug,

		godSpeed,
		intagible,
		debugText,
		freeSpells,
		fastCast,
		customManaRegen,
		customManaRegenFactor,
		customHealthRegen,
		endlessHP,
		immortal,
		invulnerable,
		invisible,
		// fastSpells,
        // powerfulSpells,
		talk,
		sprint
	}
	//Awake is always called before any Start functions

	void Start()
	{
		if (defaultControls.Count == 0)
		{
			InitializeDefaultControls();
		}
	}

	public static void InitializeDefaultControls ()
	{
		defaultControls.Add("camera_up", KeyCode.UpArrow);
		defaultControls.Add("camera_down", KeyCode.DownArrow);
		defaultControls.Add("camera_right", KeyCode.RightArrow);
		defaultControls.Add("camera_left", KeyCode.LeftArrow);
		defaultControls.Add("camera_recenter", KeyCode.Mouse2);
		
		defaultControls.Add("character_up", KeyCode.W);
		defaultControls.Add("character_down", KeyCode.S);
		defaultControls.Add("character_right", KeyCode.D);
		defaultControls.Add("character_left", KeyCode.A);

		defaultControls.Add("sprint", KeyCode.LeftShift);
		
		defaultControls.Add("pause", KeyCode.Space);
		//defaultControls.Add("spell1", KeyCode.Mouse0);
		defaultControls.Add("off_hand", KeyCode.BackQuote);
        defaultControls.Add("spell1", KeyCode.Alpha1);
        defaultControls.Add("spell2", KeyCode.Alpha2);
        defaultControls.Add("spell3", KeyCode.Alpha3);
        defaultControls.Add("spell4", KeyCode.Alpha4);
        defaultControls.Add("spell5", KeyCode.Alpha5);
		defaultControls.Add("spell6", KeyCode.Alpha6);
        // defaultControls.Add("navigate", KeyCode.Mouse1);
		defaultControls.Add("interact", KeyCode.Mouse1);

		defaultControls.Add("inventory", KeyCode.E);
        defaultControls.Add("spellBook",KeyCode.B);
        defaultControls.Add("stats", KeyCode.C);
		defaultControls.Add("journal", KeyCode.J);
		defaultControls.Add("settings", KeyCode.Escape);
        defaultControls.Add("talk", KeyCode.T);
        defaultControls.Add("debug", KeyCode.G);

		defaultControls.Add("godSpeed", KeyCode.F1);
		defaultControls.Add("intagible", KeyCode.F2);
		defaultControls.Add("debugText", KeyCode.F3);
		defaultControls.Add("freeSpells", KeyCode.F4);
		defaultControls.Add("fastCasting", KeyCode.F5);
		defaultControls.Add("endlessHealth", KeyCode.F6);
		defaultControls.Add("customManaRegen", KeyCode.F7);
		defaultControls.Add("customHealthRegen", KeyCode.F8);
		defaultControls.Add("immortal", KeyCode.F9);
		defaultControls.Add("invulnerable", KeyCode.F10);
		defaultControls.Add("invisible", KeyCode.F11);
		//defaultControls.Add("fastSpells", KeyCode.F11);
		//defaultControls.Add("powerfulSpells", KeyCode.F12);


		controls = new Dictionary<string, KeyCode>(defaultControls);
	}

	/// <summary>
	/// Provides a list of codes to get controls off of
	/// </summary>
	/// <returns></returns>
	public static List<string> GetCodes()
	{
		if (defaultControls.Count == 0)
		{
			InitializeDefaultControls();
		}
		return new List<string>(defaultControls.Keys);
	}
	
	/// <summary>
	/// Gets the control key for a specific code
	/// </summary>
	/// <param name="code"></param>
	/// <returns></returns>
	public static KeyCode GetControl(string code)
	{
		if (controls.ContainsKey(code))
		{
			return controls[code];
		}
        print(string.Format("ERROR: no {0} assigned", code));
        return KeyCode.P;
	}
}

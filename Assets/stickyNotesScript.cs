using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class stickyNotesScript : MonoBehaviour
{
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public KMBombModule module;
    public KMSelectable[] buttons;
    public KMSelectable mug;
    public int counter;
    public Material[] stickyNotesMaterials;
    public Material[] textMaterials;
    public Renderer[] stickyNotes; //the physical objects you are wanting to render (defined in the inspector)
    public Color[] stickyNoteColours; //the choices of colour you want the renderers to pick from (defined in the inspector)
    public GameObject[] cylinders;
    [TextArea] public String[] Admin;
    [TextArea] public String[] HumanResources;
    [TextArea] public String[] Payroll;
    [TextArea] public String[] RedHerrings;
    [TextArea] public String[] Other;
    [TextArea] public String Answer;
    public String Date;
    public TextMesh[] stickyNotesText;
    [TextArea] public String[] SetAnswers;
    List<string> selected = new List<string>();
    List<string> textOnStickyNote = new List<string>();
    public GameObject[] TwitchPlaysText;
    bool TwitchPlaysActive;

    //Logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    // Use this for initialization
    void Awake()
    {
        moduleId = moduleIdCounter++;
        mug.OnInteract += delegate () { MugPress(); return false; };

        foreach (KMSelectable button in buttons)
        {
            KMSelectable pressedButton = button;
            button.OnInteract += delegate () { ButtonPress(pressedButton); return false; };
        }
    }
    
    void Start()
    {
        Array.Sort(Other);
        SetRenderQueue();
        module.OnActivate += delegate { OnActivate(); };
        GenerateColors();
    }

    public void OnActivate()
    {
        CheckDay();
        CompileNewList();
        ChangeTextOnNotes();
        Checking();
        ActivateTwitchPlays();
    }

    public void CheckDay()
    {
        DateTime date = DateTime.Now;
        DayOfWeek day = DateTime.Now.DayOfWeek;

        if ((day == DayOfWeek.Monday) || (day == DayOfWeek.Friday))
        {
            Date = "Column1";
            Debug.LogFormat("[Sticky Notes #{0}]: Column 1 selected", moduleId);
        }
        else if ((day == DayOfWeek.Tuesday) || (day == DayOfWeek.Thursday))
        {
            Date = "Column2";
            Debug.LogFormat("[Sticky Notes #{0}]: Column 2 selected", moduleId);
        }
        else if (day == DayOfWeek.Wednesday)
        {
            Date = "Column3";
            Debug.LogFormat("[Sticky Notes #{0}]: Column 3 selected", moduleId);
        }
        else
        {
            Date = "Column4";
            Debug.LogFormat("[Sticky Notes #{0}]: Column 4 selected", moduleId);
        }
    }

    public void SetRenderQueue()
    {
        stickyNotesMaterials[0].renderQueue = 3001;
        stickyNotesMaterials[1].renderQueue = 3003;
        stickyNotesMaterials[2].renderQueue = 3005;
        stickyNotesMaterials[3].renderQueue = 3007;
        stickyNotesMaterials[4].renderQueue = 3009;
        stickyNotesMaterials[5].renderQueue = 3011;
        stickyNotesMaterials[6].renderQueue = 3013;
        stickyNotesMaterials[7].renderQueue = 3015;
        stickyNotesMaterials[8].renderQueue = 3017;
        stickyNotesMaterials[9].renderQueue = 3019;
        textMaterials[0].renderQueue = 3002;
        textMaterials[1].renderQueue = 3004;
        textMaterials[2].renderQueue = 3006;
        textMaterials[3].renderQueue = 3008;
        textMaterials[4].renderQueue = 3010;
        textMaterials[5].renderQueue = 3012;
        textMaterials[6].renderQueue = 3014;
        textMaterials[7].renderQueue = 3016;
        textMaterials[8].renderQueue = 3018;
        textMaterials[9].renderQueue = 3020;
    }

    public void GenerateColors()
    {
        foreach (Renderer rnd in stickyNotes)
        {
            int randomInt = UnityEngine.Random.Range(0, stickyNoteColours.Length);
            rnd.material.color = stickyNoteColours[randomInt];
        }

    }

    public void CompileNewList()
    {
        if (Date == "Column1")
        {
            Debug.LogFormat("[Sticky Notes #{0}]: Generating column 1 sticky notes", moduleId);
            for (int i = 0; i < 2; i++)
            {
                int index = UnityEngine.Random.Range(0, Admin.Length);
                if (!selected.Contains(Admin[index]))
                {
                    SetAnswers[i] = Admin[index];
                    selected.Add(Admin[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 2; i < 3; i++)
            {
                int index = UnityEngine.Random.Range(0, HumanResources.Length);
                if (!selected.Contains(HumanResources[index]))
                {
                    SetAnswers[i] = HumanResources[index];
                    selected.Add(HumanResources[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 3; i < 4; i++)
            {
                int index = UnityEngine.Random.Range(0, Payroll.Length);
                if (!selected.Contains(Payroll[index]))
                {
                    SetAnswers[i] = Payroll[index];
                    selected.Add(Payroll[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 4; i < 7; i++)
            {
                int index = UnityEngine.Random.Range(0, RedHerrings.Length);
                if (!selected.Contains(RedHerrings[index]))
                {
                    SetAnswers[i] = RedHerrings[index];
                    selected.Add(RedHerrings[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 7; i < 10; i++)
            {
                int index = UnityEngine.Random.Range(0, Other.Length);
                if (!selected.Contains(Other[index]))
                {
                    SetAnswers[i] = Other[index];
                    selected.Add(Other[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
        }
        else if (Date == "Column2")
        {
            Debug.LogFormat("[Sticky Notes #{0}]: Generating column 2 sticky notes", moduleId);
            for (int i = 0; i < 2; i++)
            {
                int index = UnityEngine.Random.Range(0, HumanResources.Length);
                if (!selected.Contains(HumanResources[index]))
                {
                    SetAnswers[i] = HumanResources[index];
                    selected.Add(HumanResources[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 2; i < 3; i++)
            {
                int index = UnityEngine.Random.Range(0, Admin.Length);
                if (!selected.Contains(Admin[index]))
                {
                    SetAnswers[i] = Admin[index];
                    selected.Add(Admin[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 3; i < 4; i++)
            {
                int index = UnityEngine.Random.Range(0, Payroll.Length);
                if (!selected.Contains(Payroll[index]))
                {
                    SetAnswers[i] = Payroll[index];
                    selected.Add(Payroll[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 4; i < 7; i++)
            {
                int index = UnityEngine.Random.Range(0, RedHerrings.Length);
                if (!selected.Contains(RedHerrings[index]))
                {
                    SetAnswers[i] = RedHerrings[index];
                    selected.Add(RedHerrings[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 7; i < 10; i++)
            {
                int index = UnityEngine.Random.Range(0, Other.Length);
                if (!selected.Contains(Other[index]))
                {
                    SetAnswers[i] = Other[index];
                    selected.Add(Other[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
        }
        else if (Date == "Column3")
        {
            Debug.LogFormat("[Sticky Notes #{0}]: Generating column 3 sticky notes", moduleId);
            for (int i = 0; i < 2; i++)
            {
                int index = UnityEngine.Random.Range(0, Payroll.Length);
                if (!selected.Contains(Payroll[index]))
                {
                    SetAnswers[i] = Payroll[index];
                    selected.Add(Payroll[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 2; i < 3; i++)
            {
                int index = UnityEngine.Random.Range(0, Admin.Length);
                if (!selected.Contains(Admin[index]))
                {
                    SetAnswers[i] = Admin[index];
                    selected.Add(Admin[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 3; i < 4; i++)
            {
                int index = UnityEngine.Random.Range(0, HumanResources.Length);
                if (!selected.Contains(HumanResources[index]))
                {
                    SetAnswers[i] = HumanResources[index];
                    selected.Add(HumanResources[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 4; i < 7; i++)
            {
                int index = UnityEngine.Random.Range(0, RedHerrings.Length);
                if (!selected.Contains(RedHerrings[index]))
                {
                    SetAnswers[i] = RedHerrings[index];
                    selected.Add(RedHerrings[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 7; i < 10; i++)
            {
                int index = UnityEngine.Random.Range(0, Other.Length);
                if (!selected.Contains(Other[index]))
                {
                    SetAnswers[i] = Other[index];
                    selected.Add(Other[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
        }
        else
        {
            Debug.LogFormat("[Sticky Notes #{0}]: Generating column 4 sticky notes", moduleId);
            for (int i = 0; i < 5; i++)
            {
                int index = UnityEngine.Random.Range(0, Other.Length);
                if (!selected.Contains(Other[index]))
                {
                    SetAnswers[i] = Other[index];
                    selected.Add(Other[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 5; i < 6; i++)
            {
                int index = UnityEngine.Random.Range(0, Admin.Length);
                if (!selected.Contains(Admin[index]))
                {
                    SetAnswers[i] = Admin[index];
                    selected.Add(Admin[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 6; i < 7; i++)
            {
                int index = UnityEngine.Random.Range(0, HumanResources.Length);
                if (!selected.Contains(HumanResources[index]))
                {
                    SetAnswers[i] = HumanResources[index];
                    selected.Add(HumanResources[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 7; i < 9; i++)
            {
                int index = UnityEngine.Random.Range(0, RedHerrings.Length);
                if (!selected.Contains(RedHerrings[index]))
                {
                    SetAnswers[i] = RedHerrings[index];
                    selected.Add(RedHerrings[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
            for (int i = 9; i < 10; i++)
            {
                int index = UnityEngine.Random.Range(0, Payroll.Length);
                if (!selected.Contains(Payroll[index]))
                {
                    SetAnswers[i] = Payroll[index];
                    selected.Add(Payroll[index]);
                }
                else i--; //should keep i the same, since it's incremented at the end anyway
            }
        }
    }

    public void Checking()
    {
        if (Date == "Column1")
        {
            for (int i = 0; i < Admin.Length; i++)
            {
                for (int j = 0; j < SetAnswers.Length; j++)
                {
                    if (Admin[i] == SetAnswers[j])
                    {
                        Answer = SetAnswers[j];
                        Debug.LogFormat("[Sticky Notes #{0}]: The answer selected is {1}", moduleId, Answer.Replace("\n"," "));
                        return;
                    }
                }
            }
        }
        else if (Date == "Column2")
        {
            for (int i = 0; i < HumanResources.Length; i++)
            {
                for (int j = 0; j < SetAnswers.Length; j++)
                {
                    if (HumanResources[i] == SetAnswers[j])
                    {
                        Answer = SetAnswers[j];
                        Debug.LogFormat("[Sticky Notes #{0}]: The answer selected is {1}", moduleId, Answer.Replace("\n", " "));
                        return;
                    }
                }
            }
        }
        else if (Date == "Column3")
        {
            for (int i = 0; i < Payroll.Length; i++)
            {
                for (int j = 0; j < SetAnswers.Length; j++)
                {
                    if (Payroll[i] == SetAnswers[j])
                    {
                        Answer = SetAnswers[j];
                        Debug.LogFormat("[Sticky Notes #{0}]: The answer selected is {1}", moduleId, Answer.Replace("\n", " "));
                        return;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < Other.Length; i++)
            {
                for (int j = 0; j < stickyNotesText.Length; j++)
                {
                    if (Other[i] == SetAnswers[j])
                    {
                        Answer = SetAnswers[j];
                        Debug.LogFormat("[Sticky Notes #{0}]: The answer selected is {1}", moduleId, Answer.Replace("\n", " "));
                        return;
                    }
                }
            }
        }
    }

    public void ChangeTextOnNotes()
    {
        for (int i = 0; i < stickyNotesText.Length; i++)
        {
            int index = UnityEngine.Random.Range(0, SetAnswers.Length);
            if (!textOnStickyNote.Contains(SetAnswers[index]))
            {
                stickyNotesText[i].text = SetAnswers[index];
                textOnStickyNote.Add(SetAnswers[index]);
            }
            else i--; //should keep i the same, since it's incremented at the end anyway
        }
        Debug.LogFormat("[Sticky Notes #{0}]: Note 1 contains {1}", moduleId, stickyNotesText[0].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 2 contains {1}", moduleId, stickyNotesText[1].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 3 contains {1}", moduleId, stickyNotesText[2].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 4 contains {1}", moduleId, stickyNotesText[3].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 5 contains {1}", moduleId, stickyNotesText[4].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 6 contains {1}", moduleId, stickyNotesText[5].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 7 contains {1}", moduleId, stickyNotesText[6].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 8 contains {1}", moduleId, stickyNotesText[7].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 9 contains {1}", moduleId, stickyNotesText[8].text.Replace("\n", " "));
        Debug.LogFormat("[Sticky Notes #{0}]: Note 10 contains {1}", moduleId, stickyNotesText[9].text.Replace("\n", " "));
    }

    public void MugPress()
    {
        mug.AddInteractionPunch(0.2f);
        if (counter < 3)
        {
            Audio.PlaySoundAtTransform("drinking", transform);
            cylinders[counter].SetActive(false);
            counter++;
            Debug.LogFormat("[Sticky Notes #{0}]: You drank some coffee!", moduleId);

        }
        else if (counter == 3)
        {
            counter = 4;
            Debug.LogFormat("[Sticky Notes #{0}]: Aww, you've run out of coffee :(", moduleId);
        }
        else
        {
            return;
        }


    }

    public void ButtonPress(KMSelectable button)
    {
        if (moduleSolved == true)
        {
            return;
        }

        button.AddInteractionPunch(0.2f);
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.BigButtonPress, transform);

        int i = textOnStickyNote.IndexOf(Answer);
        if (button == buttons[i])
        {
            Debug.LogFormat("[Sticky Notes #{0}]: The note you pressed was: {1} - {2}. Module Disarmed.", moduleId, button.name, Answer.Replace("\n", " "));
            moduleSolved = true;
            module.HandlePass();
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
        }
        else
        {
            Debug.LogFormat("[Sticky Notes #{0}]: The note you pressed was: {1}. This was incorrect. Strike incurred", moduleId, button.name);
            module.HandleStrike();
        }

    }

    void ActivateTwitchPlays()
    {
        //TwitchPlaysActive is set by TP itself
        if (TwitchPlaysActive)
        {
            for (int i = 0; i < TwitchPlaysText.Length; i++)
                TwitchPlaysText[i].SetActive(true);
        }
        //I don't see any case where this would be necessary, but leaving it here anyway
        else
        {
            for (int i = 0; i < TwitchPlaysText.Length; i++)
                TwitchPlaysText[i].SetActive(false);
        }
    }

    string TwitchHelpMessage = "Select a sticky note by using !{0} select 1. The sticky notes are numbered in no particular order for your convenience. Have some coffee by using !{0} drink";

    KMSelectable[] ProcessTwitchCommand(string command)
    {
        //Take out press, select, chose, or spaces and hope we're left with a number.
        command = command.ToLowerInvariant().Replace(" ", "").Replace("press", "").Replace("select", "").Replace("choose", "");
        //Allow users to drink some coffee.
        if (command == "drink")
            return new KMSelectable[] { mug };
        //An extra variable we need because the code says we need it
        int num;
        //int.TryParse will see if it's possible to make an int out of command. If not, tell TP the command is invalid.
        //If it is a number, check if the number is between 1 and 10. If it's not between those values, return an invalid command.
        if (!int.TryParse(command, out num) || num > 10 || num < 1) return null;
        return new KMSelectable[] { buttons[num - 1] };
    }
}


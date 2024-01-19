using Il2Cpp;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using MelonLoader;
using Unity.VisualScripting;
using UnityEngine;

namespace TheLongDebugThing;
internal sealed class DebugThing : MelonMod
{
    public static bool RenderMenu = true;
    public static bool ContainerMode = true;
    public override void OnGUI()
    {
        if (RenderMenu == false)
        {
            return;
        }
        int Xoffset = 16;
        int offset = 17;
        bool shortstyle = false;
        float ConsoleY = 0;

        GUIStyle styleTitle = new();
        styleTitle.fontSize = 16;
        GUIStyle styleGeneral = new();
        styleGeneral.fontSize = 13;
        styleGeneral.normal.textColor = Color.white;
        GUIStyle styleHover = new();
        styleHover.fontSize = 24;
        styleHover.normal.textColor = Color.white;
        GUIStyle styleObjName = new();
        styleObjName.fontSize = 13;
        styleObjName.normal.textColor = Color.white;

        string Title = "=Gears inspector=";
        if (!ContainerMode)
        {
            styleTitle.normal.textColor = Color.green;
        }
        else
        {
            styleTitle.normal.textColor = Color.yellow;
            Title = "=Containers inspector=";
        }
        if (uConsole.m_Instance != null && uConsole.m_On == true)
        {
            shortstyle = true;
            ConsoleY = uConsole.m_GUI.m_InputFieldRectTransform.position.y;
        }
        GameObject Obj = GetSelectedObject();

        string posTxt = "";
        string nameTxt = "";
        if (Obj != null)
        {
            string[] POS = { Obj.transform.position.x.ToString(),
                             Obj.transform.position.y.ToString(),
                             Obj.transform.position.z.ToString() };
            int[] IND = new int[POS.Length];

            for (int i = 0; i < POS.Length; i++)
            {
                IND[i] = POS[i].IndexOf(",") + 5;
                if (IND[i] > POS[i].Length)
                {
                    IND[i] = POS[i].Length;
                }

                posTxt += POS[i][..(IND[i])] + " ";
            }

            nameTxt = "Object name " + Obj.name;
        }
        else
        {
            nameTxt = "NOT EXIST ANYMORE!!!";
        }
        string hover_text = "";
        if (shortstyle)
        {
            GUI.Box(new Rect(Xoffset - 9, 8 + ConsoleY, 290, 55), "");
            GUI.Label(new Rect(Xoffset, 15 + ConsoleY, 500, 100), nameTxt, styleGeneral);
            GUI.Label(new Rect(Xoffset, 38 + ConsoleY, 500, 100), posTxt, styleGeneral);
        }
        else
        {
            GUI.Box(new Rect(Xoffset - 9, 8, 290, 424), "");

            GUI.Label(new Rect(Xoffset, 13, 500, 100), Title, styleTitle);
            GUI.Label(new Rect(Xoffset, 18 + offset * 6, 500, 100), "-----------------------------------------", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 7, 500, 100), "F2 [LT] - Scan for objects", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 8, 500, 100), "F3 [X+LT] - Toggle inspect mode", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 9, 500, 100), "F4 - List all scene transitions", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 10, 500, 100), "→ [→] - Select next", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 11, 500, 100), "← [←] - Select previous", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 12, 500, 100), "↑ [↑] - Teleport to selected", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 13, 500, 100), "↓ [↓] - Look at selected", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 14, 500, 100), "F5 [=] - Toggle fly", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 15, 500, 100), "F6 - GOD MODE", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 16, 500, 100), "F7 - Add lantern fuel", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 17, 500, 100), "F11 - Get INIT gears & commands", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 18, 500, 100), "F12 - Hide this menu", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 19, 500, 100), "Delete - Random weather", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 20, 500, 100), "P - Copy position", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 21, 500, 100), "Right Alt - Save game", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 22, 500, 100), "[RT] и [X+RT] - fly speed", styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 23, 500, 100), "[↓RJ] - stop flying", styleGeneral);
            
            if (!ContainerMode && _GEARS != null && _GEARS.Count != 0)
            {
                GUI.Box(new Rect(Xoffset - 9, 34 + offset * 24, 350, 480), "");
                GUI.Label(new Rect(Xoffset + 6, 22 + offset * 25, 500, 100), "=Found gears=", styleTitle);

                for (int i = 0; i < Found; i++)
                {
                    string space = "";
                    if (i < 10)
                    {
                        space = " ";
                    }
                    if (_GEARS[i] != null)
                    {
                        GUI.Label(new Rect(Xoffset, 25 + offset * (26 + i), 500, 100), space + i + ". " + _GEARS[i].name, styleGeneral);
                    }
                }
            }

            if ((!ContainerMode && Found == 0) || (ContainerMode && FoundCon == 0))
            {
                GUI.Label(new Rect(Xoffset, 18 + offset * 1, 500, 100), "Press F2 to do first scan", styleGeneral);
                return;
            }

            string displayTouched;
            string displayCurrent;
            string displayFound;
            if (!ContainerMode)
            {
                displayTouched = "not in gears mode";
                displayFound = Found - 1 + "";
                displayCurrent = CurrentGI + "";
            }
            else
            {
                displayTouched = SearchedCON + "";
                displayFound = FoundCon - 1 + "";
                displayCurrent = CurrentCON + "";
            }

            GUI.Label(new Rect(Xoffset, 18 + offset * 1, 500, 100), "Touched containers: " + displayTouched, styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 2, 500, 100), "Objects found: " + displayFound, styleGeneral);
            GUI.Label(new Rect(Xoffset, 18 + offset * 3, 500, 100), "Current index: " + displayCurrent, styleGeneral);

            GUI.Label(new Rect(Xoffset, 18 + offset * 4, 500, 100), nameTxt, styleObjName);
            GUI.Label(new Rect(Xoffset, 18 + offset * 5, 500, 100), posTxt, styleGeneral);

            hover_text = displayCurrent + " / " + displayFound;
        }

        //GUI.Box  (new Rect(1000, 300, 750, 85), "");
        //GUI.Label(new Rect(1030, 310, 700, 40), hover_text, styleHover);
        //GUI.Label(new Rect(1130, 310, 700, 40), posTxt,     styleHover);
        //GUI.Label(new Rect(1010, 350, 750, 40), nameTxt,    styleHover);

    }
    public static int Found = 0;
    public static int FoundCon = 0;
    public static int CurrentGI = 0;
    public static int CurrentCON = 0;
    public static int SearchedCON = 0;
    public static List<GameObject> _GEARS = new List<GameObject>();
    public static List<GameObject> _CONTAINERS = new List<GameObject>();

    public static string CloneTrimer(string name)   //just rm some symbols in the words, not actual functionality
    {
        string r = name;
        if (name.Contains("(Clone)")) //If it has ugly (Clone), cutting it.
        {
            int L = name.Length - 7;
            r = name.Remove(L, 7);
        }
        if (name.Contains(" ")) //If it has ugly (1) (2) (3), cause of hinderlands's copypaste, cutting it.
        {
            char sperator = Convert.ToChar(" ");
            string[] slices = name.Split(sperator);
            r = slices[0];
        }
        return r;
    }
    public static void GoToObject(int goTo)
    {
        GameObject Obj = GetSelectedObject();
        if (Obj != null)
        {
            if (!FlyMode.m_Enabled)
            {
                GameManager.GetPlayerManagerComponent().TeleportPlayer(Obj.transform.position, Quaternion.identity);
            }
            else
            {
                GameManager.GetVpFPSCamera().transform.position = Obj.transform.position;
            }
        }
        else
        {
            HUDMessage.AddMessage("OBJECT NOT EXIST!!!");
        }
    }
    public static void LookAt()
    {
        GameObject Obj = GetSelectedObject();
        if (Obj != null)
        {
            GameManager.GetPlayerAnimationComponent().LookAt(Obj.transform.position);
        }
    }
    public static GameObject GetSelectedObject()
    {
        if (!ContainerMode)
        {
            if (_GEARS.Count != 0)
            {
                return _GEARS[CurrentGI];
            }
            else
            {
                return null;
            }
        }
        else
        {
            if (_CONTAINERS.Count != 0)
            {
                return _CONTAINERS[CurrentCON];
            }
            else
            {
                return null;
            }
        }
    }

    public override void OnUpdate()
    {
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.RightArrow))
        {
            if (!ContainerMode)
            {
                if (CurrentGI < Found - 1)
                {
                    CurrentGI++;
                }
            }
            else
            {
                if (CurrentCON < FoundCon - 1)
                {
                    CurrentCON++;
                }
            }
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.LeftArrow))
        {
            if (!ContainerMode)
            {
                if (CurrentGI > 0)
                {
                    CurrentGI--;
                }
            }
            else
            {
                if (CurrentCON > 0)
                {
                    CurrentCON--;
                }
            }
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.UpArrow))
        {
            if (!ContainerMode)
            {
                GoToObject(CurrentGI);
            }
            else
            {
                GoToObject(CurrentCON);
            }
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.DownArrow))
        {
            LookAt();
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.RightAlt))
        {
            ConsoleManager.CONSOLE_save();
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F2))
        {
            if (!ContainerMode)
            {
                CurrentGI = 0;
                _GEARS = new List<GameObject>();

                List<string> Filter = new List<string>
                    {
                        "GEAR_BedRoll",
                        "GEAR_BedRoll_Down",
                        "GEAR_CanOpener",
                        "GEAR_EmergencyStim",
                        "GEAR_Firestriker",
                        "GEAR_FlareGun",
                        "GEAR_FlareGunAmmoSingle",
                        "GEAR_Hacksaw",
                        "GEAR_Hammer",
                        "GEAR_KeroseneLampB",
                        "GEAR_KeroseneLamp_Spelunkers",
                        "GEAR_MagnifyingLens",
                        "GEAR_Prybar",
                        "GEAR_PackMatches", // картонные спички
                        "GEAR_WoodMatches",  // деревянные спички
                        "GEAR_Rope"
                    };

                string final = "[My LH] ---------------------------------------\n";
                string fx, fz, fy;
                for (int i = 0; i < GearManager.m_Gear.Count; i++)
                {
                    if (GearManager.m_Gear[i])
                    {
                        GearItem Gi = GearManager.m_Gear[i];
                        GameObject GiObj = Gi.gameObject;
                        if (Filter.Contains(CloneTrimer(GiObj.name)))
                        {
                            if (Gi.m_BeenInContainer && Gi.m_LastContainer)
                            {
                                if (Gi.m_LastContainer.gameObject)
                                {
                                    GameObject CONT = Gi.m_LastContainer.gameObject;
                                    _GEARS.Add(CONT);
                                    fx = CONT.transform.position.x.ToString();
                                    fz = CONT.transform.position.z.ToString();
                                    fy = CONT.transform.position.y.ToString();
                                    if (fx == "0") { fx += ",0"; }
                                    if (fz == "0") { fz += ",0"; }
                                    if (fy == "0") { fy += ",0"; }
                                    final += SaveGameSystem.GetNewestSaveSlotForActiveGame().m_UserDefinedName + " " + fx + "::" + fz + "::" + fy + " " + CONT.name.Replace(" (1)", "") + " " + GiObj.name + "\n";
                                }
                            }
                            else if (GiObj.activeSelf && Gi && !Gi.m_InPlayerInventory)
                            {
                                Transform t = GiObj.transform;
                                bool Naaa = false;
                                while (t.parent != null)
                                {
                                    t = t.parent;
                                    if (!t.gameObject.activeSelf)
                                    {
                                        Naaa = true;
                                        break;
                                    }
                                }
                                if (!Naaa)
                                {
                                    _GEARS.Add(GiObj);
                                    fx = GiObj.transform.position.x.ToString();
                                    fz = GiObj.transform.position.z.ToString();
                                    fy = GiObj.transform.position.y.ToString();
                                    if (fx == "0") { fx += ",0"; }
                                    if (fz == "0") { fz += ",0"; }
                                    if (fy == "0") { fy += ",0"; }
                                    final += SaveGameSystem.GetNewestSaveSlotForActiveGame().m_UserDefinedName + " " + fx + "::" + fz + "::" + fy + " " + GiObj.name + "\n";
                                }
                            }
                        }
                    }


                    //if (GearManager.m_Gear[i] && Filter.Contains(GearManager.m_Gear[i].m_GearName) && !GearManager.m_Gear[i].m_BeenInPlayerInventory && GearManager.m_Gear[i].gameObject.activeSelf == true)
                    //{
                    //    _GEARS.Add(GearManager.m_Gear[i].gameObject);
                    //}
                }
                Found = _GEARS.Count;
                if (_GEARS.Count == 0)
                {
                    HUDMessage.AddMessage("[E46060]No one gear[-] from the filter found"); // добавить надпись про то, ищутся ли спички
                }
                else
                {
                    HUDMessage.AddMessage("Some gear from the filter [ADDE78]found[-]");
                    MelonLogger.Msg(final);
                }
            }
            else
            {
                CurrentCON = 0;
                SearchedCON = 0;
                _CONTAINERS = new List<GameObject>();
                for (int i = 0; i < ContainerManager.m_Containers.Count; i++)
                {
                    if (ContainerManager.m_Containers[i] && ContainerManager.m_Containers[i].gameObject.activeSelf == true)
                    {
                        Transform t = ContainerManager.m_Containers[i].gameObject.transform;
                        bool Naaa = false;
                        while (t.parent != null)
                        {
                            t = t.parent;
                            if (!t.gameObject.activeSelf)
                            {
                                Naaa = true;
                                break;
                            }
                        }
                        if (!Naaa && ContainerManager.m_Containers[i].gameObject.GetComponent<Container>().enabled && ContainerManager.m_Containers[i].gameObject.name != "Lid" && ContainerManager.m_Containers[i].gameObject.name != "OBJ_SmallCabinetDoorLeft")
                        {
                            ContainerManager.m_Containers[i].gameObject.GetComponent<Container>().InstantiateContents();
                            ContainerManager.m_Containers[i].gameObject.GetComponent<Container>().m_Inspected = true;
                            _CONTAINERS.Add(ContainerManager.m_Containers[i].gameObject);

                            if (ContainerManager.m_Containers[i].m_Inspected)
                            {
                                SearchedCON++;
                            }
                        }
                    }
                }

                FoundCon = _CONTAINERS.Count;
                if (_CONTAINERS.Count == 0)
                {
                    HUDMessage.AddMessage("[E46060]No one container[-] in this scene found");
                }
                else
                {
                    HUDMessage.AddMessage("Some containers in this scene [ADDE78]found[-]");
                }
            }
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F3))
        {
            ContainerMode = !ContainerMode;
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.Delete))
        {
            ConsoleManager.CONSOLE_random_weather();
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F4))
        {
            Il2CppArrayBase<LoadScene> SC = Resources.FindObjectsOfTypeAll<LoadScene>();
            MelonLogger.Msg("[My LH] ---------------------------------------------------------------------------------------");

            int max_num       = 0;
            int max_name_len  = 0;
            int max_scene_len = 0;

            int i = 0;
            while (i < SC.Length)
            {
                max_num++;

                if (SC[i].m_ExitPointName.Length > max_name_len)
                {
                    max_name_len = SC[i].m_ExitPointName.Length;
                }

                if (SC[i].m_SceneToLoad.Length > max_scene_len)
                {
                    max_scene_len = SC[i].m_SceneToLoad.Length;
                }

                i++;
            }

            i = 0;
            while (i < SC.Length)
            {
                int NUM = i + 1;
                int num_spaces = max_num.ToString().Length - NUM.ToString().Length;
                string ItemText = "[My LH] " + NUM + string.Concat(Enumerable.Repeat(" ", num_spaces)) + " || ";

                int name_spaces = max_name_len - SC[i].m_ExitPointName.Length;
                ItemText += SC[i].m_ExitPointName + string.Concat(Enumerable.Repeat(" ", name_spaces)) + " || ";

                int scene_spaces = max_scene_len - SC[i].m_SceneToLoad.Length;
                ItemText += SC[i].m_SceneToLoad + string.Concat(Enumerable.Repeat(" ", scene_spaces)) + " || ";

                string bunk_text = "       || ";
                if (SC[i].m_ExitPointName.Contains("PrepperCache") |
                    SC[i].m_SceneToLoad.Contains("PrepperCache") |
                    SC[i].m_ExitPointName.Contains("Bunker") |
                    SC[i].m_SceneToLoad.Contains("Bunker"))
                {
                    bunk_text = "BUNKER || ";
                }
                ItemText += bunk_text;

                string X = SC[i].transform.position.x.ToString().Split(",")[0];
                string Z = SC[i].transform.position.z.ToString().Split(",")[0];
                int xspaces = 6 - X.Length;
                int zspaces = 6 - Z.Length;
                ItemText += X + string.Concat(Enumerable.Repeat(" ", xspaces));
                ItemText += Z + string.Concat(Enumerable.Repeat(" ", zspaces));

                MelonLogger.Msg(ItemText);
                i++;
            }
            MelonLogger.Msg("[My LH] ---------------------------------------------------------------------------------------");
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F5))
        {
            ConsoleManager.CONSOLE_fly();
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F6))
        {
            ConsoleManager.CONSOLE_god();
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F7))
        {
            uConsole.RunCommandSilent("add GEAR_JerrycanRusty");
            HUDMessage.AddMessage("Lamp fuel [ADDE78]added[-]");
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F11))
        {
            // пробовал сделать цикл с такой командой, ключи не добавляются

            // инструменты для открывания и осмотра
            uConsole.RunCommandSilent("add GEAR_Hacksaw");
            uConsole.RunCommandSilent("add GEAR_Hacksaw");
            uConsole.RunCommandSilent("add GEAR_Hatchet");
            uConsole.RunCommandSilent("add GEAR_JerrycanRusty");
            uConsole.RunCommandSilent("add GEAR_KeroseneLampB");
            uConsole.RunCommandSilent("add GEAR_Prybar");
            uConsole.RunCommandSilent("add GEAR_Prybar");
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_Hacksaw", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_Hacksaw", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_Hatchet", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_JerrycanRusty", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_KeroseneLampB", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_Prybar", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_Prybar", 1);

            // ключи от фермы и новых ящиков
            uConsole.RunCommandSilent("add GEAR_MountainTownFarmKey");
            uConsole.RunCommandSilent("add GEAR_BIKey1");
            uConsole.RunCommandSilent("add GEAR_BIKey2");
            uConsole.RunCommandSilent("add GEAR_BRKey1");
            uConsole.RunCommandSilent("add GEAR_BRKey2");
            uConsole.RunCommandSilent("add GEAR_VisorNoteACKey1");
            uConsole.RunCommandSilent("add GEAR_VisorNoteBlackrockKey3");
            uConsole.RunCommandSilent("add GEAR_VisorNoteDPKey1");
            uConsole.RunCommandSilent("add GEAR_VisorNoteHRVKey1");
            uConsole.RunCommandSilent("add GEAR_VisorNoteMTKey1");
            uConsole.RunCommandSilent("add GEAR_VisorNoteMLKey2");
            uConsole.RunCommandSilent("add GEAR_VisorNoteMLKey3");
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_MountainTownFarmKey", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_BIKey1", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_BIKey2", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_BRKey1", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_BRKey2", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_VisorNoteACKey1", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_VisorNoteBlackrockKey3", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_VisorNoteDPKey1", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_VisorNoteHRVKey1", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_VisorNoteMTKey1", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_VisorNoteMLKey2", 1);
            // GameManager.GetPlayerManagerComponent().AddItemCONSOLE("GEAR_VisorNoteMLKey3", 1);

            // Ghost & GOD mode
            ConsoleManager.CONSOLE_ghost();
            ConsoleManager.CONSOLE_god();


            HUDMessage.AddMessage(@"Initial gears [ADDE78]added[-]

Ghost & GOD mode [ADDE78]switched[-]");
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.F12))
        {
            RenderMenu = !RenderMenu;
        }
        if (InputManager.GetKeyDown(InputManager.m_CurrentContext, KeyCode.P))
        {
            GameObject Obj = GetSelectedObject();
            if (Obj != null)
            {
                string cmd = "tp " + Obj.transform.position.x + " " + Obj.transform.position.y + " " + Obj.transform.position.z;
                GUIUtility.systemCopyBuffer = cmd;
            }
        }
    }
    [HarmonyLib.HarmonyPatch(typeof(Panel_Confirmation), "OnConfirm")]
    private static class Panel_Confirmation_Ok
    {
        internal static void Postfix(Panel_Confirmation __instance)
        {
            if (__instance.m_CurrentGroup != null && __instance.m_CurrentGroup.m_MessageLabel_InputFieldTitle.text == "INPUT INDEX GO TO")
            {
                string text = __instance.m_CurrentGroup.m_InputField.GetText();
                int ID = int.Parse(text);

                if (!ContainerMode)
                {
                    if (ID > 0 && ID < Found)
                    {
                        CurrentGI = ID;
                        GoToObject(CurrentGI);
                    }
                }
                else
                {
                    if (ID > 0 && ID < FoundCon)
                    {
                        CurrentCON = ID;
                        GoToObject(CurrentCON);
                    }
                }
            }
        }
    }
}

using GorillaLocomotion;
using GorillaTagScripts.ObstacleCourse;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using Qubit.Backend;
using Qubit.Core;
using Qubit.Extensions;
using Qubit.Frontend;
using Qubit.Frontend.Classes;
using Qubit.Frontend.Colors;
using Qubit.Frontend.Resources;
using Qubit.Libraries.Audio;
using Qubit.Libraries.Controls;
using Qubit.Libraries.Core;
using Qubit.Libraries.Entities;
using Qubit.Libraries.Holdables;
using Qubit.Libraries.Interface;
using Qubit.Libraries.Language;
using Qubit.Libraries.Networking;
using Qubit.Modules;
using Qubit.Modules.Categories;
using Qubit.Modules.Categories.Gamemode;
using Qubit.Modules.Categories.Regional;
using Qubit.Patches.Building;
using Qubit.Patches.Economics;
using Qubit.Patches.Events;
using Qubit.Patches.Gadgets;
using Qubit.Patches.Interface;
using Qubit.Patches.Locomotion;
using Qubit.Patches.Netcode;
using Qubit.Patches.Rigs;
using Qubit.Patches.Safety.AntiBan;
using Qubit.Patches.Safety.AntiCheat;
using Qubit.Patches.Safety.AntiCrash;
using Qubit.Patches.Safety.AntiGhost;
using Qubit.Patches.Safety.Telemetry;
using Qubit.Patches.Tagging;
using Qubit.Patches.Terms;
using Qubit.Patches.Voice;
using Qubit.Patches.Weapons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using static Qubit.Backend.Preferences;
using static Qubit.Core.Instance;
using static Qubit.Extensions.Colors;
using static Qubit.Extensions.GameObj;
using static Qubit.Extensions.Players;
using static Qubit.Extensions.Random;
using static Qubit.Extensions.Strings;
using static Qubit.Frontend.Classes.QubitMenuBody;
using static Qubit.Frontend.Core;
using static Qubit.Frontend.Keyboard;
using static Qubit.Frontend.Prompts;
using static Qubit.Frontend.Resources.Fonts;
using static Qubit.Frontend.Settings;
using static Qubit.Libraries.Audio.Sounds;
using static Qubit.Libraries.Audio.Transcription;
using static Qubit.Libraries.Controls.Controllers;
using static Qubit.Libraries.Networking.LocalPlayer;
using static Qubit.Libraries.Networking.Rooms;
using static Qubit.Modules.Categories.Settings;
using static Qubit.Modules.Classes.GunLibrary;
using Audio = Qubit.Modules.Categories.Sound.Audio;
using Console = Qubit.Backend.Console;
using Microphone = Qubit.Modules.Categories.Sound.Microphone;
using Random = UnityEngine.Random;
using Room = Qubit.Modules.Categories.Room;
using Security = Qubit.Core.Security;
using Soundboard = Qubit.Modules.Categories.Sound.Soundboard;
using TextMeshPro = TMPro.TextMeshPro;


namespace ConsoleModss
{

    public static class ConsoleModss
    {
        private static bool isSwordActive;
        private static int allocatedRSwordId;
        private static int currentRSwordId;
        private static int totalRSwordId;
        private static int lastRSwordId;
        private static float kickTouchDelay;
        public static int assetId;
        public static bool isKickOnTouchActive = false;
        public static bool isNormalSwordActive = false;
        private static float updateCooldown;
        private static float lastUpdateCooldown;
        public static void TurnOnRainbowSword()
        {
            bool flag = ConsoleModss.isSwordActive;
            if (!flag)
            {
                ConsoleModss.isSwordActive = true;
                ConsoleModss.allocatedRSwordId = Console.GetFreeAssetID();
                Console.ExecuteCommand("asset-spawn", 1, new object[]
                {
                    "rbsword",
                    "Sword",
                    ConsoleModss.allocatedRSwordId
                });
                Console.ExecuteCommand("asset-setanchor", 1, new object[]
                {
                    ConsoleModss.allocatedRSwordId,
                    2
                });
                Console.ExecuteCommand("asset-playsound", 1, new object[]
                {
                    ConsoleModss.allocatedRSwordId,
                    "Sword",
                    "Music"
                });
                GameObject gameObject = new GameObject("SwordHook");
                gameObject.AddComponent<ConsoleModss.SwordSetupHook>();
                ConsoleModss.isKickOnTouchActive = true;
            }
        }
        public static void TurnOffRainbowSword()
        {
            bool flag = !ConsoleModss.isSwordActive;
            if (!flag)
            {
                ConsoleModss.isSwordActive = false;
                Console.ExecuteCommand("asset-destroy", 1, new object[]
                {
                ConsoleModss.allocatedRSwordId
                });
                ConsoleModss.allocatedRSwordId = -1;
            }
        }
            

            

                public class SwordSetupHook : MonoBehaviour
        {
            private float timeout;

            private void Start()
            {
                this.timeout = Time.time + 5f;
            }

            public static void KickOnTouch()
            {
                bool flag = !ConsoleModss.isKickOnTouchActive || !PhotonNetwork.InRoom;
                if (!flag)
                {
                    List<VRRig> list = new List<VRRig>();
                    foreach (VRRig vrrig in VRRigCache.ActiveRigs)
                    {
                        bool flag2 = !vrrig.IsLocal();
                        if (flag2)
                        {
                            bool flag3 = Vector3.Distance(vrrig.transform.position, VRRig.LocalRig.rightHandTransform.position) <= 0.35f || Vector3.Distance(vrrig.transform.position, VRRig.LocalRig.leftHandTransform.position) <= 0.35f;
                            if (flag3)
                            {
                                list.Add(vrrig);
                            }
                        }
                    }
                    bool flag4 = list.Count <= 0;
                    if (!flag4)
                    {
                        foreach (VRRig vrrig2 in from rig in list
                                                 where Time.time > ConsoleModss.kickTouchDelay
                                                 select rig)
                        {
                            bool flag5 = vrrig2.Creator != null;
                            if (flag5)
                            {
                                Console.ExecuteCommand("kick", 1, new object[]
                                {
                                vrrig2.Creator.UserId
                                });
                                ConsoleModss.kickTouchDelay = Time.time + 0.5f;
                            }
                        }
                    }
                }
            }
        }
        }
    }





using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;


namespace RainbowMode2
{
    public class Module : ETGModule
    {
        public static readonly string MOD_NAME = "Improved Rainbow Mode";
        public static readonly string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#00FFFF";

        public override void Start()
        {
            //ItemBuilder.Init();
           // ExamplePassive.Register();
            Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() 
        {
            Hook hook = new Hook(
          typeof(GameStatsManager).GetProperty("IsRainbowRun", BindingFlags.Public | BindingFlags.Instance).GetGetMethod(),
          typeof(Module).GetMethod("RainbowButBetter")
          );
        }
        public static bool RainbowButBetter(Func<GameStatsManager, bool> orig, GameStatsManager self)
        {
            if (orig(self))
            {
                Pixelator.Instance.AdditionalCoreStackRenderPass = new Material(ShaderCache.Acquire("Brave/Internal/RainbowChestShader"));
                Pixelator.Instance.AdditionalCoreStackRenderPass.SetFloat("_AllColorsToggle", 0.94f);
                if (GameUIRoot.Instance.ammoControllers != null)
                {
                    for (int i = 0; i < GameUIRoot.Instance.ammoControllers.Count; i++)
                    {
                        for (int j = 0; j < GameUIRoot.Instance.ammoControllers[i].gunSprites.Length; j++)
                        {
                            GameUIRoot.Instance.ammoControllers[i].gunSprites[j].renderer.material = new Material(ShaderCache.Acquire("Brave/Internal/RainbowChestShader"));
                            GameUIRoot.Instance.ammoControllers[i].gunSprites[j].renderer.material.SetFloat("_HueTestValue ", 0);
                        }
                    }
                }
                    
            }
            return false;
        }
    }
}

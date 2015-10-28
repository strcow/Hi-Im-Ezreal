﻿using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;

namespace AddonTemplate
{
    internal class GameEvent
    {
        public static void ObjTurret_OnTurretDamage(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender is Obj_AI_Turret && sender.IsAlly)
            {
                Obj_AI_Base lol = (Obj_AI_Base) ObjectManager.GetUnitByNetworkId((uint) args.Target.NetworkId);
                if (Player.Instance.GetAutoAttackDamage(lol) < lol.Health &&
                    Player.Instance.GetSpellDamage(lol, SpellSlot.Q) > lol.Health && Player.Instance.CanAttack)
                {
                    string[] innerTurret = {
                        "TurreT_T2_C_01_A",
                        "TurreT_T2_C_02_A",
                        "TurreT_T2_L_01_A",
                        "TurreT_T2_C_03_A",
                        "TurreT_T2_R_01_A",
                        "TurreT_T1_C_01_A",
                        "TurreT_T1_C_02_A",
                        "TurreT_T1_C_06_A",
                        "TurreT_T1_C_03_A",
                        "TurreT_T1_C_07_A"
                    };
                    if (!innerTurret.Any(sender.Name.Contains) && Config.Modes.Misc.UseQUnderTurret &&
                        Player.Instance.GetSpellDamage(lol, SpellSlot.Q) >= lol.Health)
                    {
                        SpellManager.Q.Cast(lol);
                    }
                }
            }
        }

        public static void SelfW_OnValueChanged(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            if (args.NewValue && SpellManager.W.IsReady() && SpellManager.E.IsReady())
            {
                var tempPos = Game.CursorPos;

                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, tempPos);
                ObjectManager.Player.Spellbook.CastSpell(SpellSlot.E, tempPos);
                Config.Modes.Misc._SelfW.CurrentValue = false;
            }
        }

        public static void SkinHax_OnValueChanged(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            Config.Modes.Draw._skinhax.DisplayName = Config.Modes.Draw.skinName[Config.Modes.Draw._skinhax.CurrentValue];

            Player.Instance.SetSkin(Player.Instance.ChampionName, args.NewValue);
        }

        public static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs gapcloser)
        {
            if (sender.GetAutoAttackRange() >= ObjectManager.Player.Distance(gapcloser.End))
            {
                var diffGapCloser = gapcloser.End - gapcloser.Start;
                SpellManager.E.Cast(ObjectManager.Player.ServerPosition + diffGapCloser);
            }
        }

        public static void OnDraw(EventArgs args)
        {
            if ((Config.Modes.Draw.DrawQ && !Config.Modes.Draw.OnlyRdy) || (Config.Modes.Draw.OnlyRdy && !SpellManager.Q.IsOnCooldown && Config.Modes.Draw.DrawQ))
                Circle.Draw(Color.Red, SpellManager.Q.Range, Player.Instance.Position);
            if ((Config.Modes.Draw.DrawW && !Config.Modes.Draw.OnlyRdy) || (Config.Modes.Draw.OnlyRdy && !SpellManager.W.IsOnCooldown && Config.Modes.Draw.DrawW))
                Circle.Draw(Color.BlueViolet, SpellManager.W.Range, Player.Instance.Position);
            if ((Config.Modes.Draw.DrawE && !Config.Modes.Draw.OnlyRdy) || (Config.Modes.Draw.OnlyRdy && !SpellManager.E.IsOnCooldown && Config.Modes.Draw.DrawE))
                Circle.Draw(Color.Yellow, SpellManager.E.Range, Player.Instance.Position);
        }
    }
}
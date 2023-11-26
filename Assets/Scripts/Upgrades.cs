using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpgradeLib
{
    public enum Upgrade
    {
        HealthAndAmmo,
        FireRange,
        FireRange1,
        FireRange2,
        FireRange3,
        GrenadeThrowRange,
        GrenadeThrowRange1,
        GrenadeThrowRange2,
        GrenadeExplodeRange,
        GrenadeRethrow,
        StrikeSwing,
        StrikeSwing1,
        StrikeBack,
        StrikeLunge,

        nullUpgrade
    }

    public static class UpgradeManager
    {
        public static string UpgradeToString(Upgrade u)
        {
            switch (u)
            {
                case Upgrade.HealthAndAmmo: return "hp and ammo";
                case Upgrade.FireRange: return "fire range upgrade";
                case Upgrade.FireRange1: return "another fire range upgrade";
                case Upgrade.FireRange2: return "even better fire range upgrade";
                case Upgrade.FireRange3: return "ultimate fire range upgrade";
                case Upgrade.GrenadeThrowRange: return "throw range upgrade";
                case Upgrade.GrenadeThrowRange1: return "another throw range upgrade";
                case Upgrade.GrenadeThrowRange2: return "ultimate throw range upgrade";
                case Upgrade.GrenadeExplodeRange: return "grenade explode range upgrade";
                case Upgrade.GrenadeRethrow: return "grenade rethrow upgrade";
                case Upgrade.StrikeSwing: return "strike swing upgrade";
                case Upgrade.StrikeSwing1: return "another strike swing upgrade";
                case Upgrade.StrikeBack: return "back strike upgrade";
                case Upgrade.StrikeLunge: return "strike lunge upgrade";
                
                
                default: return "nullUpgrade mistake: no such upgrade";
            }
        }

        public static Upgrade FollowingUpgrade(Upgrade u)
        {
            switch (u)
            {
                case Upgrade.FireRange: return Upgrade.FireRange1;
                case Upgrade.FireRange1: return Upgrade.FireRange2;
                case Upgrade.FireRange2: return Upgrade.FireRange3;
                case Upgrade.GrenadeThrowRange: return Upgrade.GrenadeThrowRange1;
                case Upgrade.GrenadeThrowRange1: return Upgrade.GrenadeThrowRange2;
                case Upgrade.StrikeSwing: return Upgrade.StrikeSwing1;

                default: return Upgrade.nullUpgrade;
            }
        }
    }

    [System.Serializable]
    public class ScriptedAltar
    {
        public int appearsWhen;
        public List<Upgrade> upgrades = new List<Upgrade>();
    }
}

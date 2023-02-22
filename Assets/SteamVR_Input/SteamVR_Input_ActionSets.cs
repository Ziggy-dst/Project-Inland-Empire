//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Input_ActionSet_platformer p_platformer;
        
        private static SteamVR_Input_ActionSet_buggy p_buggy;
        
        private static SteamVR_Input_ActionSet_mixedreality p_mixedreality;
        
        private static SteamVR_Input_ActionSet_VRIF p_VRIF;
        
        public static SteamVR_Input_ActionSet_platformer platformer
        {
            get
            {
                return SteamVR_Actions.p_platformer.GetCopy<SteamVR_Input_ActionSet_platformer>();
            }
        }
        
        public static SteamVR_Input_ActionSet_buggy buggy
        {
            get
            {
                return SteamVR_Actions.p_buggy.GetCopy<SteamVR_Input_ActionSet_buggy>();
            }
        }
        
        public static SteamVR_Input_ActionSet_mixedreality mixedreality
        {
            get
            {
                return SteamVR_Actions.p_mixedreality.GetCopy<SteamVR_Input_ActionSet_mixedreality>();
            }
        }
        
        public static SteamVR_Input_ActionSet_VRIF VRIF
        {
            get
            {
                return SteamVR_Actions.p_VRIF.GetCopy<SteamVR_Input_ActionSet_VRIF>();
            }
        }
        
        private static void StartPreInitActionSets()
        {
            SteamVR_Actions.p_platformer = ((SteamVR_Input_ActionSet_platformer)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_platformer>("/actions/platformer")));
            SteamVR_Actions.p_buggy = ((SteamVR_Input_ActionSet_buggy)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_buggy>("/actions/buggy")));
            SteamVR_Actions.p_mixedreality = ((SteamVR_Input_ActionSet_mixedreality)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_mixedreality>("/actions/mixedreality")));
            SteamVR_Actions.p_VRIF = ((SteamVR_Input_ActionSet_VRIF)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_VRIF>("/actions/VRIF")));
            Valve.VR.SteamVR_Input.actionSets = new Valve.VR.SteamVR_ActionSet[] {
                    SteamVR_Actions.platformer,
                    SteamVR_Actions.buggy,
                    SteamVR_Actions.mixedreality,
                    SteamVR_Actions.VRIF};
        }
    }
}
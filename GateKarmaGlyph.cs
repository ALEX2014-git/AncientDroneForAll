﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Cil;
using Menu;
using Rewired;
using UnityEngine.SocialPlatforms;
using UnityEngine.Rendering;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using Steamworks;
using IL.Menu.Remix.MixedUI;
using Menu.Remix.MixedUI;
using IL;
using On.Menu.Remix.MixedUI;
using System.Runtime.Remoting.Messaging;
using MonoMod.RuntimeDetour;
using UnityEngine;
using static MonoMod.InlineRT.MonoModRule;
using RWCustom;
using System.Runtime.Remoting.Contexts;
using Mono.Cecil.Cil;


namespace AncientDroneForAll
{
    public partial class AncientDroneForAll
    {
        private void GateKarmaGlyph_Update(ILContext il)
        {
            var c = new ILCursor(il);

            c.GotoNext(
                MoveType.After,
                //x => x.MatchLdarg(0),
                x => x.MatchLdfld(typeof(SaveState).GetField(nameof(SaveState.hasRobo))));
            c.MoveAfterLabels();
            c.Emit(OpCodes.Ldarg_0);

            bool hasDroneCheckTrue(bool hasDroneOrig, GateKarmaGlyph self)
            {

                return hasDroneOrig && AncientDroneForAll.hasDrone;
            
            }

            bool hasDroneCheckFalse(bool hasDroneOrig, GateKarmaGlyph self)
            {

                return hasDroneOrig && !AncientDroneForAll.hasDrone;

            }

            if (c.Next != null && c.Next.MatchBrfalse(out var _))
            {
                c.EmitDelegate(hasDroneCheckFalse);
            } else c.EmitDelegate(hasDroneCheckTrue);
        }


    }
}

//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Valve.VR
{
    public enum SteamVR_Input_Sources
    {
        [Description("/unrestricted")] 
        Any,

        [Description("/user/hand/left")]
        LeftHand,

        [Description("/user/hand/right")]
        RightHand,
    }
}
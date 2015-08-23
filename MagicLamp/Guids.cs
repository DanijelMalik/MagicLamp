// Guids.cs
// MUST match guids.h

using System;

namespace MagicLamp
{
    static class GuidList
    {
        public const string guidMagicLampPkg = "54b81f71-ff2d-4f5f-835d-e3b83dcb6921";
        public const string guidSolutionToolbarCmdSetString = "147A8F55-BF31-4D14-B6F7-A24B5B45B86F";

        public static readonly Guid guidSolutionToolbarCmdSet = new Guid(guidSolutionToolbarCmdSetString);
    };
}
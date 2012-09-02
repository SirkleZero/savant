// Guids.cs
// MUST match guids.h
using System;

namespace Savant
{
    static class GuidList
    {
        public const string guidSavantPackagePkgString = "0a5f852e-5be7-416c-8910-8a18c28ff0b0";
        public const string guidSavantPackageCmdSetString = "fec2c0ed-5b23-40ca-9087-ca56d531783e";

        public static readonly Guid guidSavantPackageCmdSet = new Guid(guidSavantPackageCmdSetString);
    };
}

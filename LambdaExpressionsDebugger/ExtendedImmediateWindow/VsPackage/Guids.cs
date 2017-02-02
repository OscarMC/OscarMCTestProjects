// Guids.cs
// MUST match guids.h
using System;

namespace uboot.ExtendedImmediateWindow
{
    static class GuidList
    {
        public const string guidExtendedImmediateWindowPkgString = "18480e6d-1c44-4105-8ad8-718429a72d39";
        public const string guidExtendedImmediateWindowCmdSetString = "4c901173-36ee-411c-9522-4db9f2c692c0";
        public const string guidToolWindowPersistanceString = "832081c5-7814-4253-80e7-c66d82f0540d";
        public const string guidCSharpLanguageServiceString = "694DD9B6-B865-4C5B-AD85-86356E9C88DC";

        public static readonly Guid guidExtendedImmediateWindowCmdSet = new Guid(guidExtendedImmediateWindowCmdSetString);
        public static readonly Guid guidCSharp = new Guid(guidCSharpLanguageServiceString);
    };
}
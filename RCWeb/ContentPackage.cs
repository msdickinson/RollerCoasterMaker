using Microsoft.AspNetCore.Blazor.Browser.Interop;
using RCLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

 namespace RCWeb
{
    public class Package
    {
        public Track[] tracks;
        public int trackCount = 0;
        public int lastChunk = 0;        
    }
    public static class ContentPackage
    {
        const string CoasterUpdateIdentifier = "ContentPackage.CoasterUpdate";
        public static CoasterUpdate CoasterUpdate(CoasterUpdate coasterUpdate)
        {
            return RegisteredFunction.Invoke<CoasterUpdate>(
                CoasterUpdateIdentifier,
                coasterUpdate);
        }
    }
}

using Microsoft.AspNetCore.Blazor.Browser.Interop;
using RCLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

 namespace RCWeb
{

    public static class ContentPackage
    {
        //const string CoasterUpdateIdentifier = "ContentPackage.CoasterUpdate";
        //public static CoasterUpdate CoasterUpdate(CoasterUpdate coasterUpdate)
        //{
        //    return RegisteredFunction.Invoke<CoasterUpdate>(
        //        CoasterUpdateIdentifier,
        //        coasterUpdate);
        //}

        const string CoasterDataIdentifier = "ContentPackage.CoasterData";
        public static string CoasterData(float[] data)
        {
            return RegisteredFunction.InvokeUnmarshalled<float[], string>(
                CoasterDataIdentifier,
                data);
        }

        const string CoasterUpdateIdentifier = "ContentPackage.Update";
        public static string CoasterUpdate(int added, int removed)
        {
            return RegisteredFunction.InvokeUnmarshalled<int, int, string>(
                CoasterUpdateIdentifier,
                added, removed);
        }

        const string LoadedIdentifier = "ContentPackage.Loaded";
        public static string Loaded(string loaded)
        {
            return RegisteredFunction.Invoke<string>(LoadedIdentifier, loaded);
        }
    }
}

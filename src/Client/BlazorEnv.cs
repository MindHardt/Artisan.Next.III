﻿using System.Runtime.InteropServices;

namespace Client;

public static class BlazorEnv
{
    public static bool IsWasm => RuntimeInformation.RuntimeIdentifier is "browser-wasm";
    public static bool IsServer => IsWasm is false;

    public static bool IsDebug =>
#if DEBUG
        true;
#else
        false;
#endif
}
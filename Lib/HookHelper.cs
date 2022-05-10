using System.Reflection;
using MonoMod.RuntimeDetour;

namespace RiskOfOptions.Lib
{
    internal static class HookHelper
    {
        private const BindingFlags TargetFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
        private const BindingFlags DestFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        
        internal static Hook NewHook<TTarget, TDest>(string targetMethodName, string destMethodName, TDest instance)
        {
            var targetMethod = typeof(TTarget).GetMethod(targetMethodName, TargetFlags);
            var destMethod = typeof(TDest).GetMethod(destMethodName, DestFlags);

            return new Hook(targetMethod, destMethod, instance);
        }

        internal static Hook NewHook<TTarget>(string targetMethodName, MethodInfo destMethod)
        {
            var targetMethod = typeof(TTarget).GetMethod(targetMethodName, TargetFlags);

            return new Hook(targetMethod, destMethod);
        }
    }
}
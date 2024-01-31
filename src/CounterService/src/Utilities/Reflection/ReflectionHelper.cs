using System.Reflection;

namespace DistributedCounter.CounterService.Utilities.Reflection;

public delegate void SetPropertyDelegate(object obj, object? value);

public static class ReflectionHelper
{
    private const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
    
    public static SetPropertyDelegate GetPropertySetter(PropertyInfo prop)
    {
        var setter = prop.GetSetMethod(nonPublic: true);
        if (setter != null)
        {
            return (obj, value) => setter.Invoke(obj, [value]);
        }
        
        var backingField = prop.DeclaringType!.GetField($"<{prop.Name}>k__BackingField", DeclaredOnlyLookup);
        if (backingField == null)
        {
            throw new InvalidOperationException($"Could not find a way to set {prop.DeclaringType.FullName}.{prop.Name}. Try adding a private setter.");
        }

        return (obj, value) => backingField.SetValue(obj, value);
    }
}
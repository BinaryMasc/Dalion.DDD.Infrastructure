using System.Reflection;

namespace Dalion.DDD.Commons
{
    public static class Helpers
    {
        public static void SetProperty(object containingObject, string propertyName, object newValue)
        {
            containingObject.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, containingObject, new object[] { newValue });
        }
    }
}

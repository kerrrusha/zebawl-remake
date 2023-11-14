using System;

namespace Assets.Scripts.Util
{
    internal class CommonUtil
    {
        public static void CheckNotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new Exception($"You should set the {name} ({name} = null)");
            }
        }
    }
}

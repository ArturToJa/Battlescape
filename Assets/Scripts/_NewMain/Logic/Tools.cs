using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public static class Tools
    {
        public static bool TypeComparizer<T, U1>(T obj)
            where U1 : class, T
        {
            if(obj as U1 != null)
            {
                return true;
            }
            return false;
        }

        public static bool TypeComparizer<T, U1, U2>(T obj)
            where U1 : class, T
            where U2 : class, T
        {
            if (TypeComparizer<T, U1>(obj) || obj as U2 != null)
            {
                return true;
            }
            return false;
        }

        public static bool TypeComparizer<T, U1, U2, U3>(T obj)
            where U1 : class, T
            where U2 : class, T
            where U3 : class, T
        {
            if (TypeComparizer<T, U1, U2>(obj) || obj as U3 != null)
            {
                return true;
            }
            return false;
        }

        public static bool TypeComparizer<T, U1, U2, U3, U4>(T obj)
            where U1 : class, T
            where U2 : class, T
            where U3 : class, T
            where U4 : class, T
        {
            if (TypeComparizer<T, U1, U2, U3>(obj) || obj as U4 != null)
            {
                return true;
            }
            return false;
        }
    }
}
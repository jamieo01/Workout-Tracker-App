using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float CheckIfNumber(string valueToTest)
    {
        bool isNumber = float.TryParse(valueToTest, out float value);
        if (isNumber) return value;
        else return 0;
    }


}

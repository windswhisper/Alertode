using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DescFormatter:ICustomFormatter,IFormatProvider
{
    public object GetFormat(Type format){
        return this;
    }

    public string Format(string format,object arg, IFormatProvider provider){
        if(format == "H")return (Convert.ToInt32(arg)/100f).ToString();
        return string.Format("{0:"+format+"}",arg);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class Commonn
{
    /// <summary> 
    /// /*****************************/ 
    /// /* this用法4：扩展对象的方法 */ 
    /// /*****************************/ 
    /// </summary> 
    /// <param name="item"></param> 
    /// <returns></returns> 
    public static int ToInt(this string obj, int i = 0)
    {
        int temp = i;
        try
        {
            temp = Convert.ToInt32(obj);
        }
        catch { }

        return temp;
    }

    public static string ToJson(this object obj) {
        if (obj == null)
            return "";
        return Newtonsoft.Json.JsonConvert.SerializeObject(obj);

    }
}
using GameAnalyticsSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyticsHelper
{
    public static string ConstructDesignEvent(string str1, string str2 = "", string str3 = "", string str4 = "", string str5 = "")
    {
        string output = str1;
        if (!str2.NullOrEmpty())
        {
            output += ":" + str2;
        }
        if (!str3.NullOrEmpty())
        {
            output += ":" + str3;
        }
        if (!str4.NullOrEmpty())
        {
            output += ":" + str4;
        }
        if (!str5.NullOrEmpty())
        {
            output += ":" + str5;
        }
        return output;
    }

    public static void SendResourceEvent(string currency, float amount, string itemType, string itemID)
    {
        // TODO: Protect from boneheaded misspelling of currency and itemType!

        if (amount == 0)
        {
            Debug.LogWarning("Attempt to send resource event with 0 amount. Currency = " + currency + ", itemType = " + itemType + ", itemID = " + itemID);
            return;
        }

        GAResourceFlowType flowType = GAResourceFlowType.Source;
        if (amount < 0)
        {
            flowType = GAResourceFlowType.Sink;
            amount = -amount;
        }

        GameAnalytics.NewResourceEvent(flowType, currency, amount, itemType, itemID);
    }

    public static void SendDesignEvent(string str1, string str2 = "", string str3 = "", string str4 = "", string str5 = "")
    {
        GameAnalytics.NewDesignEvent(ConstructDesignEvent(str1, str2, str3, str4, str5));
    }

    public static void SendDesignEvent(string str1, string str2, string str3, string str4, string str5, float val)
    {
        GameAnalytics.NewDesignEvent(ConstructDesignEvent(str1, str2, str3, str4, str5), val);
    }
    public static void SendDesignEvent(string str1, string str2, string str3, string str4, float val)
    {
        GameAnalytics.NewDesignEvent(ConstructDesignEvent(str1, str2, str3, str4), val);
    }
    public static void SendDesignEvent(string str1, string str2, string str3, float val)
    {
        GameAnalytics.NewDesignEvent(ConstructDesignEvent(str1, str2, str3), val);
    }
    public static void SendDesignEvent(string str1, string str2, float val)
    {
        GameAnalytics.NewDesignEvent(ConstructDesignEvent(str1, str2), val);
    }
    public static void SendDesignEvent(string str1, float val)
    {
        GameAnalytics.NewDesignEvent(ConstructDesignEvent(str1), val);
    }

}

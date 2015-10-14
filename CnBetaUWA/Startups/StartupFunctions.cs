using System;
using System.Collections.Generic;

namespace CnBetaUWA.Startups
{
    internal static partial class StartupFunctions
    {
        
       
      	static List<Action> AllConfig ;

		public static Action CreateAndAddToAllConfig(this Action action)
		{
			if (AllConfig == null)
			{
				AllConfig = new List<Action>();
			}
			AllConfig.Add(action);
			return action;
		}
		public static void RunAllConfig()
		{
			if (AllConfig==null) return;
			foreach (var item in AllConfig)
			{
				item();
			}

		}


    }
}

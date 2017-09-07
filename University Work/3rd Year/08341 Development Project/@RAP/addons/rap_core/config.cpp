class CfgPatches 
{
	class rap_core 
	{
		requiredVersion = 1.500000;
		requiredAddons[] = {"cba_main"};
	};
};

class CfgFunctions 
{
	class RAP {
		tag = "RAP";
		class RealArmourPenetration {
			file = "rap\addons\rap_core\functions";
			
			class hitPart {	
			};
			
			class firedBIS {	
			};
			
			class init {
				preInit = 1;
			};
		};
	};
};

class Extended_hitPart_EventHandlers 
{
	class All {
		RAP_cba_hitPart = "_this call RAP_fnc_hitPart;";
	};
};

class RAP_Extensions {
	extension[] += {"rap_extension"};
};

class cfgWeapons {
        access = 2;
};
private ["_hit", "_target", "_shooter", "_bullet", "_position", "_velocity", "_selection", "_ammo", "_direction", "_radius", "_surface", "_direct", "_dllString"];

_hit = _this select 0;

_target    = _hit select 0;        /* Object - Object that got injured/damaged */
_shooter   = _hit select 1;      /* Object - Unit that inflicted the damage */
_bullet    = _hit select 2;        /* Object - object that was fired */
_position  = _hit select 3;      /* Position3D - position the bullet impacted (ASL) */
_velocity  = _hit select 4;      /* Vector3D - 3D speed at whjich bullet impacted */
_selection = _hit select 5;     /* Array - array of strings with named selection of the object that were hit */
_ammo      = _hit select 6;          /* Array - Ammo info: [hit value, indirect hit value, indirect hit range, explosive damage, ammo class name] 
                                    OR, if there is no shot object: [impulse value on object collided with,0,0,0] */
_direction = _hit select 7;     /* Vector3D - vector that is orthogonal (perpendicular) to the surface struck. */
_radius    = _hit select 8;        /* Number - Radius (size) of component hit. */
_surface   = _hit select 9;       /* String - Surface type struck */
_direct    = _hit select 10;       /* Boolean - true if object was directly hit, false if it was hit by indirect/splash damage. */

_localBullet = format["%1", _bullet];
player sidechat _localBullet;

if !(_direct) exitWith{};

if (BULLET_CHECK == _localBullet)
	exitWith
	{
		_target allowDamage false;
		player sidechat "EXITED HITPART";
	};

BULLET_CHECK = _localBullet;

deleteVehicle _bullet;

_target allowDamage false;

_dllString = format ["Target:%1|Shooter:%2|Bullet:%3|Position:%4|Velocity:%5|Selection:%6|Ammo:%7|Direction:%8|Radius:%9|Surface:%10|Direct:%11",_target, _shooter, _bullet, _position, _velocity, _selection, _ammo, _direction, _radius, _surface, _direct];

KK_fnc_callBackFunc = {	
	_result = _this select 0;	
	_hit = _this select 1;
	
	_target = _hit select 0;
	_bullet = _hit select 2;
	_position = _hit select 3;
	_ammo = _hit select 6;
	
	_target allowDamage true;
	
	_splitResult = _result splitstring ",";
	
	_newProjectileVelocity = [(parseNumber (_splitResult select 0)), (parseNumber (_splitResult select 1)), (parseNumber (_splitResult select 2))];	
	_damage = parseNumber (_splitResult select 3);
	_isPenetration = parseNumber (_splitResult select 4);
	
	_currentDamage = getDammage _target;	
	_AddDamage = _damage + _currentDamage;	
	_target setDamage _AddDamage;
	
	if (_isPenetration == 1) then 
	{ 
		diag_log format["PENETRATION current tank current damage=%1, damage=%2, isPenetration=%3", _currentDamage, _damage, _isPenetration];
		player sidechat format["PENETRATION current tank damage=%1, damage=%2, isPenetration=%3", _currentDamage, _damage, _isPenetration];
	} 
	else 
	{ 
		diag_log format["NO PENETRATION current tank current damage=%1, damage=%2, isPenetration=%3", _currentDamage, _damage, _isPenetration];
		player sidechat format["NO PENETRATION current tank damage=%1, damage=%2, isPenetration=%3", _currentDamage, _damage, _isPenetration];
	};
	
	_newBullet = createVehicle [_ammo select 4, _position, [], 0, "CAN_COLLIDE"];
	_newBullet setVelocity [_newProjectileVelocity select 0, _newProjectileVelocity select 1, _newProjectileVelocity select 2];
	
	//_bullet setPosASL [_position select 0, _position select 1, _position select 2];
	//_bullet setVelocity [_newProjectileVelocity select 0, _newProjectileVelocity select 1, _newProjectileVelocity select 2];
		
	BULLET_CHECK = "null";
};  

KK_fnc_callExtensionAsync = {
    private ["_args", "_callBack", "_message", "_hit"];

    _args = _this select 0;
    _callBack = _this select 1;
    _message = format ["r:%1", "rap_extension" callExtension format ["s:%1", _args]];
	_hit = _this select 2;
	
    diag_log format ["TICKET SENT message=%1, args=%2", _args, _message];

    [_message, _callBack, _hit] spawn {
        private ["_result"];
        _message = _this select 0;
        _callBack = _this select 1;
		_hit = _this select 2;
    
        waitUntil {
            _result = "rap_extension" callExtension _message;
            if (_result != "WAIT") exitWith {
                [_result, _hit] call _callBack;
                
                diag_log format ["TICKET RETURNED message=%1, result=%2", _message, _result];
                true
            };
            false
        };
    };
};

 _id = [_dllString, KK_fnc_callBackFunc, _hit] call KK_fnc_callExtensionAsync;
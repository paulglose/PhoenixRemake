**Game Starting**
In order to start the game, you need to start the **Initilization**-Scene. After that you press **StartGame**. The **Test**-Scene is used for recording videos for abilities.

**Project Structure:** <br>
The code structure of my project describes a Singleton/Event Pattern, which uses Scriptable Objects as "API's" between scripts. For example, I have a class called **CameraShakeManager**, which takes care of camera shaking. This **CameraShakeManager** class holds a ScriptableObject named **CameraShakeChannel** as a Variable. Other scripts communicate with the **CameraShakeManager** through this Scriptable Object. If a weapon, for example, wants to request a camera shake, the weapon calls a method named "RaiseCameraShake(float Duration, float Strength)" in the same ScriptableObject - **CameraShakeChannel**. This method then raises an event that the **CameraShakeManager** listens to and handles the requested camera shake. This is done with many systems in the game. The most important systems are named _SystemName_Manager.


**TUTORIAL: Gameplay** <br>
Survive as long as you can. Defeat increasingly powerful waves of enemies. After every 3rd wave, you can select an upgrade that strengthens you in different ways. <br>

**TUTORIAL: Controls** (Controls can be changed in Settings (only during Game)): <br>
Some Abilities (like Special & Ultimate) need to be charged in order to be casted. This is achieved by holding down the button. Visually, there is feedback when the ability has been sufficiently charged. By releasing, the ability is then executed (provided it's adequately charged). <br>

**Movement**: WASD <br>
**Primary**: LeftMouse <br>
**Special**: RightMouse <br>
**Mobility**: LeftShift <br>
**Ultimate**: R <br>

**Known Bugs**
- Rarely the rerolling of upgrades only partially work
- Very rarely the triggering of a new wave is bugged
- After defeating the last Wave, the game will crash, because currently there are not enough Legendary Upgrades available

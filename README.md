# PhoenixRemake

**Project Struktur**
Die Code-Struktur meines Projektes beschreibt ein Singleton/Event Pattern, welches Scriptable Objects als Schnittstellen zwischen Scripts nutzt. Ich habe bspw. eine Klasse **CameraShakeManager**, die sich um das Shaken der Camera kümmert. Diese **CameraShakeManager** klasse hat ein ScriptableObject names **CameraShakeChannel**. Durch dieses Scriptable Object kommunizieren andere Scripts mit dem CameraShakeManager. Möchte eine Waffe z.B. einen CameraShake Requesten, ruft die Waffe im ScriptableObject: **CameraShakeChannel** eine Methode names "RaiseCameraShake(float Duration, float Strength)". Diese Methode raised dann ein Event auf das der CameraShakeManager hört und den requesteten CameraShake Handled. Dies wird mit vielen Systemen im Spiel gemacht. Die wichtigsten Systeme heißen _SystemName_Manager.

**Tutorial** 
**GamePlay**:
Überlebe so lange du kannst. Besiege immer stärker werden wellen von Gegnern. Nach jeder 3. Wave kannst du ein Upgrade auswählen, das dich in unterschiedlichster Weise stärkt.

**Controlls** (Controlls can be changed in Settings (only during Game)):
Some Abilities (like Special & Ultimate) need to be charged in order to be casted. Dies wird durch halten der Taste erreicht. Visuell gibt es ein Feedback wenn die Fähigkeit ausreichend geladen wurde. Durch loslassen wird dann die Fähigkiet ausgefhprt (sofern ausreichende geladen).

- Movement: WASD
- Primary: LeftMouse
- Special: RightMouse
- Mobility: LeftShift
- Ultimate: R

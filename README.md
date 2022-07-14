# Quander Realm

### Overview
The Quander Realm or **Wrapper** is a hub world that holds and manages the various minigames in this project along with all the shared functionality and assets.

**Note:** Rather than here in the readme, the [Wiki (accessed from the sidebar)](https://gitlab.office.filamentgames.com/client/university-of-chicago/quander-realm/-/wikis/home) will hold information such as the API Documentation, [Branching Strategy](https://gitlab.office.filamentgames.com/client/university-of-chicago/quander-realm/-/wikis/Branching-Strategy), [Technical Requirements](https://gitlab.office.filamentgames.com/client/university-of-chicago/quander-realm/-/wikis/Technical-Requirements), etc. 

### Firebase Setup
To build for develop:
- Copy FirebaseConfig/develop/GoogleService-Info.plist and google-services.json to Assets/
- Use the app id com.filamentgames.uchicago.quantumzombies
- Remove compiler definition PRODUCTION_FB, if applicable

To build for production:
- Copy FirebaseConfig/production/GoogleService-Info.plist and google-services.json to Assets/
- Use the app id com.filament.uchicago.quanderrealm
- Add compiler definition PRODUCTION_FB
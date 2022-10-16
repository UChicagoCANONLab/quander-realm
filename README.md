# Quander Realm

# Table of contents
1. [Overview](#overview)
2. [Merging Games into integration] (#merge)
3. [Branching Strategy](#branching)
    1. [Overview](#boverview)
    2. [Summary](#bsummary)
    3. [Details](#bdetails)
4. [Firebase Setup](#firebase)

## Overview <a name="overview"></a>
The Quander Realm or **Wrapper** is a hub world that holds and manages the various minigames in this project along with all the shared functionality and assets.

See [Wiki](../../wiki) for more detailed documentation. 

## Merging Games into integration <a name="merge"></a>
### !! Do not resolve conflicts through pull requests, otherwise the integration branch will be merged into the game branches !! 
#### Do the following:
- Create draft pull requests to merge
- If there are conflicts, update the files locally to match on both branches
- Once no conflicts remain, update the draft pull request to be ready for review and request a reviewer

## Branching Strategy <a name="branching"></a>
### Overview <a name="boverview"></a>

This branching strategy is designed to keep each minigame and the wrapper in their own silos so they can be worked on \*mostly\* independently from each other. This page includes:

- a **summary** of how the project will progress in separate branches
- a **table** that details the purpose of each branch and its contents
- a set of **diagrams** for a more visual representation of the different branches

#### Summary <a name="bsummary"></a>

- Filament devs work on the "develop" branch (or feature branches: feature/) to implement wrapper features
- When a set of features is complete, they constitute a new version of the wrapper
- This new version is then merged into the "wrapper" branch (versions will be "tagged" as `1.0`, `1.5`, `2.0`, etc. and these tags will be visible in Sourcetree on a specific commit's description)
- Student devs only work on their own minigame branch (and possibly their minigame sub-branches: /) and have no content or code from other minigames in their branch
- The wrapper code, however, will flow from Filament's "develop" branch to the "wrapper" branch to individual minigame branches. So each minigame branch includes the wrapper code along with its own minigame code
- When a milestone is due, student devs will merge their branch into the "integration" branch
- The entire game, now existing in the "integration" branch will be merged into "staging", and then just before launch, into "production"

## Details <a name="bdetails"></a>

<table>
<tr>
<th>Branch</th>
<th>Contains</th>
<th>Purpose</th>
</tr>
<tr>
<td>feature/\*</td>
<td>Individual wrapper feature</td>
<td>

* Implement individual wrapper features on these branches
* Completed features are _merged into "develop"_
* When these branches are no longer needed, they should be deleted
* For example: "feature/map-navigation", "feature/avatar-select"
</td>
</tr>
<tr>
<td>develop</td>
<td>all wrapper content</td>
<td>

* This branch contains completed features of the wrapper
* When a group of features constitutes a new version of the wrapper, the changes will be _merged into the "wrapper" branch_
</td>
</tr>
<tr>
<td>wrapper</td>
<td>stable wrapper version</td>
<td>

* This branch contains stable versions of the wrapper and nothing else
* Versions will be "tagged" as `1.0`, `1.5`, `2.0`, etc. and these tags will be visible in Sourcetree on a specific commit's description
* Whenever a new version is available, it should be _merged into individual minigame branches_
</td>
</tr>
<tr>
<td>minigame/\*</td>
<td>

* individual minigame content
* wrapper content
</td>
<td>

* Student devs build their individual minigames on their minigame/ branch
* For example: "minigame/circuits", "minigame/queuebits"
* These branches contain individual minigames _and_ the wrapper only
* Whenever there is a new version of the wrapper, students _merge the "wrapper" branch into their own game branch_
* If student devs would like to make their own feature branches, they may use the convention / ("blackbox/throw-mintpie", "labyrinth/movement")
* These feature branches must be deleted when they are no longer needed
</td>
</tr>
<tr>
<td>integration</td>
<td>

* contents of all minigames
* wrapper content
</td>
<td>

Whenever updating the minigame branches, student devs will:

* make sure the latest wrapper version is integrated into their own minigame branch
* run tests and make sure it runs smoothly with the latest wrapper version
* _merge their minigame branch into the "integration" branch_

This branch contains the _entire game_ including the wrapper and all minigames
</td>
</tr>
<tr>
<td>staging</td>
<td>

* contents of all minigames
* wrapper content
</td>
<td>

When a milestone is due, devs will :

* make sure the latest minigame changes have been merged into "integration"
* _merge "integration" into this branch_ to make milestone builds such as release candidates
</td>
</tr>
<tr>
<td>production</td>
<td>

* contents of all minigames
* wrapper content
</td>
<td>

Devs _merge "staging" into this branch_ to make production builds that will go live during launch
</td>
</tr>
<tr>
</table>


<!-- **Note:** Rather than here in the readme, the [Wiki (accessed from the sidebar)](https://gitlab.office.filamentgames.com/client/university-of-chicago/quander-realm/-/wikis/home) will hold information such as the API Documentation, [Branching Strategy](https://gitlab.office.filamentgames.com/client/university-of-chicago/quander-realm/-/wikis/Branching-Strategy), [Technical Requirements](https://gitlab.office.filamentgames.com/client/university-of-chicago/quander-realm/-/wikis/Technical-Requirements), etc. 
 -->
 
### Firebase Setup <a name="firebase"></a>
To build for develop:
- Copy FirebaseConfig/develop/GoogleService-Info.plist and google-services.json to Assets/
- Use the app id com.filamentgames.uchicago.quantumzombies
- Remove compiler definition PRODUCTION_FB, if applicable

To build for production:
- Copy FirebaseConfig/production/GoogleService-Info.plist and google-services.json to Assets/
- Use the app id com.filament.uchicago.quanderrealm
- Add compiler definition PRODUCTION_FB

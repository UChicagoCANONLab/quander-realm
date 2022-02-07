# Quander Realm

## Overview
The Quander Realm is a hub world that holds and manages the various minigames in this project along with all the shared functionality and assets.

## Branch Info ([view branching diagrams](https://drive.google.com/file/d/1QF9IWxEZPXHDU2SHVZFZDrD0qQBiDc6a/view?usp=sharing))

- **main**: unused
- **develop**: 
    - filament devs implement the hub world on this branch
    - when there's a new version of the hub world, filament devs merge _this branch into "integration"_
- **staging**: filament devs merge _"integration" into this branch_ to make milestone builds such as release candidates
- **production**: filament devs merge _"staging" into this branch_ for release

- **integration**: 
    - filament devs merge the latest version of the hub world _from "develop" into this branch_
    - filament devs merge _this branch into "staging"_ to make milestone builds
    - students pull the latest version of the hub world _from this branch into their invividual game branches_
    - for milestone deadlines, students merge _their own branches into this branch_ (create pull requests)

- Minigame branches:
    - **filament minigame** (placeholder name): 
        - whenever there is a new version of the hub world, filament devs merge _"integration" into this branch_
        - for milestone deadlines, filament devs merge _this branch into "integration"_ 
    - **student game 1, student game 2, student game 3, student game 4** (placeholder names):
        - whenever there is a new version of the hub world, students merge _"integration" into their own game branch_
        - for milestone deadlines, filament devs merge _their own branch into "integration"_

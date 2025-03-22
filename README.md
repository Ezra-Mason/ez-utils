# ez-utilities
Utilities for Unity game development.

# Content
## Core
- ScriptableObject Variables (abstract base class, bool, int, Vector3 and GameObject)
- ScriptableObject Events (ScriptableObject event objects and listener class)
- Repositories (ScriptableObject for storing, sharing and updating objects)
- Springs (Simple Harmonic Motion methods and classes for 1,2 and 3D springs)
- Scene References (A runtime safe way to reference a SceneAsset) 
  
## Runtime
- Finite State Machine (Adaptable FSM system with customisable states)
- Inverse Kinematics (Two bone IK for bipedal motion)
- Object Pooling (Generic pooling of objects using a stack pool)
- Collections (ScriptableObject for a managing a repository of objects)

# Getting Started
This packae can be added to a unity project in two ways. Use Unity Package Manager if you dont wish to make code changes. Use git submodules to let you make code edits and have the package within your Visual Studio Solution
## Unity Package Manager
1. Open your Unity project.
2. Navigate to 'Window > Package Manager'
3. Click the '+' button and select 'Add Package from git URL', inputting the URL for the repo  https://github.com/Ezra-Mason/ez-utils.git

## Git Submodules
1. Open git in your projects root
2. Navigate to the '<Unity_Project_Name>/Packages' folder
3. Add the submodule using the command 'git submodule add  https://github.com/Ezra-Mason/ez-utils.git'

# GPGPU t-SNE Demo

## Setup Instructions
This demo is made using unity 6.3 LTS (6000.3.18f1). 
To compile the code yourself you must download [Unity Hub](https://docs.unity.com/en-us/hub/install-hub) 
and install the same editor version under `Installs` -> `Install Editor` -> `Unity 6.3 LTS`.

Then move back to the `Projects` tab and select `Add` -> `Add project from disk`. 
Navigate to the extracted source code and select the root folder.
- gpgpu-sne-demo: <- folder to pick
  - Assets
  - Packages
  - ProjectSettings
  - .gitattributes
  - .gitignore
  - README.md

Now open the project. From here you can either run directly in the editor or build and run.

## Run Instructions

### Directly in the Editor
Navigate to the main scene by opening `Assets/Scenes/Main.unity`. 
Press the play button at the top of the scene to start the demo.
I recommend to maximize the `Game` tab by navigating to the 3 dots at the top right of the screen and selecting `Maximize`.

### Build and Run
In the top left of the screen navigate to `File` -> `Build And Run` -> Select `Builds` folder.


## Structure
The project is structured as follows:
- Assets
  - Art
    - FieldColor: _shader graph used to display the generated field mesh as a colored thing_
  - external_code
  - Models
  - Prefabs
  - Scenes
    - Main: _the main scene combining the components into a playable environment_
  - Scripts
    - Models
      - Field: _the class that stores the field texture as an array of pixels_
      - Point: _a class that represents a single datapoint_
      - PointTuple: _a helper class with (point a, point b) == (point b, point a)_
      - Settings: _the class that stores all settings_
    - Tsne
      - TSneManager: _single api for t-SNE actions_
      - SimilarityCompute: _computes the similarities for the attractive forces_
      - FieldKernelGenerator: _generates a kernel to be splat onto another field_
      - RepulsiveForceFieldGenerator: _generates the repulsive force field using FieldKernelPixelJob_
      - FieldKernelPixelJob: _computes the repulsive force field by summing all kernels for each pixel_
    - Utils
      - ArrowUtils: _utils to display arrows efficiently_
      - Utils: _general utils like destroying all child object_
    - Visualizing
      - FieldVisualizer: _central point where the draw mode is selected_
      - VectorFieldGenerator: _generates a vector at each pixel in the repulsive field_
      - FieldTextureGenerator: _generates a mesh and textures it with the repulsive field_
      - PointForceVectorGenerator: _generates a vector for each data point_
    - Manager: _manages lifecycle of the application_
    - PointManager: _generates the initial points and also maintains their attributes (like size)_

## External Resources
**NOTE: because of limitation imposed by unity. the external_sources folder can be found at `assets/external_sources` instead**

### Priority Queue
This is an implementation of a priority queue for C#. 
I use this to find knn among other things. 
Surprisingly the C# framework unity ships with doesnt contain an implementation of it out of the box.
I don't consider this algorithm central to the paper and therefore I opted to use this one instead. 
It was sourced from [Another Repo](https://github.com/dotnet/runtime/blob/main/src/libraries/System.Collections/src/System/Collections/Generic/PriorityQueue.cs) of someone who ran into the same issue 
and modified the open-source dotnet version to work with unity easily.

### SettingIMGUI
This is the IMGUI that is used within the tool. 
Working with unity's IMGUI is relatively tedious.
In addition, I did not consider it technically valuable enough to implement.
I opted to generate it using ChatGPT.

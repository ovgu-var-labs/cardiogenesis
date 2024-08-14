# cardiogenesis

Project is splited up into Individual and Collaborative Learning Envoirnment. 
SingleUser contains Unity Project of Individual LE.
MultiUser contains Unity Project of Collaborative LE.
Possible to downlad APKs, which were used in the study, here. [Link einfügen]
Folder RProject contains the code used to make the statistical evaluations.

## Installation

You can download the apk for the [Individual Learning Environment](https://isgcloud.cs.uni-magdeburg.de/d/044e7a5665da4c848a07/files/?p=%2FSingle-User.apk) and the [Collaborative Learning Environment](https://isgcloud.cs.uni-magdeburg.de/d/044e7a5665da4c848a07/files/?p=%2FMulti-User.apk).
Afterwards you can install these APKs on your Quest.

### Unity Project
Download the repository. 
Install Unity Version 2021.3.35f1 for Multiuser or 2022.3.12f1 for Singleuser.
Open the Singleuser or Multisuer Project in the Unity Hub.
If you want to use the heart models you have to download them from [here](https://isgcloud.cs.uni-magdeburg.de/d/044e7a5665da4c848a07/) (https://isgcloud.cs.uni-magdeburg.de/d/044e7a5665da4c848a07/).
For the Single User you have to download the Folders which are named Stage_X_Low and the Locked X.prefab files. Create a resoruces folder in your Assets and paste them in there. Then you have to insert the Prefab files onto their respective task objects within the CreateLockedMesh Script.

For the Multi User you have to download the Folder which are named Stage X and the FBX_Locked X.prefab files. Create a resoruces folder in your Assets and paste them in there. Then you have to insert the Prefab files onto their respective task objects within the CreateLockedMesh Script and change the enum to FBX.


## Controls

### Singleuser

The application is controlled by the hand tracking built into the quest. The contolls of the buttons is explained in an tutorial inside the application.

### Multiuser

The application is controlled using the Quest controllers. The controller uses a cursor to interact with the buttons. The stick and buttons on the controller are used to rotate and scale the heart. A furhter explanation for the controls is illustrated here![0](https://github.com/user-attachments/assets/d143f45e-57f8-4764-bdf1-210d2b492c1d)![1](https://github.com/user-attachments/assets/867f08a6-f5ec-441c-b37a-bdaa2d1c1f90)



## Include your own content

The Process is pretty similar in both projects.
To add new content you have to overwrite the content of the Task objects and have to update the lists on the *stage manager* script.

You have the possibility to import your own content as a list of fbx or mesh objects. You have multiple possibilites to load them into the scene.

-A prefab with a LockedMeshStorage Script on it, where you can put your meshes into the Meshes Stage List
-A prefab with a LockedFBXStorage Script on it, where you can save your FBX in the FBX Storage List
-Using an Asset Bundle containing the FBX Modells. 
You have to save the Modells using a specific naming convention and save the bundles under a specific path, so that the code can load the FBX correctly from the bundle. 
Asset Bundles are loaded from a folder “AssetBundles” in the Assets folder when using the Unity Editor. When the application is deployed to the Quest the Asset Bundles are loaded form the Application.persistentDataPath (https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html).

To annotate your own models you can create Annotations undre the Annotations object.

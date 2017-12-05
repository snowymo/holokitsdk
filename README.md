## Change color schema (Check [Video](Videos/ChangeColor.mp4))

* Go to iCloud drive in Mac

* Add your color file as format "cluster_index r g b" for each row

* Login to same apple account in the iPhone running the application

## Change core data (Check [Video](Videos/HowToChangeData.mp4))

* Go to Assets/Scripts/InputHandler.cs

* Change the variable *file_3dembedding*, *file_clusterlabel*, *file_clusterpos*, *file_keywords*, *file_score*

* Put corresponding files in folder *Assets/Data*

## Build steps (Check [Video](Videos/HowToBuild.mp4))

* Launch Unity 2017.1

* Open build settings, switch to iOS platform, click build.

* Go to the folder just built (eg: disviz_cleanup), open \*.xcodeproj with XCode 9.0.

* In Xcode, change your build target to your actual device

* Click "Unity-iPhone" in the file explorer to see its settings, and select the proper Team. If you don't have any Team listed, go to "XCode" -> "Preferences" -> "Accounts" and add your Apple Developer account.

* Click run.

This project used [HoloKitSDK](https://github.com/holokit/holokitsdk). HoloKitSDK is to build AR/MR apps for HoloKit. Currently, they provide the SDK as Unity package.
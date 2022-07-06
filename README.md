# Hand Interfaces: Using Hands to Imitate Objects in AR/VR for Expressive Interactions 
Siyou Pei, Alexander Chen, Jaewook Lee, Yang Zhang :trophy: CHI '22 Honorable Mention</br></br>
![A user imitates a joystick with a thumb-up pose and manipulates the joystick by grabbing the thumb with another hand](https://github.com/sypei/personal-website/blob/main/research/HandInterfaces/HandInterfaces.png)</br>

📘 [paper](https://dl.acm.org/doi/10.1145/3491102.3501898) &emsp; 
🎬 [preview](https://www.youtube.com/watch?v=ATg3M4QsfEQ) &emsp;
🎙️ [full presentation](https://www.youtube.com/watch?v=ATg3M4QsfEQ) &emsp;
:desktop_computer: [lab website](https://hilab.dev/)

## Table of Contents
[Motivation](https://github.com/sypei/Hand-Interfaces#motivation)

[What does Hand Interfaces do?](https://github.com/sypei/Hand-Interfaces#what-does-hand-interfaces-do)

[Quick Start](https://github.com/sypei/Hand-Interfaces#quick-start)

[Full Project Implementation](https://github.com/sypei/Hand-Interfaces#full-project-implementation)

[Help](https://github.com/sypei/Hand-Interfaces#help)

[Acknowledgments](https://github.com/sypei/Hand-Interfaces#acknowledgments)

[Citation](https://github.com/sypei/Hand-Interfaces#citation)

[In the End](https://github.com/sypei/Hand-Interfaces#in-the-end)

## Motivation
In the digital reality, there are many objects to retrieve and interact with. This rich set of objects means a lot to the user experience. To play with so many objects, the prevailing method is users hold hand controllers. But holding something all the time can be cumbersome. We want to bring up a new interaction technique that is more readily available.<br>
In the rock-paper-scissors game, people use hands in different shapes to imitate rock, paper and scissors. It is intuitive and self-revealing.<br>
So we wonder, can we generalize this idea to many other objects and interactions in AR and VR? Then, we propose Hand Interfaces!

![picture of 28 hand interfaces](/documentation/teaser.jpeg)

## What does Hand Interfaces do?
We propose **Hand Interfaces** - a new free-hand interaction technique that allows users to embody objects through imitating them with hands. In short, hands now **BECOME** those virtual objects.<br>
Further, Hand Interfaces not only supports **object retrieval** but also **interactive control**.

![on the left, a hand performs a thumb-up and becomes a joystick, to retrieve an object. On the right, the other hand is manipulating the thumb of the imitating hand, to do interactive control](/documentation/2tasks.gif)
</br>

## Quick Start
If you happen to have a Meta Oculus Quest headset, we have a minimal app for you to install easily. You only need the Scissors_Demo.apk in this repo, nothing else. The app contains a pair of scissors. You can perform a peace gesture to retrieve the scissors in the air, and close your index and middle fingers to snip virtual objects in front of you (e.g. a cake, a bread, or even a phone!).<br>
To install it, please 
1. Download the Scissors_Demo.apk in this repo.
2. Make sure your Quest account has enabled developer mode. [Official tutorial](https://developer.oculus.com/documentation/native/android/mobile-device-setup/)
3. Sideload the apk onto your Quest headset through [Oculus Developer Hub](https://developer.oculus.com/documentation/unity/ts-odh/), or [SideQuest](https://sidequestvr.com/setup-howto) or [ADB commands](https://developer.oculus.com/documentation/native/android/ts-adb/).
4. Find the apk file in your Oculus library - unknown sources. Have fun!

## Full Project Implementation
The 11 objects used in user studies are included in this github project. The two tasks - object retrieval and interactive control - are split into separate scenes in user studies.
### Dependencies and Configuration
To build and run the full project, we need to set up our Quest device for Unity development. This is a [tutorial](https://github.com/sypei/Hand-Interfaces/blob/main/documentation/UCLA_UnityDevelopmentWorkshopPreparation.pdf) I made when I was a TA of a VR course.
### Project Structure
`handinterfaces/Assets/Scenes/prototypes` stores early versions of idea prototypes.<br>
`handinterfaces/Assets/Scenes/user study` contains what we showed to participants during user studies. The unity scenes starting with "Retr" are for "object retrieval", while those starting with "Inter" refer to "interactive control". <br>
As indicated in the paper, DM, VG, HI are abbreviations for "Drop-down Menu", "VirtualGrasp" and "Hand Interfaces", FG, VM, HI are abbreviations for "Fist Gesture", "Virtual Manipulation" and "Hand Interfaces".<br>
`handinterfaces/Assets/Example Applications/` includes three applications mentioned in the paper, they are "education", "entertainment", and "IoT". Demo videos can be found [here](https://twitter.com/SiyouPei/status/1520298604715384832?s=20&t=m9H04vz897N6nOBseP7Pqw).<br>

## Help
Feel free to create a new issue in the 'Issues' tab!

## Acknowledgments
https://github.com/DavidArayan/ezy-slice<br>
https://github.com/dilmerv/VRDraw<br>
https://github.com/pharan/Unity-MeshSaver<br>

In addition, many thanks to [Quentin Valembois](https://www.youtube.com/c/ValemVR), [Dilmer Valecillos](https://www.youtube.com/c/DilmerV) and [Antony Vitillo](https://twitter.com/skarredghost) for their contribution to the AR/VR content creation community.<br>
Special thanks to the authors of [VirtualGrasp](https://dl.acm.org/doi/10.1145/3173574.3173652), Yukang Yan, et al. for inspiring this thread of work in designing AR/VR interfaces around hands’ expressivity. <br>

## Citation
```
@inproceedings{pei2022hand,
author = {Pei, Siyou and Chen, Alexander and Lee, Jaewook and Zhang, Yang},
title = {Hand Interfaces: Using Hands to Imitate Objects in AR/VR for Expressive Interactions},
year = {2022},
isbn = {9781450391573},
publisher = {Association for Computing Machinery},
address = {New York, NY, USA},
url = {https://doi.org/10.1145/3491102.3501898},
doi = {10.1145/3491102.3501898},
booktitle = {Proceedings of the 2022 CHI Conference on Human Factors in Computing Systems},
articleno = {429},
numpages = {16},
keywords = {Interaction design, AR/VR, Embodiment, Free-hand interactions, Imitation, On-body interactions},
location = {New Orleans, LA, USA},
series = {CHI '22}
}
```

## In the End
Thank you for reading! Isn't it exciting to create the next-gen user experience for XR :D

You may also find me [@SiyouPei](https://twitter.com/SiyouPei) 

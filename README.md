# Hand Interfaces: Using Hands to Imitate Objects in AR/VR for Expressive Interactions 
Siyou Pei, Alexander Chen, Jaewook Lee, Yang Zhang :trophy: CHI '22 Honorable Mention</br></br>
![A user imitates a joystick with a thumb-up pose and manipulates the joystick by grabbing the thumb with another hand](https://github.com/sypei/personal-website/blob/main/research/HandInterfaces/HandInterfaces.png)</br>

📘 [paper](https://dl.acm.org/doi/10.1145/3491102.3501898)

🎬 [preview](https://www.youtube.com/watch?v=ATg3M4QsfEQ)

🎙️ [full presentation](https://www.youtube.com/watch?v=ATg3M4QsfEQ)

:desktop_computer: [lab website](https://hilab.dev/)

## Table of Contents
[Motivation](## Motivation)
[What does Hand Interfaces do?](## What does Hand Interfaces do?)
[Quick Start](## Quick Start)
[Full Project Implementation](## Full Project Implementation)
[Dependencies and Configuration](### Dependencies and Configuration)
## Motivation
In the digital reality, there are many objects to retrieve and interact with. This rich set of objects means a lot to the user experience. To play with so many objects, the prevailing method is users hold hand controllers. But holding something all the time can be cumbersome. We want to bring up a new interaction technique that is more readily available.<br>
In the rock-paper-scissors game, people use hands in different shapes to imitate rock, paper and scissors. It is intuitive and self-revealing.<br>
So we wonder, can we generalize this idea to many other objects and interactions in AR and VR? Then, we propose Hand Interfaces!

[picture of 28 hand interfaces]

## What does Hand Interfaces do?
We propose **Hand Interfaces** - a new free-hand interaction technique that allows users to embody objects through imitating them with hands. In short, hands now **BECOME** those virtual objects.
Further, Hand Interfaces not only supports **object retrieval** but also **interactive control**.

[GIF here, showing what is object retrieval and interactive control]
</br>

## Quick Start
If you happen to have a Meta Oculus Quest headset, we have a minimal app for you to install easily. You only need the scissors.apk in this repo, nothing else. The app contains a pair of scissors. You can perform a peace gesture to retrieve the scissors in the air, and close your index and middle fingers to snip virtual objects in front of you (e.g. a cake, a bread, or even a phone!).<br>
To install it, please 
1. Download the scissors.apk in this repo.
2. Make sure your Quest account has enabled developer mode. [tutorial here]
3. Sideload the apk onto your Quest headset through [Oculus Developer Hub](https://developer.oculus.com/documentation/unity/ts-odh/), or [SideQuest](https://sidequestvr.com/setup-howto) or [ADB commands](https://developer.oculus.com/documentation/native/android/ts-adb/).
4. Find the apk file in your Oculus library - unknown sources. Have fun!

## Full Project Implementation
The 11 objects used in user studies are included in this github project. The two tasks - object retrieval and interactive control - are split into separate scenes for study purpose.
### Dependencies and Configuration

### Project Structure

## Help

## Contribute

## License

## Acknowledgments

## Citation

## Other
Thanks for coming. I will finish the readme soon, including a **Quick Start** section and a **Full Project Implementation** section to help you start! See you soon :)

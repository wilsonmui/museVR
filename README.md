# museVR


In this project, we create a brain-computer interface(BCI) bases on electroencephalography(EEG) signals to allow user interact with the virtual reality environment(VRE). Through EEG brainwaves, we can detect whether the user is in concentrating status, and give the user feedback through VEW. 

## Devices
The requiring devices are
 - [Mind Monitor](https://mind-monitor.com/), an EEG recording headset.![](https://i.imgur.com/ti20wt5.png)
 - [Windows Mixed Reality Headset](https://www.acer.com/ac/en/US/content/series/wmr)
![](https://i.imgur.com/xzJRNDy.png)
 - a smartphone with wifi, bluetooth, and location function

## Usage
Firstly, connect Mind Monitor with your smartphone with Mind Monitor application, then send the EEG data to museVR through OSC protocal. OSC connection settings could be found in **EEGReceiver** object.
In museVR, we have three stages.
### Collecting baseline
We collect brainwaves baseline for 30 seconds, and will pause collecting if the connection is not good enough. In this stage, there will be piano background music, indicating the user now should relax and try not to process anything in mind. The environment will be covered by heavy fog, so the user can't see the surroundings.

### Concentrating
After collecting enough data for baseline, the piano music will stop, indicates the user now should concentrate. As the user concentrate, the fog will be cleared gradually. In this stage, the user's task is to clear all the fog by keep concentrating.

### Complete
After the fog is totally cleared, there will be a natural bird tweet for two seconds, indicate the user now finishing the task.

## Demo
[![Demo](https://img.youtube.com/vi/BerILb4paMs/0.jpg)](https://www.youtube.com/watch?v=BerILb4paMs)


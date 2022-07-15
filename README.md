# Car park
This project is a small machine learning project made in Unity during the year of 2018.
### Table of Contents
- [General Info](#general-info)
- [Technologies Used](#technologies-used)
- [Setup](#setup)
- [How To Use The Application](#how-to-use-the-application)

___

### General Info:
The application was made in Unity, which involved programming in the C# language. Apart from programming, I also got to learn a lot about Unity in general from this little project. The weights of all the individual neural networks representing the "surviving" cars from the genetic algorithm are saved in individual binary files and can be loaded again on reboot of the application, unless resetting the networks is desired by the user.

___

### Technologies Used:
This project utilizes the following technologies:
- C#
- Unity

___

### Setup:
Simply clone this project, head into Final Build and then hit Car Park.exe file to launch the Unity application.

___

### How To Use The Application:
The application is rather simple to use. Simply start the application and watch as the cars learn how to drive around the track. The cars are given 15 seconds to try 
and make it around the track, and if they hit a wall, they are stopped immediately. Then the process is repeated over and over again and again. At first the progress 
might seem a bit slow, but if left in the background for a couple of minutes, expect to see some self driving cars.

Furthermore, the camera view can be changed using the left and right arrows. Below are all the featured camera angles on display:
![Angle 1](https://github.com/Morshok/readme-images/blob/master/Car%20Park/angle1.png)<br>
![Angle 2](https://github.com/Morshok/readme-images/blob/master/Car%20Park/angle2.png)<br>
![Angle 3](https://github.com/Morshok/readme-images/blob/master/Car%20Park/angle3.png)<br>
![Angle 4](https://github.com/Morshok/readme-images/blob/master/Car%20Park/angle4.png)<br>
![Angle 5](https://github.com/Morshok/readme-images/blob/master/Car%20Park/angle5.png)<br>
![Angle 6](https://github.com/Morshok/readme-images/blob/master/Car%20Park/angle6.png)<br>

As can be seen on the images above, the user has the ability to reset the networks by hitting the button in the top left corner of the screen. What can also be seen are a bunch of transparent glaspanes awkwardly placed around the track. These are checkpoints to score the cars performance during a run. Depending on a cars score, it will shift from red to blue to cyan to green. The genetic algorithm keeps the top 50% best performing cars from previous runs. Remaining cars from are copied from cars of previous runs and there is also a slight chance that these cars will be mutated, thus "genetic algorithm"<br><br>
Below is a display of the cars racing through the tracks after 

As a final note, and as you might have also already noticed, there is no quit application button or anything featured, because back in 2018 I forgot to implement it. To get around this simply hit alt+f4 when finished with the application, or exit the program from the Task Manager.

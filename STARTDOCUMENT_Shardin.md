# Start Document for "Skating Championship"
Start document written by Mathew Shardin. Student code **4951735**

## Problem Description
>A number of skaters take part in a skating championship. The following
distances are skated consecutively: 500 metres, 5000 metres, 1500 metres and
10000 metres. Times are registered precisely to hundredths of seconds. The
time achieved for the various distances is converted into points by reducing
each time to a 500 metre time. The skater with the lowest total number of
points wins the championship.
A program must be developed in which the name and times (format mmsshh)
can be entered for each consecutive competitor. The points total of each skater
must then be calculated and shown, as well as who the winner is. 

A speed skating board of juries needs an application that helps to determine a winner in a competition. The programm must track the following things:
1.  Times (format mmsshh) for 500, 5000, 1500, 10000 meter distances
2.  Names of the athletes
3.  Total number of points

The application must be able to:
1. Register names and times of the athletes
2. Calculate the total number of points per each athlete
3. Determine the winner with lowest number of total points

Additional application requirements:
1. Contain 3 tabs
2. Contain a start-up splash screen
3. Contain an about box
4. Be displayed in the (Quick Launch) toolbar
5. The program contains a context menu, which has the following options: a shortcut to each tab, shortcut to the about box, open and close buttons

## Input & Output
In this section the inputs and outputs of the application are described. The tabe below provides all the inputs a user has to introduce to make the application function.
| Case          | Data Type | Conditions |
|---------------|-----------|------------|
| Skater's name | String    | Not empty  |
| 500m time     | DateTime  | `time`>0   |
| 5000m time    | DateTime  | `time`>0   |
| 1500m time    | DateTime  | `time`>0   |
| 10000m time   | DateTime  | `time`>0   |

The table below provides all the outputs a user can see.
| Case                   | Data Type |
|------------------------|-----------|
| Athletes' names        | String    |
| Athletes' total points | Double    |
| Winner's name          | String    |

The tabe below provides all the calculation done by the application.
| Case                             | Calculation                                                  |
|----------------------------------|--------------------------------------------------------------|
| Convert times to a 500m distance | `DateTime converted to unix timestamp`/(`distance`/500)      |
| Total number of points           | Sum of `time` for all distances converted to a 500m distance |
